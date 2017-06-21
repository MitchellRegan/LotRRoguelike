using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Enum that determines a character's gender and some stat modifiers
public enum Genders { Male, Female, Genderless };
//Enum for the different types of character races
public enum Races {Human, Elf, Dwarf, Orc, HalfMan, GillFolk, ScaleSkin, Amazon, Minotaur, Other};

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Skills))]
[RequireComponent(typeof(PhysicalState))]
[RequireComponent(typeof(CombatStats))]
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

    //This character's race
    public Races race = Races.Human;

    //This character's height (in centimeters)
    public int height = 178;

    //This character's weight (in kilograms)
    public int weight = 80;


    //References to each of the required components
    [HideInInspector]
    public Inventory charInventory;
    [HideInInspector]
    public Skills charSkills;
    [HideInInspector]
    public PhysicalState charPhysState;
    [HideInInspector]
    public CombatStats charCombatStats;


    //Function called when this character is created
    private void Awake()
    {
        //Setting the references to each of the required components
        this.charInventory = this.GetComponent<Inventory>();
        this.charSkills = this.GetComponent<Skills>();
        this.charPhysState = this.GetComponent<PhysicalState>();
        this.charCombatStats = this.GetComponent<CombatStats>();
    }
}
