using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in CharacterManager.cs and SaveLoadManager.cs to store information about an enemy encounter in the overworld
[System.Serializable]
public class EnemyTileEncounterInfo
{
    //The prefab for the encounter object
    public GameObject encounterPrefab;
    //The tile that the encounter is on
    public int encounterTileCol;
    public int encounterTileRow;

    //Constructor function for this class
    public EnemyTileEncounterInfo(EnemyEncounter encounter_)
    {
        //Getting the object prefab for the encounter
        this.encounterPrefab = encounter_.encounterPrefab;
        //Getting the tile that this encounter is on
        TileColRow encounterCoords = CreateTileGrid.globalReference.GetTileCoords(encounter_.GetComponent<Movement>().currentTile);
        this.encounterTileCol = encounterCoords.col;
        this.encounterTileRow = encounterCoords.row;
    }
}