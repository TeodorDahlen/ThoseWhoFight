using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class AttackController : MonoBehaviour
{
    public Transform myTargetToAttack;

    [SerializeField]
    private Renderer myFlag;

    [SerializeField]
    private Material myIdleFlag;

    [SerializeField]
    private Material myAttackFlag;

    [SerializeField]
    private Material myWalkFlag;

    [SerializeField]
    public List<GameObject> myHostileTargets = new List<GameObject>();

    private UnitMovement myUnitMovement;

    private int myLastHositleCount;

    [SerializeField]
    private float myArrowOfset = 1;

    [SerializeField]
    public List<GameObject> myProjectiles = new List<GameObject>();
    [SerializeField]
    private float myProjectileSpeed = 10;
    private void Start()
    {
        myUnitMovement = GetComponent<UnitMovement>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            myHostileTargets.Add(other.gameObject);
        }
        if (myUnitMovement.myIsCommandedToMove == false)
        {
            FindAttackTarget();
            myLastHositleCount = myHostileTargets.Count;
        }
    }
    private void FindAttackTarget()
    {
        for (int i = 0; i < myHostileTargets.Count; i++)
        {
            if (myHostileTargets[i] == null)
            {
                myHostileTargets.Remove(myHostileTargets[i]);
            }
        }
        for (int j = 0; j < myHostileTargets.Count; j++)
        {
            if (myHostileTargets[j] == null)
            {
                myHostileTargets.Remove(myHostileTargets[j]);
                continue;
            }
            for (int k = 0; k < myHostileTargets.Count; k++)
            {
                if (myHostileTargets[k] != null)
                {

                    if (myHostileTargets[k].GetComponent<UnitStats>().GetActiveEnemys() == j)
                    {
                        myTargetToAttack = myHostileTargets[k].transform;
                        myHostileTargets[k].GetComponent<UnitStats>().AddEnemyPrefrence();
                        break;
                    }
                }
            }

            
            if (myTargetToAttack != null)
            {
                break;
            }
        }
        if(myTargetToAttack == null && myHostileTargets.Count != 0)
        {
            for (int i = 0; i < myHostileTargets.Count; i++)
            {
                myTargetToAttack = myHostileTargets[i].transform;
            }
        }

        if (myTargetToAttack == null)
        {
            GetComponent<Animator>().SetBool("isAttacking", false);
            GetComponent<Animator>().SetBool("isFollowing", false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            myTargetToAttack = null;
        }
        myHostileTargets.Remove(other.gameObject);
    }

    public void SetIdleFlag()
    {
        myFlag.material = myIdleFlag;
    }
    public void SetAttackFlag()
    {
        myFlag.material = myAttackFlag;
    }
    public void SetWalkFlag()
    {
        myFlag.material = myWalkFlag;
    }

    private void OnDrawGizmos()
    {
        ////Follow Distance
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, 50 * transform.localScale.x);

        ////AttackDistance
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, 2);

        ////Stop Attack
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, 3);
    }
    private void Update()
    {
       

        if (myHostileTargets.Count != myLastHositleCount && myUnitMovement.myIsCommandedToMove == false)
        {
            FindAttackTarget();
        }

        if (myProjectiles.Count >= 1)
        {
            UpdateProjectile();
        }
    }
    //public void LookAtSlowStart(GameObject aProjectile)
    //{
    //    StartCoroutine(LookAtSlow(aProjectile));
    //}



    //private IEnumerator LookAtSlow(GameObject aProjectile)
    //{
    //    float time = 0;
    //    Vector3 target = myTargetToAttack.transform.position + new Vector3(Random.Range(-myArrowOfset, myArrowOfset), 0, Random.Range(-myArrowOfset, myArrowOfset));
    //    while (time < 1)
    //    {
    //        if (myTargetToAttack != null && aProjectile != null)
    //        {
    //            Vector3 direction = target - aProjectile.transform.position;
    //            Quaternion lookAtRotation = Quaternion.LookRotation(direction);
    //            aProjectile.transform.rotation = Quaternion.Slerp(aProjectile.transform.rotation, lookAtRotation, time);
    //        }
    //        time += Time.deltaTime * 0.1f;

    //        yield return null;
    //    }
    //}
    private void UpdateProjectile()
    {


        for (int i = 0; i < myProjectiles.Count; i++)
        {
            if (myProjectiles[i] == null)
            {
                myProjectiles.Remove(myProjectiles[i]);
            }
        }
        
        foreach (GameObject aProjectile in myProjectiles)
        {
            if(myTargetToAttack != null)
            { 
                ExplostionProjectile ep = aProjectile.GetComponent<ExplostionProjectile>();
                ep.myTime += Time.deltaTime;
                aProjectile.transform.position = Vector3.Lerp(ep.myStartPos, myTargetToAttack.transform.position, ep.myTime);
                aProjectile.transform.position = new(aProjectile.transform.position.x, MathRts.GetArcHeight(ep.myTime) * 40 + aProjectile.transform.position.y, aProjectile.transform.position.z);
            }
        }
    }
}