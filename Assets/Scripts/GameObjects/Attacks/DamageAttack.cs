﻿using Assets.Scripts.GameObjects.Entities;
using Assets.Scripts.GameObjects.Fractions;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Attacks
{
    public class DamageAttack : BasicAttack
    {
        public override event Action<IBasicEntity,GameObject> OnAttacking;
        public override event Action<bool> OnViewEnemy;

        [SerializeField] private int damage;
        public int Damage { get => damage; set => damage = value >= 0 ? value : 0; }

        protected override IEnumerator Attack()
        {
            bool attacking = false;
            while (!attacking && IsActive)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right, DistanceAttack);
                foreach (var hit in hits)
                    if (hit.collider != null && (hit.collider?.GetComponent<IFraction>()?.Fraction ?? this.Fraction.Fraction) != this.Fraction.Fraction)
                    {
                        if (hit.collider?.GetComponent<IBasicEntity>() is IBasicEntity entity && CheckEntity(entity))
                        {
                            OnViewEnemy?.Invoke(true);
                            entity.TakeDamage(Damage);
                            OnAttacking?.Invoke(entity,hit.collider.gameObject);
                            attacking = true;
                        }
                    }

                yield return null;
            }
        }
    }
}
