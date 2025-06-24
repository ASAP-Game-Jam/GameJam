using Assets.Scripts.GameObjects.Entities;
using Assets.Scripts.GameObjects.Fractions;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Attacks
{
    public class AttackWithTime : DamageAttack
    {
        public override event Action<IBasicEntity, GameObject> OnAttacking;
        public override event Action<bool> OnViewEnemy;
        [Header("Cooldown Parameters")]
        [SerializeField] private float _cooldown = 1.5f;
        [SerializeField] private float _forAllFirstAttackCooldown = 0.4f;
        public float Cooldown => _cooldown;
        public float ForAllFirstAttackCooldown => _forAllFirstAttackCooldown;

        protected override IEnumerator Attack()
        {
            bool viewEnemy = false;
            // Ищет противника в одной линии и атакует
            while (IsActive)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Mathf.Sign(transform.localScale.x) == -1 ? Vector2.left : Vector2.right, DistanceAttack);

                viewEnemy = false;
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider != null && (hit.collider.GetComponent<IFraction>()?.Fraction ?? this.Fraction.Fraction) != this.Fraction.Fraction)
                    {
                        if (hit.collider.GetComponent<IBasicEntity>() is IBasicEntity enemyEntity && CheckEntity(enemyEntity))
                        {
                            viewEnemy = true;
                            OnViewEnemy?.Invoke(viewEnemy);
                            yield return new WaitForSeconds(_forAllFirstAttackCooldown);
                            enemyEntity?.TakeDamage(this.Damage);
                            OnAttacking?.Invoke(enemyEntity, hit.collider.gameObject);
                            enemyEntity = null;
                            yield return new WaitForSeconds(Cooldown);
                            break;
                        }
                    }
                }
                if (!viewEnemy)
                    OnViewEnemy?.Invoke(viewEnemy);
                yield return null;
            }
        }
    }
}
