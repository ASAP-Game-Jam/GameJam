using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public static class Random
    {
        private static System.Random _random = new System.Random();
        public static int Next(int begin, int end) => _random.Next(begin, end);
    }
    public abstract class BasicManager : MonoBehaviour, IManager
    {
        protected List<IManager> managers = new List<IManager>();

        public EStatusManager Status { get; private set; }

        protected virtual void Start()
        {
            StartGame();
        }
        public void StartGame() => StartCoroutine(StartupManagers());
        public void EndGame() => StartCoroutine(ShutdownManagers());
        protected virtual IEnumerator StartupManagers()
        {
            Status = EStatusManager.Initializing;
            foreach (IManager manager in managers)
                manager.Startup();
            yield return null;
            for (int i = 0, last = 0; last < managers.Count; i = 0)
            {
                foreach (IManager manager in managers)
                    if (manager.Status == EStatusManager.Started) i++;
                if (i > last)
                {
                    last = i;
                    Debug.Log($"BasicManager: {last}/{managers.Count}");
                }
                yield return null;
            }
            Status = EStatusManager.Started;
        }
        protected virtual IEnumerator ShutdownManagers()
        {
            Status = EStatusManager.Initializing;
            foreach (IManager manager in managers)
                manager.Shutdown();
            yield return null;
            for (int i = 0, last = 0; last < managers.Count; i = 0)
            {
                foreach (IManager manager in managers)
                    if (manager.Status == EStatusManager.Shutdown) i++;
                if (i > last)
                {
                    last = i;
                    Debug.Log($"BasicManager: {last}/{managers.Count}");
                }
                yield return null;
            }
            Status = EStatusManager.Shutdown;
        }

        public void Startup()
        {
            throw new System.NotImplementedException();
        }

        public void Shutdown()
        {
            throw new System.NotImplementedException();
        }
    }
}