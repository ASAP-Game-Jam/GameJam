using Assets.Scripts.GameObjects.Entities;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EntityAnimationContoller : MonoBehaviour
{
    private Animator animator;
    private bool Attack = false;
    private bool Move = false;
    private bool ViewEnemy = false;
    private bool Destroyed = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
        if (Destroyed)
        {
            var hp = GetComponent<BasicEntity>();
            hp.OnDestroyed += (e) =>
            {
                animator.SetBool("Destroyed",true);
            };
        }
    }
}
