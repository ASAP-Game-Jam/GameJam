using System;

namespace Assets.Scripts.CustomEventArgs
{
    public class EventMarkedArgs : EventArgs
    {
        public TowerType TowerType { get; private set; }
        public uint Cost { get; private set; }
        public EventMarkedArgs(TowerType type, uint cost)
        {
            TowerType = type;
            Cost = cost;
        }
    }
}
