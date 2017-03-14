using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    //The list of characters that are currently in the player's party
    [HideInInspector]
    public List<Character> playerParty;

    //The list of characters that have died in the current game
    [HideInInspector]
    public List<Character> deadCharacters;

    //The list of all character object definitions that can be spawned based on sex and race
    public List<CharacterDefinition> listOfCharacterObjects;
}


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