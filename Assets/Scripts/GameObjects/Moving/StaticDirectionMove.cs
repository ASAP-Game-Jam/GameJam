using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Moving
{
    public interface IDirectionMove : IMovable
    {
        public Direction2 Direction { get; }
    }
    public class StaticDirectionMove : AMovable, IDirectionMove
    {
        [SerializeField] protected Direction2 _direction;
        public Direction2 Direction => _direction;
        
        protected override IEnumerator Move()
        {
            while (IsMove)
            {
                CorrectCurrentSpeed();
                transform.Translate(
                    Direction switch
                    {
                        Direction2.Left => Vector2.left,
                        Direction2.Right => Vector2.right,
                        _ => Vector2.zero
                    } * _currentSpeed * Time.deltaTime);
                yield return null;
            }
        }
        protected virtual void CorrectCurrentSpeed()
        {

        }
        public override void Reverse()
        {
            _direction = Direction switch
            {
                Direction2.Left => Direction2.Right,
                Direction2.Right => Direction2.Left,
                _ => Direction2.None
            };
        }
    }
}
