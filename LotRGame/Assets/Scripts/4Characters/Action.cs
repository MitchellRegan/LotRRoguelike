using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    //The name of this action
    public string actionName = "";

    //Description of this action
    public string actionDescription = "";

    //Enum for the type of action this is
    public enum ActionType { Standard, Secondary, Quick, FullRound };
    public ActionType type = ActionType.Standard;
}
