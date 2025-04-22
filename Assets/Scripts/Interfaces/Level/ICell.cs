using Assets.Scripts.Interfaces.Tower;
using System;
using System.Drawing;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface ICell
    {
        event EventHandler OnCellClick;
        Vector3 Position { get; }
        void AddTower(ITower tower);
        bool IsEmpty();
        ITower GetTower();
        void RemoveTower();
    }
}
