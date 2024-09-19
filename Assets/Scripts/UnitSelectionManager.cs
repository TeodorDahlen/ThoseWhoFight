using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI.Table;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; set; }

    [SerializeField]
    public List<GameObject> myAllUnitsList = new List<GameObject>();
    [SerializeField]
    public List<GameObject> myUnitsSelected = new List<GameObject>();

    private Camera myCam;
    [SerializeField]
    private LayerMask myClickableLayer;

    [SerializeField]
    private LayerMask myGroundLayer;

    [SerializeField]
    private LayerMask myAttackableLayer;

    [SerializeField]
    private GameObject myGroundMarker;

    [SerializeField]
    private GameObject myAttackMarker;

    private bool AttackCurserViable;

    public bool myDidDubbleClick;
    public GameObject myDubbleClickTarget;

    [SerializeField]
    private UnitSelectionBox myUnitSelectionBox;

    [SerializeField]
    private GameObject myUnitPlacementVisual;
    [SerializeField]
    private Material myUnitPlacementVisualMaterial;
    [SerializeField]
    private GameObject myUnitDirectionArrow;

    private Vector3 myMousePosStart;
    private float myDragDistance;
    private Vector3 myArmyDestination;

    [SerializeField]
    private GameObject myMouseHoverObject;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        myCam = Camera.main;
    }

    private float clicked = 0;
    private float clicktime = 0;
    private float clickdelay = 0.5f;

    private void DubbleClick()
    {

        if (Input.GetMouseButtonDown(0))
        {
            clicked++;
            clicktime = clickdelay;

            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, myClickableLayer))
            {
                myDubbleClickTarget = hit.transform.gameObject;
            }
        }
        if (clicktime < 0)
        {
            if (clicked >= 2)
            {
                myDidDubbleClick = true;
                clicked = 0;
            }
            else
            {
                myDidDubbleClick = false;
                clicked = 0;
            }
        }
        clicktime -= Time.deltaTime;

    }

    private void Update()
    {
        //DubbleClickCheck
        DubbleClick();
        if (myDidDubbleClick)
        {
            if (myDubbleClickTarget != null)
            {
                myUnitSelectionBox.SelectAll(myDubbleClickTarget);
            }

            myDidDubbleClick = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, myClickableLayer))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MultiSelect(hit.collider.gameObject);
                }
                else
                {

                    SelectByClicking(hit.collider.gameObject);
                }
            }
            else //unselect all units
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    DeselectAll();
                }
            }
        }
        if (myUnitsSelected.Count >= 1)
        {
            if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.I)) && myUnitsSelected.Count >= 8)
            {
                RaycastHit hit;
                Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    myMousePosStart = hit.point;
                }

            }
            if ((Input.GetMouseButton(1) || Input.GetKey(KeyCode.I)))
            {
                RaycastHit hit1;
                Ray ray1 = myCam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray1, out hit1, Mathf.Infinity)) { }

                myArmyDestination = hit1.point;
                myDragDistance = (hit1.point - myMousePosStart).magnitude;

                if (myDragDistance > 0.5f)
                {
                    myUnitPlacementVisual.SetActive(true);
                    myUnitDirectionArrow.SetActive(true);
                    myUnitPlacementVisual.transform.position = myMousePosStart + Vector3.up * 0.2f;
                    myUnitDirectionArrow.transform.position = myMousePosStart + Vector3.up * 0.3f;

                    Vector3 row = (myArmyDestination - myMousePosStart);
                    int amountOfRows = Mathf.RoundToInt(row.magnitude / 2);
                    if (amountOfRows > 8)
                    {
                        amountOfRows = 8;
                    }
                    else if (amountOfRows < 1)
                    {
                        amountOfRows = 1;
                    }
                    Vector3 lookAtPos = new Vector3(myArmyDestination.x, myUnitPlacementVisual.transform.position.y, myArmyDestination.z);
                    myUnitPlacementVisual.transform.LookAt(lookAtPos);
                    myUnitDirectionArrow.transform.LookAt(lookAtPos);
                    myUnitPlacementVisual.transform.localScale = new Vector3((-myUnitsSelected.Count / amountOfRows) * 2 - 1, 1, amountOfRows);
                    myUnitDirectionArrow.transform.localScale = new Vector3((-myUnitsSelected.Count / amountOfRows) * 2 - 1, 1, amountOfRows);
                    myUnitPlacementVisualMaterial.mainTextureScale = new Vector2(Mathf.RoundToInt((-myUnitsSelected.Count / amountOfRows) - 1), amountOfRows / 2);

                    Vector3 xOfSet = new Vector3(row.z, 0, -row.x).normalized;
                }

                myMouseHoverObject = hit1.transform.gameObject;
            }


            if ((Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.I)) && myUnitsSelected.Count >= 8 && myDragDistance > 0.5)
            {
                Debug.Log("Fail 1");
                myUnitPlacementVisual.SetActive(false);
                myUnitDirectionArrow.SetActive(false);
                Vector3 row;
                RaycastHit hit1;
                Ray ray1 = myCam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray1, out hit1, Mathf.Infinity))
                {
                    row = (hit1.point - myMousePosStart);
                }
                else
                {
                    row = Vector3.forward;
                    Debug.Log("outOfRange mousePos");
                }

                int amountOfRows = Mathf.RoundToInt(row.magnitude / 2);
                amountOfRows = Mathf.Clamp(amountOfRows, 2, 8);
                Vector3 xOfSet = new Vector3(row.z, 0, -row.x).normalized;
                Vector3 placementRow = row.normalized;
                Vector3 startOfSet = new Vector3(row.z, 0, -row.x).normalized - placementRow;
                for (int i = 0; i < myUnitsSelected.Count; i++)
                {

                    if (i % Mathf.FloorToInt(myUnitsSelected.Count / amountOfRows) == 0 && i != 0)
                    {
                        placementRow += placementRow.normalized;
                        xOfSet = new Vector3(row.z, 0, -row.x).normalized;
                    }
                    Vector3 destination = myMousePosStart - xOfSet * 2 + placementRow + startOfSet;
                    myUnitsSelected[i].GetComponent<NavMeshAgent>().SetDestination(destination);
                    myUnitsSelected[i].GetComponent<UnitMovement>().myIsCommandedToMove = true;
                    myUnitsSelected[i].GetComponent<UnitMovement>().NewCommand(0.2f, destination);
                    myUnitsSelected[i].GetComponent<NavMeshAgent>().obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
                    myUnitsSelected[i].GetComponent<NavMeshAgent>().stoppingDistance = 0.1f;
                    myUnitsSelected[i].GetComponent<Animator>().SetBool("isFollowing", false);
                    myUnitsSelected[i].GetComponent<Animator>().SetBool("isAttacking", false);

                    xOfSet += new Vector3(placementRow.z, 0, -placementRow.x).normalized;
                }
            }
            else
            {
                
                if (myMouseHoverObject == null)
                {
                    myMouseHoverObject = gameObject;
                }
                if ((Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.I)) && (myMouseHoverObject.CompareTag("Assign building") || myMouseHoverObject.CompareTag("FarmLand")))
                {
            
                    myUnitPlacementVisual.SetActive(false);
                    myUnitDirectionArrow.SetActive(false);

                    for (int i = 0; i < myUnitsSelected.Count; i++)
                    {
                        if (myUnitsSelected[i].CompareTag("Villager"))
                        {
                            Vector3 destination = myMouseHoverObject.GetComponent<ResourceBuilding>().GetWorkerLocation(myUnitsSelected[i]);
                            myMouseHoverObject.GetComponent<ResourceBuilding>().AddWorker(myUnitsSelected[i]);
                            myUnitsSelected[i].GetComponent<NavMeshAgent>().SetDestination(destination);
                            myUnitsSelected[i].GetComponent<UnitMovement>().myIsCommandedToMove = true;
                            myUnitsSelected[i].GetComponent<UnitMovement>().NewCommand(0.2f, destination);

                            myUnitsSelected[i].GetComponent<NavMeshAgent>().stoppingDistance = 0.1f;
                            myUnitsSelected[i].GetComponent<Animator>().SetBool("isFollowing", false);
                            myUnitsSelected[i].GetComponent<Animator>().SetBool("isAttacking", false);
                        }
                    }
                }
                else if ((Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.I)) && myUnitsSelected.Count >= 8)
                {
               
                    myUnitPlacementVisual.SetActive(false);
                    myUnitDirectionArrow.SetActive(false);
                    Vector3 row;
                    RaycastHit hit1;
                    Ray ray1 = myCam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray1, out hit1, Mathf.Infinity))
                    {
                        row = (hit1.point - myMousePosStart);
                    }
                    else
                    {
                        row = Vector3.forward;
                        Debug.Log("outOfRange mousePos");
                    }

                    int amountOfRows = Mathf.RoundToInt(row.magnitude / 2);
                    if (amountOfRows > 8)
                    {
                        amountOfRows = 8;
                    }
                    else if (amountOfRows < 1)
                    {
                        amountOfRows = 1;
                    }
                    Debug.Log(amountOfRows);
                    Vector3 xOfSet = new Vector3(row.z, 0, -row.x).normalized;
                    Vector3 placementRow = row.normalized;
                    Vector3 startOfSet = new Vector3(row.z, 0, -row.x).normalized - placementRow;
                    for (int i = 0; i < myUnitsSelected.Count; i++)
                    {
                        Vector3 destination = myMousePosStart;
                        myUnitsSelected[i].GetComponent<NavMeshAgent>().SetDestination(destination);
                        myUnitsSelected[i].GetComponent<UnitMovement>().myIsCommandedToMove = true;
                        myUnitsSelected[i].GetComponent<UnitMovement>().NewCommand(0.2f, destination);
                        myUnitsSelected[i].GetComponent<NavMeshAgent>().obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
                        myUnitsSelected[i].GetComponent<NavMeshAgent>().stoppingDistance = 0.1f;
                        myUnitsSelected[i].GetComponent<Animator>().SetBool("isFollowing", false);
                        myUnitsSelected[i].GetComponent<Animator>().SetBool("isAttacking", false);

                        xOfSet += new Vector3(placementRow.z, 0, -placementRow.x).normalized;
                    }
                }
                else if (Input.GetMouseButtonUp(1))
                {
                    myUnitPlacementVisual.SetActive(false);
                    myUnitDirectionArrow.SetActive(false);
            
                    RaycastHit hit;
                    Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, myGroundLayer))
                    {
                        foreach (var unit in myUnitsSelected)
                        {
                            if (unit.GetComponent<NavMeshAgent>() != null && hit.point != null)
                            {
                                unit.GetComponent<NavMeshAgent>().SetDestination(hit.point);
                                unit.GetComponent<NavMeshAgent>().stoppingDistance = 1 + 0.2f * myUnitsSelected.Count;
                            }
                        }
                        myGroundMarker.transform.position = hit.point;

                        myGroundMarker.SetActive(false);
                        myGroundMarker.SetActive(true);
                    }
                }
            }
        }
        

        //Attack Target
        if (myUnitsSelected.Count > 0)
        {
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, myAttackableLayer))
            {
                if (myAttackMarker.transform.position != hit.transform.position)
                {
                    myAttackMarker.transform.position = hit.transform.position;
                }
                myAttackMarker.SetActive(true);
                AttackCurserViable = true;
                if (Input.GetMouseButtonDown(1))
                {
                    Transform target = hit.transform;
                    myGroundMarker.SetActive(false);
                    foreach (GameObject unit in myUnitsSelected)
                    {
                        if (unit.GetComponent<AttackController>() != null)
                        {
                            unit.GetComponent<AttackController>().myTargetToAttack = target;
                        }
                    }
                }
            }
            else
            {
                myAttackMarker.SetActive(false);
                AttackCurserViable = false;
            }
        }
    }

    private void MultiSelect(GameObject aUnit)
    {
        if (myUnitsSelected.Contains(aUnit) == false)
        {
            myUnitsSelected.Add(aUnit);
            EnableUnitMovement(aUnit, true);
            TriggerSelectionIndicator(aUnit, true);
        }
        else
        {
            EnableUnitMovement(aUnit, false);
            myUnitsSelected.Remove(aUnit);
            TriggerSelectionIndicator(aUnit, false);
        }
    }

    private void SelectByClicking(GameObject aUnit)
    {
        DeselectAll();
        myUnitsSelected.Add(aUnit);

        EnableUnitMovement(aUnit, true);
        TriggerSelectionIndicator(aUnit, true);
    }

    private void EnableUnitMovement(GameObject aUnit, bool shouldMove)
    {
        //aUnit.GetComponent<UnitMovement>().enabled = shouldMove;
    }

    public void DeselectAll()
    {
        foreach (var aUnit in myUnitsSelected)
        {
            if (aUnit != null)
            {
                EnableUnitMovement(aUnit, false);
                TriggerSelectionIndicator(aUnit, false);
            }
        }

        myGroundMarker.SetActive(false);
        myUnitsSelected.Clear();
            
    }

    private void TriggerSelectionIndicator(GameObject aUnit, bool isVisable)
    {
        aUnit.transform.GetChild(0).gameObject.SetActive(isVisable);
    }

    internal void DragSelect(GameObject aUnit)
    {
        if (myUnitsSelected.Contains(aUnit) == false)
        {
            myUnitsSelected.Add(aUnit);
            TriggerSelectionIndicator(aUnit, true);
            EnableUnitMovement(aUnit, true);
        }
    }


}
