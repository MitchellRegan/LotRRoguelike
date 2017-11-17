﻿using System.Collections;
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
            //If the player-given name contains any of the illegal characters, we set the folder name to the default
            checkedName = this.defaultFolderName;
        }
        //Checking if the folder name is empty
        else if(checkedName == "")
        {
            //If the folder has no name, we give it the default
            checkedName = this.defaultFolderName;
        }

        //adding a save folder path to the beginning of the given folder name
        checkedName = "/Zein/SaveFiles/" + checkedName;

        //If there's already a save folder at the current directory, we need to modify it a bit
        if(Directory.Exists(Application.persistentDataPath + checkedName))
        {
            //Looping until we have a numbered version of the checked name that doesn't exist
            int saveNum = 2;
            while(Directory.Exists(Application.persistentDataPath + checkedName + saveNum))
            {
                ++saveNum;
            }

            //Once we find a directory that doesn't exist, we put the number at the end of the checked name
            checkedName = checkedName + saveNum;
        }

        //Creating the directory for the new save
        this.CheckSaveDirectory(checkedName);

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


    //Function called externally to save player preferences
    public void SavePlayerPreferences()
    {
        //Saving audio settings from SoundManager.cs
        PlayerPrefs.SetFloat("MusicVolume", SoundManager.globalReference.musicVolume);
        PlayerPrefs.SetFloat("SoundEffectVolume", SoundManager.globalReference.soundEffectVolume);
        PlayerPrefs.SetFloat("DialogueVolume", SoundManager.globalReference.dialogueVolume);
        PlayerPrefs.SetFloat("GlobalVolume", SoundManager.globalReference.globalVolume);

        if (SoundManager.globalReference.muteMusic)
        {
            PlayerPrefs.SetInt("MusicMuted", 1);
        }
        else
        {
            PlayerPrefs.SetInt("MusicMuted", 0);
        }

        if (SoundManager.globalReference.muteSoundEffects)
        {
            PlayerPrefs.SetInt("SoundEffectMuted", 1);
        }
        else
        {
            PlayerPrefs.SetInt("SoundEffectMuted", 0);
        }

        if (SoundManager.globalReference.muteDialogue)
        {
            PlayerPrefs.SetInt("DialogueMuted", 1);
        }
        else
        {
            PlayerPrefs.SetInt("DialogueMuted", 0);
        }

        if (SoundManager.globalReference.muteAll)
        {
            PlayerPrefs.SetInt("MuteAll", 1);
        }
        else
        {
            PlayerPrefs.SetInt("MuteAll", 0);
        }

        int soundProfileInt = 0;
        switch(SoundManager.globalReference.currentSoundProfile)
        {
            case SoundProfile.ComputerSpeakers:
                soundProfileInt = 0;
                break;
            case SoundProfile.Headphones:
                soundProfileInt = 1;
                break;
            case SoundProfile.RoomSpeakers:
                soundProfileInt = 2;
                break;
        }
        PlayerPrefs.SetInt("CurrentSoundProfile", soundProfileInt);

        //Saving video settings (NOT IMPLEMENTED YET)
    }


    //Function called externally to load player preferences
    public void LoadPlayerPreferences()
    {
        //Loading audio settings and setting them in SoundManager.cs
        SoundManager.globalReference.musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        SoundManager.globalReference.soundEffectVolume = PlayerPrefs.GetFloat("SoundEffectVolume");
        SoundManager.globalReference.dialogueVolume = PlayerPrefs.GetFloat("DialogueVolume");
        SoundManager.globalReference.globalVolume = PlayerPrefs.GetFloat("GlobalVolume");

        if(PlayerPrefs.GetInt("MusicMuted") == 1)
        {
            SoundManager.globalReference.muteMusic = true;
        }
        else
        {
            SoundManager.globalReference.muteMusic = false;
        }

        if(PlayerPrefs.GetInt("SoundEffectMuted") == 1)
        {
            SoundManager.globalReference.muteSoundEffects = true;
        }
        else
        {
            SoundManager.globalReference.muteSoundEffects = false;
        }

        if(PlayerPrefs.GetInt("DialogueMuted") == 1)
        {
            SoundManager.globalReference.muteDialogue = true;
        }
        else
        {
            SoundManager.globalReference.muteDialogue = false;
        }

        if(PlayerPrefs.GetInt("MuteAll") == 1)
        {
            SoundManager.globalReference.muteAll = true;
        }
        else
        {
            SoundManager.globalReference.muteAll = false;
        }

        switch(PlayerPrefs.GetInt("CurrentSoundProfile"))
        {
            case 0:
                SoundManager.globalReference.currentSoundProfile = SoundProfile.ComputerSpeakers;
                break;
            case 1:
                SoundManager.globalReference.currentSoundProfile = SoundProfile.Headphones;
                break;
            case 2:
                SoundManager.globalReference.currentSoundProfile = SoundProfile.RoomSpeakers;
                break;
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

        //Serializing the newMapSave class that holds all of our tile info
        string jsonMapData = JsonConvert.SerializeObject(newMapSave, Formatting.None, new JsonSerializerSettings() {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});

        //Writing the JSON map data to a new text file in the given folder's directory
        File.WriteAllText(Application.persistentDataPath + folderName_ + "/TileGrid.txt", jsonMapData);
        Debug.Log(Application.persistentDataPath + folderName_ + "/TileGrid.txt");
    }


    //Function called from CreateTileGrid to load map info from a save directory
    public void LoadTileGrid(string folderName_)
    {
        //If the folder directory doesn't exist
        if(!Directory.Exists(Application.persistentDataPath + folderName_))
        {
            //We throw an exception because the folder that's supposed to hold the TileGrid.txt file doesn't exist
            throw new System.ArgumentException("SaveLoadManager.LoadTileGrid, The folder directory given does not exist!");
        }
        //If the folder exists but the file doesn't
        else if(!File.Exists(Application.persistentDataPath + folderName_ + "/TileGrid.txt"))
        {
            //We throw an exception because the file that we're supposed to load doesn't exist
            throw new System.ArgumentException("SaveLoadManager.LoadTileGrid, The TileGrid.txt file for this save does not exist!");
        }


        //Getting all of the string data from the TileGrid.txt file
        string fileData = File.ReadAllText(Application.persistentDataPath + folderName_ + "/TileGrid.txt");

        //Getting the de-serialized TileGridSaveInfo class from the file using the JSON.net converter
        TileGridSaveInfo tileSaveInfo = JsonConvert.DeserializeObject(fileData, typeof(TileGridSaveInfo)) as TileGridSaveInfo;

        //Getting the de-serialized 2D list of TileInfo classes for the TileGrid
        List<List<TileInfo>> loadedTileGrid = new List<List<TileInfo>>();
        //Looping through every column of tiles in the tile grid
        for(int c = 0; c < tileSaveInfo.serializedTileGrid.Count; ++c)
        {
            //Creating a new list of tiles to store the tiles in this column
            List<TileInfo> newTileColumn = new List<TileInfo>();

            //Looping through every row of the current column of tiles
            for(int r = 0; r < tileSaveInfo.serializedTileGrid[0].Count; ++r)
            {
                //De-serializing the current tile from the string info using the JsonUtility
                TileInfo newTile = JsonUtility.FromJson(tileSaveInfo.serializedTileGrid[c][r], typeof(TileInfo)) as TileInfo;
                newTileColumn.Add(newTile);
            }

            //Adding the new column of tiles to the tile grid
            loadedTileGrid.Add(newTileColumn);
        }

        //Looping through all of the tiles in the tile grid to set their connections
        for(int tc = 0; tc < loadedTileGrid.Count; ++tc)
        {
            for(int tr = 0; tr < loadedTileGrid[0].Count; ++tr)
            {
                //Initializing the list of connected tiles for the current tile
                loadedTileGrid[tc][tr].connectedTiles = new List<TileInfo>()
                {
                    null,null,null,null,null,null
                };

                //Looping through all of the tile connections for the given tile
                for(int coord = 0; coord < loadedTileGrid[tc][tr].connectedTileCoordinates.Count; ++coord)
                {
                    int col = loadedTileGrid[tc][tr].connectedTileCoordinates[coord].col;
                    int row = loadedTileGrid[tc][tr].connectedTileCoordinates[coord].row;
                    loadedTileGrid[tc][tr].connectedTiles[coord] = loadedTileGrid[col][row];
                }
            }
        }

        //Getting the de-serialized list of TileInfo classes for the city tiles
        List<TileInfo> loadedCityTiles = new List<TileInfo>();
        for(int ct = 0; ct < tileSaveInfo.serializedCityTiles.Count; ++ct)
        {
            //De-serializing the current tile from the string info using JsonUtility
            TileInfo cityTile = JsonUtility.FromJson(tileSaveInfo.serializedCityTiles[ct], typeof(TileInfo)) as TileInfo;
            loadedCityTiles.Add(cityTile);
        }

        //Getting the de-serialized list of TileInfo classes for the dungeon tiles
        List<TileInfo> loadedDungeonTiles = new List<TileInfo>();
        for(int dt = 0; dt < tileSaveInfo.serializedDungeonTiles.Count; ++dt)
        {
            //De-serializing the current tile from the string info using JsonUtility
            TileInfo dungeonTile = JsonUtility.FromJson(tileSaveInfo.serializedDungeonTiles[dt], typeof(TileInfo)) as TileInfo;
            loadedDungeonTiles.Add(dungeonTile);
        }

        //Setting the CreateTileGrid references to the loaded lists of tiles
        CreateTileGrid.globalReference.tileGrid = loadedTileGrid;
        CreateTileGrid.globalReference.cityTiles = loadedCityTiles;
        CreateTileGrid.globalReference.dungeonTiles = loadedDungeonTiles;
    }
    

    //Function called externally to save player progress. Called from the pause menu in GamePlay scene, and CreateTileGrid.cs in Start
    public void SavePlayerProgress()
    {
        //Making sure the save folder exists
        this.CheckSaveDirectory(GameData.globalReference.saveFolder);
        
        //Creating a new PlayerProgress class that we'll save
        PlayerProgress currentProgress = new PlayerProgress(GameData.globalReference, TimePanelUI.globalReference, CharacterManager.globalReference);
        //Serializing the current progress
        string jsonPlayerProgress = JsonUtility.ToJson(currentProgress, true);
        //Writing the JSON progress data to a new text file in the given folder's directory
        File.WriteAllText(Application.persistentDataPath + GameData.globalReference.saveFolder + "/PlayerProgress.txt", jsonPlayerProgress);
    }


    //Function called externally to load player progress. Called from CreateTileGrid.cs on Start
    public void LoadPlayerProgress(string folderName_)
    {
        //If the folder directory doesn't exist
        if (!Directory.Exists(Application.persistentDataPath + folderName_))
        {
            //We throw an exception because the folder that's supposed to hold the TileGrid.txt file doesn't exist
            throw new System.ArgumentException("SaveLoadManager.LoadPlayerProgress, The folder directory given does not exist!");
        }
        //If the folder exists but the file doesn't
        else if (!File.Exists(Application.persistentDataPath + folderName_ + "/PlayerProgress.txt"))
        {
            //We throw an exception because the file that we're supposed to load doesn't exist
            throw new System.ArgumentException("SaveLoadManager.LoadPlayerProgress, The PlayerProgress.txt file for this save does not exist!");
        }
        Debug.Log("LoadPlayerProgress 1");

        //Getting all of the string data from the TileGrid.txt file
        string fileData = File.ReadAllText(Application.persistentDataPath + folderName_ + "/PlayerProgress.txt");

        Debug.Log("LoadPlayerProgress 2");
        //De-serializing the player progress from the text file
        PlayerProgress loadedProgress = JsonUtility.FromJson(fileData, typeof(PlayerProgress)) as PlayerProgress;

        Debug.Log("LoadPlayerProgress 3");
        //Setting the GameData.cs variables
        GameData.globalReference.currentDifficulty = loadedProgress.difficulty;
        GameData.globalReference.allowNewUnlockables = loadedProgress.allowNewUnlockables;
        GameData.globalReference.saveFolder = loadedProgress.folderName;
        GameData.globalReference.GetComponent<RandomSeedGenerator>().seed = loadedProgress.seed;

        Debug.Log("LoadPlayerProgress 4");
        //Setting the TimePanelUI.cs variables
        TimePanelUI.globalReference.daysTaken = loadedProgress.daysTaken;
        TimePanelUI.globalReference.timeOfDay = loadedProgress.timeOfDay;

        Debug.Log("LoadPlayerProgress 5");
        //Setting the PartyGroup.cs static references
        if (loadedProgress.partyGroup1 != null)
        {
            Debug.Log("LoadPlayerProgress 5.1");
            //Creating a new PartyGroup instance
            GameObject newPartyObj = GameObject.Instantiate(CreateTileGrid.globalReference.partyGroup1Prefab);
            PartyGroup partyGroup1 = newPartyObj.GetComponent<PartyGroup>();

            Debug.Log("LoadPlayerProgress 5.2");
            //Setting the party variables
            partyGroup1.groupIndex = loadedProgress.partyGroup1.groupIndex;
            partyGroup1.combatDistance = loadedProgress.partyGroup1.combatDist;

            Debug.Log("LoadPlayerProgress 5.3");
            //Looping through all of the character save data for this party group
            partyGroup1.charactersInParty = new List<Character>();
            for(int c = 0; c < loadedProgress.partyGroup1.partyCharacters.Count; ++c)
            {
                //If the current character index isn't null, we make a new character instance
                if(loadedProgress.partyGroup1.partyCharacters[c] != null)
                {
                    //Creating a new character instance
                    GameObject newCharacterObj = GameObject.Instantiate(CreateTileGrid.globalReference.testCharacter);
                    Character newCharacter = newCharacterObj.GetComponent<Character>();

                    //Passing the character save data to the character instance to set all of the component variables
                    newCharacter.LoadCharacterFromSave(loadedProgress.partyGroup1.partyCharacters[c]);

                    //Adding the new character to our new party group
                    partyGroup1.AddCharacterToGroup(newCharacter);
                }
                //If the current character index is null, we leave the gap in the list
                else
                {
                    partyGroup1.charactersInParty.Add(null);
                }
            }

            Debug.Log("LoadPlayerProgress 5.31");
            TileInfo partyLocation = loadedProgress.partyGroup1.tileLocation;
            Debug.Log("LoadPlayerProgress 5.32");
            partyGroup1.GetComponent<WASDOverworldMovement>().SetCurrentTile(partyLocation);

            Debug.Log("LoadPlayerProgress 5.4");
            //Setting the static party group reference
            PartyGroup.group1 = partyGroup1;
        }
        if (loadedProgress.partyGroup2 != null)
        {
            //Creating a new PartyGroup instance
            GameObject newPartyObj = GameObject.Instantiate(CreateTileGrid.globalReference.partyGroup1Prefab);
            PartyGroup partyGroup2 = newPartyObj.GetComponent<PartyGroup>();

            //Setting the party variables
            partyGroup2.groupIndex = loadedProgress.partyGroup2.groupIndex;
            partyGroup2.combatDistance = loadedProgress.partyGroup2.combatDist;

            //Looping through all of the character save data for this party group
            partyGroup2.charactersInParty = new List<Character>();
            for (int c = 0; c < loadedProgress.partyGroup2.partyCharacters.Count; ++c)
            {
                //If the current character index isn't null, we make a new character instance
                if (loadedProgress.partyGroup2.partyCharacters[c] != null)
                {
                    //Creating a new character instance
                    GameObject newCharacterObj = GameObject.Instantiate(CreateTileGrid.globalReference.testCharacter);
                    Character newCharacter = newCharacterObj.GetComponent<Character>();

                    //Passing the character save data to the character instance to set all of the component variables
                    newCharacter.LoadCharacterFromSave(loadedProgress.partyGroup2.partyCharacters[c]);

                    //Adding the new character to our new party group
                    partyGroup2.AddCharacterToGroup(newCharacter);
                }
                //If the current character index is null, we leave the gap in the list
                else
                {
                    partyGroup2.charactersInParty.Add(null);
                }
            }

            TileInfo partyLocation = loadedProgress.partyGroup2.tileLocation;
            partyGroup2.GetComponent<WASDOverworldMovement>().SetCurrentTile(partyLocation);

            //Setting the static party group reference
            PartyGroup.group2 = partyGroup2;
        }
        if (loadedProgress.partyGroup3 != null)
        {
            //Creating a new PartyGroup instance
            GameObject newPartyObj = GameObject.Instantiate(CreateTileGrid.globalReference.partyGroup1Prefab);
            PartyGroup partyGroup3 = newPartyObj.GetComponent<PartyGroup>();

            //Setting the party variables
            partyGroup3.groupIndex = loadedProgress.partyGroup3.groupIndex;
            partyGroup3.combatDistance = loadedProgress.partyGroup3.combatDist;

            //Looping through all of the character save data for this party group
            partyGroup3.charactersInParty = new List<Character>();
            for (int c = 0; c < loadedProgress.partyGroup3.partyCharacters.Count; ++c)
            {
                //If the current character index isn't null, we make a new character instance
                if (loadedProgress.partyGroup3.partyCharacters[c] != null)
                {
                    //Creating a new character instance
                    GameObject newCharacterObj = GameObject.Instantiate(CreateTileGrid.globalReference.testCharacter);
                    Character newCharacter = newCharacterObj.GetComponent<Character>();

                    //Passing the character save data to the character instance to set all of the component variables
                    newCharacter.LoadCharacterFromSave(loadedProgress.partyGroup3.partyCharacters[c]);

                    //Adding the new character to our new party group
                    partyGroup3.AddCharacterToGroup(newCharacter);
                }
                //If the current character index is null, we leave the gap in the list
                else
                {
                    partyGroup3.charactersInParty.Add(null);
                }
            }

            TileInfo partyLocation = loadedProgress.partyGroup3.tileLocation;
            partyGroup3.GetComponent<WASDOverworldMovement>().SetCurrentTile(partyLocation);

            //Setting the static party group reference
            PartyGroup.group3 = partyGroup3;
        }

        Debug.Log("LoadPlayerProgress 6");
        //Setting the dead characters from CharacterManager.cs
        CharacterManager.globalReference.deadCharacters = loadedProgress.deadCharacters;
        Debug.Log("LoadPlayerProgress 7");
    }
}

