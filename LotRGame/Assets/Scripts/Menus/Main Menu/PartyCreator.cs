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

    //The list of all party characters that are created and will be added to the starting party
    private List<Character> newPartyCharacters;

    //Enum to track the chosen player race
    private RaceTypes.Races chosenRace = RaceTypes.Races.None;



    //Function called when this object is created
    private void Awake()
    {
        this.newPartyCharacters = new List<Character>();
    }


    //Function called when this object is enabled
    private void OnEnable()
    {
        //If the player has chosen a different starting race since the last time this menu was up
        if (this.chosenRace == GameData.globalReference.startingRace)
        {
            //Nothing happens
            return;
        }

        //Setting the chosen race to the new race
        this.chosenRace = GameData.globalReference.startingRace;

        //Clearing all of the characters that are currently in the newParyCharacters list
        for (int i = 0; i < this.newPartyCharacters.Count; ++i)
        {
            Destroy(this.newPartyCharacters[i].gameObject);
            this.newPartyCharacters[i] = null;
        }
        this.newPartyCharacters = new List<Character>();

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
                this.allCharacterCustomizers[c].gameObject.SetActive(true);

                //Creating a new instance of a character of the selected race
                foreach(RacePartyMembers r in this.raceStartingPartyNumbers)
                {
                    //When we find the race that we're starting with
                    if(r.race == GameData.globalReference.startingRace)
                    {
                        //Getting a random index for which gender is going to be created
                        int genderIndex = Random.Range(0, r.characterPrefabs.Count);
                        //Creating a new character instance of the prefab for the gender
                        GameObject createdChar = GameObject.Instantiate(r.characterPrefabs[genderIndex]);
                        //Adding the created character to our list of party characters
                        this.newPartyCharacters.Add(createdChar.GetComponent<Character>());

                        //Setting all of the info for each Character Customizer
                        this.SetCharacterCustomizerInfo(this.allCharacterCustomizers[c], createdChar.GetComponent<Character>());

                        //Breaking out of the loop
                        break;
                    }
                }
            }
            //If we're outside the range of starting characters, we make sure they're disabled
            else
            {
                this.allCharacterCustomizers[c].gameObject.SetActive(false);
            }
        }
    }


    //Function called from OnEnable to set a CharacterCustomizer to display a character's info
    private void SetCharacterCustomizerInfo(CharacterCustomizer customizer_, Character charInfo_)
    {
        //Setting the first and last names
        customizer_.firstNameText.text = charInfo_.firstName;
        customizer_.lastNameText.text = charInfo_.lastName;

        //Setting the sex
        customizer_.sex = charInfo_.sex;
        //Setting the sex text
        switch(customizer_.sex)
        {
            case Genders.Male:
                customizer_.sexText.text = "Male";
                break;
            case Genders.Female:
                customizer_.sexText.text = "Female";
                break;
            case Genders.Genderless:
                customizer_.sexText.text = "Genderless";
                break;
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

    //The prefab for this race of character
    public List<GameObject> characterPrefabs;
}