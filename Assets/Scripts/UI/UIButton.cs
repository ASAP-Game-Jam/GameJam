using System;
using UnityEngine;
using UnityEngine.UI;

public class UIButton: MonoBehaviour {
    [SerializeField] private Image availableImage; // Изображение для состояния "доступно"
    [SerializeField] private Image unavailableImage; // Изображение для состояния "недоступно"

    private bool isEnable; // Приватное поле для отслеживания состояния кнопки

    public event EventHandler OnClick; // Событие для обработки клика

    private void Start () {
        UpdateButtonState();
    }

    // Метод для включения кнопки
    public void Enable () {
        isEnable = true;
        UpdateButtonState();
    }

    // Метод для выключения кнопки
    public void Disable () {
        isEnable = false;
        UpdateButtonState();
    }

    // Метод для обновления состояния кнопки и отображения соответствующего изображения
    private void UpdateButtonState () {
        if (availableImage != null)
            availableImage.gameObject.SetActive(isEnable);

        if (unavailableImage != null)
            unavailableImage.gameObject.SetActive(!isEnable);
    }

    // Метод для вызова события клика
    public void TriggerClick () {
        if (isEnable && OnClick != null)
        {
            OnClick.Invoke(this, EventArgs.Empty);
        }
    }
}
