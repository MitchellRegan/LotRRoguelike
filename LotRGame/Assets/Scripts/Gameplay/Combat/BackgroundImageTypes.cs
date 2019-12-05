using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in CombatManager.cs to set the combat background image
[System.Serializable]
public class BackgroundImageTypes
{
    //The type of land tile that combat is happening on
    public LandType tileType = LandType.Empty;
    //The background image associated with this tile type
    public Sprite backgroundImage;
}