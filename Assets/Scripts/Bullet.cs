using Assets.Scripts.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    public class Bullet : MonoBehaviour, IBullet
    {
        public Direction Direction { get ; set ; }
        public uint Damage { get; set; }

        public float Speed = 3f;
        public float timer = 10f;
        private void Update()
        {
            timer -= Time.deltaTime;
            transform.Translate(new Vector3(Speed * (Direction == Direction.Left ? -1 : 1), 0, 0));
            if (timer <= 0)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
