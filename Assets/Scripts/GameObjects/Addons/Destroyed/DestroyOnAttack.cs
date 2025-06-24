using Assets.Scripts.GameObjects.Attacks;
using Assets.Scripts.GameObjects.Entities;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameObjects.Addons.Destroyed

{
    public enum DestroyOnAttackType
    {
        All,
        Entity,
        Base
    }
    [RequireComponent(typeof(BasicAttack))]
    public class DestroyOnAttack : MonoBehaviour
    {
        [SerializeField] private DestroyOnAttackType destroyOnAttackType = DestroyOnAttackType.All;
        BasicAttack attack;
        private bool isDestroy = false;

        Coroutine destroyCoroutine;

        private void Start()
        {
            attack = GetComponent<BasicAttack>();
            attack.OnAttacking += (type, _) =>
            {
                bool isAttack = false;
                switch (type)
                {
                    case IBasicEntity when destroyOnAttackType == DestroyOnAttackType.All:
                    case IEntity when destroyOnAttackType == DestroyOnAttackType.Entity:
                    case IBase when destroyOnAttackType == DestroyOnAttackType.Base:
                        isAttack = true;
                        attack.Shutdown();
                        break;
                }
                if (destroyCoroutine == null && isAttack)
                    destroyCoroutine = StartCoroutine(Destroyed());
            };
        }
        private IEnumerator Destroyed()
        {
            if (!isDestroy)
            {
                isDestroy = true;
                yield return null;
                /*while (isAttack.IsAttack)
                {
                    yield return null;
                }*/
                Destroy(gameObject);
            }
            destroyCoroutine = null;
        }
    }
}
