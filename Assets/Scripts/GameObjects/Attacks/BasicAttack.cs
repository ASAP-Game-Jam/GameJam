using Assets.Scripts.GameObjects.Entities;
using Assets.Scripts.GameObjects.Fractions;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Attacks
{
    [RequireComponent(typeof(IFraction))]
    [RequireComponent(typeof(ITarget))]
    public class BasicAttack : MonoBehaviour
    {
        public virtual event Action<IBasicEntity, GameObject> OnAttacking;
        public virtual event Action<bool> OnViewEnemy;
        [Header("BasicAttack Parameters")]
        [SerializeField] private float _distanceAttack = 1f;
        public bool AutoStart = true;
        protected IFraction Fraction { get; private set; }
        protected ITarget Target { get; private set; }
        public bool IsActive { get; private set; } = false;
        public float DistanceAttack => _distanceAttack;

        private void Awake()
        {
            Fraction = GetComponent<IFraction>();
            Target = GetComponent<ITarget>();
        }
        protected virtual void Start()
        {
            if (AutoStart)
                Startup();
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + _distanceAttack * Mathf.Sign(transform.localScale.x)
                , transform.position.y, transform.position.z));
        }
        protected virtual IEnumerator Attack()
        {
            bool attacking = false;
            while (!attacking && IsActive)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right, DistanceAttack);

                foreach (RaycastHit2D hit in hits)
                    if (hit.collider != null && (hit.collider?.GetComponent<IFraction>()?.Fraction ?? this.Fraction.Fraction) != this.Fraction.Fraction)
                    {
                        if (hit.collider?.GetComponent<IBasicEntity>() is IBasicEntity entity && CheckEntity(entity))
                        {
                            OnViewEnemy?.Invoke(true);
                            entity?.TakeDamage(entity.HP);
                            OnAttacking?.Invoke(entity, hit.collider.gameObject);
                            attacking = true;
                            break;
                        }
                    }

                yield return null;
            }
        }
        // Проверка BasicEntity - атакуем ли его или нет
        protected bool CheckEntity(IBasicEntity entity)
        {
            return Target.CheckEntity(entity);
        }

        public void Startup()
        {
            if (!IsActive)
            {
                IsActive = true;
                StartCoroutine(Attack());
            }
        }
        public void Shutdown() => IsActive = false;
    }
}
