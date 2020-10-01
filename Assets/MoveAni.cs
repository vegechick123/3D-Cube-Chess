using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAni : StateMachineBehaviour
{
    public GameObject prefabParticle;



    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        var particle=GameObject.Instantiate(prefabParticle,animator.transform.position,prefabParticle.transform.rotation);
        Destroy(particle, 1);
        CAnimationMoveComponent t = animator.GetComponentInParent<CAnimationMoveComponent>();
        t.OneMoveComplete();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
