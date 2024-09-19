using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppelFarm : MonoBehaviour
{
    [SerializeField]
    private float myProductionSpeed;

    private void Update()
    {
        ResourceManager.Instance.AddFood(myProductionSpeed * Time.deltaTime);
    }
}
