using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Base;

namespace Assets.Scripts.Enemy
{
    public class BearAttack : UnitAttack
    {
        private IBase _base;
        public override void Attack(IDestroyObject enemy)
        {
            switch (enemy)
            {
                case null:
                    if (_base != null)
                    {
                        _base.TakeDamage(Damage);
                        _base = null;
                    }
                    break;
                case IBase:
                    if (_base == null) _base = enemy as IBase;
                    break;
                default:
                    enemy?.TakeDamage(Damage);
                    CoolDownReset();
                    break;
            }

        }
    }
}
