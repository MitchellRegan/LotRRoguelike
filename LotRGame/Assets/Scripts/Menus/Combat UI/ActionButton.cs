using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class used in CombatActionPanelUI.cs
[System.Serializable]
public class ActionButton
{
    //The name text for the action
    public Text nameText;
    //The description for the action
    public Text descriptionText;
    //The button component that can be enabled/disabled
    public Button buttonComponent;
}