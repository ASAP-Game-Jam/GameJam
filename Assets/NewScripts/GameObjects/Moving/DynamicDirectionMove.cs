using UnityEngine;

namespace Assets.Scripts.GameObjects.Moving
{
    public class DynamicDirectionMove : StaticDirectionMove
    {
        [SerializeField] protected float _maxSpeed = 1f;
        [SerializeField] protected float _acceleration = 1f;
        [SerializeField] protected float _deceleration = 8f;
        public float MaxSpeed => _maxSpeed;
        public float Acceleration => _acceleration;
        public float Deceleration => _deceleration;
        protected override void CorrectCurrentSpeed()
        {
            _currentSpeed = (IsMove
                ? (CurrentSpeed >= MaxSpeed ? MaxSpeed : CurrentSpeed + Acceleration * Time.deltaTime)
                : (CurrentSpeed <= 0 ? 0 : CurrentSpeed - Deceleration * Time.deltaTime));
        }
    }
}
