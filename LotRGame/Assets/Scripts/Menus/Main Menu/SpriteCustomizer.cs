﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteCustomizer : MonoBehaviour
{
    //The character whose sprites we're setting
    public CharacterSpriteBase charBaseToCustomizeSide;

    //The list of different hair styles 
    public List<SpriteViews> hairSprites;
    private int hairIndex = 0;

    //The list of different facial hair styles
    public List<SpriteViews> facialHairSprites;
    private int faceHairIndex = 0;

    //The list of different head shapes
    public List<SpriteViews> headSprites;
    private int headIndex = 0;

    //The list of different eye sprites
    public List<Sprite> eyeSprites;
    private int eyeIndex = 0;

    //The list of different arm sprites
    public List<SpriteViews> armSprites;
    private int armIndex = 0;

    //The list of different body types
    public List<SpriteViews> bodySprites;
    private int bodyIndex = 0;

    //The List of different leg types
    public List<SpriteViews> legSprites;
    private int legIndex = 0;

    //The list of different hair colors
    public List<Color> hairColors;
    private int hairColorIndex = 0;

    //The gradient for hair color
    public Gradient hairColorGradient;
    public Slider hairColorSlider;
    public Slider hairDarknessSlider;

    //The list of different facial hair colors
    public List<Color> facialHairColors;
    private int facialHairColorIndex = 0;

    //The list of different skin colors
    public List<Color> skinColors;
    private int skinColorIndex = 0;



    //Function called when this object is created
    private void Awake()
    {
        this.GenerateRandomCharacter();
    }


    //Function called internally to update the sprite bases using the current list indexes
    public void UpdateSpriteBase()
    {
        //Creating a new sprite package to pass to the sprite base
        CharSpritePackage newSprites = new CharSpritePackage();

        //Setting the character's hair
        newSprites.hairSprites = this.hairSprites[this.hairIndex];
        //Setting the character's facial hair
        newSprites.facialHairSprites = this.facialHairSprites[this.faceHairIndex];
        //Setting the character's head
        newSprites.headSprites = this.headSprites[this.headIndex];
        //Setting the character's eyes
        newSprites.eyeSprite = this.eyeSprites[this.eyeIndex];
        //Setting the character's arms
        newSprites.rightArmSprites = this.armSprites[this.armIndex];
        newSprites.leftArmSprites = this.armSprites[this.armIndex];
        //Setting the character's body
        newSprites.bodySprites = this.bodySprites[this.bodyIndex];
        //Setting the character's legs
        newSprites.legSprites = this.legSprites[this.legIndex];

        //Setting the character's hair color
        //newSprites.hairColor = this.hairColors[this.hairColorIndex];
        newSprites.hairColor = this.hairColorGradient.Evaluate(this.hairColorSlider.value);
        newSprites.hairColor = new Color(newSprites.hairColor.r * this.hairDarknessSlider.value,
                                            newSprites.hairColor.g * this.hairDarknessSlider.value,
                                            newSprites.hairColor.b * this.hairDarknessSlider.value,
                                            1);

        //Setting the character's facial hair color
        newSprites.facialHairColor = this.facialHairColors[this.facialHairColorIndex];
        //Setting the character's skin color
        newSprites.skinColor = this.skinColors[this.skinColorIndex];

        //Sending the new sprites to the base
        this.charBaseToCustomizeSide.SetSpriteImages(newSprites, CharacterSpriteBase.DirectionFacing.Right);
    }

    
    //Function called to create a randomly generated sprite
    public void GenerateRandomCharacter()
    {
        //Creating random indexes for each sprite
        this.hairIndex = Random.Range(0, this.hairSprites.Count);
        this.faceHairIndex = Random.Range(0, this.facialHairSprites.Count);
        this.headIndex = Random.Range(0, this.headSprites.Count);
        this.eyeIndex = Random.Range(0, this.eyeSprites.Count);
        this.bodyIndex = Random.Range(0, this.bodySprites.Count);
        this.legIndex = Random.Range(0, legSprites.Count);
        //Creating random indexes for each hair and skin color
        this.hairColorIndex = Random.Range(0, this.hairColors.Count);
        this.facialHairColorIndex = Random.Range(0, this.facialHairColors.Count);
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
            this.hairIndex += 1;

            //If the next hair index is greater than the size of the list, we cycle back around to the beginning
            if(this.hairIndex >= this.hairSprites.Count)
            {
                this.hairIndex = 0;
            }
        }
        //If we're cycling to the previous hair style
        else
        {
            this.hairIndex -= 1;

            //If the previous hair index is less than 0, we cycle back around to the end of the list
            if(this.hairIndex < 0)
            {
                this.hairIndex = this.hairSprites.Count - 1;
            }
        }

        //Updating the sprite bases
        this.UpdateSpriteBase();
    }


    //Function called to cycle through different facial hair styles
    public void CycleFacialHair(bool goToNext_)
    {
        //If we're cycling to the next facial hair style
        if (goToNext_)
        {
            this.faceHairIndex += 1;

            //If the next facial hair index is greater than the size of the list, we cycle back around to the beginning
            if (this.faceHairIndex >= this.facialHairSprites.Count)
            {
                this.faceHairIndex = 0;
            }
        }
        //If we're cycling to the previous facial hair style
        else
        {
            this.faceHairIndex -= 1;

            //If the previous facial hair index is less than 0, we cycle back around to the end of the list
            if (this.faceHairIndex < 0)
            {
                this.faceHairIndex = this.facialHairSprites.Count - 1;
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
            this.headIndex += 1;

            //If the next head index is greater than the size of the list, we cycle back around to the beginning
            if (this.headIndex >= this.headSprites.Count)
            {
                this.headIndex = 0;
            }
        }
        //If we're cycling to the previous head style
        else
        {
            this.headIndex -= 1;

            //If the previous head index is less than 0, we cycle back around to the end of the list
            if (this.headIndex < 0)
            {
                this.headIndex = this.headSprites.Count - 1;
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
            this.eyeIndex += 1;

            //If the next eye index is greater than the size of the list, we cycle back around to the beginning
            if (this.eyeIndex >= this.eyeSprites.Count)
            {
                this.eyeIndex = 0;
            }
        }
        //If we're cycling to the previous eye style
        else
        {
            this.eyeIndex -= 1;

            //If the previous eye index is less than 0, we cycle back around to the end of the list
            if (this.eyeIndex < 0)
            {
                this.eyeIndex = this.eyeSprites.Count - 1;
            }
        }

        //Updating the sprite bases
        this.UpdateSpriteBase();
    }

    
    //Function called to cycle through different arm styles
    public void CycleArms(bool goToNext_)
    {
        //If we're cycling to the next arm style
        if (goToNext_)
        {
            this.armIndex += 1;

            //If the next arm index is greater than the size of the list, we cycle back around to the beginning
            if (this.armIndex >= this.armSprites.Count)
            {
                this.armIndex = 0;
            }
        }
        //If we're cycling to the previous body style
        else
        {
            this.armIndex -= 1;

            //If the previous arm index is less than 0, we cycle back around to the end of the list
            if (this.armIndex < 0)
            {
                this.armIndex = this.armSprites.Count - 1;
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
            this.bodyIndex += 1;

            //If the next body index is greater than the size of the list, we cycle back around to the beginning
            if (this.bodyIndex >= this.bodySprites.Count)
            {
                this.bodyIndex = 0;
            }
        }
        //If we're cycling to the previous body style
        else
        {
            this.bodyIndex -= 1;

            //If the previous body index is less than 0, we cycle back around to the end of the list
            if (this.bodyIndex < 0)
            {
                this.bodyIndex = this.bodySprites.Count - 1;
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
            this.legIndex += 1;

            //If the next leg index is greater than the size of the list, we cycle back around to the beginning
            if (this.legIndex >= this.legSprites.Count)
            {
                this.legIndex = 0;
            }
        }
        //If we're cycling to the previous leg style
        else
        {
            this.legIndex -= 1;

            //If the previous leg index is less than 0, we cycle back around to the end of the list
            if (this.legIndex < 0)
            {
                this.legIndex = this.legSprites.Count - 1;
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
            this.hairColorIndex += 1;

            //If the next color index is greater than the size of the list, we cycle back around to the beginning
            if (this.hairColorIndex >= this.hairColors.Count)
            {
                this.hairColorIndex = 0;
            }
        }
        //If we're cycling to the previous color
        else
        {
            this.hairColorIndex -= 1;

            //If the previous color index is less than 0, we cycle back around to the end of the list
            if (this.hairColorIndex < 0)
            {
                this.hairColorIndex = this.hairColors.Count - 1;
            }
        }

        //Updating the sprite bases
        this.UpdateSpriteBase();
    }


    //Function called to cycle through different facial hair colors
    public void CycleFacialHairColor(bool goToNext_)
    {
        //If we're cycling to the next color
        if (goToNext_)
        {
            this.facialHairColorIndex += 1;

            //If the next color index is greater than the size of the list, we cycle back around to the beginning
            if (this.facialHairColorIndex >= this.facialHairColors.Count)
            {
                this.facialHairColorIndex = 0;
            }
        }
        //If we're cycling to the previous color
        else
        {
            this.facialHairColorIndex -= 1;

            //If the previous color index is less than 0, we cycle back around to the end of the list
            if (this.facialHairColorIndex < 0)
            {
                this.facialHairColorIndex = this.facialHairColors.Count - 1;
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
            this.skinColorIndex += 1;

            //If the next color index is greater than the size of the list, we cycle back around to the beginning
            if (this.skinColorIndex >= this.skinColors.Count)
            {
                this.skinColorIndex = 0;
            }
        }
        //If we're cycling to the previous color
        else
        {
            this.skinColorIndex -= 1;

            //If the previous color index is less than 0, we cycle back around to the end of the list
            if (this.skinColorIndex < 0)
            {
                this.skinColorIndex = this.skinColors.Count - 1;
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