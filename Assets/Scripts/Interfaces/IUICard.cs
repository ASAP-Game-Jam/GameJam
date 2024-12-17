using System;

namespace Assets.Scripts.Interfaces
{
    public interface IUICard
    {
        event EventHandler OnCardMarked;
        event EventHandler OnCardCancel;
        uint Cost { get; }
        void SetLevelManager(ILevelManager levelManager);
    }
}
