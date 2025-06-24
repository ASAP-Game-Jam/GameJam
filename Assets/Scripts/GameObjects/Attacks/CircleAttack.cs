using Assets.Scripts.GameObjects.Entities;
using Assets.Scripts.GameObjects.Fractions;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Attacks
{
    public class CircleAttack : DamageAttack
    {

        public override event Action<IBasicEntity, GameObject> OnAttacking;
        public override event Action<bool> OnViewEnemy;
        protected override IEnumerator Attack()
        {
            while (IsActive)
            {
                bool attackedAtLeastOne = false;

                // Находим всех противников в радиусе
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, DistanceAttack);
                foreach (Collider2D col in colliders)
                {
                    IFraction fraction = col.GetComponent<IFraction>();
                    IBasicEntity entity = col.GetComponent<IBasicEntity>();

                    if (fraction != null && entity != null &&
                        fraction.Fraction != this.Fraction.Fraction &&
                        CheckEntity(entity))
                    {
                        entity.TakeDamage(this.Damage);
                        OnAttacking?.Invoke(entity, col.gameObject);
                        OnViewEnemy?.Invoke(true);
                        attackedAtLeastOne = true;
                    }
                }

                if (attackedAtLeastOne)
                    Shutdown();
                else
                    OnViewEnemy?.Invoke(false);

                yield return null;
            }
        }

    }
}
