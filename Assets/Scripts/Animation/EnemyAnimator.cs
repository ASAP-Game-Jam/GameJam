using Assets.Scripts.CustomEventArgs;
using System;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator animator;
    private EnemyController controller;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<EnemyController>();
        controller.OnMoving += (object sender, EventArgs e) =>
        {
            if(e is EventBoolArgs args)
            {
                animator.SetBool("isMoving",args.Value);
            }
        };
    }
}
