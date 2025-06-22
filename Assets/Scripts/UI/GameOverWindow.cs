using Assets.Scripts.GameObjects.Fractions;
using Assets.Scripts.Managers;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class GameOverWindow : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private Button retryButton;
        [SerializeField] private GameObject overlayPanel;

        [Header("Messages")]
        [SerializeField] private string winMessage = "Вы победили";
        [SerializeField] private string loseMessage = "Вы проиграли";
        [SerializeField] private string drawMessage = "Ничья";

        [Header("Fade Settings")]
        [SerializeField] private float fadeDuration = 1f;
        [SerializeField, Range(0f, 1f)] private float targetAlpha = 0.7f;

        private Image backgroundImage;
        private Coroutine fadeRoutine;

        private void Awake()
        {
            backgroundImage = overlayPanel.GetComponent<Image>();
            retryButton.onClick.AddListener(OnRetryClicked);
            SetAlpha(0f);
            overlayPanel.SetActive(false);
        }

        private void Start()
        {
            LevelManager.BaseManager.OnEndingGame += HandleGameEnd;
        }

        private void HandleGameEnd(FractionType result)
        {
            string message = result switch
            {
                FractionType.Ally => winMessage,
                FractionType.Enemy => loseMessage,
                _ => drawMessage
            };

            Show(message);
        }

        public void Show(string message)
        {
            messageText.text = message;
            if (fadeRoutine != null)
                StopCoroutine(fadeRoutine);
            fadeRoutine = StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            overlayPanel.SetActive(true);
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, targetAlpha, t / fadeDuration);
                SetAlpha(alpha);
                yield return null;
            }
            SetAlpha(targetAlpha);
        }

        public void Hide()
        {
            overlayPanel.SetActive(false);
            if (fadeRoutine != null)
                StopCoroutine(fadeRoutine);
            SetAlpha(0f);
        }

        private void SetAlpha(float alpha)
        {
            Color color = backgroundImage.color;
            color.a = alpha;
            backgroundImage.color = color;
        }

        private void OnRetryClicked()
        {
            LevelManager.GameManager?.ReloadScene();
        }
    }
}