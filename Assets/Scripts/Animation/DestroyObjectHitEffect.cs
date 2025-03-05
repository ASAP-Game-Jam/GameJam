using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(IDestroyObject))]
public class DestroyObjectHitEffect : MonoBehaviour
{
    private IDestroyObject _destroyObject;
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private Coroutine _currentEffect;

    private void Start()
    {
        _destroyObject = GetComponent<IDestroyObject>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_spriteRenderer)
            _originalColor = _spriteRenderer.color;

        if (_destroyObject is not null)
        {
            _destroyObject.OnTakeDamage += OnTakeDamage;
        }
    }

    private void OnTakeDamage(object sender, EventArgs args)
    {
        if (_spriteRenderer)
        {
            // ���� �������� ��� ����������� � ����������
            if (_currentEffect != null)
                StopCoroutine(_currentEffect);

            _currentEffect = StartCoroutine(DamageCoroutine());
        }
    }

    private IEnumerator DamageCoroutine()
    {
        _spriteRenderer.color = Color.red; // ������ � �������
        yield return new WaitForSeconds(0.3f); // ��������
        _spriteRenderer.color = _originalColor; // ���������� ����
    }

    private void OnDestroy()
    {
        if (_destroyObject is not null)
        {
            _destroyObject.OnTakeDamage -= OnTakeDamage; // ������� ��������
        }
    }
}
