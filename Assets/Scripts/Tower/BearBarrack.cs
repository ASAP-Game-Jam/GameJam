using System;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class BearBarrack : TowerAttack
    {
        public override event EventHandler OnAttack;

        public override bool IsHit(GameObject pointObject)
            => true;

        public override void CreateBullet(Vector3 point)
        {
            GameObject pref = Instantiate(bulletPrefab, this.transform);
            if (pref != null)
            {
                OnAttack?.Invoke(this, EventArgs.Empty);

                attackTime = cooldown;
            }
        }
    }
}
