using Assets.Scripts.GameObjects.Entities;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Addons
{
    [RequireComponent(typeof(IBasicEntity))]
    public class HitEffect : MonoBehaviour
    {
        private static Color _selectColor = new Color(1, 0.8f, 0.8f, 1);
        private static float _delayDamage = 0.23f;
        public static Color SelectColor { get => _selectColor; set => _selectColor = value; }
        public static float DelayDamage { get => _delayDamage; set => _delayDamage = value > 0 ? value : 0.3f; }

        private IBasicEntity _destroyObject;
        private SpriteRenderer _spriteRenderer;
        private Color _originalColor;
        private Coroutine _currentEffect;

        private void Start()
        {
            _destroyObject = GetComponent<IBasicEntity>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_spriteRenderer)
                _originalColor = _spriteRenderer.color;

            if (_destroyObject is not null)
            {
                _destroyObject.OnTakenDamage += OnTakeDamage;
            }
        }

        private void OnTakeDamage(IBasicEntity entity, int damage)
        {
            if (_spriteRenderer)
            {
                if (_currentEffect != null)
                    StopCoroutine(_currentEffect);

                _currentEffect = StartCoroutine(DamageCoroutine());
            }
        }

        private IEnumerator DamageCoroutine()
        {
            _spriteRenderer.color = _selectColor;
            yield return new WaitForSeconds(0.3f);
            _spriteRenderer.color = _originalColor;
        }

        private void OnDestroy()
        {
            if (_destroyObject is not null)
            {
                _destroyObject.OnTakenDamage -= OnTakeDamage;
            }
        }
    }
}