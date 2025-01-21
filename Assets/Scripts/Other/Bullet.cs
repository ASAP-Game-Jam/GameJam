using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Base;
using Assets.Scripts.Other;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Bullet : MonoBehaviour, IBullet
    {
        [SerializeField] private Direction direction = Direction.Right;
        [SerializeField] private uint damage = 3;
        [SerializeField] private float speed = 7f;
        [SerializeField] private BaseType baseType;
        [SerializeField] private bool attackBase = false;
        [SerializeField] private bool attackObject = true;

        [SerializeField] float timeLife = 5f;

        public event EventHandler OnHit;

        public Direction Direction { get => direction; set => direction = value; }
        public uint Damage { get => damage; set => damage = (value > 0 ? value : 1); }
        public float Speed { get => speed; set => speed = (value > 0 ? value : 0.1f); }

        public BaseType BaseType { get => baseType; set => baseType = value; }

        private void Update()
        {
            timeLife -= Time.deltaTime;
            transform.Translate(new Vector3(speed * (direction == Direction.Left ? -1 : 1) * Time.deltaTime, 0, 0));
            if (timeLife <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<IDestroyObject>() is IDestroyObject destroyObject && destroyObject.BaseType != this.baseType)
            {
                if (destroyObject is IBase && attackBase || destroyObject is not IBase && attackObject)
                {
                    destroyObject.TakeDamage(damage);
                    OnHit?.Invoke(this, EventArgs.Empty);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
