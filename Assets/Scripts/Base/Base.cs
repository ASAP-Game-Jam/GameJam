using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Other;
using System;
using UnityEngine;

namespace Assets.Scripts.Base
{
    public class Base : MonoBehaviour, Interfaces.Base.IBase
    {
        [SerializeField] private uint _hp = 100;
        private uint _maxhp;
        [SerializeField] private BaseType _baseType;
        public BaseType BaseType => _baseType;

        public uint HP => _hp;

        public event EventHandler OnTakeDamage;
        public event EventHandler OnDestroy;

        private void Awake()
        {
            _maxhp = _hp;
        }

        private void Start()
        {
            FindObjectOfType<LevelManager>().HoldOnBase(this);
        }

        public void TakeDamage(uint damage)
        {
            if (_hp == 0) return;
            damage = damage < _hp ? damage : _hp;
            _hp -= damage;
            EventBaseArgs args = new EventBaseArgs(damage, _maxhp == 0 ? _hp == 0 ? 1 : _hp : _maxhp, _hp, BaseType);
            OnTakeDamage?.Invoke(this, args);
            if (_hp == 0) OnDestroy?.Invoke(this, args);
        }
    }
}
