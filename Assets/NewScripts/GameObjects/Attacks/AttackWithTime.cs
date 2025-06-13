using Assets.Scripts.GameObjects.Entities;
using Assets.Scripts.GameObjects.Fractions;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Attacks
{
    public class AttackWithTime : DamageAttack
    {
        public override event Action OnAttacking;
        public override event Action<bool> OnViewEnemy;
        [Header("Cooldown Parameters")]
        [SerializeField] private float _cooldown = 1.5f;
        [SerializeField] private float _forAllFirstAttackCooldown = 0.4f;
        public float Cooldown => _cooldown;
        public float ForAllFirstAttackCooldown => _forAllFirstAttackCooldown;

        protected override IEnumerator Attack()
        {
            // Ищет противника в одной линии и атакует
            IBasicEntity enemyEntity = null;
            while (IsActive)
            {
                if (enemyEntity == null)
                {
                    RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Mathf.Sign(transform.localScale.x) == -1 ? Vector2.left : Vector2.right, DistanceAttack);

                    enemyEntity = null;
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider != null && (hit.collider.GetComponent<IFraction>()?.Fraction ?? this.Fraction.Fraction) != this.Fraction.Fraction)
                        {
                            enemyEntity = hit.collider.GetComponent<IBasicEntity>();
                            if (enemyEntity != null && CheckEntity(enemyEntity))
                            {
                                break;
                            }
                            else enemyEntity = null;
                        }
                    }
                    //enemyEntity = hits.Select(hit => hit.collider).Where(c => c?.GetComponent<IFraction>()?.Fraction != this.Fraction.Fraction).Select(c => c.GetComponent<IBasicEntity>()).Where(e=>CheckEntity(e)).FirstOrDefault();
                    OnViewEnemy?.Invoke(enemyEntity != null);

                    if (enemyEntity != null) // Если спереди челик, то 1-я атака ждет
                        yield return new WaitForSeconds(_forAllFirstAttackCooldown);
                }
                if (enemyEntity != null)
                {
                    enemyEntity?.TakeDamage(this.Damage);
                    OnAttacking?.Invoke();
                    enemyEntity = null;
                    yield return new WaitForSeconds(Cooldown);
                }
                else
                    yield return null;
            }
        }
    }
}
