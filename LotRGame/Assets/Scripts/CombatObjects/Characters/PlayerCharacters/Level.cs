using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Component to determine how this PlayerCharacter.cs levels up*/

public class Level : MonoBehaviour
{
    //The current level of this character from 1 - 20
    [Range(1,20)]
    public int currentLevel = 1;

    //The number of hours it takes for this character to level up
    public int levelUpTime = 72;
    //The number of hours that have passed since this character has leveled up
    public int currentLevelUpTime = 0;

    //This character's current wellness level (determines HP gain on level up)
    public HealthCurveTypes wellness = HealthCurveTypes.Average;
}
