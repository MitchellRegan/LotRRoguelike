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

    //Color picker for when this tile is in range of a character's ability
    [HideInInspector]
    public Color activeColor = new Color(1, 1, 1);
    //Color picker for when this tile is not being used or in range of anything
    [HideInInspector]
    public Color unusedColor = new Color(0.75f, 0.75f, 0.75f);
    //Color picker for when this tile is highlighted by the player's mouse
    [HideInInspector]
    public Color highlightColor = Color.yellow;
    //Color picker for when this tile is highlighted by the player's mouse to designate a movement path
    [HideInInspector]
    public Color movePathColor = Color.blue;
    //Color picker for when a player character occupies this space
    [HideInInspector]
    public Color playerOccupiedColor = Color.green;
    //Color picker for when an enemy occupies this space
    [HideInInspector]
    public Color enemyOccupiedColor = Color.red;
    //Color for when a tile is blocked (can't be moved over)
    [HideInInspector]
    public Color blockedColor = Color.black;

    //Variables used in PathfindingAlgorithms.cs for different algorithms
    public CombatTile3D prevTile = null;
    public bool hasBeenChecked = false;

    //Static reference to the Combat tile that the mouse is currently over
    public static CombatTile3D mouseOverTile;
    //If this combat tile is semi-highlighted to show an attack's radius
    [HideInInspector]
    public bool inActionRange = false;



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
    private void OnMouseEnter()
    {
        //Making sure the pointer isn't over a UI object
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //Hilighting this tile's image
            this.HighlightTile(true);

            mouseOverTile = this;

            //Highlighting any effect radius if there's a selected attack ability
            this.HighlightEffectRadius(true);
        }
    }


    //Function called when the player's mouse is no longer over this tile
    private void OnMouseExit()
    {
        //Making sure the pointer isn't over a UI object
        if (!EventSystem.current.IsPointerOverGameObject())
        {
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
    }


    //Function called when the player's mouse clicks over this tile
    private void OnMouseDown()
    {
        //Making sure the pointer isn't over a UI object
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //If this button isn't in range of an action, nothing happens
            if (this.inActionRange)
            {
                //Telling the combat manager that the selected action is going to happen on this tile
                CombatManager.globalReference.PerformActionAtClickedTile(this);
            }
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
        this.inActionRange = false;
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
            this.SetTileColor(this.blockedColor);
        }

        this.HighlightTile(false);
    }


    //Determines if this tile should be hilighed or not
    public void HighlightTile(bool highlightOn_, bool inMovePath_ = false)
    {
        //Setting the image color based on what's being highlighted
        if (highlightOn_)
        {
            if (inMovePath_)
            {
                this.SetTileColor(this.movePathColor);
            }
            else
            {
                if (this.typeOnTile == TileObjectType.Enemy)
                {
                    this.SetTileColor(this.enemyOccupiedColor);
                }
                else if (this.typeOnTile == TileObjectType.Player)
                {
                    this.SetTileColor(this.playerOccupiedColor);
                }
                else if (this.typeOnTile == TileObjectType.Object)
                {
                    this.SetTileColor(this.blockedColor);
                }
                else
                {
                    this.SetTileColor(this.highlightColor);
                }
            }
        }
        //Setting the image color so that it's inactive
        else
        {
            if (this.inActionRange)
            {
                this.SetTileColor(this.activeColor);
            }
            else
            {
                this.SetTileColor(this.unusedColor);
            }
        }
    }


    //Function called internally to highlight or stop highlighting tiles inside an area of effect spell
    private void HighlightEffectRadius(bool highlightOn_)
    {
        //Only works if this tile is in the action range
        if (!this.inActionRange)
        {
            //this.SetTileColor(this.inactiveColor);
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
