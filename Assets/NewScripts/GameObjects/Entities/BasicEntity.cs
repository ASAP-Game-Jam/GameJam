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
        private IFraction _fraction;
        public bool IsDestroyed { get; private set; }
        public int HP => _hp;

        public int MaxHP { get; private set; }

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
                yield return null;
                Destroy(this.gameObject);
            }
        }
    }
}
