using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Enemy;
using Assets.Scripts.Tower;
using System;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    public class ObjectToAudio : MonoBehaviour
    {
        [SerializeField] private AudioManager audioManager;
        private TowerSpawnManager towerSpawnManager;
        private EnemySpawnManager enemySpawnManager;

        private void Start()
        {
            if(audioManager == null) 
                audioManager = FindAnyObjectByType<AudioManager>();
            towerSpawnManager = FindObjectOfType<TowerSpawnManager>();
            enemySpawnManager = FindObjectOfType<EnemySpawnManager>();

            towerSpawnManager.OnSpawn += OnSpawnTower;
            enemySpawnManager.OnSpawn -= OnSpawnEnemy;
        }

        private void OnSpawnTower(object sender, EventArgs eArgs)
        {
            if (eArgs is EventTowerSpawnArgs args)
            {
                GameObject gameObject = args.GameObject;
                if (gameObject.GetComponent<IDestroyObject>() is IDestroyObject destroyObject && destroyObject != null)
                {
                    audioManager.PlayConstructionSound();
                    switch (args.TowerType)
                    {
                        case TowerType.Cannon:
                        case TowerType.LazerGun:
                            gameObject.GetComponent<ITowerAttack>().OnAttack += (object sender, EventArgs e) => { audioManager.PlayCannonShot(); };
                            break;
                        case TowerType.Generator:

                            break;
                        case TowerType.Rocket:

                            break;
                        case TowerType.Totem:

                            break;
                    }
                }
            }
        }

        private void OnSpawnEnemy(object sender, EventArgs eArgs)
        {
            if (eArgs is EventEnemySpawnArgs args)
            {
                GameObject gameObject = args.GameObject;
                if (gameObject.GetComponent<IDestroyObject>() is IDestroyObject destroyObject && destroyObject != null)
                {
                    audioManager.PlayConstructionSound();
                    switch (args.EnemyType)
                    {
                        case EnemyType.Tank:
                            gameObject.GetComponent<IEnemyAttack>().OnAttack += (object sender, EventArgs e) => { audioManager.PlayTankShot(); };
                            break;
                        case EnemyType.Stick:
                            break;
                        case EnemyType.FingerGun:
                            gameObject.GetComponent<IEnemyAttack>().OnAttack += (object sender, EventArgs e) => { audioManager.PlayBlasterShot(); };
                            break;
                    }
                }
            }
        }
    }
}
