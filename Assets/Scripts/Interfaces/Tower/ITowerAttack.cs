using System;

namespace Assets.Scripts.Interfaces
{
    public interface ITowerAttack
    {
        event EventHandler OnAttack;
        event EventHandler OnReloaded;
        void SetLevelManager(ILevelManager levelManager);
    }
}
