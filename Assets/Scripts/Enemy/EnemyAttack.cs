using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Enemy;
using Assets.Scripts.Other;
using System;
using UnityEngine;

public class EnemyAttack : MonoBehaviour, IEnemyAttack
{
    public event EventHandler OnAttack;
    public event EventHandler OnReload;
    public event EventHandler OnViewEnemy;

    [SerializeField] private uint damage = 2;
    [SerializeField] private float cooldown = 2f;
    [SerializeField] private float delayAttack = 0.5f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject firePoint;
    public float attackRange = 1f;
    private float timeAttack = 0f;

    private void Start()
    {
        timeAttack = delayAttack >= 0 ? delayAttack : 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x - attackRange * Mathf.Sign(transform.localScale.x), transform.position.y, transform.position.z));
    }

    private void Update()
    {
        if (timeAttack <= 0) OnReload?.Invoke(this, EventArgs.Empty);
        else timeAttack -= Time.deltaTime;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Mathf.Sign(transform.localScale.x) == 1 ? Vector2.left : Vector2.right, attackRange);
        Debug.DrawLine(new Vector2(transform.position.x, transform.position.y), new Vector2(transform.position.x - attackRange * Mathf.Sign(transform.localScale.x), transform.position.y), Color.red, 0.1f);
        IDestroyObject destroyObject = null;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                destroyObject = hit.collider.GetComponent<IDestroyObject>();
                if (destroyObject != null && destroyObject.BaseType != BaseType.EnemyBase)
                {
                    OnViewEnemy?.Invoke(this, new EventBoolArgs(true));
                    break;
                }
                else destroyObject = null;
            }
        }
        if (destroyObject == null) OnViewEnemy?.Invoke(this, new EventBoolArgs(false));
        else if (timeAttack <= 0)
        {
            Attack();
        }
    }

    public void Attack()
    {
        GameObject pref = Instantiate(bulletPrefab, this.transform);
        if (pref != null)
        {
            pref.transform.position = firePoint.transform.position;
            IBullet bullet = pref?.GetComponent<IBullet>();
            if (bullet != null)
            {
                bullet.Direction = Direction.Left;
                bullet.BaseType = BaseType.EnemyBase;
                bullet.Damage = damage;
                OnAttack?.Invoke(this, EventArgs.Empty);
            }
            timeAttack = cooldown;
        }
    }
}
