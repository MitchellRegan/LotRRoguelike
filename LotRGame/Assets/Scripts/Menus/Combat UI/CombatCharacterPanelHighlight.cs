using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CombatCharacterPanelHighlight : MonoBehaviour
{
    //Reference to this object's Image component
    private Image ourImage;

    //Bool for if this panel is currently highlighted
    private bool currentlyHighlighted = false;

    //Enum for if this tile is for a player character or enemy character
    public enum PlayerOrEnemy { Player, Enemy };
    public PlayerOrEnemy playerOrEnemy = PlayerOrEnemy.Enemy;

    //The index of the character that this panel shows
    [Range(0,9)]
    public int characterIndex = 0;

    //The default color of this sprite
    public Color defaultColor = Color.grey;
    //The color of this sprite when highlighted
    public Color highlightColor = Color.white;



    //Function called the first frame this object is alive
    private void Awake()
    {
        //Getting the reference to our image
        this.ourImage = this.GetComponent<Image>();

        //Setting our image's default color
        this.ourImage.color = this.defaultColor;
    }


	// Update is called once per frame
	private void Update ()
    {
		//If this panel is being highlighted
        if(this.currentlyHighlighted)
        {
            //If the current combat tile being moused over has no object on it, we shouldn't be highlighted
            if(CombatTile.mouseOverTile == null || CombatTile.mouseOverTile.objectOnThisTile == null)
            {
                this.currentlyHighlighted = false;
                this.ourImage.color = this.defaultColor;
            }
            //If the current combat tile has an object on it, we have to check if it's a character
            else if(CombatTile.mouseOverTile.objectOnThisTile.GetComponent<Character>())
            {
                //If this panel is for player characters
                if(this.playerOrEnemy == PlayerOrEnemy.Player)
                {
                    //If the player isn't the one we track, we're no longer highlighted
                    if (CombatTile.mouseOverTile.objectOnThisTile != CombatManager.globalReference.playerCharactersInCombat[this.characterIndex].gameObject)
                    {
                        this.currentlyHighlighted = false;
                        this.ourImage.color = this.defaultColor;
                    }
                }
                //If this panel is for enemy characters
                else
                {
                    //If the enemy isn't the one we track, we're no longer highlighted
                    if(CombatTile.mouseOverTile.objectOnThisTile != CombatManager.globalReference.enemyCharactersInCombat[this.characterIndex].gameObject)
                    {
                        this.currentlyHighlighted = false;
                        this.ourImage.color = this.defaultColor;
                    }
                }
            }
        }
        //If this panel isn't currently highlighted
        else if(CombatTile.mouseOverTile != null)
        {
            //We need to check if the current tile has an object on it
            if(CombatTile.mouseOverTile.objectOnThisTile)
            {
                //We need to make sure that object is a character
                if(CombatTile.mouseOverTile.objectOnThisTile.GetComponent<Character>())
                {
                    //If this panel is for player characters
                    if (this.playerOrEnemy == PlayerOrEnemy.Player)
                    {
                        //If the player is the one we track, we're highlighted
                        if (CombatTile.mouseOverTile.objectOnThisTile == CombatManager.globalReference.playerCharactersInCombat[this.characterIndex].gameObject)
                        {
                            this.currentlyHighlighted = true;
                            this.ourImage.color = this.highlightColor;
                        }
                    }
                    //If this panel is for enemy characters
                    else
                    {
                        //If the enemy is the one we track, we're highlighted
                        if (CombatTile.mouseOverTile.objectOnThisTile == CombatManager.globalReference.enemyCharactersInCombat[this.characterIndex].gameObject)
                        {
                            this.currentlyHighlighted = true;
                            this.ourImage.color = this.highlightColor;
                        }
                    }
                }
            }
        }
	}
}
