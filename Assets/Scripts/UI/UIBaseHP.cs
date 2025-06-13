using Assets.Scripts.GameObjects.Fractions;
using Assets.Scripts.Managers;
using UnityEngine;
namespace Assets.Scripts.UI
{
    public class UIBaseHP : MonoBehaviour
    {
        [SerializeField] private RectTransform frontObject;
        [SerializeField] private RectTransform backObject;
        [SerializeField] private FractionType fractionType;
        private void Start()
        {
            LevelManager.BaseManager.OnUpdateHP += (func) =>
            {
                HP? data = func?.Invoke(fractionType);
                if (data != null)
                {
                    var (hp, maxHp) = (data.Value.Hp, data.Value.MaxHP);
                    float length = backObject.rect.width;
                    float newL = (hp / (float)maxHp - 1) * length;
                    frontObject.offsetMax = new Vector2(newL, frontObject.offsetMax.y);
                }
            };
        }
    }
}