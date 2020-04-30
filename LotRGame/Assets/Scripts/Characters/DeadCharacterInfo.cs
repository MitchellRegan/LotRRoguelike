using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in CharacterManager.cs and SaveLoadManager.cs to store information about a dead character
[System.Serializable]
public class DeadCharacterInfo
{
    //The dead character's first and last names
    public string firstName;
    public string lastName;
    //The sprites that make up the dead character's appearance
    public CharSpritePackage characterSprites;
}
