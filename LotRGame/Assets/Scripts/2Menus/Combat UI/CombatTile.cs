using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(PathPoint))]
public class CombatTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Row and column
    public int row = 0;
    public int col = 0;

    //Reference to this tile's path point reference
    private PathPoint ourPathPoint;

    //Color picker for when this tile is not in use
    public Color inactiveColor = Color.white;
    //Color picker for when this tile is hilighted
    public Color hilightColor = Color.blue;



	//Function called on the first frame this is created
	private void Awake ()
    {
        //Setting the reference to our path point component
        this.ourPathPoint = this.GetComponent<PathPoint>();
        //Setting this tile's color to the inactive color
        this.GetComponent<Image>().color = this.inactiveColor;
	}


    //Function called on the first frame of the game
    private void Start()
    {
        //Adding this combat tile to the grid in the combat manager
        CombatManager.globalReference.AddCombatTileToGrid(this, this.row, this.col);
    }
	

    //Function called when the player's mouse starts hovering over this tile
	public void OnPointerEnter(PointerEventData eventData_)
    {
        //Hilighting this tile's image
        this.GetComponent<Image>().color = this.hilightColor;
        //Looping through each connected tile and hilighting them
        foreach(PathPoint connection in this.ourPathPoint.connectedPoints)
        {
            connection.GetComponent<Image>().color = this.hilightColor;
        }
    }


    //Function called when the player's mouse is no longer over this tile
    public void OnPointerExit(PointerEventData eventData_)
    {
        //Stops hilighting this tile's image
        this.GetComponent<Image>().color = this.inactiveColor;
        //Looping through each connected tile and stops hilighting them
        foreach (PathPoint connection in this.ourPathPoint.connectedPoints)
        {
            connection.GetComponent<Image>().color = this.inactiveColor;
        }
    }
}
