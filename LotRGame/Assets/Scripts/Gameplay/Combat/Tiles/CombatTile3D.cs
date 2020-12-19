using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatTile3D : MonoBehaviour
{
    //Tile's row and column
    public int row = 0;
    public int col = 0;

    //References to the tiles connected to this one
    [HideInInspector]
    public CombatTile3D up;
    [HideInInspector]
    public CombatTile3D down;
    [HideInInspector]
    public CombatTile3D left;
    [HideInInspector]
    public CombatTile3D right;

    //The movement cost of this tile (can be changed by obstacles)
    private int movementCost = 1;

    //The game object that occupies this tile
    [HideInInspector]
    public GameObject objectOnThisTile = null;
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

    //Variables used in PathfindingAlgorithms.cs for different algorithms
    public CombatTile3D prevTile = null;
    public bool hasBeenChecked = false;

    //Static reference to the Combat tile that the mouse is currently over
    public static CombatTile3D mouseOverTile;
    //If this combat tile is semi-highlighted to show an attack's radius
    [HideInInspector]
    public bool inActionRange = false;

    //The transparency of this tile when not hilighted
    [Range(0, 1f)]
    public float inactiveTransparency = 0.3f;
    //The transparency of this tile when not highlighted, but still in attack radius
    [Range(0, 1f)]
    public float atkRadiusTransparency = 0.7f;



    //Function called on the first frame this object exists
    private void Awake()
    {
        CombatManager.globalReference.GetComponent<CombatTileHandler>().AddCombatTileToGrid(this, this.row, this.col);
    }


    //Function called when this object is enabled
    private void OnEnable()
    {
        //Setting our movement cost to 1 by default. Other effects can change it later
        this.ChangeMovementCost(1);
    }


    //Function called when the player's mouse starts hovering over this tile
    public void OnPointerEnter(PointerEventData eventData_)
    {
        Debug.Log("Mouse over tile");
        //Hilighting this tile's image
        this.HighlightTile(true);

        mouseOverTile = this;

        //Highlighting any effect radius if there's a selected attack ability
        this.HighlightEffectRadius(true);
    }


    //Function called when the player's mouse is no longer over this tile
    public void OnPointerExit(PointerEventData eventData_)
    {
        Debug.Log("Mouse off tile");
        //If a character is moving right now and this tile is in the movement path, we don't stop highlighting
        if (CombatActionPanelUI.globalReference.selectedAction != null && CombatActionPanelUI.globalReference.selectedAction.GetComponent<MoveAction>())
        {
            //If this tile isn't in the movement path, this tile isn't highlighted
            if (!CombatActionPanelUI.globalReference.selectedAction.GetComponent<MoveAction>().IsTileInMovementPath(this))
            {
                this.HighlightTile(false);
            }
        }
        else
        {
            //Stops hilighting this tile's image
            this.HighlightTile(false);

            //Stops highlighting any effect radius if there's a selected attack ability
            this.HighlightEffectRadius(false);
        }

        mouseOverTile = null;
    }


    //Function called when the player's mouse clicks over this tile
    public void OnPointerClick(PointerEventData eventData_)
    {
        //If this button isn't in range of an action, nothing happens
        if (this.inActionRange)
        {
            //Telling the combat manager that the selected action is going to happen on this tile
            CombatManager.globalReference.PerformActionAtClickedTile(this);
        }
    }


    //Function called from OnEnable and externally by objects on the combat tile grid to change this tile's movement cost
    public void ChangeMovementCost(int newCost_)
    {
        if(newCost_ < 1)
        {
            this.movementCost = 1;
        }
        else
        {
            this.movementCost = newCost_;
        }
    }


    //Function called from CombatTileHandler to reset this tile so there's nothing on it and it's set to the inactive color
    public void ResetTile()
    {
        this.typeOnTile = TileObjectType.Nothing;
        this.objectOnThisTile = null;
        this.SetTileColor(this.inactiveColor);
        this.HighlightTile(false);
    }


    //Function called from CombatManager.cs to set what object is on this tile
    public void SetObjectOnTile(GameObject objOnTile_, TileObjectType type_)
    {
        this.typeOnTile = type_;

        //If no object is added
        if (objOnTile_ == null || type_ == TileObjectType.Nothing)
        {
            this.ResetTile();
        }
        else if (type_ == TileObjectType.Player)
        {
            this.objectOnThisTile = objOnTile_;
            this.SetTileColor(this.playerOccupiedColor);
        }
        else if (type_ == TileObjectType.Enemy)
        {
            this.objectOnThisTile = objOnTile_;
            this.SetTileColor(this.enemyOccupiedColor);
        }
        else if (type_ == TileObjectType.Object)
        {
            this.objectOnThisTile = objOnTile_;
            this.SetTileColor(this.inactiveColor);
        }

        this.HighlightTile(false);
    }


    //Determines if this tile should be hilighed or not
    public void HighlightTile(bool highlightOn_)
    {
        MeshRenderer mesh = this.GetComponent<MeshRenderer>();

        if (mesh.materials.Length == 0)
        {
            return;
        }

        Color tileColor = mesh.materials[0].color;
        float r = tileColor.r;
        float g = tileColor.g;
        float b = tileColor.b;

        //Setting the image color so that there's no alpha
        if (highlightOn_)
        {
            this.SetTileColor(new Color(r, g, b, 1));
        }
        //Setting the image color so that it's transparent
        else
        {
            if (this.inActionRange)
            {
                this.SetTileColor(new Color(r, g, b, this.atkRadiusTransparency));
            }
            else
            {
                this.SetTileColor(new Color(r, g, b, this.inactiveTransparency));
            }
        }
    }


    //Function called internally to highlight or stop highlighting tiles inside an area of effect spell
    private void HighlightEffectRadius(bool highlightOn_)
    {
        //Only works if this tile is in the action range
        if (!this.inActionRange)
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
                List<CombatTile3D> tilesInEffect = PathfindingAlgorithms.FindTilesInActionRange(this, highestRadius);
                foreach (CombatTile3D tile in tilesInEffect)
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


    //Function called externally to set our tile's color
    public void SetTileColor(Color newColor_)
    {
        MeshRenderer mesh = this.GetComponent<MeshRenderer>();

        if (mesh.materials.Length > 0)
        {
            mesh.materials[0].color = newColor_;
        }
    }


    //Function called from PathfindingAlgorithms to clear the variables used in this class
    public void ClearPathfinding()
    {
        this.prevTile = null;
        this.hasBeenChecked = false;
    }
}
