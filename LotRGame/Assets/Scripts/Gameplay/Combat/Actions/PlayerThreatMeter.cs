using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in EnemyCombatAI_Basic to track each character's threat level
public class PlayerThreatMeter
{
    //The character for this threat meter
    public Character characterRef;
    //The threat value for this character
    public int threatLevel = 0;


    //Constructor function for this class
    public PlayerThreatMeter(Character characterRef_, int threatLevel_)
    {
        this.characterRef = characterRef_;
        this.threatLevel = threatLevel_;
    }
}