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
        [SerializeField] private bool _spawnPointIsFirst = false;
        [SerializeField] private bool _spawnPointIsLast = false;
        [SerializeField] private bool _allPointsIsLocal = false;
        public List<Vector3> Points => _points;
        public int CurrentIndex => _currentIndexPoint;
        public bool NextPoint => _nextPoint;
        public override void Reverse()
        {
            _nextPoint = !_nextPoint;
        }
        protected override IEnumerator Move()
        {
            if (_allPointsIsLocal)
                for (int i = 0; i < Points.Count; i++)
                    Points[i] = Points[i] + transform.position;
            if (_spawnPointIsFirst) _points.Insert(0, transform.position);
            if (_spawnPointIsLast) _points.Add(transform.position);

            transform.position = _points[0];
            _currentIndexPoint = 0;
            float currentZ = transform.position.z;
            while (CurrentIndex < Points.Count - 1 && IsMove)
            {
                
                Vector3 targetPoint = Points[_currentIndexPoint + 1];
                while (Vector2.Distance(transform.position, targetPoint) > 0.1f && IsMove)
                {
                    Vector2 newPoint = Vector2.MoveTowards(transform.position, targetPoint, CurrentSpeed * Time.deltaTime);
                    transform.position = new Vector3(newPoint.x,newPoint.y,currentZ);
                    yield return null;
                }

                if (IsMove)
                    transform.position = new Vector3(targetPoint.x, targetPoint.y, currentZ);

                _currentIndexPoint++;

                yield return null;
            }
            //if (CurrentIndex == Points.Length - 1)
            OnMovedEnd?.Invoke();
            base.Shutdown();
        }
    }
}
