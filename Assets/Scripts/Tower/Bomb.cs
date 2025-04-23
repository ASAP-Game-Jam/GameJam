using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Base;
using Assets.Scripts.Interfaces.Tower;
using Assets.Scripts.Other;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class Bomb : MonoBehaviour, ITower, ITowerAttack
    {
        [SerializeField] private float radiusDamage = 2f;
        [SerializeField] private uint damage = 10;
        [SerializeField] private float flyTime = 1f;
        [SerializeField] private Vector3 startPosition = new Vector3(-1, 4, 0);
        private Vector3 targetPosition;
        public BaseType BaseType => BaseType.TowerBase;

        public uint HP { get; set; } = 5;
        public uint Cost { get; set; }
        public TowerType TowerType { get; set; }
        public IBullet Bullet { get => null; set { } }

        public event EventHandler OnTakeDamage;
        public event EventHandler OnDestroy;
        public event EventHandler OnAttack;
        public event EventHandler OnReloaded;

        public void Activate()
        {
            targetPosition = transform.position;
            startPosition += targetPosition;
            // Телепорт на северо-запад - выше камеры
            transform.position = startPosition;
            // Запуск корутины
            StartCoroutine(FlyToTarget());
        }

        private IEnumerator FlyToTarget()
        {
            // Летит к цели
            float elapsedTime = 0f;
            while (elapsedTime < flyTime)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / flyTime));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            // Достиг цели
            transform.position = targetPosition;

            // Атаковать цели, у BaseType иной в радиусе radiusDamage
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,radiusDamage);
            foreach (Collider2D collider in colliders)
            {
                if ((collider.GetComponent<IDestroyObject>() is IDestroyObject destroyObject) && destroyObject is not IBase)
                {
                    destroyObject.TakeDamage(damage);
                }
            }
            OnAttack?.Invoke(this, EventArgs.Empty);
            // Уничтожение объекта
            TakeDamage(HP);
            Destroy(gameObject);
        }

        public void TakeDamage(uint damage)
        {
            HP -= damage;
            OnTakeDamage?.Invoke(this, EventArgs.Empty);
            if (HP <= 0)
                OnDestroy?.Invoke(this, EventArgs.Empty);
        }

        public void SetLevelManager(ILevelManager levelManager)
        {

        }
    }
}
