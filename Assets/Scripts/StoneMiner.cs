using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneMiner : MonoBehaviour
{
    [SerializeField]
    private float myProductionSpeed;

    private void Update()
    {
        ResourceManager.Instance.AddStone(myProductionSpeed * Time.deltaTime);
    }
}
