using UnityEngine;

namespace Assets.Scripts.GameObjects.Fractions
{
    public class ModifyFraction : MonoBehaviour, IModifyFraction, IFraction
    {
        [SerializeField] private FractionType _fractionType;
        public virtual FractionType Fraction
        {
            get => _fractionType;
            set => _fractionType = value;
        }
    }
}
