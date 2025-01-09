using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Base;
using Assets.Scripts.Interfaces.Enemy;
using Assets.Scripts.Interfaces.Tower;
using Assets.Scripts.Other;
using System;
using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour, IEnemyAttack
{
    public event EventHandler OnAttack;
    public event EventHandler OnReload;

    [SerializeField] private uint damage = 2;
    [SerializeField] private float cooldown = 2f;
    [SerializeField] private float delayAttack = 0.5f;
    private float timeAttack = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDestroyObject destroyObject = collision.GetComponent<IDestroyObject>();
        if (destroyObject is ITower || destroyObject is IBase towerBase && towerBase.BaseType == BaseType.TowerBase)
        {
            timeAttack = delayAttack;
            StartCoroutine(AttackRoutine(destroyObject));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StopAllCoroutines();
    }

    private IEnumerator AttackRoutine(IDestroyObject destroyObject)
    {
        while (true)
        {
            if (timeAttack > 0)
            {
                timeAttack -= Time.deltaTime;
            }
            else
            {
                OnReload?.Invoke(this, EventArgs.Empty);
                Attack(destroyObject);
                timeAttack = cooldown;
            }
            yield return null;
        }
    }

    public void Attack(IDestroyObject destroyObject)
    {
        if (destroyObject != null)
        {
            destroyObject.TakeDamage(damage);
            OnAttack?.Invoke(this, EventArgs.Empty);
        }
    }
}
