using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    [Header("General Stats")]
    [SerializeField]
    private float myMaxHealth = 50;
    [SerializeField]
    private float myHealth = 50;

    [SerializeField]
    private float myDamage = 1;

    [SerializeField]
    private float myAttackSpeed = 1;

    [SerializeField]
    private float myStrikeDefense = 0;  

    [SerializeField]
    private float myPiercingDefense = 0;

    [SerializeField]
    private float myMagicDefense = 0;

    [SerializeField]
    private int myActiveEnemys = 0; 

    [Space]
    [Header("Player Unit")]

    [SerializeField]
    private bool myIsPlayerUnit;

    private void Start()
    {
        myHealth = myMaxHealth;
    }
    public void TakeDamage(float aDamageAmount)
    {
        myHealth -= aDamageAmount;

        if(myHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void AddEnemyPrefrence()
    {
        myActiveEnemys++;
    }
    public void RemoveEnemyPrefrence()
    {
        myActiveEnemys--;
    }
    public int GetActiveEnemys()
    {
        return myActiveEnemys;
    }
}
