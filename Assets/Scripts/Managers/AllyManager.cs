using Assets.Scripts.GameObjects;
using Assets.Scripts.GameObjects.Entities;
using Assets.Scripts.Level;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Managers
{
    public enum PlacementType { OnAnchorCell,OnCell, Anywhere, Activate }
    [Serializable]
    public struct TypeObject<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;
        public PlacementType PlacementType;
    }
    public class AllyManager : MonoBehaviour, IManager
    {
        public List<TypeObject<AllyType, GameObject>> PlacementObjects = new List<TypeObject<AllyType, GameObject>>();
        public LayerMask CellTileMask;
        public event Action<AllyType,GameObject> OnSpawned;
        public EStatusManager Status { get; private set; }
        public Vector3 SpawnActivateObjects;
        private (TypeObject<AllyType, GameObject> TypeObject, int Cost) selectObject;

        public void Shutdown()
        {
            LevelManager.UIManager.OnSelected -= OnSelectObject;
            Status = EStatusManager.Shutdown;
        }

        public void Startup()
        {
            LevelManager.UIManager.OnSelected += OnSelectObject;
            selectObject.Cost = 0;
            selectObject.TypeObject.Key = AllyType.None;
            Status = EStatusManager.Started;
            StartCoroutine(GameUpdate());
        }

        private void OnSelectObject((AllyType type, int cost) obj)
        {
            selectObject = (PlacementObjects.Find(match => match.Key == obj.type), obj.cost);
        }

        private IEnumerator GameUpdate()
        {
            while (Status == EStatusManager.Started)
            {

                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && LevelManager.StateManager.IsEnergy(selectObject.Cost) && selectObject.TypeObject.Key != AllyType.None)
                {
                    bool spawn = true;
                    Vector3 clickPoint = selectObject.TypeObject.PlacementType == PlacementType.Activate ? SpawnActivateObjects : Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Cell cell = null;
                    if (selectObject.TypeObject.PlacementType <= PlacementType.OnCell)
                    {
                        spawn = false;
                        RaycastHit2D[] hits = Physics2D.RaycastAll(
                                clickPoint,
                                Vector2.zero,
                                Mathf.Infinity,
                                CellTileMask
                            );
                        foreach (var hit in hits)
                        {
                            cell = hit.collider?.GetComponent<Cell>();
                            if (cell != null && cell.IsEmpty)
                            {
                                spawn = true;
                                clickPoint = cell.transform.position;
                                break;
                            }
                        }
                    }
                    else
                        clickPoint = new Vector3(clickPoint.x, clickPoint.y, clickPoint.y / 100);
                    if (spawn)
                    {
                        // Появление
                        var obj = Instantiate(selectObject.TypeObject.Value, clickPoint, Quaternion.identity);
                        if (obj != null)
                        {
                            obj.transform.parent = null;
                            LevelManager.StateManager.ChangeEnergy(-selectObject.Cost);
                            OnSpawned?.Invoke(selectObject.TypeObject.Key, obj);
                            if (cell != null && obj.GetComponent<IBasicEntity>() is IBasicEntity entity && selectObject.TypeObject.PlacementType == PlacementType.OnAnchorCell)
                            {
                                cell.AddObject(entity);
                            }
                            foreach (var i in obj.GetComponents<IController>())
                                i.Startup();
                        }
                    }
                }

                yield return null;
            }
        }


    }
}
