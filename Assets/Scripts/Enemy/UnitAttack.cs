using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Enemy;
using Assets.Scripts.Other;
using System;
using UnityEngine;

[RequireComponent(typeof(IDestroyObject))]
public class UnitAttack : MonoBehaviour, IAttack
{
    public event EventHandler OnAttack;
    public event EventHandler OnReload;
    public event EventHandler OnViewEnemyObject;

    [SerializeField] private uint damage = 2;
    [SerializeField] private float cooldown = 2f;
    [SerializeField] private float delayAttack = 0.5f;
    [SerializeField] private GameObject spawnObject;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private bool baseAttack = true;
    private Direction direction = Direction.None;
    private BaseType baseType;
    private float timeAttack = 0f;

    private void Start()
    {
        timeAttack = delayAttack >= 0 ? delayAttack : 0;
        if (GetComponent<IDestroyObject>() is IDestroyObject dist && dist != null)
            baseType = dist.BaseType;
        else
            Debug.LogError("UnitAttack: IDestroyObject not found");
        if(GetComponent<IController>() is IController c && c!=null)
        {
            direction = c.Direction;
        }
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
                if (destroyObject != null && destroyObject.BaseType != baseType)
                {
                    OnViewEnemyObject?.Invoke(this, new EventBoolArgs(true));
                    break;
                }
                else destroyObject = null;
            }
        }
        if (destroyObject == null) OnViewEnemyObject?.Invoke(this, new EventBoolArgs(false));
        else if (timeAttack <= 0)
        {
            Attack();
        }
    }

    public void Attack()
    {
        GameObject pref = Instantiate(spawnObject, this.transform);
        if (pref != null)
        {
            pref.transform.position = spawnPoint.transform.position;
            IBullet bullet = pref?.GetComponent<IBullet>();
            if (bullet != null)
            {
                bullet.Direction = this.direction;
                bullet.BaseType = baseType;
                bullet.Damage = damage;
                OnAttack?.Invoke(this, EventArgs.Empty);
            }
            timeAttack = cooldown;
        }
    }
}
