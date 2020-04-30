using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in SaveLoadManager.SavePlayerProgress and LoadPlayerProgress
[System.Serializable]
public class PlayerProgress
{
    //Variables from GameData.cs
    public GameDifficulty difficulty = GameDifficulty.Normal;
    public bool allowNewUnlockables = true;
    public string folderName = "";
    public Random.State randState;

    //Variables from CreateTileGrid.cs
    public int gridCols = 0;
    public int gridRows = 0;

    //Variables from TimePanelUI.cs
    public int daysTaken = 0;
    public int timeOfDay = 0;

    //Variables from LevelUpManager.cs
    public int characterLevel = 0;

    //Variables for the PartyGroup.cs
    public PartySaveData partyGroup1 = null;

    //Variable from CharacterManager.cs
    public List<DeadCharacterInfo> deadCharacters;
    public List<EnemyTileEncounterInfo> enemyTileEncounters;

    //Variables from QuestTracker.cs
    public List<string> questLog;
    public List<string> finishedQuests;



    //Constructor function for this class
    public PlayerProgress(GameData gameData_, TileMapManager tileGrid_, TimePanelUI timePanel_, LevelUpManager levelUpManager_, CharacterManager charManager_, QuestTracker questTracker_)
    {
        //Setting the GameData.cs variables
        this.difficulty = gameData_.currentDifficulty;
        this.allowNewUnlockables = gameData_.allowNewUnlockables;
        this.folderName = gameData_.saveFolder;
        this.randState = Random.state;

        //Setting the CreateTileGrid.cs variables
        this.gridCols = tileGrid_.cols;
        this.gridRows = tileGrid_.rows;

        //Setting the TimePanelUI.cs variables
        this.daysTaken = timePanel_.daysTaken;
        this.timeOfDay = timePanel_.timeOfDay;

        //Setting the LevelUpManager variable
        this.characterLevel = levelUpManager_.characterLevel;

        //Setting the PartyGroup.cs variables
        this.partyGroup1 = new PartySaveData(PartyGroup.group1);

        //Looping through all of the dead character info in CharacterManager.cs
        this.deadCharacters = new List<DeadCharacterInfo>();
        for (int d = 0; d < charManager_.deadCharacters.Count; ++d)
        {
            this.deadCharacters.Add(charManager_.deadCharacters[d]);
        }

        //Looping through all of the enemy tile encounters in CharacterManager.cs
        this.enemyTileEncounters = new List<EnemyTileEncounterInfo>();
        for (int e = 0; e < CharacterManager.globalReference.tileEnemyEncounters.Count; ++e)
        {
            //Making sure the encounter isn't null first
            if (CharacterManager.globalReference.tileEnemyEncounters[e] != null)
            {
                //Creating a new tile encounter info for the enemy
                EnemyTileEncounterInfo enemyInfo = new EnemyTileEncounterInfo(CharacterManager.globalReference.tileEnemyEncounters[e]);
                //Adding the enemy encounter info to our list to serialize
                this.enemyTileEncounters.Add(enemyInfo);
            }
        }

        //Looping through all of the quests in our quest log
        this.questLog = new List<string>();
        foreach (Quest q in questTracker_.questLog)
        {
            this.questLog.Add(JsonUtility.ToJson(new QuestSaveData(q), true));
        }

        //Saving all of the finished quest names
        this.finishedQuests = questTracker_.completedQuestNames;
    }
}