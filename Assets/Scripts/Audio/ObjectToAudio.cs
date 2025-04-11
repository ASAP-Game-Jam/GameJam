using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Enemy;
using Assets.Scripts.Tower;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Audio
{
    public class ObjectToAudio : MonoBehaviour
    {
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private Button[] buttons;
        private TowerSpawnManager towerSpawnManager;
        private EnemySpawnManager enemySpawnManager;

        private void Start()
        {
            if (audioManager == null)
                audioManager = FindAnyObjectByType<AudioManager>();
            towerSpawnManager = FindObjectOfType<TowerSpawnManager>();
            enemySpawnManager = FindObjectOfType<EnemySpawnManager>();
            if (towerSpawnManager != null)
                towerSpawnManager.OnSpawn += OnSpawnTower;
            if (enemySpawnManager != null)
                enemySpawnManager.OnSpawn += OnSpawnEnemy;
            if (buttons != null && buttons.Length > 0)
                foreach (Button button in buttons)
                    button.onClick.AddListener(audioManager.PlayButtonClick);
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
                        case TowerType.Totem:

                            break;
                    }
                }
                else if (gameObject.GetComponent<IBullet>() is IBullet bullet)
                {
                    audioManager.PlayRocketLaunch();
                    gameObject.GetComponent<IBullet>().OnHit += (object sender, EventArgs e) => { audioManager.PlayRocketExplosion(); };
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
                            gameObject.GetComponent<IAttack>().OnAttack += (object sender, EventArgs e) => { audioManager.PlayTankShot(); };
                            break;
                        case EnemyType.Stick:
                            gameObject.GetComponent<IAttack>().OnAttack += (object sender, EventArgs e) => { audioManager.PlayStickHit(); };
                            break;
                        case EnemyType.FingerGun:
                            gameObject.GetComponent<IAttack>().OnAttack += (object sender, EventArgs e) => { audioManager.PlayBlasterShot(); };
                            break;
                    }
                }
            }
        }
    }
}
