using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Moving
{
    public interface IPointsMove : IMovable
    {
        public event Action OnMovedEnd;
    }
    public class StaticPointsMove : AMovable, IPointsMove
    {
        public event Action OnMovedEnd;
        [SerializeField] protected List<Vector3> _points;
        [SerializeField] private int _currentIndexPoint;
        [SerializeField] private bool _nextPoint = true;
        public List<Vector3> Points => _points;
        public int CurrentIndex => _currentIndexPoint;
        public bool NextPoint => _nextPoint;
        public override void Reverse()
        {
            _nextPoint = !_nextPoint;
        }
        protected override IEnumerator Move()
        {
            transform.position = _points[0];
            _currentIndexPoint = 0;
            while (CurrentIndex < Points.Count - 1 && IsMove)
            {
                Vector3 targetPoint = Points[_currentIndexPoint + 1];
                while (Vector3.Distance(transform.position, targetPoint) > 0.05f && IsMove)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPoint, CurrentSpeed * Time.deltaTime);
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y / 100);
                    yield return null;
                }

                if (IsMove)
                    transform.position = new Vector3(targetPoint.x, targetPoint.y, transform.position.z);

                _currentIndexPoint++;

                yield return null;
            }
            //if (CurrentIndex == Points.Length - 1)
            OnMovedEnd?.Invoke();
            base.Shutdown();
        }
    }
}
