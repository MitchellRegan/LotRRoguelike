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

        //If this land tile isn't for water, we change our mesh verts to align with nearby tiles
        if (this.tileReference.type != LandType.Ocean && this.tileReference.type != LandType.River)
        {
            this.AngleMeshVerts();
        }
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

        //0-Center,  1-E inner,  2-SE inner,  3-SW inner,  4-SW outer,  5-W outer,  6-W inner
        //7-NW inner,   8-NE inner,   9-E outer,   10-NE outer,   11-SE outer,   12-NW outer,   13-NW topFlat,   14-W topFlat,  15-W bottom,  16-NW bottom
        //17-E topFlat,  18-NE topFlat,  19-NE bottom,  20-E bottom,  21-SW topFlat,  22-SE topflat,  23-SE bottom,  24-SW bottom,  25-SE topFlat
        //26-E-topFlat,  27-E bottom,  28-SE bottom,  29-W topFlat,  30-SW topFlat,  31-SW bottom,  32-W bottom,  33-NE topFlat,  34-NW topFlat
        //35-NW bottom,  36-NE bottom

        //Inner: 1-E, 2-SE, 3-SW, 6-W, 7-NW, 8-NE
        //Outer: 4-SW, 5-W, 9-E, 10-NE, 11-SE, 12-NW
        //TopFlat: 13/34-NW,   14/29-W,   17/26-E,   18/33-NE,   21/30-SW,   22/25-SE
        //Bottom: 15-W, 16-NW, 19-NE, 20-E, 23-SE, 24-SW, 27-E, 28-SE, 31-SW, 32-W, 35-NW, 36-NE

        //Use this for Hex Tile EXTRUDE
        //East (9, 17, 26)
        float eastHeight = this.FindTileHeight(TileDirections.Northeast) - this.tileReference.elevation;
        eastHeight += this.FindTileHeight(TileDirections.Southeast) - this.tileReference.elevation;
        eastHeight = eastHeight / 3;
        vertices[9].Set(vertices[9].x, vertices[9].y + eastHeight, vertices[9].z);
        vertices[17].Set(vertices[17].x, vertices[17].y + eastHeight, vertices[17].z);
        vertices[26].Set(vertices[26].x, vertices[26].y + eastHeight, vertices[26].z);

        //South East (11, 22, 25)
        float southeastHeight = this.FindTileHeight(TileDirections.South) - this.tileReference.elevation;
        southeastHeight += this.FindTileHeight(TileDirections.Southeast) - this.tileReference.elevation;
        southeastHeight = southeastHeight / 3;
        vertices[11].Set(vertices[11].x, vertices[11].y + southeastHeight, vertices[11].z);
        vertices[22].Set(vertices[22].x, vertices[22].y + southeastHeight, vertices[22].z);
        vertices[25].Set(vertices[25].x, vertices[25].y + southeastHeight, vertices[25].z);

        //South West (4, 21, 30)
        float southwestHeight = this.FindTileHeight(TileDirections.South) - this.tileReference.elevation;
        southwestHeight += this.FindTileHeight(TileDirections.Southwest) - this.tileReference.elevation;
        southwestHeight = southwestHeight / 3;
        vertices[4].Set(vertices[4].x, vertices[4].y + southwestHeight, vertices[4].z);
        vertices[21].Set(vertices[21].x, vertices[21].y + southwestHeight, vertices[21].z);
        vertices[30].Set(vertices[30].x, vertices[30].y + southwestHeight, vertices[30].z);

        //North East (10, 18, 33)
        float northeastHeight = this.FindTileHeight(TileDirections.North) - this.tileReference.elevation;
        northeastHeight += this.FindTileHeight(TileDirections.Northeast) - this.tileReference.elevation;
        northeastHeight = northeastHeight / 3;
        vertices[10].Set(vertices[10].x, vertices[10].y + northeastHeight, vertices[10].z);
        vertices[18].Set(vertices[18].x, vertices[18].y + northeastHeight, vertices[18].z);
        vertices[33].Set(vertices[33].x, vertices[33].y + northeastHeight, vertices[33].z);

        //North West (12, 13, 34)
        float northwestHeight = this.FindTileHeight(TileDirections.North) - this.tileReference.elevation;
        northwestHeight += this.FindTileHeight(TileDirections.Northwest) - this.tileReference.elevation;
        northwestHeight = northwestHeight / 3;
        vertices[12].Set(vertices[12].x, vertices[12].y + northwestHeight, vertices[12].z);
        vertices[13].Set(vertices[13].x, vertices[13].y + northwestHeight, vertices[13].z);
        vertices[34].Set(vertices[34].x, vertices[34].y + northwestHeight, vertices[34].z);

        //West (5, 14, 29)
        float westHeight = this.FindTileHeight(TileDirections.Northwest) - this.tileReference.elevation;
        westHeight += this.FindTileHeight(TileDirections.Southwest) - this.tileReference.elevation;
        westHeight = westHeight / 3;
        vertices[5].Set(vertices[5].x, vertices[5].y + westHeight, vertices[5].z);
        vertices[14].Set(vertices[14].x, vertices[14].y + westHeight, vertices[14].z);
        vertices[29].Set(vertices[29].x, vertices[29].y + westHeight, vertices[29].z);


        //Use this for the Hex Tile FLAT mesh
        //1 = East
        /*float eastHeight = this.FindTileHeight(TileDirections.Northeast) - this.tileReference.elevation;
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
        vertices[6].Set(vertices[6].x, vertices[6].y + westHeight, vertices[6].z);*/


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
