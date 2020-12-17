using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(PathPoint))]
[RequireComponent(typeof(Image))]
public class CombatTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //Static reference to the Combat tile that the mouse is currently over
    public static CombatTile mouseOverTile;

    //Row and column
    public int row = 0;
    public int col = 0;

    //Reference to this tile's path point reference
    public PathPoint ourPathPoint;

    //If this combat tile is semi-highlighted to show an attack's radius
    [HideInInspector]
    public bool inActionRange = false;

    //The transparency of this tile when not hilighted
    [Range(0, 1f)]
    public float inactiveTransparency = 0.3f;
    //The transparency of this tile when not highlighted, but still in attack radius
    [Range(0, 1f)]
    public float atkRadiusTransparency = 0.7f;

    //Enum for what kind of object is on this tile
    [HideInInspector]
    public TileObjectType typeOnTile = TileObjectType.Nothing;

    //Color picker for when this tile is not in use
    public Color inactiveColor = Color.white;
    //Color picker for when this tile is hilighted
    public Color hilightColor = Color.blue;
    //Color picker for when a player character occupies this space
    public Color playerOccupiedColor = Color.green;
    //Color picker for when an enemy occupies this space
    public Color enemyOccupiedColor = Color.red;

    //The game object that occupies this tile
    [HideInInspector]
    public GameObject objectOnThisTile = null;



	//Function called on the first frame this is created
	private void Awake ()
    {
        //Setting the reference to our path point component
        this.ourPathPoint = this.GetComponent<PathPoint>();
        //Setting this tile's color to the inactive color
        this.HighlightTile(false);
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
        this.HighlightTile(true);

        mouseOverTile = this;

        //Highlighting any effect radius if there's a selected attack ability
        this.HighlightEffectRadius(true);

        //If we have a character on this tile, we make sure they're visible
        if(this.inActionRange && this.objectOnThisTile != null && this.objectOnThisTile.GetComponent<Character>())
        {
            //Making sure the character isn't the acting character
            if (CombatManager.globalReference.initiativeHandler.actingCharacters.Count > 0 && this.objectOnThisTile.GetComponent<Character>() != CombatManager.globalReference.initiativeHandler.actingCharacters[0])
            {
                //Getting the sprite base for the character
                CharacterSpriteBase cSprite = CombatManager.globalReference.GetCharacterSprite(this.objectOnThisTile.GetComponent<Character>());
                cSprite.MakeSpritesVisible();
            }
        }
    }


    //Function called when the player's mouse is no longer over this tile
    public void OnPointerExit(PointerEventData eventData_)
    {
        //If a character is moving right now and this tile is in the movement path, we don't stop highlighting
        if (CombatActionPanelUI.globalReference.selectedAction != null && CombatActionPanelUI.globalReference.selectedAction.GetComponent<MoveAction>())
        {
            //If this tile isn't in the movement path, this tile isn't highlighted
            if (!CombatActionPanelUI.globalReference.selectedAction.GetComponent<MoveAction>().IsTileInMovementPath(this))
            {
                this.HighlightTile(false);

                //If we have a character on this tile, we make sure they're hidden again
                if (this.inActionRange && this.objectOnThisTile != null && this.objectOnThisTile.GetComponent<Character>())
                {
                    //Making sure the character isn't the acting character
                    if (CombatManager.globalReference.initiativeHandler.actingCharacters.Count > 0 && this.objectOnThisTile.GetComponent<Character>() != CombatManager.globalReference.initiativeHandler.actingCharacters[0])
                    {
                        //Getting the sprite base for the character
                        CharacterSpriteBase cSprite = CombatManager.globalReference.GetCharacterSprite(this.objectOnThisTile.GetComponent<Character>());
                        cSprite.MakeSpritesTransparent();
                    }
                }
            }
        }
        else
        {
            //Stops hilighting this tile's image
            this.HighlightTile(false);

            //Stops highlighting any effect radius if there's a selected attack ability
            this.HighlightEffectRadius(false);

            //If we have a character on this tile, we make sure they're hidden again
            if (this.inActionRange && this.objectOnThisTile != null && this.objectOnThisTile.GetComponent<Character>())
            {
                //Making sure the character isn't the acting character
                if (this.objectOnThisTile.GetComponent<Character>() != CombatManager.globalReference.initiativeHandler.actingCharacters[0])
                {
                    //Getting the sprite base for the character
                    CharacterSpriteBase cSprite = CombatManager.globalReference.GetCharacterSprite(this.objectOnThisTile.GetComponent<Character>());
                    cSprite.MakeSpritesTransparent();
                }
            }
        }

        mouseOverTile = null;
    }


    //Function called when the player's mouse clicks over this tile
    public void OnPointerClick(PointerEventData eventData_)
    {
        //If this button isn't in range of an action, nothing happens
        if(this.inActionRange)
        {
            //Telling the combat manager that the selected action is going to happen on this tile
            CombatManager.globalReference.PerformActionAtClickedTile(this);
        }
    }


    //Function called to reset this tile so there's nothing on it and it's set to the inactive color
    public void ResetTile()
    {
        this.objectOnThisTile = null;
        this.SetTileColor(this.inactiveColor);
        this.HighlightTile(false);
    }


    //Function called from CombatManager.cs to set what object is on this tile
    public void SetObjectOnTile(GameObject objOnTile_, TileObjectType type_)
    {
        this.typeOnTile = type_;

        //If no object is added
        if(objOnTile_ == null || type_ == TileObjectType.Nothing)
        {
            this.ResetTile();
        }
        else if(type_ == TileObjectType.Player)
        {
            this.objectOnThisTile = objOnTile_;
            this.SetTileColor(this.playerOccupiedColor);
        }
        else if(type_ == TileObjectType.Enemy)
        {
            this.objectOnThisTile = objOnTile_;
            this.SetTileColor(this.enemyOccupiedColor);
        }
        else if(type_ == TileObjectType.Object)
        {
            this.objectOnThisTile = objOnTile_;
            this.SetTileColor(this.inactiveColor);
        }
        
        this.HighlightTile(false);
    }


    //Determines if this tile should be hilighed or not
    public void HighlightTile(bool highlightOn_)
    {
        //Getting the reference to our image component
        Image ourImage = this.GetComponent<Image>();

        float r = ourImage.color.r;
        float g = ourImage.color.g;
        float b = ourImage.color.b;

        //Setting the image color so that there's no alpha
        if(highlightOn_)
        {
            ourImage.color = new Color(r, g, b, 1);
        }
        //Setting the image color so that it's transparent
        else
        {
            if (this.inActionRange)
            {
                ourImage.color = new Color(r, g, b, this.atkRadiusTransparency);
            }
            else
            {
                ourImage.color = new Color(r, g, b, this.inactiveTransparency);
            }
        }
    }


    //Sets the color of this tile while retaining the same transparency
    public void SetTileColor(Color newTileColor_)
    {
        this.GetComponent<Image>().color = new Color(newTileColor_.r, newTileColor_.g, newTileColor_.b, this.GetComponent<Image>().color.a);
    }


    //Function called internally to highlight or stop highlighting tiles inside an area of effect spell
    private void HighlightEffectRadius(bool highlightOn_)
    {
        //Only works if this tile is in the action range
        if(!this.inActionRange)
        {
            return;
        }

        //If the selected action has a combat effect with a radius greater than 0, we need to highlight those tiles
        if (CombatActionPanelUI.globalReference.selectedAction != null && CombatActionPanelUI.globalReference.selectedAction.GetComponent<AttackAction>())
        {
            //Getting the reference to the attack action
            AttackAction selectedAttack = CombatActionPanelUI.globalReference.selectedAction.GetComponent<AttackAction>();

            int highestRadius = 0;
            //Looping through all effects
            foreach (AttackEffect efc in selectedAttack.effectsOnHit)
            {
                if (efc.effectRadius > highestRadius)
                {
                    highestRadius = efc.effectRadius;
                }
            }

            //If the radius is greater than 0, we need to highlight all tiles in the effect zone
            if (highestRadius > 0)
            {
                List<CombatTile> tilesInEffect = PathfindingAlgorithms.FindTilesInActionRange(this, highestRadius);
                foreach (CombatTile tile in tilesInEffect)
                {
                    //If we turn on the highlight
                    if(highlightOn_)
                    {
                        tile.HighlightTile(true);
                    }
                    //If we turn off the highlight
                    else
                    {
                        tile.HighlightTile(false);
                    }
                }
            }
        }
    }
}
