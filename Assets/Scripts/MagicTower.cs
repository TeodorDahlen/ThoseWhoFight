using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MagicTower : MonoBehaviour
{

    [Header("Building")]
    [SerializeField]
    private float myMaxHealth;
    [SerializeField]
    private float myHealth;

    [Space]
    [Header("My Tower Stats")]
    [SerializeField]
    private float myAttackDamage;
    [SerializeField]
    private float myAttackSpeed;
    private float myAttackDelay;
    
    [SerializeField]
    private float myAttackRange;

    [SerializeField]
    private GameObject myTarget;

    private Vector3 myTargetDirection;

    [SerializeField]
    private GameObject myProjectile;
    [SerializeField]
    private float myProjectileSpeed;

    [SerializeField]
    private float myExplodeRange; 
    
    [SerializeField]
    private float myDamage;

    [SerializeField]
    private List<GameObject> myProjectiles = new List<GameObject>();

    private void Start()
    {
        myHealth = myMaxHealth;
    }
    private void Update()
    {
        if(myTarget != null)
        {

            myAttackDelay -= Time.deltaTime;
            AimAtTarget();
            if (myAttackDelay <= 0)
            {
                FireProjectile();
                myAttackDelay = myAttackSpeed;
            }

            for (int i = 0; i < myProjectiles.Count; i++)
            {
                if (myProjectiles[i] == null)
                {
                    myProjectiles.Remove(myProjectiles[i]);
                }
                else if (MathRts.FindDistanceNotSqrt(myTarget.transform.position, myProjectiles[i].transform.position) <= myExplodeRange)
                {
                    myProjectiles[i].GetComponent<ExplostionProjectile>().Explode();
                    myTarget.GetComponent<UnitStats>().TakeDamage(myDamage);
                    myProjectiles.Remove(myProjectiles[i]);
                }
                else
                {
                    myProjectiles[i].transform.position += myTargetDirection * myProjectileSpeed * Time.deltaTime;
                }
            }

        }
    }
    private void FireProjectile()
    {
        myProjectiles.Add(Instantiate(myProjectile, transform.position, Quaternion.identity));
    }

    private void AimAtTarget()
    {
        myTargetDirection = (myTarget.transform.position - gameObject.transform.position).normalized;
        
    }
    

    private void OnTriggerEnter(Collider other)
    {
        myTarget = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        myTarget = null;
    }
}
