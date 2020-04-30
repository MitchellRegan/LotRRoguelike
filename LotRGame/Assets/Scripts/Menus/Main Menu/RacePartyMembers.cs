using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in PartyCreator.cs
[System.Serializable]
public class RacePartyMembers
{
    //The race that determines how many party members there will be at the beginning
    public Races race = Races.Human;

    //The number of starting party members
    public int startingPartyMembers = 1;

    //The prefab for this race of character
    public List<GameObject> characterPrefabs;
}