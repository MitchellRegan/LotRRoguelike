using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RaceTypes : MonoBehaviour
{
    //Enum for the different types of character races
    public enum Races
    {
        None,
        Human,
        Elf,
        Dwarf,
        Orc,
        HalfMan,
        GillFolk,
        ScaleSkin,
        Amazon,
        Minotaur,
        Elemental,
        Dragon
    }

    //Enum for all of the different subtypes
    public enum Subtypes
    {
        None,
        Humanoid,
        Beast,
        Insect,
        Undead,
        Specter,
        Plant,
        Aquatic,
        Flying
    }

    //This character's race
    public Races race = Races.Human;
    //List for all of this character's subtypes, since they can have multiple at once
    public List<Subtypes> subtypeList;
}
