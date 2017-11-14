using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class SaveLoadManager : MonoBehaviour
{
    //Static reference to this component 
    [HideInInspector]
    public static SaveLoadManager globalReference;

    //The default name given to folders with invalid directory names
    public string defaultFolderName = "New Game";



    //Function called when this object is created
    private void Awake()
    {
        //If the global reference doesn't exist
        if(globalReference == null)
        {
            //This component is set as the global reference
            globalReference = this;
        }
        //If the global reference does exist
        else
        {
            //We remove this component so there aren't multiple conflicting SaveLoadManagers
            Destroy(this);
        }
    }


    //Function called from SaveGameData to check for illegal characters in the save folder name
    public string CheckFolderName(string folderName_)
    {
        //String to hold the folder name while we check it
        string checkedName = folderName_;

        //Checking the string for each illegal character
        if(checkedName.Contains("<") || checkedName.Contains(">") ||
            checkedName.Contains(":") || checkedName.Contains("\"") ||
            checkedName.Contains("\\") || checkedName.Contains("/") ||
            checkedName.Contains("|") || checkedName.Contains("?") ||
            checkedName.Contains("*"))
        {
            Debug.Log("CheckFolderName. Given name is illegal");
            //If the player-given name contains any of the illegal characters, we set the folder name to the default
            checkedName = this.defaultFolderName;
        }
        //Checking if the folder name is empty
        else if(checkedName == "")
        {
            Debug.Log("CheckFolderName. Given name is empty");
            //If the folder has no name, we give it the default
            checkedName = this.defaultFolderName;
        }

        //adding a save folder path to the beginning of the given folder name
        checkedName = "/Zein/SaveFiles/" + checkedName;

        return checkedName;
    }


    //Function called from SaveGameData to make sure the save folder exists
    private void CheckSaveDirectory(string folderName_)
    {
        //If the directory doesn't exist
        if(!Directory.Exists(Application.persistentDataPath + folderName_))
        {
            //We create the folder at the application directory with the given folder name
            Directory.CreateDirectory(Application.persistentDataPath + folderName_);
        }
    }


    //Function called from CreateTileGrid.StartMapCreation to save the map info when a new game is started
    public void SaveTileGrid(string folderName_)
    {
        //Making sure the save folder exists
        this.CheckSaveDirectory(folderName_);

        //Creating a 2D list of strings to hold all of the serialized TileInfo classes in the tile grid
        List<List<string>> serializedTileGrid = new List<List<string>>();
        for(int col = 0; col < CreateTileGrid.globalReference.tileGrid.Count; ++col)
        {
            //Creating a new column (list of tile strings)
            List<string> newCol = new List<string>();

            for(int row = 0; row < CreateTileGrid.globalReference.tileGrid[0].Count; ++row)
            {
                //Serializing the current tile using JsonUtility
                string jsonTile = JsonUtility.ToJson(CreateTileGrid.globalReference.tileGrid[col][row]);
                //Adding the tile string to the list of serialized tiles
                newCol.Add(jsonTile);
            }

            //Adding the new column to our 2D list of tile strings
            serializedTileGrid.Add(newCol);
        }

        //Creating a list of strings to hold serialized TileInfo classes for city tiles
        List<string> serializedCities = new List<string>();
        for(int c = 0; c < CreateTileGrid.globalReference.cityTiles.Count; ++c)
        {
            //Serializing the current city tile using JsonUtility
            string jsonCityTile = JsonUtility.ToJson(CreateTileGrid.globalReference.cityTiles[c]);
            //Adding the city tile to the list of serialized cities
            serializedCities.Add(jsonCityTile);
        }

        //Creating a list of strings to hold serialized TileInfo classes for dungeon tiles
        List<string> serializedDungeons = new List<string>();
        for(int d = 0; d < CreateTileGrid.globalReference.dungeonTiles.Count; ++d)
        {
            //Serializing the current dungeon tile using JsonUtility
            string jsonDungeonTile = JsonUtility.ToJson(CreateTileGrid.globalReference.dungeonTiles[d]);
            //Adding the city tile to the list of serialized dungeons
            serializedDungeons.Add(jsonDungeonTile);
        }

        //Creating a new TileGridSaveInfo class to store all of the data that will be written
        TileGridSaveInfo newMapSave = new TileGridSaveInfo(serializedTileGrid, serializedCities, serializedDungeons);


        Debug.Log("Before save");
        //string jsonMapData = JsonUtility.ToJson(testTile, true);
        string jsonMapData = JsonConvert.SerializeObject(newMapSave, Formatting.None, new JsonSerializerSettings() {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
        Debug.Log("After save");
        //Creating a string to store the serialized JSON information for the new map save
        //string jsonMapData = JsonConvert.SerializeObject(CreateTileGrid.globalReference.cityTiles, Formatting.None, new JsonSerializerSettings() {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
        //jsonMapData = JsonUtility.ToJson(CreateTileGrid.globalReference.cityTiles[0]);

        //Writing the JSON map data to a new text file in the given folder's directory
        File.WriteAllText(Application.persistentDataPath + folderName_ + "/TileGrid.txt", jsonMapData);
        Debug.Log(Application.persistentDataPath + folderName_ + "/TileGrid.txt");
    }
}