using System;

namespace Assets.Scripts.CustomEventArgs
{
    public class EventBoolArgs : EventArgs
    {
        private bool _value;
        public bool Value => _value;
        public EventBoolArgs(bool value)
        {
            _value = value;
        }
    }
}
