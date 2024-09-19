using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBuilding : MonoBehaviour
{
    [SerializeField]
    private float myProductionSpeed;

    private void Update()
    {
        ResourceManager.Instance.AddWood(myProductionSpeed * Time.deltaTime);
    }
}
