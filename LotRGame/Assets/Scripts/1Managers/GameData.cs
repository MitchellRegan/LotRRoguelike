using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour
{
    //A static reference to this object so that we can reference it from anywhere
    public static GameData globalReference;

    //Public enum that lets us know what difficulty the player has set the game to
    public enum gameDifficulty { Easy, Normal, Hard };
    public gameDifficulty currentDifficulty = gameDifficulty.Normal;

    //Public enum that lets us know what race the player is starting as in a new game
    public Races startingRace = Races.Human;

    //The width and height of the game map in tiles for the different difficulty settings
    public Vector2 easyMapSize = new Vector2();
    public Vector2 normalMapSize = new Vector2();
    public Vector2 hardMapSize = new Vector2();

    //Reference to the object prefab that's instantiated to create a new map
    public GameObject newMapGenerator;



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
                this.currentDifficulty = gameDifficulty.Easy;
                break;

            case 1:
                this.currentDifficulty = gameDifficulty.Normal;
                break;

            case 2:
                this.currentDifficulty = gameDifficulty.Hard;
                break;

            default:
                this.currentDifficulty = gameDifficulty.Normal;
                break;
        }
    }


    //Function called from the New Game screen in the main menu. Starts the process of creating a new map
    public void StartNewGame()
    {
    }
}
