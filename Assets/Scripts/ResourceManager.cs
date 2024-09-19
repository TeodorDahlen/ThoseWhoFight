using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class ResourceManager : MonoBehaviour
{

    public static ResourceManager Instance { get; set; }

    [Header("Resources")]
    [SerializeField]
    private float myFood;

    [SerializeField]
    private float myWood;

    [SerializeField]
    private float myStone;

    [SerializeField]
    private float mySteal;

    [SerializeField]
    private float myGold;

    [Space]
    [Header ("UI Information")]
    [SerializeField]
    private TMP_Text myFoodDisplayText;     
    [SerializeField]
    private TMP_Text myWoodDisplayText;
    [SerializeField]
    private TMP_Text myStoneDisplayText;
    [SerializeField]
    private TMP_Text myGoldDisplayText;
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
        UpdateResourceDisplay();
    }
    public void AddFood(float aAmount)
    {
        myFood += aAmount;
        UpdateResourceDisplay();
    }
    public void AddWood(float aAmount)
    {
        myWood += aAmount;
        UpdateResourceDisplay();
    }
    public void AddStone(float aAmount)
    {
        myStone += aAmount;
        UpdateResourceDisplay();
    }
    public void AddGold(float aAmount)
    {
        myGold += aAmount;
        UpdateResourceDisplay();
    }
    public void TakeFood(float aAmount)
    {
        myFood -= aAmount;
        UpdateResourceDisplay();
    }
    public void TakeWood(float aAmount)
    {
        myWood -= aAmount;
        UpdateResourceDisplay();
    }
    public void TakeStone(float aAmount)
    {
        myStone -= aAmount;
        UpdateResourceDisplay();
    }
    public void TakeGold(float aAmount)
    {
        myGold -= aAmount;
        UpdateResourceDisplay();
    }

    public int GetFood()
    {
        return Mathf.FloorToInt(myFood);
    }
    public int GetWood()
    {
        return Mathf.FloorToInt(myWood);
    }
    public int GetStone()
    {
        return Mathf.FloorToInt(myStone);
    }
    public int GetGold()
    {
        return Mathf.FloorToInt(myGold);
    }

    
    private void UpdateResourceDisplay()
    {
        myFoodDisplayText.SetText("Food: " + Mathf.FloorToInt(myFood)); 
        myWoodDisplayText.SetText("Wood: " + Mathf.FloorToInt(myWood)); 
        myStoneDisplayText.SetText("Stone: " + Mathf.FloorToInt(myStone)); 
        myGoldDisplayText.SetText("Gold: " + Mathf.FloorToInt(myGold)); 
    }

}
