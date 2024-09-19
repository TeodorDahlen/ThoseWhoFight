using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garrison : MonoBehaviour
{
    [SerializeField]
    GameObject myNormalKnight;

    [SerializeField]
    GameObject mySpawnPoint;

    private void Update()
    {

        if(ResourceManager.Instance.GetFood() > 100 && ResourceManager.Instance.GetGold() > 50 && Input.GetKeyDown(KeyCode.P))
        {
            Instantiate(myNormalKnight, mySpawnPoint.transform.position, Quaternion.identity);
            ResourceManager.Instance.TakeFood(100);
            ResourceManager.Instance.TakeGold(100);
        }
    }
}
