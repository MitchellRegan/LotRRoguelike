using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used by Quest to define what a kill quest is
[System.Serializable]
public class QuestKillRequirement
{
    //The number of kills required to complete this quest
    public int killsRequired = 1;
    //the number of currently completed kills for this quest
    [HideInInspector]
    public int currentKills = 0;

    //The enemy that quallifies as completing a kill for this quest
    public Character killableEnemy;



    //Function called externally to check if a killed character quallifies for this kill quest
    public void CheckKill(Character killedCharacter_)
    {
        //If we're already at our limit for required kills, nothing happens
        if (this.currentKills >= this.killsRequired)
        {
            return;
        }

        //If the killed character matches, the kill counts
        if (killedCharacter_.firstName == this.killableEnemy.firstName && killedCharacter_.lastName == this.killableEnemy.lastName &&
            killedCharacter_.sex == this.killableEnemy.sex && killedCharacter_.charRaceTypes.race == this.killableEnemy.GetComponent<RaceTypes>().race)
        {
            this.currentKills += 1;
        }
    }
}