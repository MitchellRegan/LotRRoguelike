using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MapOverworldUI : MonoBehaviour
{
    //Reference to this object's Image component
    private Image ourImage;

    //Reference to the MiniMapUI.cs component that we take our sprite from
    public MiniMapUI miniMapRef;

    //Reference to the rect transform for the UI element that we use to track player location
    public RectTransform playerLocationObj;

    //Reference to the WASDOverworldMovement.cs component on the party group for quick reference
    private WASDOverworldMovement partyMove;

    //Multiplier for this image size to get the sprite correct
    public float spriteSizeMultiplier = 1;



	// Use this for initialization
	private void Start ()
    {
        //Getting the reference to our Image component
        this.ourImage = this.GetComponent<Image>();

        //Getting the reference to the player group's WASDOverworldMovement.cs component
        this.partyMove = PartyGroup.group1.GetComponent<WASDOverworldMovement>();

        //Setting our image sprite to the same image as the one the MiniMapUI.cs component created
        this.ourImage.sprite = this.miniMapRef.GetComponent<Image>().sprite;

        //Multiplying our sprite's size to scale it up so it fits the screen better
        this.GetComponent<RectTransform>().sizeDelta = this.GetComponent<RectTransform>().sizeDelta * this.spriteSizeMultiplier;
	}
	

	// Update is called once per frame
	private void Update ()
    {
        //Finding the party group's row and col coords in the overworld
        CreateTileGrid.TileColRow partyColRow = CreateTileGrid.globalReference.GetTileCoords(this.partyMove.currentTile);

        if(partyColRow != null)
        {
            //Setting the anchor position of the player location object relative to the size of this UI object
            Vector2 anchorPos = new Vector2((partyColRow.col * 1f) / (CreateTileGrid.globalReference.cols * 1f),
                                  (partyColRow.row * 1f) / (CreateTileGrid.globalReference.rows * 1f));

            this.playerLocationObj.anchorMin = anchorPos;
            this.playerLocationObj.anchorMax = anchorPos;
        }
	}
}
