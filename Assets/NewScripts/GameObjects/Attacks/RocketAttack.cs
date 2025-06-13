using System;
using System.Collections;

namespace Assets.Scripts.GameObjects.Attacks
{
    public class RocketAttack : DamageAttack
    {
        protected override IEnumerator Attack()
        {
            yield return base.Attack();
        }
    }
}
