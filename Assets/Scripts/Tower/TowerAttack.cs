using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Enemy;
using System;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class TowerAttack : MonoBehaviour, ITowerAttack
    {
        public event EventHandler OnAttack;
        public event EventHandler OnReloaded;

        [SerializeField] private float cooldown = 3f;
        [SerializeField] private float first_attack_cooldown = 1f;
        private float attackTime;

        private ILevelManager levelManager;
        // Дистанция атаки должна быть чем конец карты
        [SerializeField] private float endLevelX = 6;
        public float distanceAttack = 20;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject firePoint;
        [SerializeField] private IBullet bullet;
        public IBullet Bullet { get => bullet; set => bullet = value; }

        private void Awake()
        {
            attackTime = first_attack_cooldown < 0 ? 1 : first_attack_cooldown;
        }

        private void FixedUpdate()
        {
            if (attackTime > 0)
                attackTime -= Time.fixedDeltaTime;
            else
            {
                OnReloaded?.Invoke(this, EventArgs.Empty);
            }
        }
        private void Update()
        {
            Attack();
        }
        public void SetLevelManager(ILevelManager levelManager)
        {
            this.levelManager = levelManager;
        }
        private void Attack()
        {
            if (attackTime <= 0 && bulletPrefab != null)
            {
                float x = transform.position.x;
                x = Mathf.Min(x+distanceAttack,endLevelX)-x;
                RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y), Vector2.right,x);

                Debug.DrawLine(new Vector2(transform.position.x, transform.position.y), new Vector2(transform.position.x + distanceAttack, transform.position.y), Color.red, 0.1f);

                foreach (var hit in hits)
                {
                    IEnemy enemy = hit.collider.gameObject.GetComponent<IEnemy>();
                    if (enemy != null)
                    {
                        CreateBullet();
                        break;
                    }
                }
            }
        }

        private void CreateBullet()
        {
            GameObject pref = Instantiate(bulletPrefab, this.transform);
            if (pref != null)
            {
                pref.transform.position = firePoint.transform.position;
                IBullet bullet = pref?.GetComponent<IBullet>();
                if (bullet != null)
                {
                    bullet.Direction = Direction.Right;
                    bullet.BaseType = Other.BaseType.TowerBase;
                    OnAttack?.Invoke(this, EventArgs.Empty);
                }
                attackTime = cooldown;
            }
        }
    }
}