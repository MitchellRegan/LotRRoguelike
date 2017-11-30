using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSpriteBase : MonoBehaviour
{
    //The character that this sprite base references
    [HideInInspector]
    public Character ourCharacter;

    //The empty sprite for when there's no armor sprite to show
    public Sprite emptySprite;

    [Space(8)]

    //The parent object for the forward view sprites
    public GameObject forwardViewParent;

    //The location for the forward hair
    public Image forwardHair;
    //The location for the forward facial hair
    public Image forwardFacialHair;
    //The location for the forward head
    public Image forwardHead;
    //The location for the forward eye
    public Image forwardLeftEye;
    public Image forwardRightEye;
    //The location for the forward body
    public Image forwardBody;
    //The location for the right arm
    public Image forwardRightArm;
    //The location for the left arm
    public Image forwardLeftArm;
    //The location for the forward leg
    public Image forwardLegs;
    //The location for the forward cloak
    public Image forwardCloak;
    //The location for the forward shoes
    public Image forwardShoes;
    //The location for the forward leggings
    public Image forwardLeggings;
    //The location for the forward chestpiece
    public Image forwardChestpiece;
    //The location for the forward left glove
    public Image forwardLeftGlove;
    //The location for the forward right glove
    public Image forwardRightGlove;
    //The location for the forward helm
    public Image forwardHelm;

    [Space(8)]

    //The parent object for the right side view sprites
    public GameObject rightSideViewParent;

    //The location for the right side hair
    public Image rightSideHair;
    //The location for the right side facial hair
    public Image rightSideFacialHair;
    //The location for the right side head
    public Image rightSideHead;
    //The location for the right side eye
    public Image rightSideEye;
    //The location for the right side body
    public Image rightSideBody;
    //The location for the right arm
    public Image rightSideRightArm;
    //The location for the left arm
    public Image rightSideLeftArm;
    //The location for the right side leg
    public Image rightSideLegs;
    //The location for the right side cloak
    public Image rightSideCloak;
    //The location for the right side shoes
    public Image rightSideShoes;
    //The location for the right side leggings
    public Image rightSideLeggings;
    //The location for the right side chestpiece
    public Image rightSideChestpiece;
    //The location for the right side left glove
    public Image rightSideLeftGlove;
    //The location for the right side right glove
    public Image rightSideRightGlove;
    //The location for the right side helm
    public Image rightSideHelm;

    [Space(8)]

    //The parent object for the left side view sprites
    public GameObject leftSideViewParent;

    //The location for the left side hair
    public Image leftSideHair;
    //The location for the left side facial hair
    public Image leftSideFacialHair;
    //The location for the left side head
    public Image leftSideHead;
    //The location for the left side eye
    public Image leftSideEye;
    //The location for the left side body
    public Image leftSideBody;
    //The location for the right arm
    public Image leftSideRightArm;
    //The location for the left arm
    public Image leftSideLeftArm;
    //The location for the left side leg
    public Image leftSideLegs;
    //The location for the left side cloak
    public Image leftSideCloak;
    //The location for the left side shoes
    public Image leftSideShoes;
    //The location for the left side leggings
    public Image leftSideLeggings;
    //The location for the left side chestpiece
    public Image leftSideChestpiece;
    //The location for the left side left glove
    public Image leftSideLeftGlove;
    //The location for the left side right glove
    public Image leftSideRightGlove;
    //The location for the left side helm
    public Image leftSideHelm;

    [Space(8)]

    //The parent object for the back view sprites
    public GameObject backViewParent;

    //The location for the back hair
    public Image backHair;
    //The location for the back head
    public Image backHead;
    //The location for the back body
    public Image backBody;
    //The location for the right arm
    public Image backRightArm;
    //The location for the left arm
    public Image backLeftArm;
    //The location for the back leg
    public Image backLegs;
    //The location for the back cloak
    public Image backCloak;
    //The location for the back shoes
    public Image backShoes;
    //The location for the back leggings
    public Image backLeggings;
    //The location for the back chestpiece
    public Image backChestpiece;
    //The location for the back left glove
    public Image backLeftGlove;
    //The location for the back right glove
    public Image backRightGlove;
    //The location for the back helm
    public Image backHelm;



    //Function called externally to set all of the sprite images for a character
    public void SetSpriteImages(CharSpritePackage cSprites_, Inventory characterInventory_)
    {
        //Setting the forward sprites
        this.forwardHair.sprite = cSprites_.hairSprites.front;
        this.forwardFacialHair.sprite = cSprites_.facialHairSprites.front;
        this.forwardHead.sprite = cSprites_.headSprites.front;
        this.forwardLeftEye.sprite = cSprites_.eyeSprite;
        this.forwardRightEye.sprite = cSprites_.eyeSprite;
        this.forwardBody.sprite = cSprites_.bodySprites.front;
        this.forwardRightArm.sprite = cSprites_.rightArmSprites.front;
        this.forwardLeftArm.sprite = cSprites_.leftArmSprites.front;
        this.forwardLegs.sprite = cSprites_.legSprites.front;

        //Setting the right side sprites
        this.rightSideHair.sprite = cSprites_.hairSprites.side;
        this.rightSideFacialHair.sprite = cSprites_.facialHairSprites.side;
        this.rightSideHead.sprite = cSprites_.headSprites.side;
        this.rightSideEye.sprite = cSprites_.eyeSprite;
        this.rightSideBody.sprite = cSprites_.bodySprites.side;
        this.rightSideRightArm.sprite = cSprites_.rightArmSprites.side;
        this.rightSideLeftArm.sprite = cSprites_.leftArmSprites.side;
        this.rightSideLegs.sprite = cSprites_.legSprites.side;

        //Setting the left side sprites
        this.leftSideHair.sprite = cSprites_.hairSprites.side;
        this.leftSideFacialHair.sprite = cSprites_.facialHairSprites.side;
        this.leftSideHead.sprite = cSprites_.headSprites.side;
        this.leftSideEye.sprite = cSprites_.eyeSprite;
        this.leftSideBody.sprite = cSprites_.bodySprites.side;
        this.leftSideRightArm.sprite = cSprites_.rightArmSprites.side;
        this.leftSideLeftArm.sprite = cSprites_.leftArmSprites.side;
        this.leftSideLegs.sprite = cSprites_.legSprites.side;

        //Setting the back sprites
        this.backHair.sprite = cSprites_.hairSprites.back;
        this.backHead.sprite = cSprites_.headSprites.back;
        this.backBody.sprite = cSprites_.bodySprites.back;
        this.backRightArm.sprite = cSprites_.rightArmSprites.side;
        this.backLeftArm.sprite = cSprites_.leftArmSprites.side;
        this.backLegs.sprite = cSprites_.legSprites.back;

        //Setting the hair color
        this.forwardHair.color = cSprites_.hairColor;
        this.leftSideHair.color = cSprites_.hairColor;
        this.rightSideHair.color = cSprites_.hairColor;
        this.backHair.color = cSprites_.hairColor;

        //Setting the facial hair color
        this.forwardFacialHair.color = cSprites_.facialHairColor;
        this.leftSideFacialHair.color = cSprites_.facialHairColor;
        this.rightSideFacialHair.color = cSprites_.facialHairColor;

        //Setting the forward sprite color
        this.forwardHead.color = cSprites_.skinColor;
        this.forwardBody.color = cSprites_.skinColor;
        this.forwardRightArm.color = cSprites_.skinColor;
        this.forwardLeftArm.color = cSprites_.skinColor;
        this.forwardLegs.color = cSprites_.skinColor;

        //Setting the back sprite color
        this.backHead.color = cSprites_.skinColor;
        this.backBody.color = cSprites_.skinColor;
        this.backRightArm.color = cSprites_.skinColor;
        this.backLeftArm.color = cSprites_.skinColor;
        this.backLegs.color = cSprites_.skinColor;

        //Setting the left side sprite color
        this.leftSideHead.color = cSprites_.skinColor;
        this.leftSideBody.color = cSprites_.skinColor;
        this.leftSideLeftArm.color = cSprites_.skinColor;
        this.leftSideRightArm.color = cSprites_.skinColor;
        this.leftSideLegs.color = cSprites_.skinColor;

        //Setting the right side sprite color
        this.rightSideHead.color = cSprites_.skinColor;
        this.rightSideBody.color = cSprites_.skinColor;
        this.rightSideRightArm.color = cSprites_.skinColor;
        this.rightSideLeftArm.color = cSprites_.skinColor;
        this.rightSideLegs.color = cSprites_.skinColor;


        //If the character inventory given isn't null, we set all of the character armor sprites
        if(characterInventory_ != null)
        {
            //Setting the helm sprites if a helm is equipped
            if(characterInventory_.helm != null)
            {
                this.forwardHelm.sprite = characterInventory_.helm.armorSpriteViews[0].front;
                this.backHelm.sprite = characterInventory_.helm.armorSpriteViews[0].back;
                this.rightSideHelm.sprite = characterInventory_.helm.armorSpriteViews[0].side;
                this.leftSideHelm.sprite = characterInventory_.helm.armorSpriteViews[0].side;

                //If this helm covers up the character's hair, we set the hair sprite to empty
                if(characterInventory_.helm.replaceCharacterSprite)
                {
                    this.forwardHair.sprite = this.emptySprite;
                    this.backHair.sprite = this.emptySprite;
                    this.rightSideHair.sprite = this.emptySprite;
                    this.leftSideHair.sprite = this.emptySprite;
                }
            }
            //If there is no helm, we set them to empty
            else
            {
                this.forwardHelm.sprite = this.emptySprite;
                this.backHelm.sprite = this.emptySprite;
                this.rightSideHelm.sprite = this.emptySprite;
                this.leftSideHelm.sprite = this.emptySprite;
            }

            //Setting the chestpiece sprites if a chestpiece is equipped
            if(characterInventory_.chestPiece != null)
            {
                //Looping through to find the correct sprite view to match our character's body type
                SpriteViews bodySpriteView = null;
                foreach(SpriteViews sv in characterInventory_.chestPiece.armorSpriteViews)
                {
                    //If we find the correct sprite view for our body type
                    if(sv.bodyType == cSprites_.bodySprites.bodyType)
                    {
                        //We set the sprite view and break the loop
                        bodySpriteView = sv;
                        break;
                    }
                }

                //If we found a correct sprite view
                if(bodySpriteView != null)
                {
                    this.forwardChestpiece.sprite = bodySpriteView.front;
                    this.backChestpiece.sprite = bodySpriteView.back;
                    this.rightSideChestpiece.sprite = bodySpriteView.side;
                    this.leftSideChestpiece.sprite = bodySpriteView.side;

                    //If this chestpiece covers up the character's torso, we hide them
                    if(characterInventory_.chestPiece.replaceCharacterSprite)
                    {
                        this.forwardBody.sprite = this.emptySprite;
                        this.backBody.sprite = this.emptySprite;
                        this.rightSideBody.sprite = this.emptySprite;
                        this.leftSideBody.sprite = this.emptySprite;
                    }
                }
                //If we didn't find the correct sprite view
                else
                {
                    throw new System.Exception("ERROR! CharacterSpriteBase.SetSpriteImages, No valid body type for chestpiece");
                }
            }
            //If there's no chestpiece, we set them to empty
            else
            {
                this.forwardChestpiece.sprite = this.emptySprite;
                this.backChestpiece.sprite = this.emptySprite;
                this.rightSideChestpiece.sprite = this.emptySprite;
                this.leftSideChestpiece.sprite = this.emptySprite;
            }

            //Setting the leggings sprites if leggings are equipped
            if (characterInventory_.leggings != null)
            {
                //Looping through to find the correct sprite view to match our character's body type
                SpriteViews legSpriteView = null;
                foreach (SpriteViews sv in characterInventory_.leggings.armorSpriteViews)
                {
                    //If we find the correct sprite view for our body type
                    if (sv.bodyType == cSprites_.legSprites.bodyType)
                    {
                        //We set the sprite view and break the loop
                        legSpriteView = sv;
                        break;
                    }
                }

                //If we found a correct sprite view
                if (legSpriteView != null)
                {
                    this.forwardLeggings.sprite = legSpriteView.front;
                    this.backLeggings.sprite = legSpriteView.back;
                    this.rightSideLeggings.sprite = legSpriteView.side;
                    this.leftSideLeggings.sprite = legSpriteView.side;
                }
                //If we didn't find the correct sprite view
                else
                {
                    throw new System.Exception("ERROR! CharacterSpriteBase.SetSpriteImages, No valid body type for leggings");
                }
            }
            //If there's no leggings, we set them to empty
            else
            {
                this.forwardLeggings.sprite = this.emptySprite;
                this.backLeggings.sprite = this.emptySprite;
                this.rightSideLeggings.sprite = this.emptySprite;
                this.leftSideLeggings.sprite = this.emptySprite;
            }
            
            //Setting the shoe sprites if shoes are equipped
            if (characterInventory_.shoes != null)
            {
                //Looping through to find the correct sprite view to match our character's body type
                SpriteViews feetSpriteView = null;
                foreach (SpriteViews sv in characterInventory_.shoes.armorSpriteViews)
                {
                    //If we find the correct sprite view for our body type
                    if (sv.bodyType == cSprites_.legSprites.bodyType)
                    {
                        //We set the sprite view and break the loop
                        feetSpriteView = sv;
                        break;
                    }
                }

                //If we found a correct sprite view
                if (feetSpriteView != null)
                {
                    this.forwardShoes.sprite = feetSpriteView.front;
                    this.backShoes.sprite = feetSpriteView.back;
                    this.rightSideShoes.sprite = feetSpriteView.side;
                    this.leftSideShoes.sprite = feetSpriteView.side;
                }
                //If we didn't find the correct sprite view
                else
                {
                    throw new System.Exception("ERROR! CharacterSpriteBase.SetSpriteImages, No valid body type for shoes");
                }
            }
            //If there's no shoes, we set them to empty
            else
            {
                this.forwardShoes.sprite = this.emptySprite;
                this.backShoes.sprite = this.emptySprite;
                this.rightSideShoes.sprite = this.emptySprite;
                this.leftSideShoes.sprite = this.emptySprite;
            }

            //Setting the glove sprites if gloves are equipped
            if(characterInventory_.gloves != null)
            {
                this.forwardRightGlove.sprite = characterInventory_.gloves.armorSpriteViews[0].front;
                this.backRightGlove.sprite = characterInventory_.gloves.armorSpriteViews[0].back;
                this.rightSideRightGlove.sprite = characterInventory_.gloves.armorSpriteViews[0].side;
                this.leftSideRightGlove.sprite = characterInventory_.gloves.armorSpriteViews[0].side;

                this.forwardLeftGlove.sprite = characterInventory_.gloves.armorSpriteViews[0].front;
                this.backLeftGlove.sprite = characterInventory_.gloves.armorSpriteViews[0].back;
                this.rightSideLeftGlove.sprite = characterInventory_.gloves.armorSpriteViews[0].side;
                this.leftSideLeftGlove.sprite = characterInventory_.gloves.armorSpriteViews[0].side;

                //If these gloves cover up the character's hands, we set the hand sprites to empty
                if (characterInventory_.gloves.replaceCharacterSprite)
                {
                    this.forwardRightArm.sprite = this.emptySprite;
                    this.backRightArm.sprite = this.emptySprite;
                    this.rightSideRightArm.sprite = this.emptySprite;
                    this.leftSideRightArm.sprite = this.emptySprite;

                    this.forwardLeftArm.sprite = this.emptySprite;
                    this.backLeftArm.sprite = this.emptySprite;
                    this.rightSideLeftArm.sprite = this.emptySprite;
                    this.leftSideLeftArm.sprite = this.emptySprite;
                }
            }
            //If there are no gloves, we set them to empty
            else
            {
                this.forwardRightGlove.sprite = this.emptySprite;
                this.backRightGlove.sprite = this.emptySprite;
                this.rightSideRightGlove.sprite = this.emptySprite;
                this.leftSideRightGlove.sprite = this.emptySprite;

                this.forwardLeftGlove.sprite = this.emptySprite;
                this.backLeftGlove.sprite = this.emptySprite;
                this.rightSideLeftGlove.sprite = this.emptySprite;
                this.leftSideLeftGlove.sprite = this.emptySprite;
            }

            //Setting the cloak sprites if a cloak is equipped
            if(characterInventory_.cloak != null)
            {
                this.forwardCloak.sprite = characterInventory_.cloak.armorSpriteViews[0].front;
                this.backCloak.sprite = characterInventory_.cloak.armorSpriteViews[0].back;
                this.rightSideCloak.sprite = characterInventory_.cloak.armorSpriteViews[0].side;
                this.leftSideCloak.sprite = characterInventory_.cloak.armorSpriteViews[0].side;
            }
            //If there is no cloak, we set them to empty
            else
            {
                this.forwardCloak.sprite = this.emptySprite;
                this.backCloak.sprite = this.emptySprite;
                this.rightSideCloak.sprite = this.emptySprite;
                this.leftSideCloak.sprite = this.emptySprite;
            }
        }
        //If there's no character inventory given, all of the armor sprites are empty
        else
        {
            //Setting the helms
            this.forwardHelm.sprite = this.emptySprite;
            this.backHelm.sprite = this.emptySprite;
            this.rightSideHelm.sprite = this.emptySprite;
            this.leftSideHelm.sprite = this.emptySprite;

            //Setting the chestpieces
            this.forwardChestpiece.sprite = this.emptySprite;
            this.backChestpiece.sprite = this.emptySprite;
            this.rightSideChestpiece.sprite = this.emptySprite;
            this.leftSideChestpiece.sprite = this.emptySprite;

            //Setting the leggings
            this.forwardLeggings.sprite = this.emptySprite;
            this.backLeggings.sprite = this.emptySprite;
            this.rightSideLeggings.sprite = this.emptySprite;
            this.leftSideLeggings.sprite = this.emptySprite;

            //Setting the shoes
            this.forwardShoes.sprite = this.emptySprite;
            this.backShoes.sprite = this.emptySprite;
            this.rightSideShoes.sprite = this.emptySprite;
            this.leftSideShoes.sprite = this.emptySprite;

            //Setting the right gloves
            this.forwardRightGlove.sprite = this.emptySprite;
            this.backRightGlove.sprite = this.emptySprite;
            this.rightSideRightGlove.sprite = this.emptySprite;
            this.leftSideRightGlove.sprite = this.emptySprite;

            //Setting the left gloves
            this.forwardLeftGlove.sprite = this.emptySprite;
            this.backLeftGlove.sprite = this.emptySprite;
            this.rightSideLeftGlove.sprite = this.emptySprite;
            this.leftSideLeftGlove.sprite = this.emptySprite;

            //Setting the cloaks
            this.forwardCloak.sprite = this.emptySprite;
            this.backCloak.sprite = this.emptySprite;
            this.rightSideCloak.sprite = this.emptySprite;
            this.leftSideCloak.sprite = this.emptySprite;
        }
    }


    //Function called externally to set the direction this sprite base is facing
    public enum DirectionFacing { Left, Right, Up, Down};
    public void SetDirectionFacing(DirectionFacing direction_)
    {
        switch(direction_)
        {
            //If we look up, we see the character from the back
            case DirectionFacing.Up:
                this.backViewParent.SetActive(true);
                this.rightSideViewParent.SetActive(false);
                this.leftSideViewParent.SetActive(false);
                this.forwardViewParent.SetActive(false);
                break;

            //If we look down, we see the character from the front
            case DirectionFacing.Down:
                this.backViewParent.SetActive(false);
                this.rightSideViewParent.SetActive(false);
                this.leftSideViewParent.SetActive(false);
                this.forwardViewParent.SetActive(true);
                break;

            //If we look left, we see the character from the left
            case DirectionFacing.Left:
                this.backViewParent.SetActive(false);
                this.rightSideViewParent.SetActive(false);
                this.leftSideViewParent.SetActive(true);
                this.forwardViewParent.SetActive(false);
                break;

            //If we look right, we see the character from the right
            case DirectionFacing.Right:
                this.backViewParent.SetActive(false);
                this.rightSideViewParent.SetActive(true);
                this.leftSideViewParent.SetActive(false);
                this.forwardViewParent.SetActive(false);
                break;
        }
    }
}