//Class used in SaveLoadManager.SavePlayerProgress and LoadPlayerProgress
[System.Serializable]
public class PlayerProgress
{
    //Variables from GameData.cs
    public GameData.gameDifficulty difficulty = GameData.gameDifficulty.Normal;
    public bool allowNewUnlockables = true;
    public string folderName = "";
    public string seed = "";

    //Variables from TimePanelUI.cs
    public int daysTaken = 0;
    public int timeOfDay = 0;

    //Variables for the PartyGroup.cs
    public PartySaveData partyGroup1 = null;
    public PartySaveData partyGroup2 = null;
    public PartySaveData partyGroup3 = null;

    //Variable from CharacterManager.cs
    public List<DeadCharacterInfo> deadCharacters;


    //Constructor function for this class
    public PlayerProgress(GameData gameData_, TimePanelUI timePanel_, CharacterManager charManager_)
    {
        //Setting the GameData.cs variables
        this.difficulty = gameData_.currentDifficulty;
        this.allowNewUnlockables = gameData_.allowNewUnlockables;
        this.folderName = gameData_.saveFolder;
        this.seed = gameData_.GetComponent<RandomSeedGenerator>().seed;

        //Setting the TimePanelUI.cs variables
        this.daysTaken = timePanel_.daysTaken;
        this.timeOfDay = timePanel_.timeOfDay;

        //Setting the PartyGroup.cs variables
        if (PartyGroup.group1 != null)
        {
            this.partyGroup1 = new PartySaveData(PartyGroup.group1);
        }
        if(PartyGroup.group2 != null)
        {
            this.partyGroup2 = new PartySaveData(PartyGroup.group2);
        }
        if(PartyGroup.group3 != null)
        {
            this.partyGroup3 = new PartySaveData(PartyGroup.group3);
        }

        //Looping through all of the dead character info in CharacterManager.cs
        this.deadCharacters = new List<DeadCharacterInfo>();
        for(int d = 0; d < charManager_.deadCharacters.Count; ++d)
        {
            this.deadCharacters.Add(charManager_.deadCharacters[d]);
        }
    }
}