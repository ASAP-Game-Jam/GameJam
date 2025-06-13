using Assets.Scripts.Managers;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Activate
{
    public class Generator : MonoBehaviour, IController
    {
        [SerializeField] private int _generateCount = 5;
        [SerializeField] private float _cooldown = 3f;
        public bool IsActive { get; private set; }

        public void Shutdown()
        {
            IsActive = false;
        }
        public void Startup()
        {
            StartCoroutine(Generate());

        }
        private void Start()
        {
            Startup();
        }
        private IEnumerator Generate()
        {
            if (!IsActive)
            {
                IsActive = true;
                yield return null;
                while (IsActive)
                {
                    LevelManager.StateManager.ChangeEnergy(_generateCount);
                    yield return new WaitForSeconds(_cooldown);
                }
            }
        }
    }
}
