using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class UnitFollowState : StateMachineBehaviour
{
    private AttackController myAttackController;
    private NavMeshAgent myAgent;

    public float myAttackDistance = 1f;

    private UnitMovement myUnitMovement;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        myUnitMovement = animator.transform.GetComponent<UnitMovement>();
        if (myUnitMovement.myIsCommandedToMove == false)
        {
            myAttackController = animator.transform.GetComponent<AttackController>();
            myAgent = animator.transform.GetComponent<NavMeshAgent>();
            myAttackController.SetWalkFlag();
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (myUnitMovement.myIsCommandedToMove == false)
        {
            if(myAttackController == null)
            {
                myAttackController = animator.transform.GetComponent<AttackController>();
            }
            if (myAttackController.myTargetToAttack == null)
            {
                animator.SetBool("isFollowing", false);
            }
            else
            {
                if (myUnitMovement.myIsCommandedToMove == false && myAttackController.myTargetToAttack != null && myAgent != null)
                {
                    myAgent.SetDestination(myAttackController.myTargetToAttack.position);
                    animator.transform.LookAt(myAttackController.myTargetToAttack);
                }
            }

            if (myAttackController.myTargetToAttack != null)
            {

                float distanceFromTarget = MathRts.FindDistanceNotSqrt(myAttackController.myTargetToAttack.position, animator.transform.position);
                if (distanceFromTarget <= myAttackDistance) 
                {
                    myAgent.SetDestination(animator.transform.position);
                    animator.SetBool("isAttacking", true);
                }
            }
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (myUnitMovement == false)
        {
            myAgent.SetDestination(animator.transform.position);
        }
    }


}
