﻿using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Tower;
using Assets.Scripts.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Tower
{
    public class TowerSpawnManager : MonoBehaviour, ISpawnerManager
    {
        public event EventHandler OnSpawn;
        private GameObject currentPlant;
        private Sprite currentPlantSprite;
        private uint currentCost;
        private TowerType currentTowerType;
        private List<Transform> tiles;
        public LayerMask tileMask;
        private ITowerFabric fabric;
        private ILevelManager levelManager;
        [SerializeField] private Vector3 rocketStartPoint = new Vector3(-11.3039055f, 4.54456806f, 0);

        private void Start()
        {
            UIModifyButton removeButton = FindFirstObjectByType<UIModifyButton>();
            if (removeButton != null) 
                removeButton.OnSelect += (object sender, EventArgs args) => { Clear(); };
            levelManager = FindAnyObjectByType<LevelManager>();
            foreach (var i in FindObjectsOfType<UIButton>())
                i.OnCardMarked += BuyEntity;
            fabric = FindObjectOfType<TowerFabric>();
            tiles = new List<Transform>();
            foreach (var i in FindObjectsOfType<Cell>())
                tiles.Add(i.GetComponent<Transform>());
            FindTowers();
        }
        private void FindTowers()
        {
            foreach (Tower tower in FindObjectsOfType<Tower>())
                AddEventForTower(tower.gameObject);
        }
        private void BuyEntity(object sender, EventArgs args)
        {
            if (args is EventMarkedArgs mark && levelManager?.Score >= mark.Cost)
            {
                currentPlant = fabric?.GetPrefab(mark.TowerType);
                currentPlantSprite = currentPlant?.GetComponent<SpriteRenderer>().sprite;
                currentCost = mark.Cost;
                currentTowerType = mark.TowerType;
                if (mark.TowerType == TowerType.Rocket)
                {
                    Spawn(rocketStartPoint);
                    Clear();
                }
            }
        }

        private void Update()
        {
            RaycastHit2D hit = Physics2D.Raycast(
                Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero,
                Mathf.Infinity,
                tileMask
                );

            foreach (Transform tile in tiles)
                tile.GetComponent<SpriteRenderer>().enabled = false;

            if (hit.collider && currentPlant)
            {
                ICell cell = hit.collider.GetComponent<ICell>();
                if (cell.IsEmpty() && levelManager.Score >= currentCost)
                {
                    hit.collider.GetComponent<SpriteRenderer>().sprite = currentPlantSprite;
                    hit.collider.GetComponent<SpriteRenderer>().enabled = true;

                    if (Input.GetMouseButtonDown(0))
                    {
                        Spawn(hit.collider.transform.position, cell);
                    }
                    else if (Input.GetMouseButtonDown(1))
                        Clear();
                }
            }
        }

        public void Clear()
        {
            currentCost = 0;
            currentPlant = null;
            currentPlantSprite = null;
        }

        private void Spawn(Vector3 position, ICell cell = null)
        {
            if (position == null) return;
            if (currentTowerType==TowerType.BearBarracks && !EnergyInLines(cell)) return;
            GameObject obj = Instantiate(currentPlant, position, Quaternion.identity);
            var tower = obj.GetComponent<ITower>();
            if(tower != null)
                tower.TowerType = currentTowerType;
            if (cell != null)
                cell.AddTower(tower);
            if (levelManager != null) levelManager.Score -= currentCost; // Убрать монетки тк купили башню
            AddEventForTower(obj);
#if !UNITY_EDITOR
            Clear();
#endif
            OnSpawn?.Invoke(this, new EventTowerSpawnArgs(obj, currentTowerType));
        }

        private void AddEventForTower(GameObject obj)
        {
            switch (currentTowerType)
            {
                case TowerType.Generator:
                    if (obj.GetComponent<IEnergyTower>() is IEnergyTower energy)
                    {
                        energy.OnActivated += UpdateEnergy;
                    }
                    break;
            }
        }

        private bool EnergyInLines(ICell cell)
        {
            if (cell == null) return false;
            
            Debug.DrawRay(cell.Position, Vector2.right * 2, Color.green, 1f);
            Debug.DrawRay(cell.Position, Vector2.left * 2, Color.red, 1f);

            RaycastHit2D[] hits = {
                Physics2D.Raycast(
                    new Vector3(cell.Position.x+1,cell.Position.y,cell.Position.z),
                    Vector2.right, 1,tileMask),
                Physics2D.Raycast(
                    new Vector3(cell.Position.x-1,cell.Position.y,cell.Position.z),
                    Vector2.right, 1,tileMask)
            };

            foreach (var hit in hits)
            {
                if (hit.collider && hit.collider.GetComponent<ICell>()?.GetTower()?.TowerType == TowerType.Generator)
                    return true;
            }
            return false;
        }
        private void UpdateEnergy(object sender, EventArgs args)
        {
            if (args is EventEnergyArgs energyArgs)
            {
                levelManager.Score += energyArgs.Energy;
            }
        }
    }
}
