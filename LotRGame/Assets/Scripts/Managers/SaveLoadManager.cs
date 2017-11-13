using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

        Debug.Log(Application.dataPath);
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
            //If the player-given name contains any of the illegal characters, we set the folder name to the default
            checkedName = this.defaultFolderName;
        }
        //Checking if the folder name is empty
        else if(checkedName == "")
        {
            //If the folder has no name, we give it the default
            checkedName = this.defaultFolderName;
        }



        return checkedName;
    }


    //Function called from SaveGameData to make sure the save folder exists
    private void CheckSaveDirectory(string folderName_)
    {
        //If the directory doesn't exist
        if(!Directory.Exists(Application.dataPath + folderName_))
        {
            Debug.Log("SaveLoadManager.CheckSaveDirectory. Directory DOES exist");
            //We create the folder at the application directory with the given folder name
            Directory.CreateDirectory(Application.dataPath + folderName_);
        }
        else
        {
            Debug.Log("SaveLoadManager.CheckSaveDirectory. Directory does NOT exist");
        }
    }


    //Function called from CreateTileGrid.StartMapCreation to save the map info when a new game is started
    public void SaveTileGrid(string folderName_)
    {
        //Making sure the save folder exists
        this.CheckSaveDirectory(folderName_);

        //Creating a new TileGridSaveInfo class to store all of the data that will be written
        TileGridSaveInfo newMapSave = new TileGridSaveInfo(CreateTileGrid.globalReference.tileGrid,
                                                           CreateTileGrid.globalReference.cityTiles,
                                                           CreateTileGrid.globalReference.dungeonTiles);

        //Creating a string to store the serialized JSON information for the new map save
        string jsonMapData = JsonUtility.ToJson(newMapSave);

        //Writing the JSON map data to a new text file in the given folder's directory
        File.WriteAllText(Application.dataPath + folderName_ + "/TileGrid.txt", jsonMapData);
    }
}