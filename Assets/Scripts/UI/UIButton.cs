using System;
using UnityEngine;
using UnityEngine.UI;

public class UIButton: MonoBehaviour {
    [SerializeField] private Image availableImage; // ����������� ��� ��������� "��������"
    [SerializeField] private Image unavailableImage; // ����������� ��� ��������� "����������"

    private bool isEnable; // ��������� ���� ��� ������������ ��������� ������

    public event EventHandler OnClick; // ������� ��� ��������� �����

    private void Start () {
        UpdateButtonState();
    }

    // ����� ��� ��������� ������
    public void Enable () {
        isEnable = true;
        UpdateButtonState();
    }

    // ����� ��� ���������� ������
    public void Disable () {
        isEnable = false;
        UpdateButtonState();
    }

    // ����� ��� ���������� ��������� ������ � ����������� ���������������� �����������
    private void UpdateButtonState () {
        if (availableImage != null)
            availableImage.gameObject.SetActive(isEnable);

        if (unavailableImage != null)
            unavailableImage.gameObject.SetActive(!isEnable);
    }

    // ����� ��� ������ ������� �����
    public void TriggerClick () {
        if (isEnable && OnClick != null)
        {
            OnClick.Invoke(this, EventArgs.Empty);
        }
    }
}
