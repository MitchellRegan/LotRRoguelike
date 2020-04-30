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
    private Races chosenRace = Races.None;

    //Static int for how many hit dice player characters start with
    public static int startingHitDice = 3;



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

                        //Initilizing the created character's default inventory items
                        this.InitializeStartingItems(createdChar.GetComponent<Inventory>(), r.characterPrefabs[genderIndex].GetComponent<Inventory>());

                        //Setting all of the info for each Character Customizer
                        this.SetCharacterCustomizerInfo(this.allCharacterCustomizers[c], createdChar.GetComponent<Character>());

                        //Setting the character's starting hit dice
                        createdChar.GetComponent<Character>().charPhysState.GenerateStartingHitDice();

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


    //Function called from GenerateStartingCharacters to correctly initialize a character's item prefabs
    private void InitializeStartingItems(Inventory createdCharInv_, Inventory prefabCharInv_)
    {
        //Setting all of the equipped object references
        if (createdCharInv_.helm != null)
        {
            createdCharInv_.helm.GetComponent<Item>().itemPrefabRoot = prefabCharInv_.helm.gameObject;
        }
        if (createdCharInv_.chestPiece != null)
        {
            createdCharInv_.chestPiece.GetComponent<Item>().itemPrefabRoot = prefabCharInv_.chestPiece.gameObject;
        }
        if (createdCharInv_.leggings != null)
        {
            createdCharInv_.leggings.GetComponent<Item>().itemPrefabRoot = prefabCharInv_.leggings.gameObject;
        }
        if (createdCharInv_.gloves != null)
        {
            createdCharInv_.gloves.GetComponent<Item>().itemPrefabRoot = prefabCharInv_.gloves.gameObject;
        }
        if (createdCharInv_.shoes != null)
        {
            createdCharInv_.shoes.GetComponent<Item>().itemPrefabRoot = prefabCharInv_.shoes.gameObject;
        }
        if (createdCharInv_.cloak != null)
        {
            createdCharInv_.cloak.GetComponent<Item>().itemPrefabRoot = prefabCharInv_.cloak.gameObject;
        }
        if (createdCharInv_.necklace != null)
        {
            createdCharInv_.necklace.GetComponent<Item>().itemPrefabRoot = prefabCharInv_.necklace.gameObject;
        }
        if (createdCharInv_.ring != null)
        {
            createdCharInv_.ring.GetComponent<Item>().itemPrefabRoot = prefabCharInv_.ring.gameObject;
        }

        if (createdCharInv_.leftHand != null)
        {
            createdCharInv_.leftHand.GetComponent<Item>().itemPrefabRoot = prefabCharInv_.leftHand.gameObject;
        }
        if (createdCharInv_.rightHand != null)
        {
            createdCharInv_.rightHand.GetComponent<Item>().itemPrefabRoot = prefabCharInv_.rightHand.gameObject;
        }

        //Looping through all of the inventory item slots
        for(int i = 0; i < createdCharInv_.itemSlots.Count; ++i)
        {
            //If the current slot isn't empty
            if(createdCharInv_.itemSlots[i] != null)
            {
                //We set the item's prefab reference to the item in the same slot in the prefab character's inventory
                createdCharInv_.itemSlots[i].itemPrefabRoot = prefabCharInv_.itemSlots[i].gameObject;

                //If there is a stack of items in this slot, we loop through all of the stacked items
                if(createdCharInv_.itemSlots[i].currentStackSize > 1)
                {
                    for(int c = 0; c < createdCharInv_.transform.childCount; ++c)
                    {
                        createdCharInv_.transform.GetChild(c).GetComponent<Item>().itemPrefabRoot = prefabCharInv_.itemSlots[i].gameObject;
                    }
                }
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
                this.newPartyCharacters[c].charSkills.LevelUpSkill(SkillList.Unarmed, this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Unarmed]);
                this.newPartyCharacters[c].charSkills.LevelUpSkill(SkillList.Daggers, this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Daggers]);
                this.newPartyCharacters[c].charSkills.LevelUpSkill(SkillList.Swords, this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Swords]);
                this.newPartyCharacters[c].charSkills.LevelUpSkill(SkillList.Mauls, this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Mauls]);
                this.newPartyCharacters[c].charSkills.LevelUpSkill(SkillList.Poles, this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Poles]);
                this.newPartyCharacters[c].charSkills.LevelUpSkill(SkillList.Bows, this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Bows]);
                this.newPartyCharacters[c].charSkills.LevelUpSkill(SkillList.Shields, this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Shields]);

                this.newPartyCharacters[c].charSkills.LevelUpSkill(SkillList.ArcaneMagic, this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.ArcaneMagic]);
                this.newPartyCharacters[c].charSkills.LevelUpSkill(SkillList.HolyMagic, this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.HolyMagic]);
                this.newPartyCharacters[c].charSkills.LevelUpSkill(SkillList.DarkMagic, this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.DarkMagic]);
                this.newPartyCharacters[c].charSkills.LevelUpSkill(SkillList.FireMagic, this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.FireMagic]);
                this.newPartyCharacters[c].charSkills.LevelUpSkill(SkillList.WaterMagic, this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.WaterMagic]);
                this.newPartyCharacters[c].charSkills.LevelUpSkill(SkillList.WindMagic, this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.WindMagic]);
                this.newPartyCharacters[c].charSkills.LevelUpSkill(SkillList.ElectricMagic, this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.ElectricMagic]);
                this.newPartyCharacters[c].charSkills.LevelUpSkill(SkillList.StoneMagic, this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.StoneMagic]);

                this.newPartyCharacters[c].charSkills.LevelUpSkill(SkillList.Survivalist, this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Survivalist]);
                this.newPartyCharacters[c].charSkills.LevelUpSkill(SkillList.Social, this.allCharacterCustomizers[c].allocatedSkillPoints[SkillList.Social]);
            }


            //Parenting the character object to the global data object so we can transfer it over to the gameplay scene
            this.newPartyCharacters[c].transform.SetParent(GameData.globalReference.transform);
        }
    }
}

