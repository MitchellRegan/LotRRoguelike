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
    //The location for the forward left weapon
    public Image forwardLeftWeapon;
    //The location for the forward right weapon
    public Image forwardRightWeapon;
    //The location for the forward overlapping left weapon
    public Image forwardLeftWeaponOverlap;
    //The location for the forward overlapping right weapon
    public Image forwardRightWeaponOverlap;

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
    //The location for the right side left weapon
    public Image rightSideLeftWeapon;
    //The location for the right side right weapon
    public Image rightSideRightWeapon;
    //The location for the right side right overlapping weapon
    public Image rightSideRightWeaponOverlap;

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
    //The location for the left side left weapon
    public Image leftSideLeftWeapon;
    //The location for the left side right weapon
    public Image leftSideRightWeapon;
    //The location for the left side left overlapping weapon
    public Image leftSideLeftWeaponOverlap;

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
    //The location for the back left weapon
    public Image backLeftWeapon;
    //The location for the back right weapon
    public Image backRightWeapon;



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


        //If the character inventory given isn't null, we set all of the character armor and weapon sprites
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

            //Setting the right hand weapon sprites if there's a weapon equipped in that hand
            if(characterInventory_.rightHand != null)
            {
                //If the weapon sprite overlaps the character's hand (like if it's a shield or wrist claw)
                if(characterInventory_.rightHand.overlapCharacterHand)
                {
                    this.forwardRightWeapon.sprite = this.emptySprite;
                    this.backRightWeapon.sprite = characterInventory_.rightHand.weaponSpriteViews.back;
                    this.rightSideRightWeapon.sprite = this.emptySprite;
                    this.leftSideRightWeapon.sprite = characterInventory_.rightHand.weaponSpriteViews.side;
                    this.forwardRightWeaponOverlap.sprite = characterInventory_.rightHand.weaponSpriteViews.front;
                    this.rightSideRightWeaponOverlap.sprite = characterInventory_.rightHand.weaponSpriteViews.side;

                    //If the weapon has a reverse view
                    if (characterInventory_.rightHand.reverseView != null)
                    {
                        this.leftSideRightWeapon.sprite = characterInventory_.rightHand.reverseView;
                    }
                }
                //If the weapon doesn't overlap the character hand
                else
                {
                    this.forwardRightWeapon.sprite = characterInventory_.rightHand.weaponSpriteViews.front;
                    this.backRightWeapon.sprite = characterInventory_.rightHand.weaponSpriteViews.back;
                    this.rightSideRightWeapon.sprite = characterInventory_.rightHand.weaponSpriteViews.side;
                    this.leftSideRightWeapon.sprite = characterInventory_.rightHand.weaponSpriteViews.side;
                    this.forwardRightWeaponOverlap.sprite = this.emptySprite;
                    this.rightSideRightWeaponOverlap.sprite = this.emptySprite;

                    //If the weapon has a reverse view
                    if (characterInventory_.rightHand.reverseView != null)
                    {
                        this.leftSideRightWeapon.sprite = characterInventory_.rightHand.reverseView;
                    }
                }
            }
            //If there is no weapon, we set them to empty
            else
            {
                this.forwardRightWeapon.sprite = this.emptySprite;
                this.backRightWeapon.sprite = this.emptySprite;
                this.rightSideRightWeapon.sprite = this.emptySprite;
                this.leftSideRightWeapon.sprite = this.emptySprite;
                this.forwardRightWeaponOverlap.sprite = this.emptySprite;
                this.rightSideRightWeaponOverlap.sprite = this.emptySprite;
            }

            //Setting the left hand weapon sprites if there's a weapon equipped in that hand
            if(characterInventory_.leftHand != null)
            {
                //If the weapon sprite overlaps the character's hand (like if it's a shield or wrist claw)
                if (characterInventory_.leftHand.overlapCharacterHand)
                {
                    this.forwardLeftWeapon.sprite = this.emptySprite;
                    this.backLeftWeapon.sprite = characterInventory_.leftHand.weaponSpriteViews.back;
                    this.rightSideLeftWeapon.sprite = characterInventory_.leftHand.weaponSpriteViews.side;
                    this.leftSideLeftWeapon.sprite = this.emptySprite;
                    this.forwardLeftWeaponOverlap.sprite = characterInventory_.leftHand.weaponSpriteViews.front;
                    this.leftSideLeftWeaponOverlap.sprite = characterInventory_.leftHand.weaponSpriteViews.side;

                    //If the weapon has a reverse view
                    if(characterInventory_.leftHand.reverseView != null)
                    {
                        this.rightSideLeftWeapon.sprite = characterInventory_.leftHand.reverseView;
                    }
                }
                //If the weapon doesn't overlap the character hand
                else
                {
                    this.forwardLeftWeapon.sprite = characterInventory_.leftHand.weaponSpriteViews.front;
                    this.backLeftWeapon.sprite = characterInventory_.leftHand.weaponSpriteViews.back;
                    this.rightSideLeftWeapon.sprite = characterInventory_.leftHand.weaponSpriteViews.side;
                    this.leftSideLeftWeapon.sprite = characterInventory_.leftHand.weaponSpriteViews.side;
                    this.forwardLeftWeaponOverlap.sprite = this.emptySprite;
                    this.leftSideLeftWeaponOverlap.sprite = this.emptySprite;

                    //If the weapon has a reverse view
                    if (characterInventory_.leftHand.reverseView != null)
                    {
                        this.rightSideLeftWeapon.sprite = characterInventory_.leftHand.reverseView;
                    }
                }
            }
            //If there is no weapon, we set them to empty
            else
            {
                this.forwardLeftWeapon.sprite = this.emptySprite;
                this.backLeftWeapon.sprite = this.emptySprite;
                this.rightSideLeftWeapon.sprite = this.emptySprite;
                this.leftSideLeftWeapon.sprite = this.emptySprite;
                this.forwardLeftWeaponOverlap.sprite = this.emptySprite;
                this.leftSideLeftWeaponOverlap.sprite = this.emptySprite;
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

            //Setting the right hand weapons
            this.forwardRightWeapon.sprite = this.emptySprite;
            this.backRightWeapon.sprite = this.emptySprite;
            this.rightSideRightWeapon.sprite = this.emptySprite;
            this.leftSideRightWeapon.sprite = this.emptySprite;
            this.forwardRightWeaponOverlap.sprite = this.emptySprite;
            this.rightSideRightWeaponOverlap.sprite = this.emptySprite;

            //Setting the left hand weapons
            this.forwardLeftWeapon.sprite = this.emptySprite;
            this.backLeftWeapon.sprite = this.emptySprite;
            this.rightSideLeftWeapon.sprite = this.emptySprite;
            this.leftSideLeftWeapon.sprite = this.emptySprite;
            this.forwardLeftWeaponOverlap.sprite = this.emptySprite;
            this.leftSideLeftWeaponOverlap.sprite = this.emptySprite;
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


    //Function called externally from CombatActionPanel.cs to make all of these sprites transparent
    public void MakeSpritesTransparent()
    {
        //The color transparency that is used for the hair
        Color hairColor = new Color(this.forwardHair.color.r, this.forwardHair.color.g, this.forwardHair.color.b, 0.1f);
        //The color transparency that is used for the skin
        Color skinColor = new Color(this.forwardBody.color.r, this.forwardBody.color.g, this.forwardBody.color.b, 0.2f);
        //The color transparency that is used for armor
        Color armorColor = new Color(1, 1, 1, 0.1f);
        //The color transparency that is used to completely hide sprites
        Color invisible = new Color(0, 0, 0, 0);

        //Hiding all of the forward sprites
        this.forwardHair.color = hairColor;
        this.forwardFacialHair.color = invisible;
        this.forwardHead.color = skinColor;
        this.forwardLeftEye.color = invisible;
        this.forwardRightEye.color = invisible;
        this.forwardBody.color = skinColor;
        this.forwardRightArm.color = skinColor;
        this.forwardLeftArm.color = skinColor;
        this.forwardLegs.color = skinColor;
        this.forwardCloak.color = armorColor;
        this.forwardShoes.color = armorColor;
        this.forwardLeggings.color = armorColor;
        this.forwardChestpiece.color = armorColor;
        this.forwardLeftGlove.color = armorColor;
        this.forwardRightGlove.color = armorColor;
        this.forwardHelm.color = armorColor;
        this.forwardLeftWeapon.color = armorColor;
        this.forwardRightWeapon.color = armorColor;
        this.forwardLeftWeaponOverlap.color = armorColor;
        this.forwardRightWeaponOverlap.color = armorColor;

        //Showing all of the right side sprites
        this.rightSideHair.color = hairColor;
        this.rightSideFacialHair.color = invisible;
        this.rightSideHead.color = skinColor;
        this.rightSideEye.color = invisible;
        this.rightSideBody.color = skinColor;
        this.rightSideRightArm.color = skinColor;
        this.rightSideLeftArm.color = skinColor;
        this.rightSideLegs.color = skinColor;
        this.rightSideCloak.color = armorColor;
        this.rightSideShoes.color = armorColor;
        this.rightSideLeggings.color = armorColor;
        this.rightSideChestpiece.color = armorColor;
        this.rightSideLeftGlove.color = armorColor;
        this.rightSideRightGlove.color = armorColor;
        this.rightSideHelm.color = armorColor;
        this.rightSideLeftWeapon.color = armorColor;
        this.rightSideRightWeapon.color = armorColor;
        this.rightSideRightWeaponOverlap.color = armorColor;

        //Showing all of the left side sprites
        this.leftSideHair.color = hairColor;
        this.leftSideFacialHair.color = invisible;
        this.leftSideHead.color = skinColor;
        this.leftSideEye.color = invisible;
        this.leftSideBody.color = skinColor;
        this.leftSideRightArm.color = skinColor;
        this.leftSideLeftArm.color = skinColor;
        this.leftSideLegs.color = skinColor;
        this.leftSideCloak.color = armorColor;
        this.leftSideShoes.color = armorColor;
        this.leftSideLeggings.color = armorColor;
        this.leftSideChestpiece.color = armorColor;
        this.leftSideLeftGlove.color = armorColor;
        this.leftSideRightGlove.color = armorColor;
        this.leftSideHelm.color = armorColor;
        this.leftSideLeftWeapon.color = armorColor;
        this.leftSideRightWeapon.color = armorColor;
        this.leftSideLeftWeaponOverlap.color = armorColor;

        //Showing all of the back sprites
        this.backHair.color = hairColor;
        this.backHead.color = skinColor;
        this.backBody.color = skinColor;
        this.backRightArm.color = skinColor;
        this.backLeftArm.color = skinColor;
        this.backLegs.color = skinColor;
        this.backCloak.color = armorColor;
        this.backShoes.color = armorColor;
        this.backLeggings.color = armorColor;
        this.backChestpiece.color = armorColor;
        this.backLeftGlove.color = armorColor;
        this.backRightGlove.color = armorColor;
        this.backHelm.color = armorColor;
        this.backLeftWeapon.color = armorColor;
        this.backRightWeapon.color = armorColor;
    }

    
    //Function called externally to make all of our sprites visible again
    public void MakeSpritesVisible()
    {
        //Showing all of the forward sprites
        this.forwardHair.color = new Color(this.forwardHair.color.r, this.forwardHair.color.g, this.forwardHair.color.b, 1f);
        this.forwardFacialHair.color = new Color(this.forwardFacialHair.color.r, this.forwardFacialHair.color.g, this.forwardFacialHair.color.b, 1f);
        this.forwardHead.color = new Color(this.forwardHead.color.r, this.forwardHead.color.g, this.forwardHead.color.b, 1f);
        this.forwardLeftEye.color = Color.white;
        this.forwardRightEye.color = Color.white;
        this.forwardBody.color = new Color(this.forwardBody.color.r, this.forwardBody.color.g, this.forwardBody.color.b, 1f);
        this.forwardRightArm.color = new Color(this.forwardRightArm.color.r, this.forwardRightArm.color.g, this.forwardRightArm.color.b, 1f);
        this.forwardLeftArm.color = new Color(this.forwardLeftArm.color.r, this.forwardLeftArm.color.g, this.forwardLeftArm.color.b, 1f);
        this.forwardLegs.color = new Color(this.forwardLegs.color.r, this.forwardLegs.color.g, this.forwardLegs.color.b, 1f);
        this.forwardCloak.color = Color.white;
        this.forwardShoes.color = Color.white;
        this.forwardLeggings.color = Color.white;
        this.forwardChestpiece.color = Color.white;
        this.forwardLeftGlove.color = Color.white;
        this.forwardRightGlove.color = Color.white;
        this.forwardHelm.color = Color.white;
        this.forwardLeftWeapon.color = Color.white;
        this.forwardRightWeapon.color = Color.white;
        this.forwardLeftWeaponOverlap.color = Color.white;
        this.forwardRightWeaponOverlap.color = Color.white;

        //Showing all of the right side sprites
        this.rightSideHair.color = new Color(this.forwardHair.color.r, this.forwardHair.color.g, this.forwardHair.color.b, 1f);
        this.rightSideFacialHair.color = new Color(this.forwardFacialHair.color.r, this.forwardFacialHair.color.g, this.forwardFacialHair.color.b, 1f);
        this.rightSideHead.color = new Color(this.forwardHead.color.r, this.forwardHead.color.g, this.forwardHead.color.b, 1f);
        this.rightSideEye.color = Color.white;
        this.rightSideBody.color = new Color(this.forwardBody.color.r, this.forwardBody.color.g, this.forwardBody.color.b, 1f);
        this.rightSideRightArm.color = new Color(this.forwardRightArm.color.r, this.forwardRightArm.color.g, this.forwardRightArm.color.b, 1f);
        this.rightSideLeftArm.color = new Color(this.forwardLeftArm.color.r, this.forwardLeftArm.color.g, this.forwardLeftArm.color.b, 1f);
        this.rightSideLegs.color = new Color(this.forwardLegs.color.r, this.forwardLegs.color.g, this.forwardLegs.color.b, 1f);
        this.rightSideCloak.color = Color.white;
        this.rightSideShoes.color = Color.white;
        this.rightSideLeggings.color = Color.white;
        this.rightSideChestpiece.color = Color.white;
        this.rightSideLeftGlove.color = Color.white;
        this.rightSideRightGlove.color = Color.white;
        this.rightSideHelm.color = Color.white;
        this.rightSideLeftWeapon.color = Color.white;
        this.rightSideRightWeapon.color = Color.white;
        this.rightSideRightWeaponOverlap.color = Color.white;

        //Showing all of the left side sprites
        this.leftSideHair.color = new Color(this.forwardHair.color.r, this.forwardHair.color.g, this.forwardHair.color.b, 1f);
        this.leftSideFacialHair.color = new Color(this.forwardFacialHair.color.r, this.forwardFacialHair.color.g, this.forwardFacialHair.color.b, 1f);
        this.leftSideHead.color = new Color(this.forwardHead.color.r, this.forwardHead.color.g, this.forwardHead.color.b, 1f);
        this.leftSideEye.color = Color.white;
        this.leftSideBody.color = new Color(this.forwardBody.color.r, this.forwardBody.color.g, this.forwardBody.color.b, 1f);
        this.leftSideRightArm.color = new Color(this.forwardRightArm.color.r, this.forwardRightArm.color.g, this.forwardRightArm.color.b, 1f);
        this.leftSideLeftArm.color = new Color(this.forwardLeftArm.color.r, this.forwardLeftArm.color.g, this.forwardLeftArm.color.b, 1f);
        this.leftSideLegs.color = new Color(this.forwardLegs.color.r, this.forwardLegs.color.g, this.forwardLegs.color.b, 1f);
        this.leftSideCloak.color = Color.white;
        this.leftSideShoes.color = Color.white;
        this.leftSideLeggings.color = Color.white;
        this.leftSideChestpiece.color = Color.white;
        this.leftSideLeftGlove.color = Color.white;
        this.leftSideRightGlove.color = Color.white;
        this.leftSideHelm.color = Color.white;
        this.leftSideLeftWeapon.color = Color.white;
        this.leftSideRightWeapon.color = Color.white;
        this.leftSideLeftWeaponOverlap.color = Color.white;

        //Showing all of the back sprites
        this.backHair.color = new Color(this.forwardHair.color.r, this.forwardHair.color.g, this.forwardHair.color.b, 1f);
        this.backHead.color = new Color(this.forwardHead.color.r, this.forwardHead.color.g, this.forwardHead.color.b, 1f);
        this.backBody.color = new Color(this.forwardBody.color.r, this.forwardBody.color.g, this.forwardBody.color.b, 1f);
        this.backRightArm.color = new Color(this.forwardRightArm.color.r, this.forwardRightArm.color.g, this.forwardRightArm.color.b, 1f);
        this.backLeftArm.color = new Color(this.forwardLeftArm.color.r, this.forwardLeftArm.color.g, this.forwardLeftArm.color.b, 1f);
        this.backLegs.color = new Color(this.forwardLegs.color.r, this.forwardLegs.color.g, this.forwardLegs.color.b, 1f);
        this.backCloak.color = Color.white;
        this.backShoes.color = Color.white;
        this.backLeggings.color = Color.white;
        this.backChestpiece.color = Color.white;
        this.backLeftGlove.color = Color.white;
        this.backRightGlove.color = Color.white;
        this.backHelm.color = Color.white;
        this.backLeftWeapon.color = Color.white;
        this.backRightWeapon.color = Color.white;
    }
}
