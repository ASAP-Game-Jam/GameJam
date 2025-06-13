using UnityEngine;
namespace Assets.Scripts.GameObjects.Moving
{
    [RequireComponent(typeof(IPointsMove))]
    [RequireComponent(typeof(IDirectionMove))]
    public class MoveDirectionOnMoveEndPoint : MonoBehaviour
    {
        private void Start()
        {
            IDirectionMove directionMove = GetComponent<IDirectionMove>();
            IPointsMove pointsMove = GetComponent<IPointsMove>();

            directionMove.Shutdown();
            pointsMove.Startup();

            pointsMove.OnMovedEnd += () =>
            {
                pointsMove.Shutdown();
                directionMove.Startup();
            };
        }
    }
}
