﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Action : MonoBehaviour
{
    //The name of this action
    public string actionName = "";

    //Description of this action
    public string actionDescription = "";

    //The range of this action in terms of spaces on the combat tile grid
    [Range(0, 12)]
    public int range = 1;

    //Enum for the type of action this is
    [System.Serializable]
    public enum ActionType { Major, Minor, Fast, Massive };
    public ActionType type = ActionType.Major;

    //The amount of time in seconds that this action takes to be performed
    public float timeToCompleteAction = 3;


    
    //Function that is overrided by inheriting classes and called from the CombatManager to use this ability
    public virtual void PerformAction(CombatTile targetTile_)
    {
        //Nothing here, because regular actions don't do anything.
    }
}
