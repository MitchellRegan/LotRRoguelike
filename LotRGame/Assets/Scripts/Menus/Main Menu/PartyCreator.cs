using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyCreator : MonoBehaviour
{
    //The list of each character customizer for all the characters that can be in the party
    public List<CharacterCustomizer> allCharacterCustomizers;

    //The list that determines how many party members each race starts with
    public List<RacePartyMembers> raceStartingPartyNumbers;



    //Function called when this object is enabled
    private void OnEnable()
    {
        //Looping through our list of RacePartyMembers so we can find out how many characters we start with
        int numCharacters = 1;
        foreach(RacePartyMembers r in this.raceStartingPartyNumbers)
        {
            //If we find the race that we're starting with
            if(r.race == GameData.globalReference.startingRace)
            {
                //Getting the number of starting party members and breaking the loop
                numCharacters = r.startingPartyMembers;
                break;
            }
        }

        //Making sure there's not more starting party characters than the number of Character Customizers
        if(numCharacters > this.allCharacterCustomizers.Count)
        {
            numCharacters = this.allCharacterCustomizers.Count;
        }

        //Activating each of the character customizers for the number of starting characters
        for(int c = 0; c < this.allCharacterCustomizers.Count; ++c)
        {
            //If we're in the range of the number of starting characters, we make sure they're activated
            if(c < numCharacters)
            {
                this.allCharacterCustomizers[c].enabled = true;
            }
            //If we're outside the range of starting characters, we make sure they're disabled
            else
            {
                this.allCharacterCustomizers[c].enabled = false;
            }
        }
    }
}

[System.Serializable]
public class RacePartyMembers
{
    //The race that determines how many party members there will be at the beginning
    public RaceTypes.Races race = RaceTypes.Races.Human;

    //The number of starting party members
    public int startingPartyMembers = 1;
}