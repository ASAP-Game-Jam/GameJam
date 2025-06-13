using UnityEngine;

namespace Assets.Scripts.GameObjects.Fractions
{
    public class EnemyFraction : MonoBehaviour, IFraction
    {
        public  FractionType Fraction => FractionType.Enemy;
    }
}
