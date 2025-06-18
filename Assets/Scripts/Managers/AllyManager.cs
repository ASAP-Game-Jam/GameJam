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
    public enum PlacementType { OnAnchorCell, OnCell, Anywhere, Activate }

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
        public event Action<AllyType, GameObject> OnSpawned;
        public EStatusManager Status { get; private set; }
        public Vector3 SpawnActivateObjects;

        // Выбранный объект и связанные значения
        private (TypeObject<AllyType, GameObject> TypeObject, int Cost) _selectedObject;
        private Sprite _selectedSprite;
        private SpriteRenderer _selectedRenderer;

        public void Startup()
        {
            LevelManager.UIManager.OnSelected += OnSelectObject;
            _selectedObject = (PlacementObjects.Find(match => match.Key == AllyType.None), 0);
            Status = EStatusManager.Started;
            StartCoroutine(GameUpdate());
        }

        public void Shutdown()
        {
            LevelManager.UIManager.OnSelected -= OnSelectObject;
            Status = EStatusManager.Shutdown;
        }

        private void OnSelectObject((AllyType type, int cost) obj)
        {
            _selectedObject = (PlacementObjects.Find(match => match.Key == obj.type), obj.cost);
            _selectedSprite = _selectedObject.TypeObject.Value?.GetComponent<SpriteRenderer>()?.sprite;
        }

        private IEnumerator GameUpdate()
        {
            while (Status == EStatusManager.Started)
            {
                // Если не выбран объект или недостаточно энергии - пропускаем кадр
                if (_selectedObject.TypeObject.Key == AllyType.None || !LevelManager.StateManager.IsEnergy(_selectedObject.Cost))
                {
                    yield return null;
                    continue;
                }

                HandlePlacement();

                yield return null;
            }
        }

        /// <summary>
        /// Разбивка логики обработки пользовательского ввода и спауна объекта.
        /// </summary>
        private void HandlePlacement()
        {
            bool isActivate = _selectedObject.TypeObject.PlacementType == PlacementType.Activate;
            Vector3 clickPoint = isActivate ? SpawnActivateObjects : GetMouseWorldPoint();
            if (!isActivate && clickPoint == Vector3.zero)
                return;

            Cell cell = null;
            bool canSpawn = DetermineSpawnPosition(ref clickPoint, out cell);
            // Делаем дополнительную проверку через компонент проверки размещения (если он есть)
            if (!CheckCustomPlacement(clickPoint, cell))
                return;

            if (canSpawn && IsSpawnTriggered(isActivate))
            {
                SpawnAlly(clickPoint, cell, isActivate);
            }
        }

        /// <summary>
        /// Получает мировую координату мыши. Если указатель над UI - возвращает Vector3.zero.
        /// </summary>
        private Vector3 GetMouseWorldPoint()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return Vector3.zero;

            Camera cam = Camera.main;
            return cam != null ? cam.ScreenToWorldPoint(Input.mousePosition) : Vector3.zero;
        }

        /// <summary>
        /// Если выбран PlacementType должен ставиться на ячейку (OnCell / OnAnchorCell),
        /// производит поиск ячейки по позиции клика.
        /// </summary>
        private bool DetermineSpawnPosition(ref Vector3 clickPoint, out Cell cell)
        {
            cell = null;
            bool canSpawn = true;

            if (_selectedObject.TypeObject.PlacementType <= PlacementType.OnCell)
            {
                canSpawn = false;
                RaycastHit2D[] hits = Physics2D.RaycastAll(clickPoint, Vector2.zero, Mathf.Infinity, CellTileMask);
                foreach (var hit in hits)
                {
                    cell = hit.collider?.GetComponent<Cell>();
                    if (cell != null && cell.IsEmpty)
                    {
                        ViewSelect(hit.collider?.GetComponent<SpriteRenderer>());
                        canSpawn = true;
                        clickPoint = cell.transform.position;
                        break;
                    }
                }
            }
            else
            {
                // Для произвольного размещения (Anywhere) z зависит от y
                clickPoint = new Vector3(clickPoint.x, clickPoint.y, clickPoint.y / 100f);
            }

            return canSpawn;
        }

        /// <summary>
        /// Проверяем, была ли нажата кнопка мыши для спауна (если не режим Activate).
        /// </summary>
        private bool IsSpawnTriggered(bool isActivate) =>
            isActivate || Input.GetMouseButtonDown(0);

        /// <summary>
        /// Перед созданием объекта запрашиваем у префаба, есть ли компонент проверки условий размещения.
        /// Если компонент есть, делегируем ему проверку.
        /// </summary>
        private bool CheckCustomPlacement(Vector3 spawnPosition, Cell cell)
        {
            IPlacementConditionChecker checker =
                _selectedObject.TypeObject.Value.GetComponent<IPlacementConditionChecker>();
            if (checker != null)
            {
                return checker.CheckPlacement(spawnPosition, cell);
            }
            return true;
        }

        /// <summary>
        /// Создаёт объект, изменяет энергию, оповещает подписчиков и запускает контроллеры созданного объекта.
        /// </summary>
        private void SpawnAlly(Vector3 spawnPosition, Cell cell, bool isActivate)
        {
            GameObject obj = Instantiate(_selectedObject.TypeObject.Value, spawnPosition, Quaternion.identity);
            if (obj == null)
                return;

            obj.transform.parent = null;
            LevelManager.StateManager.ChangeEnergy(-_selectedObject.Cost);
            OnSpawned?.Invoke(_selectedObject.TypeObject.Key, obj);

            ResetSelectionView();

            if (cell != null && _selectedObject.TypeObject.PlacementType == PlacementType.OnAnchorCell)
            {
                if (obj.TryGetComponent<IBasicEntity>(out IBasicEntity entity))
                {
                    cell.AddObject(entity);
                }
            }

            foreach (var controller in obj.GetComponents<IController>())
            {
                controller.Startup();
            }

            if (isActivate)
                _selectedObject.TypeObject.Key = AllyType.None;
        }

        /// <summary>
        /// Обновляет визуальное представление выделения ячейки.
        /// </summary>
        private void ViewSelect(SpriteRenderer renderer)
        {
            if (renderer != null && _selectedSprite != null)
            {
                if (_selectedRenderer != renderer)
                {
                    if (_selectedRenderer != null)
                        _selectedRenderer.enabled = false;

                    _selectedRenderer = renderer;
                    _selectedRenderer.enabled = true;
                    _selectedRenderer.sprite = _selectedSprite;
                }
            }
        }

        /// <summary>
        /// Сбрасывает визуализацию выбранного объекта.
        /// </summary>
        private void ResetSelectionView()
        {
            _selectedSprite = null;
            if (_selectedRenderer != null)
            {
                _selectedRenderer.enabled = false;
                _selectedRenderer = null;
            }
        }
    }
}

// Интерфейс для проверки условий размещения
public interface IPlacementConditionChecker
{
    /// <summary>
    /// Проверяет, удовлетворяет ли размещение объекта заданной позиции и ячейки.
    /// </summary>
    /// <param name="spawnPosition">Позиция, где будет создан объект.</param>
    /// <param name="cell">Ячейка, в которой производится попытка спауна (если применимо).</param>
    /// <returns>True, если условия размещения удовлетворены.</returns>
    bool CheckPlacement(Vector3 spawnPosition, Cell cell = null);
}