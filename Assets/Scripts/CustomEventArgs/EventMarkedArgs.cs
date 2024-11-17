using System;

namespace Assets.Scripts.CustomEventArgs
{
    public class EventMarkedArgs : EventArgs
    {
        public TowerType TowerType { get; private set; }
        public EventMarkedArgs(TowerType type)
        {
            TowerType = type;
        }
    }
}
