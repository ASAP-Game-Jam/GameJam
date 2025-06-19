using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class TimerManager : MonoBehaviour, IManager
    {
        /// <summary>
        /// Событие, вызываемое каждую минуту.
        /// </summary>
        public event Action OnMinutePassed;
        /// <summary>
        /// Событие, вызываемое, когда общий таймер истекает.
        /// </summary>
        public event Action OnTimeExpired;
        /// <summary>
        /// Событие, вызываемое каждую секунду.
        /// </summary>
        public event Action<float> OnSecondPassed;

        [Header("Timer Settings")]
        public float totalDurationSeconds = 300f;

        public EStatusManager Status { get; private set; }

        // Остаток времени в секундах, актуализируется каждую секунду.
        [SerializeField]
        private float _remainingTime;

        public float RemainigTime => _remainingTime;

        private Coroutine timerCoroutine;

        public void Startup()
        {
            if (Status == EStatusManager.Started)
                return;

            Status = EStatusManager.Initializing;
            _remainingTime = totalDurationSeconds;
            timerCoroutine = StartCoroutine(TimerRoutine());
            Status = EStatusManager.Started;
            OnSecondPassed?.Invoke(RemainigTime); // Вызываем сразу, чтобы UI обновился с первого кадра
        }

        public void Shutdown()
        {
            Status = EStatusManager.Shutdown;
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
                timerCoroutine = null;
            }
        }

        private IEnumerator TimerRoutine()
        {
            while (_remainingTime > 0f && Status != EStatusManager.Shutdown)
            {
                yield return new WaitForSeconds(1f);
                _remainingTime -= 1f;
                OnSecondPassed?.Invoke(RemainigTime);

                // Если прошла целая минута
                if (Mathf.Approximately(_remainingTime % 60f, 0f) || (_remainingTime % 60f) < 1f)
                {
                    if (_remainingTime > 0f)
                        OnMinutePassed?.Invoke();
                    else
                        OnTimeExpired?.Invoke();
                }
            }

            // Если время истекло, гарантированно вызвать OnTimeExpired
            if (_remainingTime <= 0f)
                OnTimeExpired?.Invoke();
        }
    }
}