using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Enemy;
using System;
using UnityEngine;

public class UnitController : MonoBehaviour, IController
{
    public event EventHandler OnMoving;

    [SerializeField] private float radiusLife = 20f;
    private Vector3 spawnPoint;

    [SerializeField] float maxSpeed = 3f;
    [SerializeField] Direction direction = Direction.Left;
    [SerializeField] float speed = 0f;
    [SerializeField] float acceleration = 2f;
    [SerializeField] float accelerationStop = 10f;
    public bool IsMove { get; private set; } = true;

    public Direction Direction
    {
        get { return direction; }
        set
        {
            if (direction != value)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            direction = value;
        }
    }

    private void Start()
    {
        spawnPoint = transform.position;
        maxSpeed = Math.Abs(maxSpeed);
        speed = Math.Abs(speed);
        acceleration = Math.Abs(acceleration);
        accelerationStop = Math.Abs(accelerationStop);
        
        transform.localScale = new Vector3(
        direction switch
        {
            Direction.Right => -1,
            Direction.Left => 1,
            _ => 0
        } * transform.localScale.x,
        transform.localScale.y,
        transform.localScale.z);
        IAttackHandle(GetComponent<IAttack>());
    }

    protected virtual void IAttackHandle(IAttack attack)
    {
        if (attack != null)
            attack.OnViewEnemyObject += (object sender, EventArgs args) =>
            {
                if (args is EventUnitViewArgs bArgs && bArgs.Value) StopMove();
                else StartMove();
            };
    }

    private void FixedUpdate()
    {
        speed = (IsMove
            ? (speed >= maxSpeed ? maxSpeed : speed + acceleration * Time.fixedDeltaTime)
            : (speed <= 0 ? 0 : speed - accelerationStop * Time.fixedDeltaTime));
        Move();
        if (transform.localScale.x < -10 || transform.localScale.x > 10)
            Destroy(this.gameObject);
    }

    private void Update()
    {
        if(Vector3.Distance(spawnPoint, transform.position) > radiusLife)
        {
            Destroy(this.gameObject);
        }
    }

    private void Move()
    {

        transform.Translate(
            new Vector2[3] {
                    Vector2.zero,Vector2.left, Vector2.right
            }[(int)direction]
            * speed * Time.fixedDeltaTime);
        OnMoving?.Invoke(this, EventArgs.Empty);
    }
    public void StartMove()
    {
        IsMove = true;
    }

    public void StopMove()
    {
        IsMove = false;
    }
}
