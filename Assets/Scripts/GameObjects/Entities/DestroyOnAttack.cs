using Assets.Scripts.GameObjects.Attacks;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Entities

{
    [RequireComponent(typeof(BasicAttack))]
    public class DestroyOnAttack : MonoBehaviour
    {
        BasicAttack attack;
        private bool isDestroy = false;

        private void Start()
        {
            attack = GetComponent<BasicAttack>();
            attack.OnViewEnemy += (e) => { if (e) attack.Shutdown(); };
            attack.OnAttacking += () =>
            {
                StartCoroutine(Destroyed());
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
        }
    }
}
