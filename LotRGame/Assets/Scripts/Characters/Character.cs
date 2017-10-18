using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Enum that determines a character's gender and some stat modifiers
public enum Genders { Male, Female, Genderless };

[RequireComponent(typeof(RaceTypes))]
[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Skills))]
[RequireComponent(typeof(PhysicalState))]
[RequireComponent(typeof(CombatStats))]
[RequireComponent(typeof(ActionList))]
[RequireComponent(typeof(CharacterSprites))]
public class Character : MonoBehaviour
{
    //This character's first name
    public string firstName = "Generic";
    //This character's last name
    public string lastName = "McPersonface";

    //The name of the clan this character is from
    public string clanName = "Killjoy";

    //This character's gender
    public Genders sex = Genders.Male;

    //This character's height (in centimeters)
    public int height = 178;

    //This character's weight (in kilograms)
    public int weight = 80;

    //The sprite that represents this character in combat
    public Sprite combatSprite;


    //References to each of the required components
    [HideInInspector]
    public RaceTypes charRaceTypes;
    [HideInInspector]
    public Inventory charInventory;
    [HideInInspector]
    public Skills charSkills;
    [HideInInspector]
    public PhysicalState charPhysState;
    [HideInInspector]
    public CombatStats charCombatStats;
    [HideInInspector]
    public ActionList charActionList;
    [HideInInspector]
    public CharacterSprites charSprites;


    //Function called when this character is created
    private void Awake()
    {
        //Setting the references to each of the required components
        this.charRaceTypes = this.GetComponent<RaceTypes>();
        this.charInventory = this.GetComponent<Inventory>();
        this.charSkills = this.GetComponent<Skills>();
        this.charPhysState = this.GetComponent<PhysicalState>();
        this.charCombatStats = this.GetComponent<CombatStats>();
        this.charActionList = this.GetComponent<ActionList>();
        this.charSprites = this.GetComponent<CharacterSprites>();
    }
}
