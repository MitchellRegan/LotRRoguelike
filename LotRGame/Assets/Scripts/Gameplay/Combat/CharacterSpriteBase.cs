using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteBase : MonoBehaviour
{
    //The parent object for the forward view sprites
    public GameObject forwardViewParent;

    //The location for the forward hair
    public SpriteRenderer forwardHair;
    //The location for the forward head
    public SpriteRenderer forwardHead;
    //The location for the forward eye
    public SpriteRenderer forwardLeftEye;
    public SpriteRenderer forwardRightEye;
    //The location for the forward body
    public SpriteRenderer forwardBody;
    //The location for the right arm
    public SpriteRenderer forwardRightArm;
    //The location for the left arm
    public SpriteRenderer forwardLeftArm;
    //The location for the forward leg
    public SpriteRenderer forwardLegs;

    [Space(8)]

    //The parent object for the right side view sprites
    public GameObject rightSideViewParent;

    //The location for the right side hair
    public SpriteRenderer rightSideHair;
    //The location for the right side head
    public SpriteRenderer rightSideHead;
    //The location for the right side eye
    public SpriteRenderer rightSideEye;
    //The location for the right side body
    public SpriteRenderer rightSideBody;
    //The location for the right arm
    public SpriteRenderer rightSideRightArm;
    //The location for the left arm
    public SpriteRenderer rightSideLeftArm;
    //The location for the right side leg
    public SpriteRenderer rightSideLegs;

    [Space(8)]

    //The parent object for the left side view sprites
    public GameObject leftSideViewParent;

    //The location for the left side hair
    public SpriteRenderer leftSideHair;
    //The location for the left side head
    public SpriteRenderer leftSideHead;
    //The location for the left side eye
    public SpriteRenderer leftSideEye;
    //The location for the left side body
    public SpriteRenderer leftSideBody;
    //The location for the right arm
    public SpriteRenderer leftSideRightArm;
    //The location for the left arm
    public SpriteRenderer leftSideLeftArm;
    //The location for the left side leg
    public SpriteRenderer leftSideLegs;

    [Space(8)]

    //The parent object for the back view sprites
    public GameObject backViewParent;

    //The location for the back hair
    public SpriteRenderer backHair;
    //The location for the back head
    public SpriteRenderer backHead;
    //The location for the back body
    public SpriteRenderer backBody;
    //The location for the right arm
    public SpriteRenderer backRightArm;
    //The location for the left arm
    public SpriteRenderer backLeftArm;
    //The location for the back leg
    public SpriteRenderer backLegs;



    //Function called externally to set all of the sprite images for a character
    public void SetSpriteImages(CharSpritePackage cSprites_, DirectionFacing direction_ = DirectionFacing.Right)
    {
        //Setting the forward sprites
        this.forwardHair.sprite = cSprites_.hairSprites.front;
        this.forwardHead.sprite = cSprites_.headSprites.front;
        this.forwardLeftEye.sprite = cSprites_.eyeSprite;
        this.forwardRightEye.sprite = cSprites_.eyeSprite;
        this.forwardBody.sprite = cSprites_.bodySprites.front;
        this.forwardRightArm.sprite = cSprites_.rightArmSprites.front;
        this.forwardLeftArm.sprite = cSprites_.leftArmSprites.front;
        this.forwardLegs.sprite = cSprites_.legSprites.front;

        //Setting the right side sprites
        this.rightSideHair.sprite = cSprites_.hairSprites.side;
        this.rightSideHead.sprite = cSprites_.headSprites.side;
        this.rightSideEye.sprite = cSprites_.eyeSprite;
        this.rightSideBody.sprite = cSprites_.bodySprites.side;
        this.rightSideRightArm.sprite = cSprites_.rightArmSprites.side;
        this.rightSideLeftArm.sprite = cSprites_.leftArmSprites.side;
        this.rightSideLegs.sprite = cSprites_.legSprites.side;

        //Setting the left side sprites
        this.leftSideHair.sprite = cSprites_.hairSprites.side;
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

        //Setting the forward sprite color
        this.forwardHair.color = cSprites_.skinColor;
        this.forwardHead.color = cSprites_.skinColor;
        this.forwardBody.color = cSprites_.skinColor;
        this.forwardRightArm.color = cSprites_.skinColor;
        this.forwardLeftArm.color = cSprites_.skinColor;
        this.forwardLegs.color = cSprites_.skinColor;

        //Setting the back sprite color
        this.backHair.color = cSprites_.skinColor;
        this.backHead.color = cSprites_.skinColor;
        this.backBody.color = cSprites_.skinColor;
        this.backRightArm.color = cSprites_.skinColor;
        this.backLeftArm.color = cSprites_.skinColor;
        this.backLegs.color = cSprites_.skinColor;

        //Setting the left side sprite color
        this.leftSideHair.color = cSprites_.skinColor;
        this.leftSideHead.color = cSprites_.skinColor;
        this.leftSideBody.color = cSprites_.skinColor;
        this.leftSideLeftArm.color = cSprites_.skinColor;
        this.leftSideRightArm.color = cSprites_.skinColor;
        this.leftSideLegs.color = cSprites_.skinColor;

        //Setting the right side sprite color
        this.rightSideHair.color = cSprites_.skinColor;
        this.rightSideHead.color = cSprites_.skinColor;
        this.rightSideBody.color = cSprites_.skinColor;
        this.rightSideRightArm.color = cSprites_.skinColor;
        this.rightSideLeftArm.color = cSprites_.skinColor;
        this.rightSideLegs.color = cSprites_.skinColor;


        //Setting the initial direction the character is facing
        this.SetDirectionFacing(direction_);
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
