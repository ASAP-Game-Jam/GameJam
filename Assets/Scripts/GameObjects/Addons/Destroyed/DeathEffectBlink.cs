using Assets.Scripts.GameObjects.Entities;
using Assets.Scripts.Managers;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Addons.Destroyed
{
    [RequireComponent(typeof(BasicEntity))]
    public class DeathEffectBlink : MonoBehaviour
    {
        [Header("Локальные настройки мигания (используются, если DeathEffectManager отсутствует)")]
        [Tooltip("Количество циклов мигания (один цикл = исчезновение + восстановление прозрачности)")]
        public int blinkCount = 5;

        [Tooltip("Длительность одного состояния мигания (в секундах)")]
        public float blinkInterval = 0.1f;

        private SpriteRenderer spriteRenderer;
        private Color originalColor;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                originalColor = spriteRenderer.color;

            // Получаем базовую сущность и подписываемся на событие уничтожения
            BasicEntity basicEntity = GetComponent<BasicEntity>();
            basicEntity.OnDestroyed += (_) => PlayDeathEffect();
            // Отключаем нативное уничтожение объекта при достижении HP <= 0,
            // чтобы эффект мигания сработал перед удалением.
            basicEntity.StopDestroyedByHPIsZero();
        }

        /// <summary>
        /// Запускает эффект мигания, по завершении которого объект уничтожается.
        /// </summary>
        public void PlayDeathEffect()
        {
            StartCoroutine(BlinkAndDestroy());
        }

        private IEnumerator BlinkAndDestroy()
        {
            // Если у LevelManager установлены настройки DeathEffectManager,
            // берем параметры оттуда, иначе используем локально заданные.
            int effectiveBlinkCount = LevelManager.DeathEffectManager != null ?
                LevelManager.DeathEffectManager.blinkCount : blinkCount;
            float effectiveBlinkInterval = LevelManager.DeathEffectManager != null ?
                LevelManager.DeathEffectManager.blinkInterval : blinkInterval;

            for (int i = 0; i < effectiveBlinkCount; i++)
            {
                SetAlpha(0f);
                yield return new WaitForSeconds(effectiveBlinkInterval);
                SetAlpha(originalColor.a);
                yield return new WaitForSeconds(effectiveBlinkInterval);
            }
            Destroy(gameObject);
        }

        /// <summary>
        /// Устанавливает заданное значение альфа-канала для компонента SpriteRenderer.
        /// </summary>
        /// <param name="alpha">Новое значение прозрачности</param>
        private void SetAlpha(float alpha)
        {
            if (spriteRenderer != null)
            {
                Color newColor = spriteRenderer.color;
                newColor.a = alpha;
                spriteRenderer.color = newColor;
            }
        }
    }
}