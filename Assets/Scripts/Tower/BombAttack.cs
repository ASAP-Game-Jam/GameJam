using Assets.Scripts.Interfaces;
using System;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    class BombAttack : MonoBehaviour, ITowerAttack
    {
        [SerializeField] private float damageRange = 1.5f;
        [SerializeField] private float damage = 10f;
        public IBullet Bullet { get; set; }

        public event EventHandler OnAttack;
        public event EventHandler OnReloaded;

        public void Attack()
        {

        }

        public void SetLevelManager(ILevelManager levelManager)
        {
            throw new NotImplementedException();
        }
    }
}
