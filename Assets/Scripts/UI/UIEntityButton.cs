using Assets.Scripts.GameObjects;
using Assets.Scripts.Managers;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIEntityButton : MonoBehaviour
    {
        [SerializeField] public AllyType AlliedType;
        [SerializeField] public int Cost = 10;
        [SerializeField] public float CooldownPerSeconds = 5f;
        [SerializeField] private GameObject fog;
        private float originalWidth;
        RectTransform rect;

        private bool isActive = true;
        private Coroutine coroutine;

        public bool IsActive
        {
            get => isActive;
            set
            {
                isActive = value;
                if (fog != null)
                    fog.SetActive(!isActive);
            }
        }

        private void Awake()
        {
            rect = fog.GetComponent<RectTransform>();
            originalWidth = rect.rect.width;
            GetComponent<Button>().onClick.AddListener(Select);
        }
        public void Select()
        {
            if (isActive)
                LevelManager.UIManager.Select(this);
        }
        public void UnSelect()
        {

        }
        public void ActivateCooldown()
        {
            coroutine = StartCoroutine(CooldownCoroutine());
        }

        private IEnumerator CooldownCoroutine()
        {
            // Деактивируем кнопку
            IsActive = false;

            // Получаем RectTransform элемента fog
            if (rect == null)
            {
                Debug.LogWarning("RectTransform не найден на элементе fog.");
                yield break;
            }

            // Устанавливаем pivot — правый край фиксирован, чтобы изменение ширины происходило слева направо
            rect.pivot = new Vector2(1f, 0.5f);

            // Сохраняем исходную ширину (учитывая, что anchors должны быть настроены так, чтобы размер не изменялся автоматически)
            float elapsedTime = 0f;

            // В процессе кулдауна изменяем ширину от исходной до 0
            while (elapsedTime < CooldownPerSeconds)
            {
                elapsedTime += Time.deltaTime;
                float newWidth = Mathf.Lerp(originalWidth, 0f, elapsedTime / CooldownPerSeconds);
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);

                yield return null;
            }

            // Гарантированно устанавливаем ширину 0 в конце
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);

            // Режим активности кнопки возвращаем к исходному состоянию
            IsActive = true;
        }
    }
}
