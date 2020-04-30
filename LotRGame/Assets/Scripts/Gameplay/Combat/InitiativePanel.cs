using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class used in CombatManager.cs that represents an individual character's name/health/initiative panel
[System.Serializable]
public class InitiativePanel
{
    public Slider initiativeSlider;
    public Text characterName;
    public Image background;
    public Slider healthSlider;
}