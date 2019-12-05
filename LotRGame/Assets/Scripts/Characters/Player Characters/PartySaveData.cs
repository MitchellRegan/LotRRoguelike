using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in PartyGroup.cs and SaveLoadManager.cs to save info for a player party
[System.Serializable]
public class PartySaveData
{
    //The combat distance that this party is at
    public GroupCombatDistance combatDist;
    //The list of save data for each character in this party
    public List<CharacterSaveData> partyCharacters;
    //The tile that this party is currently on
    public int tileCol = -1;
    public int tileRow = -1;


    //Constructor function for this class
    public PartySaveData(PartyGroup groupToSave_)
    {
        this.combatDist = groupToSave_.combatDistance;

        TileColRow tileLocation = CreateTileGrid.globalReference.GetTileCoords(groupToSave_.GetComponent<WASDOverworldMovement>().currentTile);
        this.tileCol = tileLocation.col;
        this.tileRow = tileLocation.row;

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