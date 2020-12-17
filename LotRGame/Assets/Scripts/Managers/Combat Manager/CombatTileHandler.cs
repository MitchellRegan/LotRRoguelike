﻿using System.Collections;
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

    //Reference to the object that highlights the tile for the acting character
    public TileHighlight tileHighlight;



    // Function called when this object is created
    private void Awake()
    {
        //Hiding our highlight
        this.StopHighlightingTile();

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


    //Called on the first frame to connect all the tiles after they've been added to the grid
    private void Start()
    {
        //Looping through each row and column to connect tiles to their neighbors
        for(int r = 0; r < this.numRows; r++)
        {
            for(int c = 0; c < this.numCols; c++)
            {
                this.ConnectTile(r, c);
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
    }


    //Function called from Start to connect a tile to the surrounding tiles
    private void ConnectTile(int row_, int col_)
    {
        //Connecting up
        if(row_ != this.numRows - 1)
        {
            this.combatTileGrid[col_][row_].up = this.combatTileGrid[col_][row_ + 1];
            this.combatTileGrid[col_][row_ + 1].down = this.combatTileGrid[col_][row_];
        }
        
        //Connecting down
        if (row_ != 0)
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
        if (col_ != this.numCols - 1)
        {
            this.combatTileGrid[col_][row_].right = this.combatTileGrid[col_ + 1][row_];
            this.combatTileGrid[col_ + 1][row_].left = this.combatTileGrid[col_][row_];
        }
    }


    //Function called from CombatManager.InitiateCombat to reset all combat tiles
    public void ResetCombatTiles()
    {
        for(int r = 0; r < this.numRows; r++)
        {
            for(int c = 0; c < this.numCols; c++)
            {
                this.combatTileGrid[c][r].ResetTile();
            }
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


    //Function called externally to highlight a specific tile
    public void HighlightTile(int row_, int col_)
    {
        this.tileHighlight.gameObject.SetActive(true);

        Vector3 tileLoc = this.combatTileGrid[col_][row_].transform.position;
        this.tileHighlight.transform.position = tileLoc;
    }


    //Function called externally to hide the highlight object
    public void StopHighlightingTile()
    {
        this.tileHighlight.gameObject.SetActive(false);
    }
}
