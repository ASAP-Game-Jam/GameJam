using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Other;
using System;
using UnityEngine;

namespace Assets.Scripts.Base
{
    public class Base : MonoBehaviour, Interfaces.Base.IBase
    {
        [SerializeField] private uint _hp;
        [SerializeField] private BaseType _baseType;
        public BaseType BaseType => _baseType;

        public event EventHandler OnTakeDamage;
        public event EventHandler OnDestroy;

        public void TakeDamage(uint damage)
        {
            damage = damage < _hp ? damage : _hp;
            _hp -= damage;
            EventBaseArgs args = new EventBaseArgs(damage, _hp, BaseType);
            OnTakeDamage?.Invoke(this, args);
            if (_hp == 0) OnDestroy?.Invoke(this, args);
        }
    }
}
