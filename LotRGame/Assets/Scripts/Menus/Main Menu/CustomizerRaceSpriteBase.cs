using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used by CharacterCustomizer.cs so we can distinguish which sprite base to use
[System.Serializable]
public class CustomizerRaceSpriteBase
{
    //The race that this customizer uses
    public Races race;
    //The customizer that is tied to the selected race
    public SpriteCustomizer customizer;
}