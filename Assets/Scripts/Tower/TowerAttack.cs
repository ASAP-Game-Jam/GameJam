using Assets.Scripts.Interfaces;
using System;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class TowerAttack : MonoBehaviour, ITowerAttack
    {
        public event EventHandler OnAttack;
        public event EventHandler OnReloaded;
        [SerializeField] private float cooldown = 3f;
        private float attackTime;
        private ILevelManager levelManager;
        public uint Index;
        [SerializeField] private uint damage;
        [SerializeField] private float speedBullet = 3f;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private IBullet bullet;
        public IBullet Bullet { get => bullet; set => bullet = value; }

        private void Awake()
        {
            bullet = bulletPrefab.GetComponent<Bullet>();
        }
        private void FixedUpdate()
        {
            if (attackTime > 0)
                attackTime -= Time.fixedDeltaTime;
            else
                OnReloaded?.Invoke(this, EventArgs.Empty);
        }
        private void Update()
        {
            if (levelManager != null && levelManager.IsEnemyOnLine(Index) || true)
                Attack();
        }
        public void SetLevelManager(ILevelManager levelManager)
        {
            this.levelManager = levelManager;
        }
        private void Attack()
        {
            if (attackTime <= 0 && bulletPrefab != null && bullet != null)
            {
                GameObject pref = Instantiate(bulletPrefab, this.transform);
                if (pref != null)
                {
                    pref.transform.position = this.transform.position;
                    IBullet bullet = pref?.GetComponent<IBullet>();
                    if (bullet != null)
                    {
                        bullet.Speed = this.speedBullet;
                        bullet.Direction = Direction.Right;
                        bullet.Damage = this.damage;
                        OnAttack?.Invoke(this, EventArgs.Empty);
                    }
                    attackTime = cooldown;
                }
            }
        }
    }

}
