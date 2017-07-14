using UnityEngine;
using System.Collections;

public class CreateMapButtons : MonoBehaviour
{
    //Reference to this object's Text child
    public GameObject TextChild = null;
    private UnityEngine.UI.Text TextChildGUI = null;

    //Quantity of Tiles starting out
    public int TilePercentage = 10;

    /*public enum Land {NotLand, Ocean, Plains, Forrest, Swamp, Mountains, Volcano};
    public Land CurrentLand = Land.NotLand;

    public enum Special {NotSpecial, Villages, EnemyCamps, ExplorePoints};
    public Special CurrentSpecial = Special.NotSpecial;*/

    //Max number of Special Zone Tiles
    public int MaxSpecialTiles = 5;

    //Max/Min number of Rows/Columns
    public int MaxRowColumn = 40;
    public int MinRowColumn = 12;

    public enum enType {LandTiles, SpecialTiles, MapSize}
    public enType ButtonType = enType.LandTiles;
     

	// Use this for initialization
	void Start ()
    {
        TextChildGUI = TextChild.GetComponent<UnityEngine.UI.Text>();
        

        //If this button changes any Land tiles, it's true
        if(ButtonType == enType.SpecialTiles)
        {
            TilePercentage = 1;
        }
        else if(ButtonType == enType.MapSize)
        {
            TilePercentage = 20;
        }
	}


    // Updates the text to show the current value
    void Update()
    {
        if (TilePercentage == -1)
        {
            TextChildGUI.text = "Random";
        }
        else if(ButtonType == enType.LandTiles)
        {
            TextChildGUI.text = TilePercentage.ToString() + "%";
        }
        else
        {
            TextChildGUI.text = TilePercentage.ToString();
        }
    }


    //Adds numTiles value to TilePercentage if it changes Land Tiles
    //Adds 1 to TilePercentage if it changes Special Tiles
    //Adds 1 to TilePercentage if it changes Row or Column
    //Can't go above 100 if Land, MaxSpecialTiles if Special, or MaxRowColumn if MapSize
    public void AddTiles()
    {
        //Adds 5% to the current land type
        if(ButtonType == enType.LandTiles)
        {
            TilePercentage += 1;

            if(TilePercentage > 100)
            {
                TilePercentage = 100;
            }
        }
        //Adds 1 to the number of Special tiles
        else if(ButtonType == enType.SpecialTiles)
        {
            TilePercentage += 1;

            if(TilePercentage > MaxSpecialTiles)
            {
                TilePercentage = MaxSpecialTiles;
            }
        }
        //Adds 1 to the current row/column
        else if(ButtonType == enType.MapSize)
        {
            TilePercentage += 1;

            if(TilePercentage > MaxRowColumn)
            {
                TilePercentage = MaxRowColumn;
            }
        }
    }

    
    //Subtracts numTiles value from TilePercentage if it changes Land Tiles
    //Subtracts 1 from TilePercentage if it changes Special Tiles
    //Subtracts 1 from TilePercentage if it changes Row or Column
    //Can't go below 0
    public void SubtractTiles()
    {
        //Subtracts 5% to the current land tile
        if (ButtonType == enType.LandTiles)
        {
            TilePercentage -= 1;

            if (TilePercentage < 0)
            {
                TilePercentage = 0;
            }
        }
        //Subtracts 1 from the current number of special tiles or row/column
        else if(ButtonType == enType.SpecialTiles)
        {
            TilePercentage -= 1;

            if (TilePercentage < -1)
            {
                TilePercentage = -1;
            }
        }
        else if(ButtonType == enType.MapSize)
        {
            TilePercentage -= 1;

            if(TilePercentage < MinRowColumn)
            {
                TilePercentage = MinRowColumn;
            }
        }
    }


    //Returns the current TilePercentage value.
    //Used by CreatemapGenerate.cs
    public int GetTilePercentage()
    {
        if(TilePercentage == -1)
        {
            var rand = Random.Range(0, MaxSpecialTiles + 1);

            if(rand < MaxSpecialTiles)
            {
                rand = MaxSpecialTiles;
            }

            return rand;
        }

        return TilePercentage;
    }
}
