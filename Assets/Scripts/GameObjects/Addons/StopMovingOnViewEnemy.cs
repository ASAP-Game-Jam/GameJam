using Assets.Scripts.GameObjects.Attacks;
using Assets.Scripts.GameObjects.Moving;
using UnityEngine;
namespace Assets.Scripts.GameObjects.Addons
{
    [RequireComponent(typeof(BasicAttack))]
    [RequireComponent(typeof(AMovable))]
    public class StopMovingOnViewEnemy : MonoBehaviour
    {
        [SerializeField] private bool isPause = true;
        private BasicAttack basicAttack;
        private AMovable amovable;
        void Start()
        {
            basicAttack = GetComponent<BasicAttack>();
            amovable = GetComponent<AMovable>();

            basicAttack.OnViewEnemy += Pause;
        }

        void Pause(bool isView)
        {
            if (isView) amovable.Shutdown();
            else if (!amovable.IsMove) amovable.Startup();
            
            if(!isPause && isView)
                basicAttack.OnViewEnemy -= Pause;
        }
    }
}
