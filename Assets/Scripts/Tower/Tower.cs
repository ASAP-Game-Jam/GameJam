using Assets.Scripts.Interfaces.Tower;
using System;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class Tower : MonoBehaviour, ITower
    {
        public event EventHandler OnGetDamage;
        public event EventHandler OnDestroy;

        [SerializeField] private uint hp = 10;
        public uint HP
        {
            get => hp;
            set
            {
                hp = (value < 0 ? 0 : value);
                OnGetDamage?.Invoke(this, EventArgs.Empty);
                if (hp == 0)
                    OnDestroy?.Invoke(this, EventArgs.Empty);
            }
        }

        public void TakeDamage(uint damage)
        {
            HP -= damage;
        }
    }
}
