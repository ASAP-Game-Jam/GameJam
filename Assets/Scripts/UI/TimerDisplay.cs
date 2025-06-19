using Assets.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Класс для отображения оставшегося времени таймера на экране.
    /// Подписывается на событие OnSecondPassed менеджера TimerManager.
    /// </summary>
    public class TimerDisplay : MonoBehaviour
    {
        [Header("UI Reference")]
        // Ссылка на UI-элемент для отображения времени (например, компонент Text из UnityEngine.UI)
        public TMP_Text timerText;

        private float remainingTime;

        private void Start()
        {
            // Подписываемся на обновление каждую секунду через LevelManager
            if (LevelManager.TimerManager != null)
            {
                remainingTime = LevelManager.TimerManager.totalDurationSeconds;
                LevelManager.TimerManager.OnSecondPassed += UpdateTimerDisplay;
            }
            else
            {
                Debug.LogError("TimerDisplay: TimerManager не найден в LevelManager!");
            }
        }

        private void OnDestroy()
        {
            if (LevelManager.TimerManager != null)
            {
                LevelManager.TimerManager.OnSecondPassed -= UpdateTimerDisplay;
            }
        }


        /// <summary>
        /// Форматирует время в строку MM:SS и обновляет текст UI элемента.
        /// </summary>
        /// <param name="timeInSeconds">Оставшееся время в секундах</param>
        private void UpdateTimerDisplay(float timeInSeconds)
        {
            int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
            string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);
            if (timerText != null)
                timerText.text = formattedTime;
            else
                Debug.Log("Осталось времени: " + formattedTime);
        }
    }
}