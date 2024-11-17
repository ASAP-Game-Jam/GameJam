using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Enemy;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    public class Bullet : MonoBehaviour, IBullet
    {
        public Direction Direction { get; set; }
        public uint Damage { get; set; }
        public float Speed{ get;set; }

        public float timer = 5f;
        private void Update()
        {
            timer -= Time.deltaTime;
            transform.Translate(new Vector3(Speed * (Direction == Direction.Left ? -1 : 1) * Time.deltaTime, 0, 0));
            if (timer <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        private void OnTriggerEnter2D (Collider2D other)
        {
            IEnemy enemy = other.GetComponent<EnemyController>();
            if(enemy != null) {
                enemy.TakeDamage(Damage);
                Destroy(this.gameObject);
            }
        }
    }
}
