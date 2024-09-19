using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;
public class UnitMovement : MonoBehaviour
{
    Camera myCam;
    NavMeshAgent myAgent;

    [SerializeField]
    private LayerMask myGroundLayer;

    public bool myIsCommandedToMove;

    private AttackController myAttackController;

    private float myNewCommandTimer = 0.5f;

    private Vector3 myLastCommandPoint;
    private void Start()
    {
        myCam = Camera.main;
        myAgent = GetComponent<NavMeshAgent>();
        myAttackController = GetComponent<AttackController>();
        myLastCommandPoint = Vector3.zero;
    }
        
    private void Update()
    {
        myNewCommandTimer -= Time.deltaTime;
        //if (Input.GetMouseButtonDown(1))
        //{
        //    RaycastHit hit;
        //    Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

        //    if (Physics.Raycast(ray, out hit, Mathf.Infinity, myGroundLayer))
        //    {
        //        myAgent.SetDestination(hit.point);
        //        Debug.Log("Commanded");
        //        myIsCommandedToMove = true;
        //    }
        //}

        //Agent Reach destination

        

        if (myAgent.remainingDistance <= myAgent.stoppingDistance && myIsCommandedToMove == true && myNewCommandTimer <= 0)
        {
            myIsCommandedToMove = false;    
            myAgent.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
            myAgent.stoppingDistance = 0.5f;
        }
    }
    public void NewCommand(float aTime, Vector3 aPostition)
    {
        myNewCommandTimer = aTime;
        myLastCommandPoint = aPostition;
    }
    public void GoToLastCommandPoint()
    {
        if(myLastCommandPoint != Vector3.zero && myLastCommandPoint != null)
        {
            if(TryGetComponent(out myAgent))
            {
                myAgent.SetDestination(myLastCommandPoint);
            }
        }
    }
}
