using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Moving
{
    public interface IMovable
    {
        public bool IsMove { get; }
        public float CurrentSpeed { get; }

        public void Startup();
        public void Shutdown();
        public abstract void Reverse();

    }
    public abstract class AMovable : MonoBehaviour, IMovable
    {
        [SerializeField] private bool _isMove = true;
        [SerializeField] protected float _currentSpeed;
        public bool IsMove => _isMove;
        public float CurrentSpeed => _currentSpeed;
        private void Start()
        {
            if (IsMove) StartCoroutine(Move());
        }
        protected abstract IEnumerator Move();
        public virtual void Startup()
        {
            if (!IsMove)
            {
                _isMove = true;
                StartCoroutine(Move());
            }
        }
        public virtual void Shutdown() => _isMove = false;
        public abstract void Reverse();
    }
}
