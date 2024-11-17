using System;

namespace Assets.Scripts.CustomEventArgs
{
    public class EventCellArgs : EventArgs
    {
        public bool IsTaken {  get; set; }
        public Cell Cell { get; set; }
        public EventCellArgs(Cell cell, bool isTaken)
        {
            IsTaken = isTaken;
            Cell = cell;
        }
    }
}
