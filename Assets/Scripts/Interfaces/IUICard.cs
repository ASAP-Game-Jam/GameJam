using System;

namespace Assets.Scripts.Interfaces
{
    public interface IUICard
    {
        event EventHandler OnCardMarked;
        event EventHandler OnCardCancel;
        
    }
}
