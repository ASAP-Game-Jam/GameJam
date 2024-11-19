using Assets.Scripts.Interfaces.Enemy;
using System;
using UnityEngine;

public class EnemyAttack : MonoBehaviour, IEnemyAttack
{
    public event EventHandler OnAttack;

    public void Attack()
    {
        OnAttack?.Invoke(this,EventArgs.Empty);
    }
}
