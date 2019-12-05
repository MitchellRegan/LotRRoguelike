using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used by Quest to define what an escort quest is
[System.Serializable]
public class QuestEscortCharacter
{
    //The character that needs to be escorted
    public Character characterToEscort;
    //Bool that determines if this escourt character has died
    [HideInInspector]
    public bool isCharacterDead = false;


    //Function called externally to check if one of our escort characters was killed
    public void CheckIfEscortCharacterDied(Character deadCharacter_)
    {
        //If the dead character is the one we're escorting, the quest is failed
        if (this.characterToEscort == deadCharacter_)
        {
            this.isCharacterDead = true;
        }
    }
}