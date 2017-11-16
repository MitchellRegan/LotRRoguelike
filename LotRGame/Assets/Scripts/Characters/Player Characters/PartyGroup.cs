﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ReceiveEvent))]
public class PartyGroup : MonoBehaviour
{
    //Static references to each of the party groups
    public static PartyGroup group1;
    public static PartyGroup group2;
    public static PartyGroup group3;

    //The index for which group this object is
    [Range(1, 3)]
    public int groupIndex = 1;

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
        switch(this.groupIndex)
        {
            case 1:
                if(PartyGroup.group1 != null)
                {
                    Destroy(this);
                }
                else
                {
                    PartyGroup.group1 = this;
                }
                break;
            case 2:
                if (PartyGroup.group2 != null)
                {
                    Destroy(this);
                }
                else
                {
                    PartyGroup.group2 = this;
                }
                break;
            case 3:
                if (PartyGroup.group3 != null)
                {
                    Destroy(this);
                }
                else
                {
                    PartyGroup.group3 = this;
                }
                break;
        }
    }


	//Function called externally to add a character to this party group
    public bool AddCharacterToGroup(Character charToAdd_)
    {
        //If the character is already in the total player party
        if(CharacterManager.globalReference.playerParty.Contains(charToAdd_))
        {
            //Making sure the character isn't already in this group
            if (!this.charactersInParty.Contains(charToAdd_))
            {
                //Checking to see if the character is in group 1 (as long as this group isn't group 1)
                if(PartyGroup.group1 != this && PartyGroup.group1.charactersInParty.Contains(charToAdd_))
                {
                    //Making sure the group is on the same tile as we are
                    if(this.GetComponent<Movement>().currentTile == PartyGroup.group1.GetComponent<Movement>().currentTile)
                    {
                        //Adding the character to our group
                        this.charactersInParty.Add(charToAdd_);
                        //Removing the character from the other group
                        PartyGroup.group1.charactersInParty.Remove(charToAdd_);
                        //Parenting the character to this group's transform
                        charToAdd_.transform.SetParent(this.transform);
                    }
                }
                //Checking group 2
                else if(PartyGroup.group2 != this && PartyGroup.group2.charactersInParty.Contains(charToAdd_))
                {
                    //Making sure the group is on the same tile as we are
                    if (this.GetComponent<Movement>().currentTile == PartyGroup.group2.GetComponent<Movement>().currentTile)
                    {
                        //Adding the character to our group
                        this.charactersInParty.Add(charToAdd_);
                        //Removing the character from the other group
                        PartyGroup.group2.charactersInParty.Remove(charToAdd_);
                        //Parenting the character to this group's transform
                        charToAdd_.transform.SetParent(this.transform);
                    }
                }
                //Checking group 3
                else if(PartyGroup.group3 != this && PartyGroup.group3.charactersInParty.Contains(charToAdd_))
                {
                    //Making sure the group is on the same tile as we are
                    if (this.GetComponent<Movement>().currentTile == PartyGroup.group3.GetComponent<Movement>().currentTile)
                    {
                        //Adding the character to our group
                        this.charactersInParty.Add(charToAdd_);
                        //Removing the character from the other group
                        PartyGroup.group3.charactersInParty.Remove(charToAdd_);
                        //Parenting the character to this group's transform
                        charToAdd_.transform.SetParent(this.transform);
                    }
                }

                //Checking the character's combat position so that it doesn't overlap with someone else
                this.CheckCombatPositionForCharacter(charToAdd_);
                
                return true;
            }
        }
        //If the character isn't in the party, we make sure the player doesn't already have a full group of characters
        else if (CharacterManager.globalReference.FindEmptyPartySlots() > 0)
        {
            //Adding the character to the CharacterManager's player party
            CharacterManager.globalReference.AddCharacterToParty(charToAdd_);
            //Adding the character to our group
            this.charactersInParty.Add(charToAdd_);
            //Parenting the character to this group's transform
            charToAdd_.transform.SetParent(this.transform);
            charToAdd_.transform.position = new Vector3(0, 0, 0);
            //Checking the character's combat position so that it doesn't overlap with someone else
            this.CheckCombatPositionForCharacter(charToAdd_);
            return true;
        }

        //If none of the other parameters were met, the character couldn't be added
        return false;
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
                if (charToCheck_.charCombatStats.gridPositionCol == charInGroup.charCombatStats.gridPositionCol && charToCheck_.charCombatStats.gridPositionRow == charInGroup.charCombatStats.gridPositionRow)
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
    //The index for which party group this is
    public int groupIndex;
    //The combat distance that this party is at
    public CombatManager.GroupCombatDistance combatDist;
    //The list of save data for each character in this party
    public List<CharacterSaveData> partyCharacters;
    //The tile that this party is currently on
    public TileInfo tileLocation;


    //Constructor function for this class
    public PartySaveData(PartyGroup groupToSave_)
    {
        this.groupIndex = groupToSave_.groupIndex;
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