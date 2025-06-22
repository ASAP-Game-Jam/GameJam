using UnityEngine;

namespace Assets.Scripts.Level
{
    public static class GlobalSettings
    {
        /// <summary>Максимально допустимая X-координата для лучей.</summary>
        public static float MaxRayX { get; set; } = 5f;
    }
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private float maxRayX = 8f;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector3(maxRayX, -10, 0), new Vector3(maxRayX, 10, 0));
        }
        private void Awake()
        {
            GlobalSettings.MaxRayX = maxRayX;
        }
    }
}
