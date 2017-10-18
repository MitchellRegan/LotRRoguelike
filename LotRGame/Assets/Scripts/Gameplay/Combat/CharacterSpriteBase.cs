using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteBase : MonoBehaviour
{
    //The parent object for the forward view sprites
    public GameObject forwardViewParent;

    //The location for the forward hair location
    public Sprite forwardHair;
    //The location for the forward head location
    public Sprite forwardHead;
    //The location for the forward eye location
    public Sprite forwardLeftEye;
    public Sprite forwardRightEye;
    //The location for the forward body location
    public Sprite forwardBody;
    //The location for the forward leg position
    public Sprite forwardLegs;

    [Space(8)]

    //The parent object for the side view sprites
    public GameObject sideViewParent;

    //The location for the side hair location
    public Sprite sideHair;
    //The location for the side head location
    public Sprite sideHead;
    //The location for the side eye location
    public Sprite sideLeftEye;
    public Sprite sideRightEye;
    //The location for the side body location
    public Sprite sideBody;
    //The location for the side leg position
    public Sprite sideLegs;

    [Space(8)]

    //The parent object for the back view sprites
    public GameObject backViewParent;

    //The location for the back hair location
    public Sprite backHair;
    //The location for the back head location
    public Sprite backHead;
    //The location for the back body location
    public Sprite backBody;
    //The location for the back leg position
    public Sprite backLegs;



    //Function called externally to set all of the sprite images for a character
    public void SetSpriteImages(Character characterRef_)
    {
        .
    }
}
