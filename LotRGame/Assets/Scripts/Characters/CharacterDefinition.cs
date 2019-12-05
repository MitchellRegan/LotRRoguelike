using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in CharacterManager.cs. Lets us create a list in the inspector to set character objects based on race and sex
[System.Serializable]
public class CharacterDefinition
{
    //The race of the character
    public Races race = Races.Human;
    //The sex of the character
    public Genders gender = Genders.Male;
    //The game object for the character that's spawned
    public GameObject characterObject;
}