using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RandomSeedGenerator))]
[RequireComponent(typeof(GoToLevel))]
public class GameData : MonoBehaviour
{
    //A static reference to this object so that we can reference it from anywhere
    public static GameData globalReference;

    //Public enum that lets us know what difficulty the player has set the game to
    public GameDifficulty currentDifficulty = GameDifficulty.Normal;

    //Public enum that lets us know what race the player is starting as in a new game
    public Races startingRace = Races.Human;

    //The width and height of the game map in tiles for the different difficulty settings
    public Vector2 easyMapSize = new Vector2();
    public Vector2 normalMapSize = new Vector2();
    public Vector2 hardMapSize = new Vector2();

    //The scene that we transition to when we start a new game
    public string gameplayLevelName;

    //Enum that CreateTileGrid.cs uses on awake to figure out if it should be loading a map or creating a new one
    [HideInInspector]
    public LevelLoadType loadType = LevelLoadType.GenerateNewLevel;

    //Bool that determines if new unlockables can be spawned in a game based on if the player input a string
    [HideInInspector]
    public bool allowNewUnlockables = true;

    //The name of the current game folder where our save directory is
    // Current path: C:/Users/Me/AppData/LocalLow/DefaultCompany/LotRGame/SaveFiles
    [HideInInspector]
    public string saveFolder = "";



    //Function called when this object is initialized
    private void Awake()
    {
        //Makes sure this object persists through scene changes
        DontDestroyOnLoad(transform.gameObject);

        //Making sure that we don't have more than one Game Data component active at a time
        if(globalReference != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            globalReference = this;
        }
    }


    //Function called the first frame that this object is alive
    private void Start()
    {
        //Loading in the player preferences
        SaveLoadManager.globalReference.LoadPlayerPreferences();
    }


    //Function called externally to quit the game application
    public void QuitGame()
    {
        Application.Quit();
    }


    //Function called from the New Game screen in the main menu. Sets the player's starting race
    public void SetStartingRace(int startingRace_)
    {
        switch(startingRace_)
        {
            case 0:
                this.startingRace = Races.Human;
                break;

            case 1:
                this.startingRace = Races.Elf;
                break;

            case 2:
                this.startingRace = Races.Dwarf;
                break;

            case 3:
                this.startingRace = Races.HalfMan;
                break;

            case 4:
                this.startingRace = Races.Amazon;
                break;

            case 5:
                this.startingRace = Races.Orc;
                break;

            case 6:
                this.startingRace = Races.GillFolk;
                break;

            case 7:
                this.startingRace = Races.ScaleSkin;
                break;

            case 8:
                this.startingRace = Races.Minotaur;
                break;

            default:
                this.startingRace = Races.Human;
                break;
        }
    }


    //Function called from the New Game screen in the main menu. Sets the difficulty for the new game
    public void SetGameDifficulty(int difficulty_)
    {
        switch(difficulty_)
        {
            case 0:
                this.currentDifficulty = GameDifficulty.Easy;
                break;

            case 1:
                this.currentDifficulty = GameDifficulty.Normal;
                break;

            case 2:
                this.currentDifficulty = GameDifficulty.Hard;
                break;

            default:
                this.currentDifficulty = GameDifficulty.Normal;
                break;
        }
    }


    //Function called externally to set the name of the save folder
    public void SetSaveFolderName(string saveFolder_)
    {
        this.saveFolder = saveFolder_;
    }


    //Function called from the New Game screen in the main menu. Starts the process of creating a new map
    public void StartNewGame()
    {
        //Determining if new items will be available in this game based on the seed value
        if(this.GetComponent<RandomSeedGenerator>().seed == "")
        {
            this.allowNewUnlockables = true;
        }
        else
        {
            this.allowNewUnlockables = false;
        }

        //Making sure our save folder name is valid
        this.saveFolder = SaveLoadManager.globalReference.CreateNewSaveFolder();

        //Telling the map generator to create a new level instead of loading one from a save
        this.loadType = LevelLoadType.GenerateNewLevel;

        //Saving the current save folder name so we know which game was played last. Used for "Continuing"
        PlayerPrefs.SetString("MostRecentSave", GameData.globalReference.saveFolder);

        //Transitioning to the gameplay level
        //this.GetComponent<GoToLevel>().LoadLevelByName(this.gameplayLevelName);
        EVTData transitionEVT = new EVTData();
        transitionEVT.sceneTransition = new SceneTransitionEVT(this.gameplayLevelName, 0.5f, 1);
        EventManager.TriggerEvent(SceneTransitionEVT.eventNum, transitionEVT);
    }


    //Function called from the MainMenu scene to continue the most recent save
    public void ContinueGame()
    {
        //If the most recent save doesn't exist, we're trying to load settings that don't exist
        if (!PlayerPrefs.HasKey("MostRecentSave"))
        {
            Debug.Log("ContinueGame, MostRecentSave doesn't exist");
            return;
        }
        else if(PlayerPrefs.GetString("MostRecentSave") == "")
        {
            Debug.Log("ContinueGame, MostRecentSave is empty");
            return;
        }
        else if(!System.IO.Directory.Exists(Application.persistentDataPath + PlayerPrefs.GetString("MostRecentSave")))
        {
            Debug.Log("ContinueGame, MostRecentSave folder directory doesn't exist");
            return;
        }

        //Loading the folder name for the most recent save so we can "Continue"
        this.saveFolder = PlayerPrefs.GetString("MostRecentSave");

        //Telling the map generator to load our existing level
        this.loadType = LevelLoadType.LoadLevel;

        //Transitioning to the gameplay level
        //this.GetComponent<GoToLevel>().LoadLevelByName(this.gameplayLevelName);
        EVTData transitionEVT = new EVTData();
        transitionEVT.sceneTransition = new SceneTransitionEVT(this.gameplayLevelName, 0.5f, 1);
        EventManager.TriggerEvent(SceneTransitionEVT.eventNum, transitionEVT);
    }


    //Function called from LoadSavePanel.cs to load a saved game
    public void LoadSaveFile(string saveFolder_)
    {
        //Making sure the given save folder exists
        if (!System.IO.Directory.Exists(Application.persistentDataPath + saveFolder_))
        {
            return;
        }

        //Setting the most recent save to the given folder
        PlayerPrefs.SetString("MostRecentSave", saveFolder_);

        //Setting the save folder as our current save
        this.saveFolder = saveFolder_;

        //Telling the map generator to load our existing level
        this.loadType = LevelLoadType.LoadLevel;

        //Transitioning to the gameplay level
        this.GetComponent<GoToLevel>().LoadLevelByName(this.gameplayLevelName);
    }
}
