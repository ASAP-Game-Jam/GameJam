using Assets.Scripts.GameObjects.Attacks;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Addons.Destroyed

{
    [RequireComponent(typeof(BasicAttack))]
    public class DestroyOnAttack : MonoBehaviour
    {
        BasicAttack attack;
        private bool isDestroy = false;

        Coroutine destroyCoroutine;

        private void Start()
        {
            attack = GetComponent<BasicAttack>();
            attack.OnViewEnemy += (e) => { if (e) attack.Shutdown(); };
            attack.OnAttacking += (_, _) =>
            {
                if (destroyCoroutine == null)
                    destroyCoroutine = StartCoroutine(Destroyed());
            };
        }
        private IEnumerator Destroyed()
        {
            if (!isDestroy)
            {
                isDestroy = true;
                yield return null;
                /*while (attack.IsAttack)
                {
                    yield return null;
                }*/
                Destroy(gameObject);
            }
            destroyCoroutine = null;
        }
    }
}
