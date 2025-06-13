using Assets.Scripts.GameObjects.Fractions;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour, IManager
    {
        public EStatusManager Status { get; private set; }

        public void Shutdown()
        {
            Status = EStatusManager.Shutdown;
        }

        public void Startup()
        {
            LevelManager.BaseManager.OnEndingGame += EndGame;
            Status = EStatusManager.Started;
        }

        public void EndGame(FractionType type)
        {
            Debug.Log($"Win: {type}");
            StartCoroutine(ReloadScene());
        }

        public IEnumerator ReloadScene()
        {
            var level = GetComponent<LevelManager>();
            level.EndGame();
            while (level.Status != EStatusManager.Shutdown)
                yield return null;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
