using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class ActivateManager : MonoBehaviour, IManager
    {
        public Vector3 SpawnObject;
        public EStatusManager Status { get; private set; }

        public void Select(UIActivateButton button)
        {
        }

        public void Shutdown()
        {
            Status = EStatusManager.Shutdown;
        }

        public void Startup()
        {
            Status = EStatusManager.Started;
        }
    }
}
