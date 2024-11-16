using System;

namespace Assets.Scripts.Interfaces
{
    public interface ITowerAttack
    {
        event EventHandler OnAttack;
        event EventHandler OnReloaded;
        IBullet Bullet { get;set; }
        void SetLevelManager(ILevelManager levelManager);
    }
}
