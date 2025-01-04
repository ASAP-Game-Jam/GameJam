using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Tower;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.VersionControl;
using UnityEngine;

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

        private void Start()
        {
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
                hit.collider.GetComponent<SpriteRenderer>().sprite = currentPlantSprite;
                hit.collider.GetComponent<SpriteRenderer>().enabled = true;

                if (Input.GetMouseButtonDown(0))
                {
                    GameObject obj = Instantiate(currentPlant, hit.collider.transform.position, Quaternion.identity);
                    if (levelManager != null) levelManager.Score -= currentCost;
                    currentCost = 0;
                    AddEventForTower(obj);
                    OnSpawn?.Invoke(this, EventArgs.Empty);
                    currentPlant = null;
                    currentPlantSprite = null;
                }
            }
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

        private void UpdateEnergy(object sender, EventArgs args)
        {
            if (args is EventEnergyArgs energyArgs)
            {
                levelManager.Score += energyArgs.Energy;
            }
        }
    }
}
