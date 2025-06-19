using Assets.Scripts.GameObjects;
using Assets.Scripts.GameObjects.Attacks;
using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Audio
{
    public class ObjectToAudio : MonoBehaviour
    {
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private Button[] buttons;

        private void Start()
        {
            if (audioManager == null)
                audioManager = FindAnyObjectByType<AudioManager>();
            if (LevelManager.AllyManager != null)
                LevelManager.AllyManager.OnSpawned += OnSpawnTower;
            if (LevelManager.EnemyManager != null)
                LevelManager.EnemyManager.OnEnemySpawned += OnSpawnEnemy;
            if (buttons != null && buttons.Length > 0)
                foreach (Button button in buttons)
                    button.onClick.AddListener(audioManager.PlayButtonClick);
        }

        private void OnSpawnTower(AllyType type, GameObject obj)
        {

            audioManager.PlayConstructionSound();
            var attack = obj.GetComponent<BasicAttack>();
            switch (type)
            {
                case AllyType.Cannon:
                case AllyType.LazerGun:
                    attack.OnAttacking += () => { audioManager.PlayCannonShot(); };
                    break;
                case AllyType.Generator:

                    break;
                case AllyType.Totem:

                    break;
            }
        }


        private void OnSpawnEnemy(EnemyType type, GameObject obj)
        {
            audioManager.PlayConstructionSound();
            var attack = obj.GetComponent<BasicAttack>();
            switch (type)
            {
                case EnemyType.Tank:
                    attack.OnAttacking += () => audioManager.PlayTankShot();
                    break;
                case EnemyType.Stick:
                    attack.OnAttacking += () => audioManager.PlayStickHit();
                    break;
                case EnemyType.FingerGun:
                    attack.OnAttacking += () => audioManager.PlayBlasterShot();
                    break;
            }
        }
    }
}

