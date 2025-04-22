using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Interfaces.Base;
using Assets.Scripts.Interfaces.Enemy;
using System;

namespace Assets.Scripts.Enemy
{
    public class BearController : UnitController
    {
        protected override void IAttackHandle(IAttack attack)
        {
            if (attack != null)
                attack.OnViewEnemyObject += (object sender, EventArgs args) =>
                {
                    if (args is EventUnitViewArgs bArgs && bArgs.Value && bArgs.ViewObject is not IBase) 
                        StopMove();
                    else StartMove();
                };
        }
    }
}
