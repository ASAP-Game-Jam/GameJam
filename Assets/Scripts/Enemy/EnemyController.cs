using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Enemy;
using System;
using UnityEngine;

public class EnemyController : MonoBehaviour, IEnemyController
{
    public event EventHandler OnMoving;

    [SerializeField] float speed = 3f;
    [SerializeField] Direction direction = Direction.Left;

    private bool isMove = true;

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StopMove();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StartMove();
    }

    private void Move()
    {
        if (isMove)
        {
            transform.Translate(
                new Vector2[2] { 
                    Vector2.left, Vector2.right
                }[(int)direction]
                * speed * Time.fixedDeltaTime);
            OnMoving?.Invoke(this, EventArgs.Empty);
        }
    }

    public void StartMove()
    {
        isMove = true;
        Debug.Log("EnemyStart");
    }

    public void StopMove()
    {
        isMove = false;
        Debug.Log("EnemyStop");
    }
}
