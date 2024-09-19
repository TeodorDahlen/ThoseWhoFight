using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    [SerializeField]
    workerState myState;

    public void SetState(workerState newState)
    {
        myState = newState;
    }

    internal workerState GetState()
    {
        return this.myState;
    }
}


public enum workerState
{
    Villager,
    Farmer,
    StoneMiner,
    Lumberjack,
    GoldMiner
}