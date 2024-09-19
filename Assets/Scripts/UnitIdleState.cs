using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class UnitIdleState : StateMachineBehaviour
{
    private NavMeshAgent myAgent;

    private float myIdleTime = 5.0f;
    public float myIdleTimer;
    public bool myWentToCommandPoint;

    private AttackController myAttackController;

    private UnitMovement myUnitMovmement;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        myAttackController = animator.transform.GetComponent<AttackController>();
        myAttackController.SetIdleFlag();   
        myAgent = animator.GetComponent<NavMeshAgent>();
        myUnitMovmement = animator.transform.GetComponent<UnitMovement>();

        myIdleTimer = myIdleTime;
        myWentToCommandPoint = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(myAttackController.myTargetToAttack != null)
        {
            animator.SetBool("isFollowing", true);  
        }
        myIdleTimer -= Time.deltaTime;
        if(myIdleTimer <= 0 && myWentToCommandPoint == false)
        {
            myUnitMovmement.enabled = true;
            myUnitMovmement.GoToLastCommandPoint();
            myWentToCommandPoint = true;
        }
    }
}
