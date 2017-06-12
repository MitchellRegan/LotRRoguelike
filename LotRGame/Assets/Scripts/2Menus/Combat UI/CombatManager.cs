using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    //Static reference to this combat manager
    public static CombatManager globalReference;

    //2D List of all combat tiles in the combat screen map. Col[row}
    public List<List<CombatTile>> combatTileGrid;

    //The mathematical interpolator that we'll use to find damage using min/max ranges
    private Interpolator ourInterpolator;

    //The list of all characters involved in this combat
    [HideInInspector]
    public List<Character> playerCharactersInCombat;
    [HideInInspector]
    public List<Character> enemyCharactersInCombat;

    //Reference to the background image that's set at the start of combat based on the type of land tile
    public Image backgroundImageObject;

    //Dictionary that determines which background sprite to set based on the land tile type
    public List<BackgroundImageTypes> tileTypeBackgrounds;



	// Use this for initialization
	private void Awake ()
    {
        //Setting the static reference
        if(globalReference == null)
        {
            globalReference = this;
        }
        //If a static reference already exists, this component is destroyed
        else
        {
            Destroy(this);
        }

        //Initializing our interpolator
        this.ourInterpolator = new Interpolator();

        //Initializing our combat tile grid
        this.combatTileGrid = new List<List<CombatTile>>();
        //Setting up each column of rows
        for (int col = 0; col < 13; ++col)
        {
            this.combatTileGrid.Add(new List<CombatTile>());
            //Setting up each row inside the current column
            for(int row = 0; row < 8; ++row)
            {
                this.combatTileGrid[col].Add(null);
            }
        }
	}


    //Adds a combat tile to our combat tile grid at the row and column given
    public void AddCombatTileToGrid(CombatTile tileToAdd_, int row_, int col_)
    {
        if(this.combatTileGrid[col_][row_] != null)
        {
            Debug.LogError(col_ + ", " + row_);
        }

        //Setting the given tile to the correct row and column
        this.combatTileGrid[col_][row_] = tileToAdd_;
    }


    //Function called to initiate combat
    public void InitiateCombat(LandType combatLandType_, List<Character> charactersInCombat_, EnemyEncounter encounter_)
    {
        //Setting the background image
        this.SetBackgroundImage(combatLandType_);
    }


    //Function called from InitiateCombat to set the background Image object's sprite
    private void SetBackgroundImage(LandType tileType_)
    {
        //Looping through each background image types until we find the correct land type
        foreach(BackgroundImageTypes bgType in this.tileTypeBackgrounds)
        {
            //If we find the matching type, the associated image is set to the background image's sprite
            if(bgType.tileType == tileType_)
            {
                this.backgroundImageObject.sprite = bgType.backgroundImage;
                return;
            }
        }
    }


    //Function called when one character attacks another. Returns the damage based on the mathematical distribution
    public int CalculateDamage(int minDamage_, int maxDamage_, EaseType distribution_)
    {
        //If the max damage is 0, no damage can be dealt
        if(maxDamage_ == 0)
        {
            return 0;
        }

        //Int to hold the damage that's returned. It's automatically set to the min
        int totalDamage = minDamage_;
        //Finding the difference between the min and max
        int damageDiff = maxDamage_ - minDamage_;

        //Setting the distribution type of the interpolator and resetting the progress
        this.ourInterpolator.ease = distribution_;
        this.ourInterpolator.ResetTime();
        //Setting the interp to a random value from 0 - 1
        this.ourInterpolator.AddTime(Random.Range(0, 1));

        //Adding a weighted percentage of the difference to the total
        totalDamage += damageDiff * Mathf.RoundToInt(this.ourInterpolator.GetProgress());

        return totalDamage;
    }
}

[System.Serializable]
public class BackgroundImageTypes
{
    //The type of land tile that combat is happening on
    public LandType tileType = LandType.Empty;
    //The background image associated with this tile type
    public Sprite backgroundImage;
}