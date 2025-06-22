using UnityEngine;

namespace Assets.Scripts.Level
{
    public class SetupFPS : MonoBehaviour
    {
        public int FrameRate;
        void Start()
        {
            UnityEngine.Application.targetFrameRate = FrameRate;
            UnityEngine.Debug.Log($"Target frame rate set to {FrameRate} FPS.");
        }
    }
}
