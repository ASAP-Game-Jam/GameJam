using Assets.Scripts.Interfaces.Tower;
using Assets.Scripts.Other;
using System;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class Tower : MonoBehaviour, ITower
    {
        public event EventHandler OnTakeDamage;
        public event EventHandler OnDestroy;
        [SerializeField] private uint hp = 10;
        public uint HP
        {
            get => hp;
            set
            {
                hp = (value > 10000 ? 0 : value);
                OnTakeDamage?.Invoke(this, EventArgs.Empty);
                if (hp == 0)
                {
                    OnDestroy?.Invoke(this, EventArgs.Empty);
                    Destroy(this.gameObject);
                }
            }
        }

        public BaseType BaseType => BaseType.TowerBase;

        public uint Cost { get; set; }
        public TowerType TowerType { get; set; }

        public void TakeDamage(uint damage)
        {
            HP = (HP >=damage?HP-damage:0);
        }
    }
}
