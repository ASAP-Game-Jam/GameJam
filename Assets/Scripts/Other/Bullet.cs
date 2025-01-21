using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Base;
using Assets.Scripts.Interfaces.Bullet;
using Assets.Scripts.Other;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Bullet : MonoBehaviour, ILineBullet
    {
        [SerializeField] private Direction direction = Direction.Right;
        // По этим точкам направляются
        [SerializeField] public Queue<Vector2> points = new Queue<Vector2>();
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
        Queue<Vector2> ILineBullet.Points
        {
            get => points;
            set => points = value;
        }

        private void Update()
        {
            timeLife -= Time.deltaTime;
            if (points != null && points.Count > 0) MoveTowardsNextPoint();
            else MoveInDirection();

            if (timeLife <= 0)
            {
                Destroy(this.gameObject);
            }
        }

        private void MoveTowardsNextPoint()
        {
            Vector2 targetPoint = points.Peek();
            Vector3 directionToTarget = (targetPoint - (Vector2)transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);

            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            if (Vector2.Distance(transform.position, targetPoint) < 0.1f)
            {
                points.Dequeue();
            }

        }

        private void MoveInDirection()
        {
            float moveDirection = direction == Direction.Left ? -1 : 1;
            transform.Translate(new Vector3(speed * moveDirection * Time.deltaTime, 0, 0),Space.World);
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
