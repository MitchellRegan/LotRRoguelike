using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in CreateTileGrid.cs to hold info for each tile in the map's tile grid
public class TileInfo
{
    //A list of all of the tiles that are connected to this point
    public List<TileInfo> connectedTiles = new List<TileInfo>(6);

    //Reference in pathfinding algorithms to the tile that lead to this one. Used in CreateTileGrid algorithms
    [HideInInspector]
    public TileInfo previousTile;

    //Bool used in the pathfinding algorithms in CreateTileGrid.cs. If true, it will be ignored during the search.
    [HideInInspector]
    public bool hasBeenChecked = false;

    //Bool used in the pathfinding algorithms in CreateTileGrid.cs. Represents the number of turns it takes to traverse this point
    [HideInInspector]
    public int movementCost = 1;
    //The current number of cycles in the pathfinding algorithms that have been spent on this point.
    [HideInInspector]
    public int currentMovement = 0;

    //The name of the region that this tile is in
    public string regionName = "";

    //The position of this tile in world space
    public Vector3 tilePosition = new Vector3();

    //The environment type for the tile
    public LandType type = LandType.Empty;

    //The material used for this tile hex's mesh
    public Material tileMaterial;

    //The elevation of this tile
    public float elevation = 0;

    //The game object that's used to decorate the top of this tile
    public GameObject decorationModel;
    //The rotation of the decoration model (for variation)
    public float decorationRotation = 0;

    //The list of all game objects on this tile
    public List<GameObject> objectsOnThisTile;

    //The encounter chance for this tile
    private float randomEncounterChance = 0;
    //The list of encounters that can happen on this tile
    private List<EncounterBlock> randomEncounterList;



    //Empty constructor for this class
    public TileInfo()
    {
        //Setting this tile's type as "Empty"
        this.type = LandType.Empty;
        //Initializing the (now empty) list of objects on this tile
        this.objectsOnThisTile = new List<GameObject>();
        this.connectedTiles = new List<TileInfo>();
        for (int c = 0; c < 6; ++c)
        {
            this.connectedTiles.Add(null);
        }
    }


    //Constructor for this class using RegionInfo.cs
    public TileInfo(RegionInfo thisTilesRegion_)
    {
        this.SetTileBasedOnRegion(thisTilesRegion_);
        this.connectedTiles = new List<TileInfo>();
        for(int c = 0; c < 6; ++c)
        {
            this.connectedTiles.Add(null);
        }
    }


    //Function called externally and from the second constructor to set this tile's info based on a RegionInfo class
    public void SetTileBasedOnRegion(RegionInfo thisTilesRegion_)
    {
        //Setting the tile's name, type, and texture based on the region
        this.regionName = thisTilesRegion_.regionName;
        this.type = thisTilesRegion_.environmentType;
        this.tileMaterial = thisTilesRegion_.tileMaterial;

        //Setting this tile's elevation between the height min/max
        this.elevation = Random.Range(thisTilesRegion_.heightMinMax.x, thisTilesRegion_.heightMinMax.y);
        //Setting the movement cost between the region's min/max
        this.movementCost = Mathf.RoundToInt(Random.Range(thisTilesRegion_.movementCostMinMax.x, thisTilesRegion_.movementCostMinMax.y));

        //Setting the decoration for this tile as long as there's a model to choose from
        if (thisTilesRegion_.tileDecorations.Count > 0)
        {
            this.decorationModel = thisTilesRegion_.tileDecorations[Mathf.RoundToInt(Random.Range(0, thisTilesRegion_.tileDecorations.Count - 1))];
        }
        //Setting the decoration's rotation in degrees
        this.decorationRotation = Random.Range(0, 360);

        //Initializing the (now empty) list of objects on this tile
        this.objectsOnThisTile = new List<GameObject>();

        //Setting the random encounter chance and encounters
        this.randomEncounterChance = thisTilesRegion_.randomEncounterChance;
        this.randomEncounterList = thisTilesRegion_.randomEncounterList;
    }


    //Function called externally through PathfindingAlgorithms.cs in StepOutRegionEdge. Sets this tile's info based on another tile
    public void SetTileBasedOnAnotherTile(TileInfo otherTile_)
    {
        //Setting the tile's name, type, and texture to be the same as the other tile
        this.regionName = otherTile_.regionName;
        this.type = otherTile_.type;
        this.tileMaterial = otherTile_.tileMaterial;

        //Setting this tile's elevation between the its current height and the other tile's height
        this.elevation = (Random.value * (this.elevation - otherTile_.elevation)) + otherTile_.elevation;
        //Setting the tile's movement cost between its current cost and the other tile's cost
        this.movementCost = Mathf.RoundToInt(Random.value * (this.movementCost - otherTile_.movementCost)) + otherTile_.movementCost;
    }


    //Function called in the pathfinding algorithms in CreateTileGrid.cs. Clears this tile's previous point and the fact that it's been checked
    public void ClearPathfinding()
    {
        this.previousTile = null;
        this.hasBeenChecked = false;
        this.currentMovement = 0;
    }


