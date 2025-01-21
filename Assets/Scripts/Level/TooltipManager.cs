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
        public string text; // ����� ���������
        public Sprite image; // ������������� �����������
    }

    public TMP_Text leftTooltipText; // ������ �� TMP_Text ��� ����� ���������
    public Image leftTooltipImage; // ������ �� UI Image ��� ����� ���������
    public TMP_Text rightTooltipText; // ������ �� TMP_Text ��� ������ ���������
    public Image rightTooltipImage; // ������ �� UI Image ��� ������ ���������

    public List<Tooltip> leftTooltips; // ������ ��������� ��� ����� �������
    public List<Tooltip> rightTooltips; // ������ ��������� ��� ������ �������

    public float displayTime = 5f; // ����� ����������� ���������
    public float fadeDuration = 1f; // ������������ ����������

    private int leftTooltipIndex = 0;
    private int rightTooltipIndex = 0;
    private bool showLeft = true; // ���� ��� ����������� ������

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
                // ��������� ������� ����� ���������
                yield return FadeOut(leftTooltipText, leftTooltipImage);

                // �������� ����� ���������
                Tooltip currentTooltip = leftTooltips[leftTooltipIndex];
                leftTooltipText.text = currentTooltip.text;
                leftTooltipImage.sprite = currentTooltip.image;

                // �������� ����� ����� ���������
                yield return FadeIn(leftTooltipText, leftTooltipImage);

                // ��������� ��������� �����
                yield return new WaitForSeconds(displayTime);

                // ����������� �� ��������� ��������� ��� ����� �������
                leftTooltipIndex = (leftTooltipIndex + 1) % leftTooltips.Count;
            }
            else
            {
                // ��������� ������� ������ ���������
                yield return FadeOut(rightTooltipText, rightTooltipImage);

                // �������� ������ ���������
                Tooltip currentTooltip = rightTooltips[rightTooltipIndex];
                rightTooltipText.text = currentTooltip.text;
                rightTooltipImage.sprite = currentTooltip.image;

                // �������� ����� ������ ���������
                yield return FadeIn(rightTooltipText, rightTooltipImage);

                // ��������� ��������� �����
                yield return new WaitForSeconds(displayTime);

                // ����������� �� ��������� ��������� ��� ������ �������
                rightTooltipIndex = (rightTooltipIndex + 1) % rightTooltips.Count;
            }

            // ����������� �������
            showLeft = !showLeft;
        }
    }

    private IEnumerator FadeIn(TMP_Text tooltipText, Image tooltipImage)
    {
        // ������� ��������� ������ � �����������
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
        // ������� ������������ ������ � �����������
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
