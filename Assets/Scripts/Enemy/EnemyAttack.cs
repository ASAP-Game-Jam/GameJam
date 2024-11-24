using Assets.Scripts.Interfaces.Enemy;
using Assets.Scripts.Interfaces.Tower;
using System;
using UnityEngine;

public class EnemyAttack : MonoBehaviour, IEnemyAttack
{
    public event EventHandler OnAttack;
    public event EventHandler OnReload;

    [SerializeField] private uint damage = 2;
    [SerializeField] private float cooldown = 2f;
    [SerializeField] private float delayAttack = 0.5f;
    private float timeAttack = 0f;

    private ITower tower;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ITower>() is ITower tower)
        {
            timeAttack = delayAttack;
            this.tower = tower;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<ITower>() is ITower tower)
            this.tower = tower;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        tower = null;
    }

    private void FixedUpdate()
    {
        if (timeAttack > 0)
            timeAttack -= Time.fixedDeltaTime;
        else
        {
            OnReload?.Invoke(this, EventArgs.Empty);
            Attack(this.tower);
        }
    }

    public void Attack(ITower tower)
    {
        if (timeAttack <= 0 && tower != null)
        {
            tower.TakeDamage(damage);
            OnAttack?.Invoke(this, EventArgs.Empty);
            timeAttack = cooldown;
        }
    }
}
