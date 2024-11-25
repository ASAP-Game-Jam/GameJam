using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Enemy;
using System;
using UnityEngine;

public class EnemyController : MonoBehaviour, IEnemyController
{
    public event EventHandler OnMoving;

    [SerializeField] float maxSpeed = 3f;
    [SerializeField] Direction direction = Direction.Left;
    [SerializeField] float speed = 0f;
    [SerializeField] float acceleration = 2f;
    [SerializeField] float accelerationStop = 10f;
    private bool isMove = true;

    private void Start()
    {
        maxSpeed = Math.Abs(maxSpeed);
        speed = Math.Abs(speed);
        acceleration = Math.Abs(acceleration);
        accelerationStop = Math.Abs(accelerationStop);
    }

    private void FixedUpdate()
    {
        speed = (isMove
            ? (speed >= maxSpeed ? maxSpeed : speed + acceleration * Time.fixedDeltaTime)
            : (speed <= 0 ? 0 : speed - accelerationStop * Time.fixedDeltaTime));
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tower")
            StopMove();
        else
            StartMove();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Tower")
            StopMove();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StartMove();
    }

    private void Move()
    {

        transform.Translate(
            new Vector2[2] {
                    Vector2.left, Vector2.right
            }[(int)direction]
            * speed * Time.fixedDeltaTime);
        OnMoving?.Invoke(this, EventArgs.Empty);

    }
    public void StartMove()
    {
        isMove = true;
    }

    public void StopMove()
    {
        isMove = false;
    }
}
