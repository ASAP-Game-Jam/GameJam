using Assets.Scripts.Interfaces.Enemy;
using Assets.Scripts.Other;
using System;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        public event EventHandler OnDestroy;
        public event EventHandler OnTakeDamage;

        [SerializeField] private uint hp = 5;
        public uint HP
        {
            get => hp;
            set
            {
                hp = (value < hp ? value : hp); OnTakeDamage?.Invoke(this, EventArgs.Empty); if (hp == 0) OnDestroy?.Invoke(this, EventArgs.Empty);
            }
        }

        public BaseType BaseType => BaseType.EnemyBase;

        private void Awake()
        {
            Activate();
        }

        public void TakeDamage(uint damage)
        {
            HP -= damage > hp ? hp : damage;
        }

        public void Activate()
        {
            OnDestroy += (object sender, EventArgs args) => { Destroy(this.gameObject); };
        }
    }
}
