using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCustomizer : MonoBehaviour
{
    //The character whose sprites we're setting
    public CharacterSpriteBase charBaseToCustomizeFront;
    public CharacterSpriteBase charBaseToCustomizeSide;

    //The list of different hair styles 
    public List<SpriteViews> hairSprites;
    private int hairIndex = 0;

    //The list of different head shapes
    public List<SpriteViews> headSprites;
    private int headIndex = 0;

    //The list of different eye sprites
    public List<Sprite> eyeSprites;
    private int eyeIndex = 0;

    //The list of different body types
    public List<SpriteViews> bodySprites;
    private int bodyIndex = 0;

    //The List of different leg types
    public List<SpriteViews> legSprites;
    private int legIndex = 0;

    //The list of different hair colors
    public List<Color> hairColors;
    private int hairColorIndex = 0;

    //The list of different skin colors
    public List<Color> skinColors;
    private int skinColorIndex = 0;



    //Function called internally to update the sprite bases using the current list indexes
    private void UpdateSpriteBase()
    {
        //Creating a new sprite package to pass to the sprite base
        CharSpritePackage newSprites = new CharSpritePackage();

        //Setting the character's hair
        newSprites.hairSprites = this.hairSprites[this.hairIndex];
        //Setting the character's head
        newSprites.headSprites = this.headSprites[this.headIndex];
        //Setting the character's eyes
        newSprites.eyeSprite = this.eyeSprites[this.eyeIndex];
        //Setting the character's body
        newSprites.bodySprites = this.bodySprites[this.bodyIndex];
        //Setting the character's legs
        newSprites.legSprites = this.legSprites[this.legIndex];

        //Setting the character's hair color
        newSprites.hairColor = this.hairColors[this.hairColorIndex];
        //Setting the character's skin color
        newSprites.skinColor = this.skinColors[this.skinColorIndex];

        //Sending the new sprites to the base
        this.charBaseToCustomizeFront.SetSpriteImages(newSprites, CharacterSpriteBase.DirectionFacing.Down);
        this.charBaseToCustomizeFront.SetSpriteImages(newSprites, CharacterSpriteBase.DirectionFacing.Right);
    }

    
    //Function called to create a randomly generated sprite
    public void GenerateRandomCharacter()
    {
        //Creating random indexes for each sprite
        this.hairIndex = Random.Range(0, this.hairSprites.Count);
        this.headIndex = Random.Range(0, this.headSprites.Count);
        this.eyeIndex = Random.Range(0, this.eyeSprites.Count);
        this.bodyIndex = Random.Range(0, this.bodySprites.Count);
        this.legIndex = Random.Range(0, legSprites.Count);
        //Creating random indexes for each hair and skin color
        this.hairColorIndex = Random.Range(0, this.hairColors.Count);
        this.skinColorIndex = Random.Range(0, this.skinColors.Count);

        //Updating the sprite bases
        this.UpdateSpriteBase();
    }


    //Function called to cycle through different hair styles
    public void CycleHair(bool goToNext_)
    {
        //If we're cycling to the next hair style
        if(goToNext_)
        {
            int nextIndex = this.hairIndex + 1;

            //If the next hair index is greater than the size of the list, we cycle back around to the beginning
            if(nextIndex >= this.hairSprites.Count)
            {
                nextIndex = 0;
            }
        }
        //If we're cycling to the previous hair style
        else
        {
            int prevIndex = this.hairIndex - 1;

            //If the previous hair index is less than 0, we cycle back around to the end of the list
            if(prevIndex < 0)
            {
                prevIndex = this.hairSprites.Count - 1;
            }
        }

        //Updating the sprite bases
        this.UpdateSpriteBase();
    }


    //Function called to cycle through different head styles
    public void CycleHead(bool goToNext_)
    {
        //If we're cycling to the next head style
        if (goToNext_)
        {
            int nextIndex = headIndex + 1;

            //If the next head index is greater than the size of the list, we cycle back around to the beginning
            if (nextIndex >= this.headSprites.Count)
            {
                nextIndex = 0;
            }
        }
        //If we're cycling to the previous head style
        else
        {
            int prevIndex = headIndex - 1;

            //If the previous head index is less than 0, we cycle back around to the end of the list
            if (prevIndex < 0)
            {
                prevIndex = this.headSprites.Count - 1;
            }
        }

        //Updating the sprite bases
        this.UpdateSpriteBase();
    }


    //Function called to cycle through different eye styles
    public void CycleEye(bool goToNext_)
    {
        //If we're cycling to the next eye style
        if (goToNext_)
        {
            int nextIndex = eyeIndex + 1;

            //If the next eye index is greater than the size of the list, we cycle back around to the beginning
            if (nextIndex >= this.eyeSprites.Count)
            {
                nextIndex = 0;
            }
        }
        //If we're cycling to the previous eye style
        else
        {
            int prevIndex = eyeIndex - 1;

            //If the previous eye index is less than 0, we cycle back around to the end of the list
            if (prevIndex < 0)
            {
                prevIndex = this.eyeSprites.Count - 1;
            }
        }

        //Updating the sprite bases
        this.UpdateSpriteBase();
    }


    //Function called to cycle through different body styles
    public void CycleBody(bool goToNext_)
    {
        //If we're cycling to the next body style
        if (goToNext_)
        {
            int nextIndex = bodyIndex + 1;

            //If the next body index is greater than the size of the list, we cycle back around to the beginning
            if (nextIndex >= this.bodySprites.Count)
            {
                nextIndex = 0;
            }
        }
        //If we're cycling to the previous body style
        else
        {
            int prevIndex = bodyIndex - 1;

            //If the previous body index is less than 0, we cycle back around to the end of the list
            if (prevIndex < 0)
            {
                prevIndex = this.bodySprites.Count - 1;
            }
        }

        //Updating the sprite bases
        this.UpdateSpriteBase();
    }


    //Function called to cycle through different leg styles
    public void CycleLegs(bool goToNext_)
    {
        //If we're cycling to the next leg style
        if (goToNext_)
        {
            int nextIndex = legIndex + 1;

            //If the next leg index is greater than the size of the list, we cycle back around to the beginning
            if (nextIndex >= this.legSprites.Count)
            {
                nextIndex = 0;
            }
        }
        //If we're cycling to the previous leg style
        else
        {
            int prevIndex = legIndex - 1;

            //If the previous leg index is less than 0, we cycle back around to the end of the list
            if (prevIndex < 0)
            {
                prevIndex = this.legSprites.Count - 1;
            }
        }

        //Updating the sprite bases
        this.UpdateSpriteBase();
    }


    //Function called to cycle through different hair colors
    public void CycleHairColor(bool goToNext_)
    {
        //If we're cycling to the next color
        if (goToNext_)
        {
            int nextIndex = this.skinColorIndex + 1;

            //If the next color index is greater than the size of the list, we cycle back around to the beginning
            if (nextIndex >= this.hairColors.Count)
            {
                nextIndex = 0;
            }
        }
        //If we're cycling to the previous color
        else
        {
            int prevIndex = this.skinColorIndex - 1;

            //If the previous color index is less than 0, we cycle back around to the end of the list
            if (prevIndex < 0)
            {
                prevIndex = this.hairColors.Count - 1;
            }
        }

        //Updating the sprite bases
        this.UpdateSpriteBase();
    }


    //Function called to cycle through different skin colors
    public void CycleSkinColor(bool goToNext_)
    {
        //If we're cycling to the next color
        if (goToNext_)
        {
            int nextIndex = this.skinColorIndex + 1;

            //If the next color index is greater than the size of the list, we cycle back around to the beginning
            if (nextIndex >= this.skinColors.Count)
            {
                nextIndex = 0;
            }
        }
        //If we're cycling to the previous color
        else
        {
            int prevIndex = this.skinColorIndex - 1;

            //If the previous color index is less than 0, we cycle back around to the end of the list
            if (prevIndex < 0)
            {
                prevIndex = this.skinColors.Count - 1;
            }
        }

        //Updating the sprite bases
        this.UpdateSpriteBase();
    }
}

//The class used by SpriteCustomizer.cs to set the different sprite views for a character
[System.Serializable]
public class SpriteViews
{
    public Sprite front;
    public Sprite side;
    public Sprite back;
}