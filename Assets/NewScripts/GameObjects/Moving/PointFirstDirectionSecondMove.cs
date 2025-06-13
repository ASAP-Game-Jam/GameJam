using UnityEngine;

namespace Assets.Scripts.GameObjects.Moving
{
    public class PointFirstDirectionSecondMove
    {
        private StaticDirectionMove _directionMove;
        private StaticPointsMove _pointsMove;

        public PointFirstDirectionSecondMove(GameObject bullet, params Vector3[] points)
        {
            _directionMove = bullet.GetComponent<StaticDirectionMove>();
            _pointsMove = bullet.GetComponent<StaticPointsMove>();

            _pointsMove.Points.AddRange(points);

            _directionMove.Shutdown();
            _pointsMove.Startup();

            _pointsMove.OnMovedEnd += MovedEnd;
        }
        private void MovedEnd()
        {
            _directionMove.Startup();
            _pointsMove.OnMovedEnd -= MovedEnd;
        }
    }
}
