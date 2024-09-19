using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField]
    private List <GameObject> myHouses = new List<GameObject>();
    [SerializeField]
    private GameObject myFarm;
    [SerializeField]
    private GameObject myFarmLand;
    [SerializeField]
    private GameObject myWoodHut;
    [SerializeField]
    private GameObject myBarrack;
    [SerializeField]
    private GameObject myArhcerMakingBuilding;
    [SerializeField]
    private GameObject myStoneMine;  
    [SerializeField]
    private GameObject myGoldMine;

    private GameObject[,] myGrid; 
    private Camera myCam;

    [SerializeField]
    private LayerMask myGroundLayer;
    private void Start()
    {
        myCam = Camera.main;
        myGrid = new GameObject[50,50];     
    }
    private void Update()   
    {           
        if(Input.GetKeyDown(KeyCode.T))
        {
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, myGroundLayer))
            {
                Vector3 buildPoint = new Vector3(Mathf.Round(hit.point.x / 10) , hit.point.y, Mathf.Round(hit.point.z / 10));
                if (myGrid[(int)buildPoint.x,(int)buildPoint.z] == null)
                {
                   myGrid[(int)buildPoint.x, (int)buildPoint.z] = Instantiate(myHouses[Random.Range(0, myHouses.Count)], new Vector3(buildPoint.x * 10, buildPoint.y, buildPoint.z * 10), Quaternion.identity);
                    Debug.Log("New house my Lord  " + buildPoint);
                }
                else
                {
                    Debug.Log("Cant place there my lord!  " + (int)buildPoint.x + "  " + (int)buildPoint.z );
                    Debug.Log(myGrid[(int)buildPoint.x, (int)buildPoint.z]);
                }
            }
        }
        //Farm buildings
        if (Input.GetKeyDown(KeyCode.V))
        {
            SpawnBuilding(myFarm, 100);
        }
        //Farm Land
        if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnBuilding(myFarmLand, 50);
        }
        //Wood Building
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SpawnBuilding(myWoodHut, 100);
        }
        //Stone mine
        if (Input.GetKeyDown(KeyCode.M))    
        {
            SpawnBuilding(myStoneMine, 100);
        }
        //Gold mine
        if (Input.GetKeyDown(KeyCode.N))    
        {
            SpawnBuilding(myGoldMine, 100);
        }
        //Knight barracks
        if (Input.GetKeyDown(KeyCode.G))
        {
            SpawnBuilding(myBarrack, 300);
        }
        //Archer building
        if (Input.GetKeyDown(KeyCode.H))
        {
            SpawnBuilding(myArhcerMakingBuilding, 100);
        }
    }

    private void SpawnBuilding(GameObject aBuilding, float aWoodCost)
    {
        RaycastHit hit;
        Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, myGroundLayer))
        {
            Vector3 buildPoint = new Vector3(Mathf.Round(hit.point.x / 10), hit.point.y, Mathf.Round(hit.point.z / 10));
            if (myGrid[(int)buildPoint.x, (int)buildPoint.z] == null
                //&&
                //myGrid[(int)buildPoint.x + 1, (int)buildPoint.z] == null &&
                //myGrid[(int)buildPoint.x, (int)buildPoint.z - 1] == null &&
                //myGrid[(int)buildPoint.x + 1, (int)buildPoint.z - 1] == null 
                && ResourceManager.Instance.GetWood() >= aWoodCost)
            {
                GameObject newFarm = Instantiate(aBuilding, new Vector3(buildPoint.x * 10, buildPoint.y, buildPoint.z * 10), Quaternion.identity);
                myGrid[(int)buildPoint.x, (int)buildPoint.z] = newFarm;
                //myGrid[(int)buildPoint.x + 1, (int)buildPoint.z] = newFarm;
                //myGrid[(int)buildPoint.x, (int)buildPoint.z - 1] = newFarm;
                //myGrid[(int)buildPoint.x + 1, (int)buildPoint.z - 1] = newFarm;
                Debug.Log("New " + "building" + " my Lord" );
                ResourceManager.Instance.TakeWood(aWoodCost);
            }
            else if (ResourceManager.Instance.GetWood() < aWoodCost)
            {
                Debug.Log("Not enough wood my lord");
            }
            else
            {
                Debug.Log("Cant place there my lord!  " + (int)buildPoint.x + "  " + (int)buildPoint.z);
                Debug.Log(myGrid[(int)buildPoint.x, (int)buildPoint.z]);
            }
        }
    }
}
