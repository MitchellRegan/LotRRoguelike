using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class LandTile : MonoBehaviour
{
    //Static reference to the tile that the player has selected
    public static LandTile selectedTile;

    //The reference to the TileInfo class it represents
    [HideInInspector]
    public TileInfo tileReference;

    //Bool where if true, this tile is animating to come into frame. Set to TRUE on creation
    private bool isAnimating = true;

    //The starting Y position of the tile when created
    public float startingYPos = -20;

    //The interpolation speed that this tile moves toward the base elevation
    [Range(0.2f, 0.99f)]
    public float animationInterpSpeed = 0.8f;

    

    //Function called on initialization
    private void Start()
    {
        this.HilightThisTile(false);
        this.transform.position = new Vector3(this.transform.position.x, this.startingYPos, this.transform.position.z);
        this.AngleMeshVerts();
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
                    //Making sure the clicked tile isn't the one that the group is already on
                    if(CharacterManager.globalReference.selectedGroup.GetComponent<Movement>().currentTile != LandTile.selectedTile.tileReference)
                    {
                        //Using the Dijkstra search to find the dravel path for each character
                        TileInfo startingTile = CharacterManager.globalReference.selectedGroup.GetComponent<Movement>().currentTile;
                        TileInfo endTile = LandTile.selectedTile.tileReference;
                        List<TileInfo> pathToFollow = PathfindingAlgorithms.DijkstraSearchLandTile(startingTile, endTile);

                        //Setting the path to follow for the character's movement
                        CharacterManager.globalReference.selectedGroup.GetComponent<Movement>().TravelToPath(pathToFollow);

                        //Setting the selection mode to nothing so that it doesn't have to be turned off constantly
                        TileSelectionMode.GlobalReference.ClearSelectionMode();
                    }
                    //If the clicked tile IS the one the player group is already on, the selection mode is turned off
                    else
                    {
                        TileSelectionMode.GlobalReference.ClearSelectionMode();
                    }
                }
            }
        }
    }


    //Function called every frame
    private void Update()
    {
        //If this tile is animating, we interpolate to the correct tile height
        if(this.isAnimating)
        {
            //Finding the difference in elevation from where we are and where we need to be
            float yDiff = this.tileReference.elevation - this.transform.position.y;
            //Multiplying that difference by our interpolation speed
            yDiff *= this.animationInterpSpeed;

            //And adding the result to our current Y position to move up
            this.transform.position += new Vector3(0, yDiff, 0);

            //If our current Y position is the correct elevation, we're done animating
            if(this.transform.position.y == this.tileReference.elevation)
            {
                this.isAnimating = false;
            }
        }

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
        }
        //If it's off, the material is dark
        else
        {
            this.GetComponent<MeshRenderer>().materials[0].color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
    }


    //Function called from Start to move our mesh's verts to angle toward the adjacent tiles
    private void AngleMeshVerts()
    {
        //Getting the reference to our mesh and the vert array
        MeshFilter ourMesh = this.GetComponent<MeshFilter>();
        Vector3[] vertices = ourMesh.mesh.vertices;

        //Our object's global transform position
        Vector3 ourPos = this.transform.position;

        //1 = East
        float eastHeight = this.FindTileHeight(TileDirections.Northeast) - this.tileReference.elevation;
        eastHeight += this.FindTileHeight(TileDirections.Southeast) - this.tileReference.elevation;
        eastHeight = eastHeight / 3;
        vertices[1].Set(vertices[1].x, vertices[1].y + eastHeight, vertices[1].z);

        //2 = South East
        float southeastHeight = this.FindTileHeight(TileDirections.South) - this.tileReference.elevation;
        southeastHeight += this.FindTileHeight(TileDirections.Southeast) - this.tileReference.elevation;
        southeastHeight = southeastHeight / 3;
        vertices[2].Set(vertices[2].x, vertices[2].y + southeastHeight, vertices[2].z);

        //3 = South West
        float southwestHeight = this.FindTileHeight(TileDirections.South) - this.tileReference.elevation;
        southwestHeight += this.FindTileHeight(TileDirections.Southwest) - this.tileReference.elevation;
        southwestHeight = southwestHeight / 3;
        vertices[3].Set(vertices[3].x, vertices[3].y + southwestHeight, vertices[3].z);

        //4 = North East
        float northeastHeight = this.FindTileHeight(TileDirections.North) - this.tileReference.elevation;
        northeastHeight += this.FindTileHeight(TileDirections.Northeast) - this.tileReference.elevation;
        northeastHeight = northeastHeight / 3;
        vertices[4].Set(vertices[4].x, vertices[4].y + northeastHeight, vertices[4].z);

        //5 = North West
        float northwestHeight = this.FindTileHeight(TileDirections.North) - this.tileReference.elevation;
        northwestHeight += this.FindTileHeight(TileDirections.Northwest) - this.tileReference.elevation;
        northwestHeight = northwestHeight / 3;
        vertices[5].Set(vertices[5].x, vertices[5].y + northwestHeight, vertices[5].z);

        //6 = West
        float westHeight = this.FindTileHeight(TileDirections.Northwest) - this.tileReference.elevation;
        westHeight += this.FindTileHeight(TileDirections.Southwest) - this.tileReference.elevation;
        westHeight = westHeight / 3;
        vertices[6].Set(vertices[6].x, vertices[6].y + westHeight, vertices[6].z);
        
        
        //Setting the mesh's verts to the changed array
        ourMesh.mesh.vertices = vertices;
    }


    //Function called from AngleMeshVerts to get the height of the tile in a direction relative to this tile
    private enum TileDirections { North, Northeast, Southeast, South, Southwest, Northwest };
    private float FindTileHeight(TileDirections direction)
    {
        //Reference to the tile that we get the height for
        TileInfo foundTile = null;

        //Switch statement for the direction we need to check
        switch(direction)
        {
            case TileDirections.North:
                //Looping through all of the connected tiles in our tile
                foreach(TileInfo connection in this.tileReference.connectedTiles)
                {
                    //Making sure the tile isn't null
                    if(connection != null)
                    {
                        //If the tile has a higher Z coord and the X coord is the same, then it's the tile we're looking for
                        if(connection.tilePosition.z > this.tileReference.tilePosition.z)
                        {
                            if(Mathf.Round(connection.tilePosition.x) == Mathf.Round(this.tileReference.tilePosition.x))
                            {
                                foundTile = connection;
                                break;
                            }
                        }
                    }
                }
                break;

            case TileDirections.Northeast:
                //Looping through all of the connected tiles in our tile
                foreach (TileInfo connection in this.tileReference.connectedTiles)
                {
                    //Making sure the tile isn't null
                    if (connection != null)
                    {
                        //If the tile has a higher Z and X coord, then it's the tile we're looking for
                        if (connection.tilePosition.z > this.tileReference.tilePosition.z)
                        {
                            if (connection.tilePosition.x > this.tileReference.tilePosition.x)
                            {
                                foundTile = connection;
                                break;
                            }
                        }
                    }
                }
                break;

            case TileDirections.Southeast:
                //Looping through all of the connected tiles in our tile
                foreach (TileInfo connection in this.tileReference.connectedTiles)
                {
                    //Making sure the tile isn't null
                    if (connection != null)
                    {
                        //If the tile has a lower Z coord and a higher X coord, then it's the tile we're looking for
                        if (connection.tilePosition.z < this.tileReference.tilePosition.z)
                        {
                            if (connection.tilePosition.x > this.tileReference.tilePosition.x)
                            {
                                foundTile = connection;
                                break;
                            }
                        }
                    }
                }
                break;

            case TileDirections.South:
                //Looping through all of the connected tiles in our tile
                foreach (TileInfo connection in this.tileReference.connectedTiles)
                {
                    //Making sure the tile isn't null
                    if (connection != null)
                    {
                        //If the tile has a lower Z coord and the X coord is the same, then it's the tile we're looking for
                        if (connection.tilePosition.z < this.tileReference.tilePosition.z)
                        {
                            if (Mathf.Round(connection.tilePosition.x) == Mathf.Round(this.tileReference.tilePosition.x))
                            {
                                foundTile = connection;
                                break;
                            }
                        }
                    }
                }
                break;

            case TileDirections.Southwest:
                //Looping through all of the connected tiles in our tile
                foreach (TileInfo connection in this.tileReference.connectedTiles)
                {
                    //Making sure the tile isn't null
                    if (connection != null)
                    {
                        //If the tile has a lower Z coord and a lower X coord, then it's the tile we're looking for
                        if (connection.tilePosition.z < this.tileReference.tilePosition.z)
                        {
                            if (connection.tilePosition.x < this.tileReference.tilePosition.x)
                            {
                                foundTile = connection;
                                break;
                            }
                        }
                    }
                }
                break;

            case TileDirections.Northwest:
                //Looping through all of the connected tiles in our tile
                foreach (TileInfo connection in this.tileReference.connectedTiles)
                {
                    //Making sure the tile isn't null
                    if (connection != null)
                    {
                        //If the tile has a higher Z and lower X coord, then it's the tile we're looking for
                        if (connection.tilePosition.z > this.tileReference.tilePosition.z)
                        {
                            if (connection.tilePosition.x < this.tileReference.tilePosition.x)
                            {
                                foundTile = connection;
                                break;
                            }
                        }
                    }
                }
                break;
        }

        //If the found tile is still null, we return 0 for the height
        if(foundTile == null)
        {
            return 0;
        }
        //Otherwise we return the tile's elevation
        else
        {
            return foundTile.elevation;
        }
    }
}
