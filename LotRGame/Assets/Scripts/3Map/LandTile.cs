using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(PathPoint))]
public class LandTile : MonoBehaviour
{
    //Static reference to the tile that the player has selected
    public static LandTile selectedTile;

    //The list of items that can be found on this tile from foraging. MUST be listed in ascending order of skill checks needed
    public List<Item> forageList;
    //The list of skill rolls needed for each foraged item. MUST align with the same index of the item in the forage list
    [Range(0, 100)]
    public List<int> forageSkillList;

    //The list of items that can be found on this tile from fishing. MUST be listed in ascending order of skill checks needed
    public List<Item> fishingList;
    //The list of skill rolls needed for each fished item. MUST align with the same index of the item in the fishing list
    [Range(0, 100)]
    public List<int> fishingSkillList;

    //The list of animals that can be encountered on this tile from tracking. MUST be listed in ascending order of skill checks needed
    public List<GameObject> trackingList;
    //The list of skill rolls needed for each animal encounter. MUST align with the same index of the item in the tracking list
    [Range(0, 100)]
    public List<int> trackingSkillList;




    //Function called on initialization
    private void Start()
    {
        this.HilightThisTile(false);
    }


    //Function called on the frame when the player's mouse enters this object's collider
    private void OnMouseEnter()
    {
        //If the selection mode is on anything but "None", this tile is hilighted
        if (TileSelectionMode.GlobalReference.currentSelectionMode != TileSelectionMode.SelectionMode.None)
        {
            this.HilightThisTile(true);
        }
    }


    //Function called every frame when the player's mouse is over this object's collider
    private void OnMouseOver()
    {
        //If the selection mode is switched to "None", this tile is no longer hilighted
        if(TileSelectionMode.GlobalReference.currentSelectionMode == TileSelectionMode.SelectionMode.None)
        {
            this.HilightThisTile(false);
            return;
        }

        //If the player left clicks over this tile and the selection mode is anything but "None", it becomes the one that's selected
        if(Input.GetMouseButtonDown(0) && TileSelectionMode.GlobalReference.currentSelectionMode != TileSelectionMode.SelectionMode.None)
        {
            //If the left Alt button isn't held down (for rotating the camera)
            if (!Input.GetKey(KeyCode.LeftAlt))
            {
                //Un-hilighting the currently selected file
                if (LandTile.selectedTile != null)
                {
                    LandTile.selectedTile.HilightThisTile(false);
                }

                LandTile.selectedTile = this;

                //If the selection mode is on "Movement" then the selected characters are told to move to this tile
                if(TileSelectionMode.GlobalReference.currentSelectionMode == TileSelectionMode.SelectionMode.Movement)
                {
                    //Looping through each character that's selected
                    foreach(Character person in CharacterManager.globalReference.selectedCharacters)
                    {
                        //Using the Dijkstra search to find the dravel path for each character
                        PathPoint startingTile = person.GetComponent<Movement>().currentTile.GetComponent<PathPoint>();
                        PathPoint endTile = LandTile.selectedTile.GetComponent<PathPoint>();
                        List<LandTile> pathToFollow = PathfindingAlgorithms.DijkstraSearchLandTile(startingTile, endTile);

                        //Setting the path to follow for the character's movement
                        person.GetComponent<Movement>().TravelToPath(pathToFollow);
                    }
                }
            }
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
        
        //If the player right clicks or if the selection mode goes back to "None", the selected tile is cleared
        if (Input.GetMouseButtonDown(1) || TileSelectionMode.GlobalReference.currentSelectionMode == TileSelectionMode.SelectionMode.None)
        {
            //If this tile was the one selected, it becomes un-hilighted
            this.HilightThisTile(false);

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
            this.HilightThisTile(false);
        }
    }


    //Function called to hilight (or stop hilighting) this tile and show path points
    private void HilightThisTile(bool hilightOn_)
    {
        //If the hilight is on, the material is bright
        if (hilightOn_)
        {
            this.GetComponent<MeshRenderer>().materials[0].color = new Color(1, 1, 1, 1);
            this.GetComponent<LineRenderer>().enabled = true;
        }
        //If it's off, the material is dark
        else
        {
            this.GetComponent<MeshRenderer>().materials[0].color = new Color(0.5f, 0.5f, 0.5f, 1);
            this.GetComponent<LineRenderer>().enabled = false;
        }
    }
}
