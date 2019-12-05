using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[RequireComponent(typeof(Image))]
public class MiniMapUI : MonoBehaviour
{
    //Static reference for this script so that MapCreator.cs can set our sprite
    public static MiniMapUI globalReference;

    //Reference to this object's sprite renderer component
    private Image ourSprite;

    //The size on screen that each pixel should take
    public float pixelSizeOnScreen = 0.5f;

    //The amount that the map image moves when it shifts in the UI
    public float mapShiftOnMove = 0.5f;

    //The tile that the current party group is on
    private TileInfo currentTile;



	// Use this for initialization
	private void Start ()
    {
        //Setting the global reference to this component if one doesn't already exist
		if(globalReference == null)
        {
            globalReference = this;
        }
        //If there's already a global reference, we destroy this component
        else
        {
            Destroy(this);
        }

        //Getting the reference to this object's sprite renderer
        this.ourSprite = this.GetComponent<Image>();
        

        //Getting the file path for this game's map image
        string mapFilePath = Application.persistentDataPath + GameData.globalReference.saveFolder + "/Map.png";

        //As long as we have the correct file directory the file will load and fill in the texture
        if(File.Exists(mapFilePath))
        {
            //Loading the map png file as a new texture
            byte[] mapFileData = File.ReadAllBytes(mapFilePath);
            Texture2D mapTexture = new Texture2D(2, 2);
            mapTexture.LoadImage(mapFileData);

            //Creating a new sprite from the map texture to set as our object's sprite
            Sprite mapSprite = Sprite.Create(mapTexture, new Rect(0, 0, mapTexture.width, mapTexture.height), new Vector2(0, 0));
            this.ourSprite.sprite = mapSprite;

            //Resizing our sprite size
            this.ourSprite.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, mapTexture.height * this.mapShiftOnMove);
            this.ourSprite.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, mapTexture.width * this.mapShiftOnMove);

            //Setting the starting position of the image
        }
    }
	

	// Update is called once per frame
	private void Update ()
    {
		//If we don't have a sprite, nothing happens
        if(this.ourSprite == null)
        {
            return;
        }

        //Positioning the map to the correct location
        this.RepositionMap();
	}


    //Function called from Update to shift the map based on the player party's current location
    private void RepositionMap()
    {
        //Finding the selected party's tile so we know where the map should show
        TileInfo partyTile = CharacterManager.globalReference.selectedGroup.GetComponent<WASDOverworldMovement>().currentTile;

        //If the party tile is different from before, we update the tile position
        if (this.currentTile == null || this.currentTile != partyTile)
        {
            //Setting our current tile to the party tile
            this.currentTile = partyTile;

            //Getting the row and column of the current tile
            TileColRow newTileColRow = CreateTileGrid.globalReference.GetTileCoords(partyTile);

            if (newTileColRow != null)
            {
                //Getting the rect transform component for this game object
                RectTransform ourRect = this.GetComponent<RectTransform>();

                //Setting the pivot position based on the offsets
                ourRect.pivot = new Vector2((newTileColRow.col * 1f) / (CreateTileGrid.globalReference.cols * 1f),
                                  (newTileColRow.row * 1f) / (CreateTileGrid.globalReference.rows * 1f));
                //Re-centering the map sprite so that our pivot position is now at 0,0 in the middle of the minimap
                ourRect.localPosition = new Vector3();
            }
        }
    }
}
