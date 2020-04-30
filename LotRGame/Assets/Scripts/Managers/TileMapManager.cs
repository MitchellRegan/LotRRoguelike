using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreateTileGrid))]
[RequireComponent(typeof(CreateMapRegions))]
[RequireComponent(typeof(PopulateMapLocations))]
[RequireComponent(typeof(DistortRegionBorders))]
[RequireComponent(typeof(CreateRoads))]
[RequireComponent(typeof(CreateMiniMap))]
public class TileMapManager : MonoBehaviour
{
    //Static reference to this manager
    public static TileMapManager globalReference;
    //2 dimensional array of tiles that's generated (cols, rows)
    public List<List<TileInfo>> tileGrid;

    //The number of columns of hexes this grid has
    public int cols = 6;
    //The number of rows of hexes there are in each column
    public int rows = 6;

    //The list of tiles where region cities are
    [HideInInspector]
    public List<TileInfo> cityTiles;
    //The list of tiles where region dungeons are
    [HideInInspector]
    public List<TileInfo> dungeonTiles;

    //The number of tiles that are visible at a given time
    [Range(2, 25)]
    public int visibilityRange = 12;

    //The list of all tiles that are currently visible
    public Dictionary<TileInfo, GameObject> visibleTileObjects;

    //Prefab for the group that the player characters are added to
    public GameObject partyGroupPrefab;

    //Empty character prefab to use while testing
    public GameObject testCharacter;
    public GameObject testCharacter2;

    //Enemy encounter for testing
    public GameObject testEnemyEncounter;



