using System;

namespace Assets.Scripts.CustomEventArgs
{
    public class EventEnergyArgs : EventArgs
    {
        public uint Energy { get; private set; }
        public EventEnergyArgs(uint energy)
        {
            Energy = energy;
        }
    }
}
