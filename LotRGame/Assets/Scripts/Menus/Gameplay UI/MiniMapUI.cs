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


        Debug.Log(GameData.globalReference.saveFolder);
        Debug.Log(Application.persistentDataPath + GameData.globalReference.saveFolder);
        //Getting the file path for this game's map image
        string mapFilePath = Application.persistentDataPath + GameData.globalReference.saveFolder + "/Map.png";

        //Creating a new Texture to hold the loaded map image
        Texture2D mapTexture;
        byte[] mapFileData;

        //As long as we have the correct file directory the file will load and fill in the texture
        if(File.Exists(mapFilePath))
        {
            mapFileData = File.ReadAllBytes(mapFilePath);
            mapTexture = new Texture2D(2, 2);
            mapTexture.LoadImage(mapFileData);

            //Setting the new texture for our image texture
            this.ourSprite.sprite = mapTexture;
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

        //Finding the selected party's tile so we know where the map should show
        TileInfo partyTile = CharacterManager.globalReference.selectedGroup.GetComponent<WASDOverworldMovement>().currentTile;

        //If the party tile is different from before, we update the tile position
        if (this.currentTile == null || this.currentTile != partyTile)
        {
            //Setting our current tile to the party tile
            this.currentTile = partyTile;

            //Getting the row and column of the current tile
            CreateTileGrid.TileColRow newTileColRow = CreateTileGrid.globalReference.GetTileCoords(partyTile);

            if (newTileColRow != null)
            {
                //Getting the rect transform component for this game object
                RectTransform ourRect = this.GetComponent<RectTransform>();

                /*Debug.Log("This is where the minimap is having problems");
                Debug.Log("Col/Row: " + newTileColRow.col + ", " + newTileColRow.row);
                Debug.Log("Tile 0,0 pos: " + CreateTileGrid.globalReference.tileGrid[0][0].tilePosition + ", tile type: " + CreateTileGrid.globalReference.tileGrid[0][0].type);
                Debug.Log("Opposite corner: " + CreateTileGrid.globalReference.tileGrid[CreateTileGrid.globalReference.tileGrid.Count - 1][CreateTileGrid.globalReference.tileGrid[0].Count - 1].tilePosition + ", tile type: " + CreateTileGrid.globalReference.tileGrid[CreateTileGrid.globalReference.tileGrid.Count - 1][CreateTileGrid.globalReference.tileGrid[0].Count - 1].type);
                *///Finding the map offset
                float xOffset = this.mapShiftOnMove * newTileColRow.col;
                float yOffset = this.mapShiftOnMove * newTileColRow.row;

                //Setting the position based on the offsets
                ourRect.localPosition = new Vector3(-yOffset, -xOffset, 0);
            }
        }
	}


    //Function called externally from CreateTileGrid.Start when the map is loaded
    public void SetMapTexture(string spriteImagePath_)
    {
        //Making sure the file exists
        if(!System.IO.File.Exists(spriteImagePath_))
        {
            Debug.LogError("Map Sprite is null. Path given: " + spriteImagePath_);
            this.gameObject.SetActive(false);
            return;
        }

        //Creating variables to hold the loaded file data
        Texture2D spriteTexture;
        byte[] fileData;

        //Loading the file data to create a new texture
        fileData = System.IO.File.ReadAllBytes(spriteImagePath_);
        spriteTexture = new Texture2D(2, 2);
        spriteTexture.LoadImage(fileData);
        spriteTexture.filterMode = FilterMode.Point;

        //Creating a new sprite based on the loaded texture
        this.ourSprite.sprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0, 0), 100f);
        
        //Scaling this sprite image based on the pixel resolution of the sprite
        RectTransform ourRect = this.GetComponent<RectTransform>();
        ourRect.localScale = new Vector3(this.ourSprite.sprite.texture.width * this.pixelSizeOnScreen,
                                         this.ourSprite.sprite.texture.height * this.pixelSizeOnScreen,
                                         1);
    }
}
