using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class DeathEffectManager : MonoBehaviour, IManager
    {
        [Header("Настройки эффекта мигания при смерти")]
        [Tooltip("Количество циклов мигания (один цикл = исчезновение + восстановление прозрачности)")]
        public int blinkCount = 5;

        [Tooltip("Длительность одного состояния мигания (в секундах)")]
        public float blinkInterval = 0.1f;

        // Состояние менеджера (например, Started, Shutdown и т.п.)
        public EStatusManager Status { get; private set; } = EStatusManager.Shutdown;

        /// <summary>
        /// Вызывается LevelManager для инициализации настроек.
        /// </summary>
        public void Startup()
        {
            Status = EStatusManager.Started;
            // Дополнительная инициализация (если необходимо)
        }

        /// <summary>
        /// Вызывается LevelManager при завершении работы.
        /// </summary>
        public void Shutdown()
        {
            Status = EStatusManager.Shutdown;
            // Освобождение ресурсов (если необходимо)
        }
    }
}