    //Called on initialization
    private void Awake()
    {
        //Setting the static reference
        if (globalReference == null)
        {
            globalReference = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        //Initializing our list of visible tile objects
        this.visibleTileObjects = new Dictionary<TileInfo, GameObject>();

        //If we're starting a new game, we need to generate the map first
        if (GameData.globalReference.loadType == LevelLoadType.GenerateNewLevel)
        {
            this.CreateNewTileMap();
        }
        //If we're loading a previous game we let the SaveLoadManager handle it
        else
        {
            //Loading the tile grid using the SaveLoadManager
            SaveLoadManager.globalReference.LoadTileGrid(GameData.globalReference.saveFolder);
        }
    }


    //Function called the first frame this object is alive
    private void Start()
    {
        //If we just created a new game, we need to save player progress immediately
        if (GameData.globalReference.loadType == LevelLoadType.GenerateNewLevel)
        {
            //Assigning the main quests to the character quest log
            QuestTracker.globalReference.AssignMainQuests();
            //Saving the current party progress
            SaveLoadManager.globalReference.SavePlayerProgress();
        }
        //If we just loaded a game save, we load our player progress
        else if (GameData.globalReference.loadType == LevelLoadType.LoadLevel)
        {
            //Loading the party progress
            SaveLoadManager.globalReference.LoadPlayerProgress(GameData.globalReference.saveFolder);

            //Setting the selected group
            CharacterManager.globalReference.selectedGroup = PartyGroup.group1;
        }
    }


    //Function called from Awake to create our tile map from scratch for a new game
    private void CreateNewTileMap()
    {
        //Have CreateTileGrid.cs make a blank grid and connect the tiles together
        this.GetComponent<CreateTileGrid>().GenerateHexGrid();

        //Have CreateMapRegions.cs divide the map into difficulty bands
        this.GetComponent<CreateMapRegions>().GenerateRegions();

        //Have PopulateMapLocations.cs fill in the map with points of interest
        this.GetComponent<PopulateMapLocations>().PopulateTileMap();

        //Have DistortRegionBorders.cs alter the map
        this.GetComponent<DistortRegionBorders>().DistortTileMap();

        //Have CreateRoads.cs connect the locations on the map with roads
        this.GetComponent<CreateRoads>().DrawMapRoads();

        this.SetPlayerPartyPosition();

        //Have CreateMiniMap.cs output the map.png image
        this.GetComponent<CreateMiniMap>().CreateMapTexture();

        //Saving this tile grid using the folder name in GameData.cs
        SaveLoadManager.globalReference.SaveTileGrid(GameData.globalReference.saveFolder);
    }


    //Function called from CreateNewTileMap to put the test player party on the start tile
    private void SetPlayerPartyPosition()
    {
        //Creating a variable to hold the starting location (initialized to 0,0 to avoid an error)
        TileInfo startTile = this.tileGrid[0][0];

        //Looping through to each region in the very easy regions to find a starting tile for the player group
        foreach (RegionInfo ver in this.GetComponent<CreateMapRegions>().veryEasy.regions)
        {
            //Looping through each city tile until we find one that matches this region
            foreach (TileInfo city in this.cityTiles)
            {
                //If we find a city that's in this very easy region
                if (city.regionName == ver.regionName)
                {
                    //We set the players to this region and break out of these loops
                    startTile = city;
                    break;
                }
            }
        }

        //Instantiating the player group at the starting tile's location
        GameObject playerParty1 = GameObject.Instantiate(this.partyGroupPrefab, startTile.tilePosition, new Quaternion());

        playerParty1.GetComponent<WASDOverworldMovement>().SetCurrentTile(startTile, false);

        //Looping through all of the children for the GameData object to get the created characters
        foreach (Character t in GameData.globalReference.transform.GetComponentsInChildren<Character>())
        {
            playerParty1.GetComponent<PartyGroup>().AddCharacterToGroup(t.GetComponent<Character>());
        }

        //If there are no characters that were added (either because of a glitch or just testing), we create the test characters
        if (playerParty1.GetComponent<PartyGroup>().charactersInParty.Count == 0)
        {
            //Instantiating the test characters at the starting tile's location
            GameObject startChar1 = GameObject.Instantiate(this.testCharacter, startTile.tilePosition, new Quaternion());
            GameObject startChar2 = GameObject.Instantiate(this.testCharacter2, startTile.tilePosition, new Quaternion());

            //Adding the starting characters to the party group
            playerParty1.GetComponent<PartyGroup>().AddCharacterToGroup(startChar1.GetComponent<Character>());
            playerParty1.GetComponent<PartyGroup>().AddCharacterToGroup(startChar2.GetComponent<Character>());
        }

        //Setting the character manager to be selecting the player party 1
        CharacterManager.globalReference.selectedGroup = playerParty1.GetComponent<PartyGroup>();

        //Creating the test enemy and adding them to a tile next to the start tile
        int connectedTileIndex = 0;
        for (int c = 0; c < startTile.connectedTiles.Count; ++c)
        {
            if (startTile.connectedTiles[c] != null)
            {
                connectedTileIndex = c;
                break;
            }
        }

        //Creating the enemy encounter
        if (this.testEnemyEncounter != null)
        {
            CharacterManager.globalReference.CreateEnemyEncounter(this.testEnemyEncounter.GetComponent<EnemyEncounter>(), 
                        startTile.connectedTiles[connectedTileIndex]);
        }

        int connectedTileIndex2 = connectedTileIndex;
        for (int c2 = connectedTileIndex + 1; c2 < startTile.connectedTiles.Count; ++c2)
        {
            if (startTile.connectedTiles[c2] != null)
            {
                connectedTileIndex2 = c2;
            }
        }

        //Creating the enemy encounter
        if (this.testEnemyEncounter != null)
        {
            CharacterManager.globalReference.CreateEnemyEncounter(this.testEnemyEncounter.GetComponent<EnemyEncounter>(), 
                        startTile.connectedTiles[connectedTileIndex2]);
        }

        int connectedTileIndex3 = connectedTileIndex2;
        for (int c3 = connectedTileIndex2 + 1; c3 < startTile.connectedTiles.Count; ++c3)
        {
            if (startTile.connectedTiles[c3] != null)
            {
                connectedTileIndex3 = c3;
            }
        }
        //Creating the enemy encounter
        if (this.testEnemyEncounter != null)
        {
            CharacterManager.globalReference.CreateEnemyEncounter(this.testEnemyEncounter.GetComponent<EnemyEncounter>(), 
                        startTile.connectedTiles[connectedTileIndex3]);
        }
    }


    //Function called externally to get the column and row for a given tile
    public TileColRow GetTileCoords(TileInfo tileToSearchFor_)
    {
        //The coordinates that we'll return
        TileColRow tileCoord = null;

        //Looping through all of the tiles connected to the tile we're searching for
        for (int ct = 0; ct < tileToSearchFor_.connectedTiles.Count; ++ct)
        {
            //If this current tile connection isn't empty and one of its tile connections is the tile we're searching for
            if (tileToSearchFor_.connectedTiles[ct] != null && tileToSearchFor_.connectedTiles[ct].connectedTiles.Contains(tileToSearchFor_))
            {
                //Getting the index of the tile we're searching for in the connected tile's list of connected tiles
                int ourTileIndex = tileToSearchFor_.connectedTiles[ct].connectedTiles.IndexOf(tileToSearchFor_);

                //Returning the tile coordinates using the index of the tile we're searching for
                GridCoordinates coords = tileToSearchFor_.connectedTiles[ct].connectedTileCoordinates[ourTileIndex];

                tileCoord = new TileColRow();
                tileCoord.col = coords.col;
                tileCoord.row = coords.row;
                return tileCoord;
            }
        }

        //Returning the empty tile coordinates
        Debug.LogError("TileMapManager.GetTileCoords >>> NULL RETURN");
        return tileCoord;
    }


    //Function called externally from Movement.cs and from StartMapCreation. Spawns the nearest tiles around the player party
    public void GenerateVisibleLand(TileInfo currentTile_)
    {
        //Getting all of the tiles within view range of the current tile
        List<TileInfo> tilesInRange = PathfindingAlgorithms.FindLandTilesInRange(currentTile_, this.visibilityRange);

        //Creating a list to hold all of the tiles that are just coming into view
        List<TileInfo> newTiles = new List<TileInfo>();
        //Creating a list to hold all of the tiles that are now outside our view
        List<TileInfo> oldTiles = new List<TileInfo>();

        //Looping through all of the tiles that are currently in view
        foreach (TileInfo inRangeTile in tilesInRange)
        {
            //If the tile in range isn't in the current dictionary of visible objects
            if (!this.visibleTileObjects.ContainsKey(inRangeTile))
            {
                //The tile is new
                newTiles.Add(inRangeTile);
            }
        }

        //Looping through all of the tiles that are currently visible
        foreach (TileInfo visibleTile in this.visibleTileObjects.Keys)
        {
            //If the visible tile isn't in range of the new tile
            if (!tilesInRange.Contains(visibleTile))
            {
                oldTiles.Add(visibleTile);
            }
        }

        //Looping through and removing each tile in the list of old tiles
        foreach (TileInfo oldTile in oldTiles)
        {
            //Deleting the game object for this tile in the visible tile objects dictionary
            Destroy(this.visibleTileObjects[oldTile]);
            //Removing this tile from the list
            this.visibleTileObjects.Remove(oldTile);
        }

        //Looping through all of the new tiles and creating an instance of its game object
        foreach (TileInfo newTile in newTiles)
        {
            //Resetting the tile to say it hasn't been checked
            newTile.hasBeenChecked = false;

            GameObject hexMesh = this.GetComponent<CreateTileGrid>().hexMesh.gameObject;

            //Creating an instance of the hex mesh for this tile
            GameObject tileMesh = Instantiate(hexMesh, new Vector3(newTile.tilePosition.x, newTile.elevation, newTile.tilePosition.z), new Quaternion());
            //Setting the mesh's material to the correct one for the tile
            Material[] tileMat = tileMesh.GetComponent<MeshRenderer>().materials;
            tileMat[0] = newTile.tileMaterial;
            tileMesh.GetComponent<MeshRenderer>().materials = tileMat;
            //Setting the tile's reference in the LandTile component
            tileMesh.GetComponent<LandTile>().tileReference = newTile;

            //Adding the hex mesh to our list of visible tile objects
            this.visibleTileObjects.Add(newTile, tileMesh);

            //If this tile has a decoration model, an instance of it is created and parented to this tile's mesh object
            if (newTile.decorationModel != null)
            {
                GameObject decor = Instantiate(newTile.decorationModel.gameObject, tileMesh.transform.position, new Quaternion());
                decor.transform.SetParent(tileMesh.transform);
                decor.transform.eulerAngles = new Vector3(0, newTile.decorationRotation, 0);
            }
        }
    }
}
