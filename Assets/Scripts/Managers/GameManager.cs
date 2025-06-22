using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour, IManager
    {
        public EStatusManager Status { get; private set; }

        [Header("Loading Settings")]
        [Tooltip("Если нужно показать прогресс загрузки, привяжите сюда слайдер или бар")]
        [SerializeField] private float sceneActivationDelay = 0.1f;

        private void Awake() => Status = EStatusManager.Initializing;

        public void Startup()
        {
            Status = EStatusManager.Started;
        }

        public void Shutdown()
        {
            Status = EStatusManager.Shutdown;
        }

        /// <summary>Перезагрузить текущую сцену.</summary>
        public void ReloadScene() =>
            LoadScene(SceneManager.GetActiveScene().name);

        /// <summary>Загрузить сцену меню.</summary>
        public void LoadMenuScene(string menuSceneName = "MainMenu") =>
            LoadScene(menuSceneName);

        /// <summary>Универсальный метод загрузки сцены.</summary>
        public void LoadScene(string sceneName)
        {
            if (Status != EStatusManager.Started)
                return;

            StartCoroutine(CoLoadScene(sceneName));
        }

        private IEnumerator CoLoadScene(string sceneName)
        {
            // 1) Сигналим LevelManager завершить всё
            var level = GetComponent<LevelManager>();
            if (level != null)
            {
                level.EndGame();
                // ждём, пока он не поставит себя на Shutdown
                while (level.Status != EStatusManager.Shutdown)
                    yield return null;
            }

            // 2) Запускаем асинхронную загрузку (позволяет показывать прогресс-бар)
            var asyncOp = SceneManager.LoadSceneAsync(sceneName);
            asyncOp.allowSceneActivation = false;

            // здесь можно, например, обновлять UI-панель загрузки:
            // while (asyncOp.progress < 0.9f) { loadingBar.value = asyncOp.progress; yield return null; }

            // небольшой хак: прогресс останавливается на 0.9, далее активация
            while (asyncOp.progress < 0.9f)
                yield return null;

            // опциональная задержка перед показом новой сцены
            yield return new WaitForSeconds(sceneActivationDelay);

            asyncOp.allowSceneActivation = true;
        }
    }
}