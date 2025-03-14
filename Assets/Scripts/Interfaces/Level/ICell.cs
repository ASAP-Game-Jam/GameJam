using Assets.Scripts.Interfaces.Tower;
using System;

namespace Assets.Scripts.Interfaces
{
    public interface ICell
    {
        event EventHandler OnCellClick;
        void AddTower(ITower tower);
        bool IsEmpty();
        ITower GetTower();
        void RemoveTower();
    }
}
