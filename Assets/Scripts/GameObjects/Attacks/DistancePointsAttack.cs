using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Attacks
{
    public class DistancePointsAttack : DistanceAttack
    {
        [SerializeField] private GameObject[] attackPoints;
        private int indexPoint = 0;
        protected override void Start()
        {
            base.Start();
            base.OnViewEnemy += (e) =>
            {
                if (!e) indexPoint = (indexPoint+1)%attackPoints.Length;
            };
        }
        protected override void OnDrawGizmos()
        {
            if (attackPoints.Length > 0)
            {
                Gizmos.color = Color.green;
                foreach (var attackPoint in attackPoints)
                {
                    var point = attackPoint.transform.position;// + this.transform.position;
                    Gizmos.DrawLine(point,
                        new Vector3(point.x + CurrentDistance * Mathf.Sign(transform.localScale.x),
                        point.y, point.z));
                }
            }
        }
        protected override Vector3 AttackPoint => attackPoints[indexPoint].transform.position;
    }
}
