using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForageSkillButton : MonoBehaviour
{
    //The number of hours that pass when this skill is used
    [Range(0, 9)]
    public int hoursPassed = 3;



    //Function called externally from UI buttons to roll for foraging skills
    public void ForagingSkillCheck()
    {
        //Telling the time panel how many hours will pass while foraging
        TimePanelUI.globalReference.AdvanceTime(this.hoursPassed);

        //Getting the reference to the tile that the player party is currently on
        TileInfo playerTile = PartyGroup.group1.GetComponent<WASDOverworldMovement>().currentTile;

        //Int to hold the highest skill roll
        int highestRoll = 0;

        //Looping through each player character in the party
        for (int pc = 0; pc < PartyGroup.group1.charactersInParty.Count; ++pc)
        {
            //Making sure the current character slot isn't null
            if (PartyGroup.group1.charactersInParty[pc] != null)
            {
                //Rolling to see what the current character's skill check is
                int skillCheck = PartyGroup.group1.charactersInParty[pc].charSkills.survivalist;
                skillCheck += PartyGroup.group1.charactersInParty[pc].charSkills.survivalistMod;
                skillCheck += Random.Range(1, 100);

                //If the current skill check is higher than the current highest, this check becomes the new highest
                if (skillCheck > highestRoll)
                {
                    highestRoll = skillCheck;
                }
            }
        }

        Debug.Log("This is where we need to check foraging results. Highest roll: " + highestRoll);
    }
}