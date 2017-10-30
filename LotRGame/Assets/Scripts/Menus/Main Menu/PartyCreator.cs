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
    


    //Function called when this object is enabled
    private void OnEnable()
    {
        //Generating characters
        this.GenerateStartingCharacters();
    }


    //Function called from OnEnable and externally to generate the starting characters
    public void GenerateStartingCharacters()
    {
        //If the player has chosen a different starting race since the last time this menu was up
        if (this.chosenRace == GameData.globalReference.startingRace)
        {
            //Nothing happens
            return;
        }

        //Making sure we have an initialized list of party characters
        if(this.newPartyCharacters == null)
        {
            this.newPartyCharacters = new List<Character>();
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
        foreach (RacePartyMembers r in this.raceStartingPartyNumbers)
        {
            //If we find the race that we're starting with
            if (r.race == GameData.globalReference.startingRace)
            {
                //Getting the number of starting party members and breaking the loop
                numCharacters = r.startingPartyMembers;
                break;
            }
        }

        //Making sure there's not more starting party characters than the number of Character Customizers
        if (numCharacters > this.allCharacterCustomizers.Count)
        {
            numCharacters = this.allCharacterCustomizers.Count;
        }

        //Activating each of the character customizers for the number of starting characters
        for (int c = 0; c < this.allCharacterCustomizers.Count; ++c)
        {
            //Enabling each character customizer so it can be initialized
            this.allCharacterCustomizers[c].InitializeCustomizer();

            //If we're in the range of the number of starting characters, we make sure they're activated
            if (c < numCharacters)
            {
                this.allCharacterCustomizers[c].gameObject.SetActive(true);

                //Looping through and creating a random character sprite for each race
                for(int sb = 0; sb < this.allCharacterCustomizers[c].raceSpriteBases.Count; ++sb)
                {
                    this.allCharacterCustomizers[c].raceSpriteBases[sb].customizer.GenerateRandomCharacter();
                }

                //Creating a new instance of a character of the selected race
                foreach (RacePartyMembers r in this.raceStartingPartyNumbers)
                {
                    //When we find the race that we're starting with
                    if (r.race == GameData.globalReference.startingRace)
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


    //Function called when a new game is started to finalize each character
    public void FinalizeCharactersForNewGame()
    {
        //Looping through each party character
        for(int c = 0; c < this.newPartyCharacters.Count; ++c)
        {
            //Making sure the CharacterCustomizer isn't null
            if (this.allCharacterCustomizers[c] != null)
            {
                //Looping through the selected CharacterCustomizer to get the sprite customizer for the selected race
                foreach(CustomizerRaceSpriteBase rsb in this.allCharacterCustomizers[c].raceSpriteBases)
                {
                    //If the current race sprite base is the one for the starting race
                    if(rsb.race == GameData.globalReference.startingRace)
                    {
                        //Setting the new character's sprites
                        this.newPartyCharacters[c].charSprites.allSprites = rsb.customizer.spritePackage;
                        //Breaking out of the loop
                        break;
                    }
                }

                //Setting the character's first and last names
                this.newPartyCharacters[c].firstName = this.allCharacterCustomizers[c].firstNameText.text;
                this.newPartyCharacters[c].lastName = this.allCharacterCustomizers[c].lastNameText.text;

                //Setting the character's sex
                this.newPartyCharacters[c].sex = this.allCharacterCustomizers[c].sex;

                //Adding all of the allocated skill points to the designated skills
                this.newPartyCharacters[c].charCombatStats.punching += this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Punching];
                this.newPartyCharacters[c].charCombatStats.daggers += this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Daggers];
                this.newPartyCharacters[c].charCombatStats.swords += this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Swords];
                this.newPartyCharacters[c].charCombatStats.axes += this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Axes];
                this.newPartyCharacters[c].charCombatStats.spears += this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Spears];
                this.newPartyCharacters[c].charCombatStats.bows += this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Bows];
                this.newPartyCharacters[c].charCombatStats.improvised += this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Improvised];

                this.newPartyCharacters[c].charCombatStats.holyMagic += this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.HolyMagic];
                this.newPartyCharacters[c].charCombatStats.darkMagic += this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.DarkMagic];
                this.newPartyCharacters[c].charCombatStats.natureMagic += this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.NatureMagic];

                this.newPartyCharacters[c].charSkills.cooking += this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Cooking];
                this.newPartyCharacters[c].charSkills.healing += this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Healing];
                this.newPartyCharacters[c].charSkills.crafting += this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Crafting];

                this.newPartyCharacters[c].charSkills.foraging += this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Foraging];
                this.newPartyCharacters[c].charSkills.tracking += this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Tracking];
                this.newPartyCharacters[c].charSkills.fishing += this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Fishing];

                this.newPartyCharacters[c].charSkills.climbing += this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Climbing];
                this.newPartyCharacters[c].charSkills.hiding += this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Hiding];
                this.newPartyCharacters[c].charSkills.swimming += this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Swimming];
            }


            //Parenting the character object to the global data object so we can transfer it over to the gameplay scene
            this.newPartyCharacters[c].transform.SetParent(GameData.globalReference.transform);
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