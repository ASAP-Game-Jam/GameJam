using Assets.Scripts.Interfaces;
using System;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class TowerSpawnManager : MonoBehaviour, ISpawnerManager
    {
        public event EventHandler OnSpawn;
        public GameObject currentPlant;
        public Sprite currentPlantSprite;
        public Transform tiles;
        public LayerMask tileMask;

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
            }
        }
    }
}
