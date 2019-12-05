using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in PlayerHealthManager.cs for each of the different health curves
[System.Serializable]
public class HealthCurve
{
    //The type of progression curve tied to this
    public HealthCurveTypes curveType = HealthCurveTypes.Average;

    //The minimum amount of health that can be added
    public int minHealthGiven = 0;
    //The maximum amount of health that can be added
    public int maxHealthGiven = 10;

    //The curve between the min and max health given over the number of health increases
    public AnimationCurve curveBetweenMinMax;

    //The number of dice rolled for bonus random health
    public int diceRolled = 1;

    //The sides of the dice rolled for random health
    public int numberOfDiceSides = 6;

    //Bool to determine if we roll twice and take the best result
    public bool rollTwiceTakeBest = false;
    //Bool to determine if we roll twice and take the worst result
    public bool rollTwiceTakeWorst = false;
}