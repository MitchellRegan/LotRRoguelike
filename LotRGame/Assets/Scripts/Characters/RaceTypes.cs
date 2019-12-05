using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RaceTypes : MonoBehaviour
{
    //This character's race
    public Races race = Races.Human;
    //List for all of this character's subtypes, since they can have multiple at once
    public List<Subtypes> subtypeList;
}
