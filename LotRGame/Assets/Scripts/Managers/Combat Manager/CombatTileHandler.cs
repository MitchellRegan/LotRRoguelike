using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTileHandler : MonoBehaviour
{
    //The number of rows and columns in this grid
    public int numRows = 15;
    public int numCols = 15;

    //2D List of all combat tiles in the combat screen map. [col][row]
    public List<List<CombatTile3D>> combatTileGrid;

    //Reference to the game object that displays the combat tiles
    public GameObject tileGridHolder;



    // Function called when this object is created
    private void Awake()
    {
        //Initializing our combat tile grid
        this.combatTileGrid = new List<List<CombatTile3D>>();

        //Setting up each column of rows
        for (int col = 0; col < this.numCols; ++col)
        {
            this.combatTileGrid.Add(new List<CombatTile3D>());

            //Setting up each row inside the current column
            for (int row = 0; row < this.numRows; ++row)
            {
                this.combatTileGrid[col].Add(null);
            }
        }
    }


    //Function called externally from CombatTile3D.cs. Adds a 3D combat tile to our combat tile grid at the row and column given
    public void AddCombatTileToGrid(CombatTile3D tileToAdd_, int row_, int col_)
    {
        //Making sure the row and column values are within bounds
        if(row_ < 0 || row_ > this.numRows)
        {
            return;
        }
        if(col_ < 0 || col_ > this.numCols)
        {
            return;
        }

        //Making sure a tile doesn't already exist in this location
        if(this.combatTileGrid[col_][row_] != null)
        {
            return;
        }

        //Setting the given tile to the correct row and column
        this.combatTileGrid[col_][row_] = tileToAdd_;
        this.ConnectTile(row_, col_);
    }


    //Function called from AddCombatTileToGrid to connect a tile to the surrounding tiles
    private void ConnectTile(int row_, int col_)
    {
        //Connecting up
        if(row_ != this.numRows - 1)
        {
            this.combatTileGrid[col_][row_].up = this.combatTileGrid[col_][row_ + 1];
            this.combatTileGrid[col_][row_ + 1].down = this.combatTileGrid[col_][row_];
        }

        //Connecting down
        if(row_ != 0)
        {
            this.combatTileGrid[col_][row_].down = this.combatTileGrid[col_][row_ - 1];
            this.combatTileGrid[col_][row_ - 1].up = this.combatTileGrid[col_][row_];
        }

        //Connecting left
        if (col_ != 0)
        {
            this.combatTileGrid[col_][row_].left = this.combatTileGrid[col_ - 1][row_];
            this.combatTileGrid[col_ - 1][row_].right = this.combatTileGrid[col_][row_];
        }

        //Connecting right
        if (row_ != this.numCols - 1)
        {
            this.combatTileGrid[col_][row_].right = this.combatTileGrid[col_ + 1][row_];
            this.combatTileGrid[col_ + 1][row_].left = this.combatTileGrid[col_][row_];
        }
    }


    //Function called externally to find out which combat tile the given character is on
    public CombatTile3D FindCharactersTile(Character characterToFind_)
    {
        //Getting less confusing references to the character's row/column position
        int row = characterToFind_.charCombatStats.gridPositionRow;
        int col = characterToFind_.charCombatStats.gridPositionCol;

        return this.combatTileGrid[col][row];
    }
}
