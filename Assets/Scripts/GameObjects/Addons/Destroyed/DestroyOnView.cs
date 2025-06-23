using Assets.Scripts.GameObjects.Attacks;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Addons.Destroyed
{
    [RequireComponent(typeof(BasicAttack))]
    public class DestroyOnView : MonoBehaviour
    {
        [SerializeField] private bool onViewEnemy = true;
        [SerializeField] private float destroyTime = 0.5f;
        private BasicAttack attack;
        private bool isDestroy = false;
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
            attack = GetComponent<BasicAttack>();
            attack.OnViewEnemy += (e) =>
            {
                if (!onViewEnemy || e)
                    attack.Shutdown();
                StartCoroutine(Destroyed());
            };
        }
        private IEnumerator Destroyed()
        {
            if (!isDestroy)
            {
                isDestroy = true;
                if (animator != null)
                {
                    animator.SetBool("Destroyed", true);
                    yield return new WaitForSeconds(destroyTime);
                }
                else
                    Debug.LogWarning("Animator is not set on DestroyOnView component. Destroying without animation.");
                yield return null;
                Destroy(gameObject);
            }
        }
    }
}
