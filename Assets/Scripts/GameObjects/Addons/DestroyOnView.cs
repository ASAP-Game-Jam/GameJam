using Assets.Scripts.GameObjects.Attacks;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Addons
{
    [RequireComponent(typeof(BasicAttack))]
    public class DestroyOnView : MonoBehaviour
    {
        [SerializeField] private bool onViewEnemy = true;
        private BasicAttack attack;
        private bool isDestroy = false;

        private void Start()
        {
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
                yield return null;
                Destroy(gameObject);
            }
        }
    }
}
