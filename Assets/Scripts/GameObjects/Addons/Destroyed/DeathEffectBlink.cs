using Assets.Scripts.GameObjects.Entities;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Addons.Destroyed
{
    [RequireComponent(typeof(BasicEntity))]
    public class DeathEffectBlink : MonoBehaviour
    {
        [Header("Настройки мигания")]
        [Tooltip("Количество циклов мигания (один цикл = затухание и восстановление прозрачности)")]
        public int blinkCount = 5;

        [Tooltip("Длительность одного состояния (затухания или восстановления) в секундах")]
        public float blinkInterval = 0.1f; // быстрое мигание

        private SpriteRenderer spriteRenderer;
        private Color originalColor;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                originalColor = spriteRenderer.color;

            BasicEntity basicEntity = GetComponent<BasicEntity>();
            basicEntity.OnDestroyed += (_) => PlayDeathEffect(); // Подписываемся на событие уничтожения
            basicEntity.StopDestroyedByHPIsZero(); // Останавливаем уничтожение при HP = 0, чтобы использовать эффект мигания
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
            // Выполняем заданное число циклов мигания
            for (int i = 0; i < blinkCount; i++)
            {
                // Устанавливаем полную прозрачность (миг – исчезновение)
                SetAlpha(0f);
                yield return new WaitForSeconds(blinkInterval);
                // Восстанавливаем оригинальную прозрачность
                SetAlpha(originalColor.a);
                yield return new WaitForSeconds(blinkInterval);
            }

            // По завершении эффекта можно дополнительно вызвать событие OnDestroyed,
            // если подобное событие требуется (либо его уже вызывает BasicEntity),
            // а затем удалить объект.
            Destroy(gameObject);
        }

        /// <summary>
        /// Устанавливает заданное значение альфа-канала для SpriteRenderer.
        /// </summary>
        /// <param name="alpha">Новое значение альфа-канала</param>
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
