﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public enum ActionType { Standard, Secondary, Quick, FullRound };
    public ActionType type = ActionType.Standard;
}
