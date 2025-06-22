using Assets.Scripts.GameObjects.Entities;
using Assets.Scripts.GameObjects.Fractions;
using Assets.Scripts.GameObjects.Moving;
using Assets.Scripts.Level;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Attacks
{
    public class DistanceAttack : AttackWithTime
    {
        public override event Action OnAttacking;
        public override event Action<bool> OnViewEnemy;
        public Vector2 AttackPointOffset = Vector2.zero;
        [SerializeField] protected GameObject Bullet;

        protected float GetDistance(Vector3 point)
            => base.Fraction.Fraction == FractionType.Ally ? Math.Min(DistanceAttack, Math.Abs(point.x - GlobalSettings.MaxRayX)) : DistanceAttack;
        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(AttackPoint, new Vector3(AttackPoint.x + GetDistance(AttackPoint) * Mathf.Sign(transform.localScale.x)
                , AttackPoint.y, AttackPoint.z));
        }
        protected override IEnumerator Attack()
        {
            // Ищет противника в одной линии и атакует
            IBasicEntity enemyEntity = null;
            Transform currentTransform = transform;
            while (IsActive)
            {
                enemyEntity = null;
                if (enemyEntity == null)
                {
                    RaycastHit2D[] hits = Physics2D.RaycastAll(AttackPoint, Mathf.Sign(currentTransform.localScale.x) == -1 ? Vector2.left : Vector2.right, GetDistance(AttackPoint));

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
                    OnViewEnemy?.Invoke(enemyEntity != null);

                    if (enemyEntity != null) // Если спереди челик, то 1-я атака ждет
                        yield return new WaitForSeconds(ForAllFirstAttackCooldown);
                }
                if (enemyEntity != null)
                {
                    GameObject obj = Instantiate(Bullet, AttackPoint, Quaternion.identity, currentTransform);
                    if (obj != null)
                    {
                        OnAttacking?.Invoke();
                        enemyEntity = null;

                        obj.GetComponent<Transform>()?.SetParent(null);

                        if (obj.GetComponent<StaticPointsMove>() != null)
                            new PointFirstDirectionSecondMove(obj, transform.position, AttackPoint);

                        if (obj.TryGetComponent<DamageAttack>(out DamageAttack basicAttack))
                            basicAttack.Damage = this.Damage;

                        yield return new WaitForSeconds(Cooldown);
                    }
                    else
                        yield return null;
                }
                else
                    yield return null;
            }
        }
        protected virtual Vector3 AttackPoint
            => this.transform.position + new Vector3(AttackPointOffset.x, AttackPointOffset.y);
    }
}
