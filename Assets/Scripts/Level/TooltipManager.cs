using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    [System.Serializable]
    public class Tooltip
    {
        public string text; // Текст подсказки
        public Sprite image; // Сопутствующее изображение
    }

    public TMP_Text leftTooltipText; // Ссылка на TMP_Text для левой подсказки
    public Image leftTooltipImage; // Ссылка на UI Image для левой подсказки
    public TMP_Text rightTooltipText; // Ссылка на TMP_Text для правой подсказки
    public Image rightTooltipImage; // Ссылка на UI Image для правой подсказки

    public List<Tooltip> leftTooltips; // Список подсказок для левой стороны
    public List<Tooltip> rightTooltips; // Список подсказок для правой стороны

    public float displayTime = 5f; // Время отображения подсказки
    public float fadeDuration = 1f; // Длительность затемнения

    private int leftTooltipIndex = 0;
    private int rightTooltipIndex = 0;
    private bool showLeft = true; // Флаг для чередования сторон

    void Start()
    {
        if (leftTooltips.Count > 0 && rightTooltips.Count > 0 &&
            leftTooltipText != null && leftTooltipImage != null &&
            rightTooltipText != null && rightTooltipImage != null)
        {
            StartCoroutine(DisplayTooltips());
        }
    }

    private IEnumerator DisplayTooltips()
    {
        while (true)
        {
            if (showLeft)
            {
                // Затемнить текущую левую подсказку
                yield return FadeOut(leftTooltipText, leftTooltipImage);

                // Обновить левую подсказку
                Tooltip currentTooltip = leftTooltips[leftTooltipIndex];
                leftTooltipText.text = currentTooltip.text;
                leftTooltipImage.sprite = currentTooltip.image;

                // Показать новую левую подсказку
                yield return FadeIn(leftTooltipText, leftTooltipImage);

                // Подождать указанное время
                yield return new WaitForSeconds(displayTime);

                // Переключить на следующую подсказку для левой стороны
                leftTooltipIndex = (leftTooltipIndex + 1) % leftTooltips.Count;
            }
            else
            {
                // Затемнить текущую правую подсказку
                yield return FadeOut(rightTooltipText, rightTooltipImage);

                // Обновить правую подсказку
                Tooltip currentTooltip = rightTooltips[rightTooltipIndex];
                rightTooltipText.text = currentTooltip.text;
                rightTooltipImage.sprite = currentTooltip.image;

                // Показать новую правую подсказку
                yield return FadeIn(rightTooltipText, rightTooltipImage);

                // Подождать указанное время
                yield return new WaitForSeconds(displayTime);

                // Переключить на следующую подсказку для правой стороны
                rightTooltipIndex = (rightTooltipIndex + 1) % rightTooltips.Count;
            }

            // Переключить сторону
            showLeft = !showLeft;
        }
    }

    private IEnumerator FadeIn(TMP_Text tooltipText, Image tooltipImage)
    {
        // Плавное появление текста и изображения
        Color textColor = tooltipText.color;
        Color imageColor = tooltipImage.color;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            textColor.a = alpha;
            imageColor.a = alpha;
            tooltipText.color = textColor;
            tooltipImage.color = imageColor;
            yield return null;
        }

        textColor.a = 1;
        imageColor.a = 1;
        tooltipText.color = textColor;
        tooltipImage.color = imageColor;
    }

    private IEnumerator FadeOut(TMP_Text tooltipText, Image tooltipImage)
    {
        // Плавное исчезновение текста и изображения
        Color textColor = tooltipText.color;
        Color imageColor = tooltipImage.color;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            textColor.a = alpha;
            imageColor.a = alpha;
            tooltipText.color = textColor;
            tooltipImage.color = imageColor;
            yield return null;
        }

        textColor.a = 0;
        imageColor.a = 0;
        tooltipText.color = textColor;
        tooltipImage.color = imageColor;
    }
}
