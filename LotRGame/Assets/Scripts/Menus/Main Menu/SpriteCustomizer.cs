using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCustomizer : MonoBehaviour
{
    //The character whose sprites we're setting
    [HideInInspector]
    public Character charToCustomize;

    //The list of different hair styles 
    public List<SpriteViews> hairSprites;

    //The list of different head shapes
    public List<SpriteViews> headSprites;

    //The list of different eye sprites
    public List<Sprite> eyeSprites;

    //The list of different body types
    public List<SpriteViews> bodySprites;

    //The List of different leg types
    public List<SpriteViews> legSprites;

    //The list of different hair colors
    public List<Color> hairColors;

    //The list of different skin colors
    public List<Color> skinColors;


    
    //Function called to create a randomly generated sprite
    public void GenerateSpriteCharacter()
    {
        //If the character to customize is null, nothing happens
        if(this.charToCustomize == null)
        {
            return;
        }

        //Creating random indexes for each sprite
        int hairIndex = Random.Range(0, this.hairSprites.Count);
        int headIndex = Random.Range(0, this.headSprites.Count);
        int eyeIndex = Random.Range(0, this.eyeSprites.Count);
        int bodyIndex = Random.Range(0, this.bodySprites.Count);
        int legIndex = Random.Range(0, legSprites.Count);
        //Creating random indexes for each hair and skin color
        int hairColorIndex = Random.Range(0, this.hairColors.Count);
        int skinColorIndex = Random.Range(0, this.skinColors.Count);

        //Setting the character's hair
        this.charToCustomize.charSprites.hairSprites = this.hairSprites[hairIndex];
        //Setting the character's head
        this.charToCustomize.charSprites.headSprites = this.headSprites[headIndex];
        //Setting the character's eyes
        this.charToCustomize.charSprites.eyeSprite = this.eyeSprites[eyeIndex];
        //Setting the character's body
        this.charToCustomize.charSprites.bodySprites = this.bodySprites[bodyIndex];
        //Setting the character's legs
        this.charToCustomize.charSprites.legSprites = this.legSprites[legIndex];

        //Setting the character's hair color
        this.charToCustomize.charSprites.hairColor = this.hairColors[hairColorIndex];
        //Setting the character's skin color
        this.charToCustomize.charSprites.skinColor = this.skinColors[skinColorIndex];
    }


    //Function called to cycle through different hair styles
    public void CycleHair(bool goToNext_)
    {
        //Finding the index of the current hair style
        int hairIndex = this.hairSprites.IndexOf(this.charToCustomize.charSprites.hairSprites);

        //If we're cycling to the next hair style
        if(goToNext_)
        {
            int nextIndex = hairIndex + 1;

            //If the next hair index is greater than the size of the list, we cycle back around to the beginning
            if(nextIndex >= this.hairSprites.Count)
            {
                nextIndex = 0;
            }

            //Setting the character's hair
            this.charToCustomize.charSprites.hairSprites = this.hairSprites[nextIndex];
        }
        //If we're cycling to the previous hair style
        else
        {
            int prevIndex = hairIndex - 1;

            //If the previous hair index is less than 0, we cycle back around to the end of the list
            if(prevIndex < 0)
            {
                prevIndex = this.hairSprites.Count - 1;
            }

            //Setting the character's hair
            this.charToCustomize.charSprites.hairSprites = this.hairSprites[prevIndex];
        }
    }


    //Function called to cycle through different head styles
    public void CycleHead(bool goToNext_)
    {
        //Finding the index of the current head style
        int headIndex = this.headSprites.IndexOf(this.charToCustomize.charSprites.headSprites);

        //If we're cycling to the next head style
        if (goToNext_)
        {
            int nextIndex = headIndex + 1;

            //If the next head index is greater than the size of the list, we cycle back around to the beginning
            if (nextIndex >= this.headSprites.Count)
            {
                nextIndex = 0;
            }

            //Setting the character's head
            this.charToCustomize.charSprites.headSprites = this.headSprites[nextIndex];
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

            //Setting the character's head
            this.charToCustomize.charSprites.headSprites = this.headSprites[prevIndex];
        }
    }


    //Function called to cycle through different eye styles
    public void CycleEye(bool goToNext_)
    {
        //Finding the index of the current eye style
        int eyeIndex = this.eyeSprites.IndexOf(this.charToCustomize.charSprites.eyeSprite);

        //If we're cycling to the next eye style
        if (goToNext_)
        {
            int nextIndex = eyeIndex + 1;

            //If the next eye index is greater than the size of the list, we cycle back around to the beginning
            if (nextIndex >= this.eyeSprites.Count)
            {
                nextIndex = 0;
            }

            //Setting the character's eyes
            this.charToCustomize.charSprites.eyeSprite = this.eyeSprites[nextIndex];
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

            //Setting the character's eyes
            this.charToCustomize.charSprites.eyeSprite = this.eyeSprites[prevIndex];
        }
    }


    //Function called to cycle through different body styles
    public void CycleBody(bool goToNext_)
    {
        //Finding the index of the current body style
        int bodyIndex = this.bodySprites.IndexOf(this.charToCustomize.charSprites.bodySprites);

        //If we're cycling to the next body style
        if (goToNext_)
        {
            int nextIndex = bodyIndex + 1;

            //If the next body index is greater than the size of the list, we cycle back around to the beginning
            if (nextIndex >= this.bodySprites.Count)
            {
                nextIndex = 0;
            }

            //Setting the character's body
            this.charToCustomize.charSprites.bodySprites = this.bodySprites[nextIndex];
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

            //Setting the character's body
            this.charToCustomize.charSprites.bodySprites = this.bodySprites[prevIndex];
        }
    }


    //Function called to cycle through different leg styles
    public void CycleLegs(bool goToNext_)
    {
        //Finding the index of the current leg style
        int legIndex = this.legSprites.IndexOf(this.charToCustomize.charSprites.legSprites);

        //If we're cycling to the next leg style
        if (goToNext_)
        {
            int nextIndex = legIndex + 1;

            //If the next leg index is greater than the size of the list, we cycle back around to the beginning
            if (nextIndex >= this.legSprites.Count)
            {
                nextIndex = 0;
            }

            //Setting the character's legs
            this.charToCustomize.charSprites.legSprites = this.legSprites[nextIndex];
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

            //Setting the character's legs
            this.charToCustomize.charSprites.legSprites = this.legSprites[prevIndex];
        }
    }


    //Function called to cycle through different hair colors
    public void CycleHairColor(bool goToNext_)
    {
        //Finding the index of the current color
        int colorIndex = this.hairColors.IndexOf(this.charToCustomize.charSprites.hairColor);

        //If we're cycling to the next color
        if (goToNext_)
        {
            int nextIndex = colorIndex + 1;

            //If the next color index is greater than the size of the list, we cycle back around to the beginning
            if (nextIndex >= this.hairColors.Count)
            {
                nextIndex = 0;
            }

            //Setting the character's hair color
            this.charToCustomize.charSprites.hairColor = this.hairColors[nextIndex];
        }
        //If we're cycling to the previous color
        else
        {
            int prevIndex = colorIndex - 1;

            //If the previous color index is less than 0, we cycle back around to the end of the list
            if (prevIndex < 0)
            {
                prevIndex = this.hairColors.Count - 1;
            }

            //Setting the character's hair color
            this.charToCustomize.charSprites.hairColor = this.hairColors[prevIndex];
        }
    }


    //Function called to cycle through different skin colors
    public void CycleSkinColor(bool goToNext_)
    {
        //Finding the index of the current color
        int colorIndex = this.skinColors.IndexOf(this.charToCustomize.charSprites.skinColor);

        //If we're cycling to the next color
        if (goToNext_)
        {
            int nextIndex = colorIndex + 1;

            //If the next color index is greater than the size of the list, we cycle back around to the beginning
            if (nextIndex >= this.skinColors.Count)
            {
                nextIndex = 0;
            }

            //Setting the character's skin color
            this.charToCustomize.charSprites.skinColor = this.skinColors[nextIndex];
        }
        //If we're cycling to the previous color
        else
        {
            int prevIndex = colorIndex - 1;

            //If the previous color index is less than 0, we cycle back around to the end of the list
            if (prevIndex < 0)
            {
                prevIndex = this.skinColors.Count - 1;
            }

            //Setting the character's skin color
            this.charToCustomize.charSprites.skinColor = this.skinColors[prevIndex];
        }
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