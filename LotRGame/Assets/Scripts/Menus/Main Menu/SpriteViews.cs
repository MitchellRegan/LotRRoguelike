using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The class used by SpriteCustomizer.cs to set the different sprite views for a character
[System.Serializable]
public class SpriteViews
{
    public Sprite front;
    public Sprite side;
    public Sprite back;
    public BodyTypes bodyType = BodyTypes.IgnoreBodyType;
}