using System;

namespace Assets.Scripts.Interfaces.Tower
{
    public interface ITower
    {
        event EventHandler OnGetDamage;
        event EventHandler OnDestroy;
    }
}
