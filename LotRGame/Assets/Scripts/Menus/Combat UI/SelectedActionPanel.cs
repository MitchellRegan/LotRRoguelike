using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class used in CombatActionPanelUI.cs
[System.Serializable]
public class SelectedActionPanel
{
    //The name of the action
    public Text nameText;
    //The description for the action
    public Text descriptionText;
    //The range of the action
    public Text rangeText;

    [Space(8)]

    //The parent object of the damage display
    public GameObject attackDetails;

    //The crit range of the attack
    public Text critText;
    //The crit multiplier of the attack
    public Text multiplierText;

    //The touch type of this attack
    public Text touchTypeText;

    //The accuracy of this attack
    public Text accuracyText;

    //The damage of the attack
    public Text damageText;

    //Effect on hit
    public Text effectNameText;
}