    //Function called from Movement.SetCurrentTile to indicate that an object is occupying this tile
    public void AddObjectToThisTile(GameObject objectToAdd_)
    {
        //Making sure we aren't adding a character more than once
        if (!this.objectsOnThisTile.Contains(objectToAdd_))
        {
            //If an enemy encounter is added to this tile
            if (objectToAdd_.GetComponent<EnemyEncounter>())
            {
                //Looping through all of the objects on this tile to see if a player party is on it
                foreach (GameObject currentObj in this.objectsOnThisTile)
                {
                    //If the current object is a player party
                    if (currentObj.GetComponent<PartyGroup>())
                    {
                        //Initiating combat with the first group of characters found.
                        //NOTE: Even if multiple parties are on the same tile, they're still considered as separated, so only 1 group at a time
                        PartyGroup playerGroupOnTile = currentObj.GetComponent<PartyGroup>();
                        EnemyEncounter newEncounter = objectToAdd_.GetComponent<EnemyEncounter>();
                        CombatManager.globalReference.InitiateCombat(this.type, playerGroupOnTile, newEncounter);

                        //After combat is initiated, the enemy encounter is destroyed before it is added to this tile
                        GameObject.Destroy(objectToAdd_);
                        //Breaking out of the function before multiple combats start at once
                        return;
                    }
                }
                
                //If there weren't any party groups on this tile, the enemy encounter is added
                this.objectsOnThisTile.Add(objectToAdd_);
            }
            //If a Party Group is added to this tile
            else if (objectToAdd_.GetComponent<PartyGroup>())
            {
                //The player group is added
                this.objectsOnThisTile.Add(objectToAdd_);
                
                //Looping through all of the objects on this tile to see if an enemy encounter is on it
                foreach (GameObject currentObj in this.objectsOnThisTile)
                {
                    //If the current object is an enemy encounter
                    if (currentObj.GetComponent<EnemyEncounter>())
                    {
                        //Initiating combat with the first group of characters found.
                        //NOTE: Even if multiple parties are on the same tile, they're still considered as separated, so only 1 group at a time
                        PartyGroup playerGroupOnTile = objectToAdd_.GetComponent<PartyGroup>();
                        EnemyEncounter newEncounter = currentObj.GetComponent<EnemyEncounter>();
                        CombatManager.globalReference.InitiateCombat(this.type, playerGroupOnTile, newEncounter);

                        //After combat is initiated, the enemy encounter is destroyed and we break out of the loop before multiple combats start at once
                        this.objectsOnThisTile.Remove(currentObj);
                        GameObject.Destroy(currentObj);
                        return;
                    }
                }
                
                //If we made it this far, there wasn't an enemy encounter on the tile, so we need to check for an encounter
                this.RollForRandomEncounter();
            }
        }
    }


    //Function called from Movement.SetCurrentTile to indicate that an object is no longer occupying this tile
    public void RemoveObjectFromThisTile(GameObject objectToRemove_)
    {
        //Making sure we aren't removing a character that's not on this tile
        if (this.objectsOnThisTile.Contains(objectToRemove_))
        {
            this.objectsOnThisTile.Remove(objectToRemove_);
        }
    }


    //Function called from AddObjectToThisTile to see if we should start a random encounter
    public void RollForRandomEncounter()
    {
        //If we have no random encounters, this function does nothing
        if(this.randomEncounterList.Count < 1)
        {
            return;
        }

        //Rolling to see if we meet the encounter chance
        float encounterRoll = Random.Range(0, 1);
        if (encounterRoll < this.randomEncounterChance)
        {
            //Rolling to see which encounter is spawned
            float whichEncounter = Random.Range(0, 1);

            //Looping through each encounter
            for(int e = 0; e < this.randomEncounterList.Count; ++e)
            {
                //If we find one that has a greater spawn chance than our roll
                if (this.randomEncounterList[e].encounterChance >= whichEncounter)
                {
                    //Looping through and finding the object on this tile that has the player party
                    PartyGroup playerParty = null;
                    foreach(GameObject o in this.objectsOnThisTile)
                    {
                        if(o.GetComponent<PartyGroup>())
                        {
                            playerParty = o.GetComponent<PartyGroup>();
                            break;
                        }
                    }

                    //If we couldn't find the player party object for some reason, we stop the combat from happening
                    if(playerParty == null)
                    {
                        return;
                    }
                    
                    //Making sure there's not an encounter before the game even begins
                    if(TimePanelUI.globalReference == null || TimePanelUI.globalReference.daysTaken < 1)
                    {
                        return;
                    }

                    //We tell the combat manager to initiate combat with this encounter
                    CombatManager.globalReference.InitiateCombat(this.type, playerParty, this.randomEncounterList[e].encounterEnemies);
                    //Now we exit out of this function so we don't try to keep spawning more encounters
                    return;
                }
            }
        }
    }
}


//Enum used in TileInfo.cs to denote the type of environment it's on
public enum LandType
{
    Empty,
    Ocean,
    River,
    Swamp,
    Grasslands,
    Forest,
    Desert,
    Mountain,
    Volcano
}
