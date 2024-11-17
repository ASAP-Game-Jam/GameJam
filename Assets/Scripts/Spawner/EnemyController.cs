using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Enemy;
using Assets.Scripts.Tower;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IEnemyAttack, IEnemy
{
    public Vector3 FinelDestination;
    private bool isStopped;

    public uint Health;
    public uint Damage;
    public float MovementSpeed;

    public event EventHandler OnAttack;
    public event EventHandler OnGetDamage;
    public event EventHandler OnDestroy;

    // Update is called once per frame
    void Update()
    {
        if (!isStopped)
        { 
            transform.Translate(new Vector3(MovementSpeed * -1, 0, 0));
        }
        
    }

    public void OnTriggerEnter2D (Collider2D collision) {
        if (collision.gameObject.layer == 9)
        {
            isStopped = true;
            collision.GetComponent<Tower>()?.TakeDamage(Damage);
        }
    }

    public void OnTriggerExit2D (Collider2D collision) {
        if (collision.gameObject.layer == 9)
        {
            isStopped = false;
        }
    }

    public void TakeDamage(uint damage)
    {
        Health -= damage;
        if(Health < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
