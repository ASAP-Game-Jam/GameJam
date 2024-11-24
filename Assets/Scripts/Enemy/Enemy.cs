using Assets.Scripts.Interfaces.Enemy;
using System;
using UnityEngine;

namespace Assets.Scripts.Spawner
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        public event EventHandler OnGetDamage;
        public event EventHandler OnDestroy;

        [SerializeField] private uint hp = 5;
        public uint HP
        {
            get => hp;
            set
            {
                hp = (value < hp ? value : hp); OnGetDamage?.Invoke(this, EventArgs.Empty); if (hp == 0) OnDestroy?.Invoke(this, EventArgs.Empty);
            }
        }

        private void Awake()
        {
            OnDestroy += (object sender, EventArgs args) => { Destroy(this.gameObject); };
        }

        public void TakeDamage(uint damage)
        {
            HP -= damage>hp?hp:damage;
        }


    }
}
