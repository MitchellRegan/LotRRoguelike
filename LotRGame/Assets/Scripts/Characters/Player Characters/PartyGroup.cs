using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for the object on the overworld that the player moves
public class PartyGroup : MonoBehaviour
{
    //Static references to each of the party groups
    public static PartyGroup group1;

    //The list of all characters currently in this party
    [HideInInspector]
    public List<Character> charactersInParty;

    //Public enum that determines what distance this party prefers to engage enemies at in combat
    public CombatManager.GroupCombatDistance combatDistance = CombatManager.GroupCombatDistance.Medium;



    //Function called when this object is created
    private void Awake()
    {
        //Initializing the character in party dictionary
        this.charactersInParty = new List<Character>();

        //Setting the static reference for this group
        if (PartyGroup.group1 != null)
        {
            Destroy(this);
        }
        else
        {
            PartyGroup.group1 = this;
        }

        Debug.Log("PartyGroup AWAKE --------------------------------------------------------------------------");
        //If the character manager has already been initialized, we set this party group as the selected group
        if(CharacterManager.globalReference != null)
        {
            CharacterManager.globalReference.selectedGroup = this;
        }
    }


	//Function called externally to add a character to this party group
    public bool AddCharacterToGroup(Character charToAdd_)
    {
        //If the character isn't in the party, we make sure the player doesn't already have a full group of characters
        if (!this.charactersInParty.Contains(charToAdd_) && CharacterManager.globalReference.FindEmptyPartySlots() > 0)
        {
            //Adding the character to the CharacterManager's player party
            CharacterManager.globalReference.AddCharacterToParty(charToAdd_);
            //Adding the character to our group
            this.charactersInParty.Add(charToAdd_);
            //Parenting the character to this group's transform
            charToAdd_.transform.SetParent(this.transform);
            charToAdd_.transform.position = new Vector3(0, 0, 0);
            //Setting a starting combat position for this character
            this.SetDefaultCombatPosition();
            //Checking the character's combat position so that it doesn't overlap with someone else
            this.CheckCombatPositionForCharacter(charToAdd_);
            return true;
        }
        
        //If none of the other parameters were met, the character couldn't be added
        return false;
    }


    //Function called from AddCharacterToGroup to set default starting positions for new characters
    private void SetDefaultCombatPosition()
    {
        //Setting a different default position based on how many characters are already in the group
        switch(this.charactersInParty.Count)
        {
            case 1:
                this.charactersInParty[0].charCombatStats.startingPositionCol = 1;
                this.charactersInParty[0].charCombatStats.startingPositionRow = 2;
                break;

            case 2:
                this.charactersInParty[1].charCombatStats.startingPositionCol = 1;
                this.charactersInParty[1].charCombatStats.startingPositionRow = 5;
                break;

            case 3:
                this.charactersInParty[2].charCombatStats.startingPositionCol = 2;
                this.charactersInParty[2].charCombatStats.startingPositionRow = 1;
                break;

            case 4:
                this.charactersInParty[3].charCombatStats.startingPositionCol = 2;
                this.charactersInParty[3].charCombatStats.startingPositionRow = 6;
                break;

            case 5:
                this.charactersInParty[4].charCombatStats.startingPositionCol = 0;
                this.charactersInParty[4].charCombatStats.startingPositionRow = 1;
                break;

            case 6:
                this.charactersInParty[5].charCombatStats.startingPositionCol = 0;
                this.charactersInParty[5].charCombatStats.startingPositionRow = 6;
                break;

            default:
                //????
                break;
        }
    }


    //Making sure two characters in the group aren't taking up the same spot in combat
    private void CheckCombatPositionForCharacter(Character charToCheck_)
    {
        //Looping through each character in this party
        foreach (Character charInGroup in this.charactersInParty)
        {
            //Making sure we aren't checking against the same character, because that will ALWAYS cause a problem
            if (charInGroup != charToCheck_)
            {
                //If someone else is already occupying the same spot as the character we're checking
                if (charToCheck_.charCombatStats.gridPositionCol == charInGroup.charCombatStats.gridPositionCol &&
                    charToCheck_.charCombatStats.gridPositionRow == charInGroup.charCombatStats.gridPositionRow)
                {
                    //The character we're checking finds an empty spot using the Set Combat Position function in Combat Stats
                    charToCheck_.charCombatStats.SetCombatPosition();
                }
            }
        }
    }
}

//Class used in PartyGroup.cs and SaveLoadManager.cs to save info for a player party
[System.Serializable]
public class PartySaveData
{
    //The combat distance that this party is at
    public CombatManager.GroupCombatDistance combatDist;
    //The list of save data for each character in this party
    public List<CharacterSaveData> partyCharacters;
    //The tile that this party is currently on
    public TileInfo tileLocation;


    //Constructor function for this class
    public PartySaveData(PartyGroup groupToSave_)
    {
        this.combatDist = groupToSave_.combatDistance;
        this.tileLocation = groupToSave_.GetComponent<WASDOverworldMovement>().currentTile;

        //Looping through all of the characters in the given party and getting their save data
        this.partyCharacters = new List<global::CharacterSaveData>();
        for (int c = 0; c < groupToSave_.charactersInParty.Count; ++c)
        {
            //If the current character isn't null, we save it's data
            if (groupToSave_.charactersInParty[c] != null)
            {
                CharacterSaveData charData = new CharacterSaveData(groupToSave_.charactersInParty[c]);
                this.partyCharacters.Add(charData);
            }
            //If the current character slot is null, we add the empty slot
            else
            {
                this.partyCharacters.Add(null);
            }
        }
    }
}