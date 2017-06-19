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
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(ReceiveEvent))]
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
}
