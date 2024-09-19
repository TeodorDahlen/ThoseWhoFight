using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ResourceBuilding : MonoBehaviour
{
    [SerializeField]
    private float myProductionSpeed;

    [SerializeField]
    private workerState myState;

    

    [SerializeField]
    private GameObject myWorkerLocation;

    [SerializeField]
    private List<GameObject> myWorkers = new List<GameObject>();
    [SerializeField]
    private List<GameObject> myFarmLands = new List<GameObject>();

    [Space]
    [Header ("Farm")]
    //Farm land
    [SerializeField]
    private bool myIsFarmland;
    private ResourceBuilding myFarm;
    private bool myHasWorker;

    [SerializeField]
    public GameObject myArrivingWorker;
    [SerializeField]
    private int myActiveFarmlands;

    private void Start()
    {
      
    }
    private void Update()
    {
        if (myIsFarmland == false)
        {
            switch (myState)
            {
                case workerState.Farmer:
                    myActiveFarmlands = 0;
                    for (int i = 0; i < myFarmLands.Count; i++)
                    {
                        if (myFarmLands[i].GetComponent<ResourceBuilding>().HasWorker())
                        {
                            myActiveFarmlands += 1;
                        }
                    }
                    ResourceManager.Instance.AddFood(myProductionSpeed * myActiveFarmlands * Time.deltaTime);
                    break;
                case workerState.StoneMiner:
                    ResourceManager.Instance.AddStone(myProductionSpeed * myWorkers.Count * Time.deltaTime);
                    break;
                case workerState.Lumberjack:
                    ResourceManager.Instance.AddWood(myProductionSpeed * myWorkers.Count * Time.deltaTime);
                    break;
                case workerState.GoldMiner:
                    ResourceManager.Instance.AddGold(myProductionSpeed * myWorkers.Count * Time.deltaTime);
                    break;
            }
        }
    }

    public void AddFarmlandWorker(GameObject aWorker)
    {
        if(myWorkers.Count < 1)
        {
            myWorkers.Add(aWorker);
        }
    }
    public void AddWorker(GameObject aWorker)
    {
        if (myWorkers.Contains(aWorker.gameObject) == false && myIsFarmland == false)
        {
            if(myWorkers.Contains(aWorker) == false)
            {
                myWorkers.Add(aWorker);
                aWorker.GetComponent<Worker>().SetState(myState);
            }
        }
        else if(myWorkers.Contains(aWorker.gameObject) == false && myIsFarmland == true)
        {
            if(myFarm != null)
            {
                myFarm.AddWorker(aWorker);
            }
            else
            {
                Debug.Log("Farmland Without farm");
            }
        }
    }
    public Vector3 GetWorkerLocation(GameObject aWorker)
    {
        if (myFarm != null)
        {
            return myFarm.GetWorkerLocation(aWorker);
        }
        else
        {
            if(myState == workerState.Farmer && myFarmLands.Count >= 2)
            {

                //int randomFarmland = Random.Range(0, myFarmLands.Count - 1);
                for (int i = 0; i < myFarmLands.Count; i++)
                {
                    if(myFarmLands[i].GetComponent<ResourceBuilding>().HasWorker() == false)
                    {
                        myFarmLands[i].GetComponent<ResourceBuilding>().SetWorker(true);
                        myFarmLands[i].GetComponent<ResourceBuilding>().myArrivingWorker = aWorker;
                        return myFarmLands[i].transform.position;
                    }
                    else if(i == myFarmLands.Count)
                    {
                        return myWorkerLocation.transform.position;
                    }
                }
            }
        }
        
        return myWorkerLocation.transform.position;
        
    }

    public void SetWorker(bool aBool)
    { 
        myHasWorker = aBool;    
    }
    public bool HasWorker()
    {
        if (myWorkers.Count >= 1)
        {
            myHasWorker = true;
        }
        
        return myHasWorker;
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Worker>() != null)
        { 
            if(other.GetComponent<Worker>().GetState() == myState)
            {
                if(myWorkers.Contains(other.gameObject))
                {
                    myWorkers.Remove(other.gameObject);
                    if(myWorkers.Count < 1)
                    {
                        SetWorker(false);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (myIsFarmland == false)
        {
            if (myState == workerState.Farmer && other.CompareTag("FarmLand"))
            {
                if (myFarmLands.Contains(other.gameObject) == false)
                {
                    myFarmLands.Add(other.gameObject);
                    other.gameObject.GetComponent<ResourceBuilding>().SetFarmLandParrent(this);
                }
            }
            else if (other.GetComponent<Worker>() != null)
            {
                if (other.GetComponent<Worker>().GetState() == myState)
                {
                    if (myWorkers.Contains(other.gameObject) == false)  
                    {
                        AddWorker(other.gameObject);
                        Debug.Log("added worker");
                    }
                }
            }
            else
            {
                Debug.Log("not worker or farmbuilding");
            }
        }
        else
        {
            if(myArrivingWorker == other.gameObject)
            {
                AddFarmlandWorker(other.gameObject);    
                SetWorker(true);
            }
        }   
    }

    public void SetFarmLandParrent(ResourceBuilding aFarm)
    {
        myFarm = aFarm;
    }
}