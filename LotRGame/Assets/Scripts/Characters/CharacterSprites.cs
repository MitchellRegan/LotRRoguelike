using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSprites : MonoBehaviour
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
    //The sprite views for this character's legs
    public SpriteViews legSprites;

    //The color for this character's hair
    public Color hairColor;
    //The color for this character's skin
    public Color skinColor;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
