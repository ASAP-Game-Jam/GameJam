using System;
using System.Collections;
using Assets.Scripts.GameObjects.Fractions;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Entities
{
    [RequireComponent(typeof(IFraction))]
    [RequireComponent(typeof(Collider2D))]
    public class BasicEntity : MonoBehaviour, IBasicEntity
    {
        public event Action<IBasicEntity, int> OnTakenDamage;
        public event Action<IBasicEntity> OnDestroyed;
        [SerializeField] private int _hp = 1;
        [SerializeField] private float destroyedTime = 0.5f;
        private IFraction _fraction;
        public bool IsDestroyed { get; private set; }
        public int HP => _hp;

        public int MaxHP { get; private set; }

        private int _cost = 0;
        public int Cost
        {
            get => _cost;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Cost cannot be negative.");
                _cost = value;
            }
        }
        private void Awake()
        {
            MaxHP = _hp;
        }

        public void TakeDamage(int damage)
        {
            _hp = Math.Clamp(_hp - damage, 0, _hp);
            OnTakenDamage?.Invoke(this, damage);
            if (_hp == 0 && !IsDestroyed)
            {
                StartCoroutine(Destroyed());
            }
        }

        private IEnumerator Destroyed()
        {
            if (!IsDestroyed)
            {
                IsDestroyed = true;
                OnDestroyed?.Invoke(this);
                yield return new WaitForSeconds(destroyedTime);
                Destroy(this.gameObject);
            }
        }
    }
}
