using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(IDestroyObject))]
public class DestroyObjectHitEffect : MonoBehaviour
{
    private static Color _selectColor = new Color(1, 0.8f, 0.8f, 1);
    private static float _delayDamage = 0.23f;
    public static Color SelectColor { get => _selectColor; set => _selectColor = value; }
    public static float DelayDamage { get => _delayDamage; set => _delayDamage = value > 0 ? value : 0.3f; }

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
            // Если корутина уже выполняется — остановить
            if (_currentEffect != null)
                StopCoroutine(_currentEffect);

            _currentEffect = StartCoroutine(DamageCoroutine());
        }
    }

    private IEnumerator DamageCoroutine()
    {
        _spriteRenderer.color = _selectColor; // Красим в красный
        yield return new WaitForSeconds(0.3f); // Задержка
        _spriteRenderer.color = _originalColor; // Возвращаем цвет
    }

    private void OnDestroy()
    {
        if (_destroyObject is not null)
        {
            _destroyObject.OnTakeDamage -= OnTakeDamage; // Удаляем подписку
        }
    }
}
