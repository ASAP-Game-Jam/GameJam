using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Interfaces.Enemy;
using System;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator animator;
    private UnitController controller;
    private UnitAttack attack;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            controller = GetComponent<UnitController>();
            attack = GetComponent<UnitAttack>();

            if (controller != null)
            {
                controller.OnMoving += (object sender, EventArgs e) =>
                {
                    if (sender is IController ctr)
                    {
                        animator.SetBool("isMoving", ctr.IsMove);
                    }
                };
            }

            if (attack != null)
            {
                attack.OnAttack += (object sender, EventArgs e) =>
                {
                    animator.SetBool("isAttack", true);
                };
                attack.OnViewEnemyObject += (object sender, EventArgs e) =>
                {
                    if (e is EventUnitViewArgs args && !args.Value)
                    {
                        animator.SetBool("isAttack", false);
                    }
                };
            }
        }
    }
}
