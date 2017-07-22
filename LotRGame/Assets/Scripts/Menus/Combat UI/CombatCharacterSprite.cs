using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatCharacterSprite : MonoBehaviour
{
    //Image that holds this object's sprite
    public Image spriteImage;

    //Reference to the character that this object represents
    [HideInInspector]
    public Character ourCharacter;



    //Function called in CombatManager.cs InitiateCombat function. Sets this object's position and sprite image
    public void SetSpriteOnTile(Character spriteCharacter_, Vector3 screenPos_)
    {
        //Sets this game object's position
        this.transform.position = screenPos_;
        //Sets the character reference
        this.ourCharacter = spriteCharacter_;
        //Sets this image sprite
        this.spriteImage.sprite = ourCharacter.combatSprite;
    }
}
