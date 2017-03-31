using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelectionMode : MonoBehaviour
{
    //Static reference to this component so it can be referenced anywhere
    public static TileSelectionMode GlobalReference;

    //Enum used to determine what happens when tiles are selected
    public enum SelectionMode { None, Info, Movement };
    public SelectionMode currentSelectionMode = SelectionMode.None;


	
    //Function called on initialize
    private void Awake()
    {
        //Making sure there's only one global reference to the Tile Selection Mode
        if(GlobalReference != null)
        {
            this.enabled = false;
        }
        else
        {
            GlobalReference = this;
        }
    }


    //Function called externally to clear the current selection mode
    public void ClearSelectionMode()
    {
        this.currentSelectionMode = SelectionMode.None;
    }


    //Function called externally to set the selection mode to movement
    public void SelectionModeMove()
    {
        this.currentSelectionMode = SelectionMode.Movement;
    }


    //Function called externally to set the selection mode to info
    public void SelectionModeInfo()
    {
        this.currentSelectionMode = SelectionMode.Info;
    }
}
