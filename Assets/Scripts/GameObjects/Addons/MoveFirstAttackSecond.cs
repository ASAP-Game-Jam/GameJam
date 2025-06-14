using Assets.Scripts.GameObjects.Attacks;
using Assets.Scripts.GameObjects.Moving;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Addons
{
    [RequireComponent(typeof(BasicAttack))]
    [RequireComponent(typeof(IPointsMove))]
    public class MoveFirstAttackSecond : MonoBehaviour
    {
        private BasicAttack attack;
        private IPointsMove move;
        private void Start()
        {
            attack = GetComponent<BasicAttack>();
            move = GetComponent<IPointsMove>();

            move.OnMovedEnd += () =>
            {
                attack.enabled = true;
                attack.Startup();
            };
        }
    }
}
