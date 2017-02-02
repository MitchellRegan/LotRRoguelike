using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjManager : MonoBehaviour
{
    private GameObject[] CharacterList;
    
    private GameObject[,] LandTileArray;


    //Initializes the LandTileList with the correct number of rows and columns. ONLY CALLED ONCE WHEN THE MAP IS MADE
    public void InitializeLandTileList(int columns_, int rows_)
    {
        //prevents us from initializing this list more than once
        if(LandTileArray != null)
        {
            Debug.LogError("ERROR: ObjManager.InitializeLandTileList, LandTileList already initialized");
            return;
        }
        
        //Sets the number of columns
        LandTileArray = new GameObject[columns_,rows_];
    }


    //Loops through to find the position of a given tile
    public int[] FindTilePosition(GameObject landTile_)
    {
        int[] tileColRow = new int[2];

        //Loops through all of the columns
        for(int col = 0; col < LandTileArray.GetLength(0); ++col)
        {
            //Loops through all of the rows
            for(int row = 0; row < LandTileArray.GetLength(1); ++row)
            {
                //If the tile is found, return the column and row
                if(LandTileArray[col, row] == landTile_)
                {
                    tileColRow[0] = col;
                    tileColRow[1] = row;
                }
            }
        }

        return tileColRow;
    }


    //Returns the tile at the given column and row coordinates
    public GameObject FindTileAtPosition(int col_, int row_)
    {
        //Debug.Log("Col: " + col_);
        //Debug.Log("Row: " + row_);
        if (LandTileArray.GetLength(0) <= col_ || 0 >= col_)
        {
            return null;
        }
        else if (LandTileArray.GetLength(1) <= row_ || 0 >= row_)
        {
            return null;
        }
        else
        {
            return LandTileArray[col_, row_];
        }
    }

    
    //Sets a new tile in the given column and row if there isn't already one there
    public void SetTileAtPosition(int col_, int row_, GameObject newTile_)
    {
        //Makes sure that there isn't already a tile in the given position
        if(LandTileArray[col_, row_] != null)
        {
            Debug.Log("ERROR: ObjManager.SetTileAtPosition, position is not null");
            return;
        }
        
        LandTileArray[col_, row_] = newTile_;
    }



    //Clears the LandTileList cleanly
    public void ClearTileList()
    {
        for(int c = 0; c < LandTileArray.GetLength(0); ++c)
        {
            //Null all entities in each "row"
            for(int r = 0; r < LandTileArray.GetLength(1); ++r)
            {
                LandTileArray[c, r] = null;
            }
        }

        //Makes sure that the list is cleared and null
        LandTileArray = null;
    }


    //Clears the CharacterList cleanly
    public void ClearCharacterList()
    {
        //Loops through all of the characters and nulls them
        for(int c = 0; c < CharacterList.Length; ++c)
        {
            CharacterList[c] = null;
        }

        //makes sure that the list is cleared and null
        CharacterList = null;
    }


    //Clears all data in the ObjectManager
    public void ClearData()
    {
        ClearTileList();
        ClearCharacterList();
    }
}
