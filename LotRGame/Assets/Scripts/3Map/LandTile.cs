using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class LandTile : MonoBehaviour
{
    //Static reference to the tile that the player has selected
    public static LandTile selectedTile;

    //References to all pathpoints on this tile that connect with surrounding tiles
    public PathPoint northPoint;
    public PathPoint northEastPoint;
    public PathPoint southEastPoint;
    public PathPoint southPoint;
    public PathPoint southWestPoint;
    public PathPoint northWestPoint;

    //List of all paht points that are inside this tile
    public List<PathPoint> allTilePoints;



    //Function called on initialization
    private void Start()
    {
        this.HilightThisTile(false, false, 0.25f);
    }


    //Function called on the frame when the player's mouse enters this object's collider
    private void OnMouseEnter()
    {
        this.HilightThisTile(true, false, 1);
    }


    //Function called every frame when the player's mouse is over this object's collider
    private void OnMouseOver()
    {
        //If the player left clicks over this tile, it becomes the one that's selected
        if(Input.GetMouseButtonDown(0))
        {
            //Un-hilighting the currently selected file
            if (LandTile.selectedTile != null)
            {
                LandTile.selectedTile.HilightThisTile(false, false, 0.25f);
            }

            LandTile.selectedTile = this;
        }
    }


    //Function called every frame
    private void Update()
    {
        //If this tile isn't selected, nothing happens
        if (LandTile.selectedTile != this)
        {
            return;
        }

        //If the player right clicks, the selected tile is cleared
        if (Input.GetMouseButtonDown(1))
        {
            //If this tile was the one selected, it becomes un-hilighted
            this.HilightThisTile(false, false, 0.25f);

            //Clearing the reference to the selected tile
            LandTile.selectedTile = null;
        }
    }


    //Function called on the frame when the player's mouse leaves this object's collider
    private void OnMouseExit()
    {
        //If this tile isn't currently selected, it becomes un-hilighted when the mouse leaves
        if (LandTile.selectedTile != this)
        {
            this.HilightThisTile(false, false, 0.25f);
        }
    }


    //Function called to hilight (or stop hilighting) this tile and show path points
    private void HilightThisTile(bool hilightOn_, bool hilightConnections_, float opacity_)
    {
        //Fades this tile's sprite to the correct opacity
        this.GetComponent<SpriteRenderer>().color = new Color(this.GetComponent<SpriteRenderer>().color.r,
                                                                this.GetComponent<SpriteRenderer>().color.g,
                                                                this.GetComponent<SpriteRenderer>().color.b,
                                                                opacity_);

        //Turns on/off all path points connected to this tile
        foreach (PathPoint point in this.allTilePoints)
        {
            point.HilightConnectedPoints(hilightOn_, hilightConnections_);
        }
    }
}
