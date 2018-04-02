using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingSkillButton : MonoBehaviour
{
    //The number of hours that pass when this skill is used
    [Range(0, 9)]
    public int hoursPassed = 3;



    //Function called externally from UI buttons to roll for tracking skills
    public void TrackingSkillCheck()
    {
        //Telling the time panel how many hours will pass while tracking
        TimePanelUI.globalReference.AdvanceTime(this.hoursPassed);

        //Getting the reference to the tracking block of the tile that the player party is currently on
        List<EncounterBlock> trackingEncounters = PartyGroup.group1.GetComponent<WASDOverworldMovement>().currentTile.getTrackingEncounters();

        //Int to hold the highest skill roll
        int highestRoll = 0;

        //Looping through each player character in the party
        for (int pc = 0; pc < PartyGroup.group1.charactersInParty.Count; ++pc)
        {
            //Making sure the current character slot isn't null
            if (PartyGroup.group1.charactersInParty[pc] != null)
            {
                //Rolling to see what the current character's skill check is
                int skillCheck = PartyGroup.group1.charactersInParty[pc].charSkills.GetSkillLevelValueWithMod(SkillList.Survivalist);
                skillCheck += Random.Range(1, 100);

                //If the current skill check is higher than the current highest, this check becomes the new highest
                if (skillCheck > highestRoll)
                {
                    highestRoll = skillCheck;
                }
            }
        }

        Debug.Log("This is where we need to check tracking results. Highest roll: " + highestRoll);

        //Looping through this tile's tracking encounter blocks to see what the highest success is
        int currentHighestIndex = -1;
        for(int t = 0; t < trackingEncounters.Count; ++t)
        {
            //If the current encounter's skill check is less than or equal to the best roll
            if(trackingEncounters[t].skillCheck <= highestRoll)
            {
                //If the current encounter's skill is higher than the current highest encounter's, it becomes the new highest
                if(trackingEncounters[t].skillCheck > trackingEncounters[currentHighestIndex].skillCheck)
                {
                    currentHighestIndex = t;
                }
            }
        }

        //If the current highest index is greater than -1, the player has succeeded and we start an encounter
        if(currentHighestIndex > -1)
        {
            //Getting the variables for the player party and tile to begin combat
            LandType tileType = PartyGroup.group1.GetComponent<WASDOverworldMovement>().currentTile.type;
            PartyGroup playerParty = PartyGroup.group1;
            //Telling the combat manager to start the fight
            CombatManager.globalReference.InitiateCombat(tileType, playerParty, trackingEncounters[currentHighestIndex].encounterEnemies);
        }
    }
}