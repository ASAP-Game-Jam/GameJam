using Assets.Scripts.Interfaces;
using System;

namespace Assets.Scripts.CustomEventArgs
{
    public class EventUnitViewArgs : EventArgs
    {
        public bool Value { get;private set; }
        public IDestroyObject ViewObject { get;private set; }
        public EventUnitViewArgs(bool value,IDestroyObject destroyObject)
        {
            Value = value;
            ViewObject = destroyObject;
        }
    }
}
