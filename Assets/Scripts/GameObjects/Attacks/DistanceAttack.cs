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
        public override event Action<IBasicEntity, GameObject> OnAttacking;
        public override event Action<bool> OnViewEnemy;
        public Vector2 AttackPointOffset = Vector2.zero;
        [SerializeField] protected GameObject Bullet;

        protected float GetDistance(Vector3 point)
            => !Application.isPlaying ? DistanceAttack : (base.Fraction.Fraction == FractionType.Ally ? Math.Min(DistanceAttack, Math.Abs(point.x - GlobalSettings.MaxRayX)) : DistanceAttack);
        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(AttackPoint, new Vector3(AttackPoint.x + GetDistance(AttackPoint) * Mathf.Sign(transform.localScale.x)
                , AttackPoint.y, AttackPoint.z));
        }
        protected override IEnumerator Attack()
        {
            bool viewEnemy = false;
            Transform currentTransform = transform;
            while (IsActive)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(AttackPoint, Mathf.Sign(currentTransform.localScale.x) == -1 ? Vector2.left : Vector2.right, GetDistance(AttackPoint));

                viewEnemy = false;
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider != null && (hit.collider.GetComponent<IFraction>()?.Fraction ?? this.Fraction.Fraction) != this.Fraction.Fraction)
                    {
                        if (hit.collider.GetComponent<IBasicEntity>() is IBasicEntity enemyEntity && CheckEntity(enemyEntity))
                        {
                            viewEnemy = true;
                            OnViewEnemy?.Invoke(viewEnemy);
                            yield return new WaitForSeconds(ForAllFirstAttackCooldown);
                            GameObject obj = Instantiate(Bullet, AttackPoint, Quaternion.identity, currentTransform);
                            if (obj != null)
                            {
                                OnAttacking?.Invoke(enemyEntity, hit.collider.gameObject);
                                enemyEntity = null;

                                obj.GetComponent<Transform>()?.SetParent(null);

                                if (obj.GetComponent<StaticPointsMove>() != null)
                                    new PointFirstDirectionSecondMove(obj, transform.position, AttackPoint);

                                if (obj.TryGetComponent<DamageAttack>(out DamageAttack basicAttack))
                                    basicAttack.Damage = this.Damage;

                                yield return new WaitForSeconds(Cooldown);
                            }
                            break;
                        }
                        else enemyEntity = null;

                    }

                }
                if (!viewEnemy)
                    OnViewEnemy?.Invoke(viewEnemy);
                yield return null;
            }
        }
        protected virtual Vector3 AttackPoint
            => this.transform.position + new Vector3(AttackPointOffset.x, AttackPointOffset.y);
    }
}
