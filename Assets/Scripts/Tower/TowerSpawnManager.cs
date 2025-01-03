using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Tower;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class TowerSpawnManager : MonoBehaviour, ISpawnerManager
    {
        public event EventHandler OnSpawn;
        public GameObject currentPlant;
        public Sprite currentPlantSprite;
        public List<Transform> tiles;
        public LayerMask tileMask;
        private ITowerFabric fabric;

        private void Start()
        {
            foreach (var i in FindObjectsOfType<UIButton>())
                i.OnCardMarked += BuyEntity;
            fabric = FindObjectOfType<TowerFabric>();
            tiles = new List<Transform>();
            foreach (var i in FindObjectsOfType<Cell>())
                tiles.Add(i.GetComponent<Transform>());
        }

        private void BuyEntity(object sender, EventArgs args)
        {
            if(args is EventMarkedArgs mark)
            {
                currentPlant = fabric.GetPrefab(mark.TowerType);
                currentPlantSprite = currentPlant.GetComponent<SpriteRenderer>().sprite;
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

            foreach(Transform tile in tiles)
                tile.GetComponent<SpriteRenderer>().enabled = false;

            if(hit.collider && currentPlant)
            {
                hit.collider.GetComponent<SpriteRenderer>().sprite = currentPlantSprite;
                hit.collider.GetComponent<SpriteRenderer>().enabled = true;

                if (Input.GetMouseButtonDown(0))
                {
                    Instantiate(currentPlant,hit.collider.transform.position, Quaternion.identity);
                    OnSpawn?.Invoke(this, EventArgs.Empty);
                    currentPlant = null;
                    currentPlantSprite = null;
                }
            }
        }
    }
}
