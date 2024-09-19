using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.HighDefinition;

public class UnitAttackState : StateMachineBehaviour
{

    private NavMeshAgent myAgent;
    private AttackController myAttackController;

    [SerializeField]
    private float myStopAttackingDistance = 1f;    
    [SerializeField]
    private float myDamage = 1f;

    [SerializeField]
    private bool myIsLongRangeUnit;

    [SerializeField]
    private GameObject myProjectile;

   
    [SerializeField]
    private float myAttackSpeed;
    private float myAttackTimer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        myAgent = animator.GetComponent<NavMeshAgent>();

        myAttackController = animator.GetComponent<AttackController>();
        myAttackController.SetAttackFlag();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (animator.transform.GetComponent<UnitMovement>().myIsCommandedToMove == true)
        {
            animator.SetBool("isAttacking", false);
            myAttackController.myTargetToAttack = null  ;
        }
        else if (myAttackController.myTargetToAttack != null && animator.transform.GetComponent<UnitMovement>().myIsCommandedToMove == false)
        {
            LookAtTarget();
            if (myIsLongRangeUnit)
            {
                myAgent.SetDestination(animator.transform.position);
                if(myAttackTimer <= 0)
                {
                    if(Random.Range(1,11) > 7)
                    {
                        GameObject newProejctile = Instantiate(myProjectile, animator.transform.position + Vector3.up * 2, myProjectile.transform.rotation);
                        Destroy(newProejctile, 1);
                        myAttackController.myProjectiles.Add(newProejctile);
                        newProejctile.transform.LookAt(myAttackController.myTargetToAttack);
                        //newProejctile.GetComponent<ExplostionProjectile>().SetTarget(myAttackController.myTargetToAttack.transform.position);
                    
                    }
                    myAttackTimer = myAttackSpeed;
                    //myAttackController.LookAtSlowStart(newProejctile);
                }

                myAttackController.myTargetToAttack.GetComponent<UnitStats>().TakeDamage(myDamage * Time.deltaTime);
                myAttackTimer -= Time.deltaTime;
            }
            else
            {
                myAgent.SetDestination(myAttackController.myTargetToAttack.position);
                myAttackController.myTargetToAttack.GetComponent<UnitStats>().TakeDamage(myDamage * Time.deltaTime);
            }

            float distanceFromTarget = MathRts.FindDistanceNotSqrt(myAttackController.myTargetToAttack.position, animator.transform.position);
            if (distanceFromTarget > myStopAttackingDistance || myAttackController.myTargetToAttack == null)
            {
                animator.SetBool("isAttacking", false);
            }
        }
        //else
        //{
        //    animator.SetBool("isAttacking", false);
        //    myAttackController.myTargetToAttack = null;
        //}
    }

    private void LookAtTarget()
    {
        Vector3 direction = myAttackController.myTargetToAttack.position - myAgent.transform.position;
        myAgent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = myAgent.transform.eulerAngles.y;
        myAgent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
   
}
