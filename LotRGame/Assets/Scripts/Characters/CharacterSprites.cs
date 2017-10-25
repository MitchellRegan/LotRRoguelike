using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSprites : MonoBehaviour
{
    //The sprite package for all of this character's sprites
    public CharSpritePackage allSprites;

    /*//The prefab for which CharacterSpriteBase this character uses
    public CharacterSpriteBase spriteBase;

    //The sprite views for this character's hair
    public SpriteViews hairSprites;
    //The sprite views for this character's head
    public SpriteViews headSprites;
    //The sprite for this character's eyes
    public Sprite eyeSprite;
    //The sprite views for this character's body
    public SpriteViews bodySprites;
    //The sprite views for this character's legs
    public SpriteViews legSprites;

    //The color for this character's hair
    public Color hairColor;
    //The color for this character's skin
    public Color skinColor;*/
}

//Class used by CharacterSprites.cs, CharacterSpriteBase.cs, and SpriteCustomizer.cs
//Holds all of the character sprite info so it can be transferred
[System.Serializable]
public class CharSpritePackage
{
    //The prefab for which CharacterSpriteBase this character uses
    public CharacterSpriteBase spriteBase;

    //The sprite views for this character's hair
    public SpriteViews hairSprites;
    //The sprite views for this character's head
    public SpriteViews headSprites;
    //The sprite for this character's eyes
    public Sprite eyeSprite;
    //The sprite views for this character's body
    public SpriteViews bodySprites;
    //The sprite views for this character's right arm
    public SpriteViews rightArmSprites;
    //The sprite views for this character's left arm
    public SpriteViews leftArmSprites;
    //The sprite views for this character's legs
    public SpriteViews legSprites;

    //The color for this character's hair
    public Color hairColor;
    //The color for this character's skin
    public Color skinColor;
}