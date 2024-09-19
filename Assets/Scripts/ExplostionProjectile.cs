using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplostionProjectile : MonoBehaviour
{
    [SerializeField]
    private GameObject myExplostionEffect;

    [SerializeField]
    private float myDamage;

    [SerializeField]
    private bool myShouldExplode;

    public Vector3 myStartPos;

    private Vector3 myTarget;
    [SerializeField]
    private float myProjecitleSpeed;
    [SerializeField]
    private float myArchHight = 1;
    public float myTime;


    private void Start()
    {
        myStartPos = transform.position;
    }

    public void SetTarget(Vector3 aTarget)
    {
        myTarget = aTarget;
    }
    private void Update()
    {
        //myTime += myProjecitleSpeed * Time.deltaTime;
        //transform.position = Vector3.Lerp(myStartPos, myTarget, myTime);
        //transform.position = new(transform.position.x, MathRts.GetArcHeight(myTime) * myArchHight + transform.position.y, transform.position.z);

    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.GetComponent<UnitStats>() != null)
    //    {
    //        collision.gameObject.GetComponent<UnitStats>().TakeDamage(myDamage);
    //    }
    //    Explode();
    //}
    public void Explode()
    {
        if (myShouldExplode)
        {
            GameObject Explosion = Instantiate(myExplostionEffect, transform.position, Quaternion.identity);
            Destroy(Explosion, 2);
        }
        Destroy(gameObject);
    }
    


}
