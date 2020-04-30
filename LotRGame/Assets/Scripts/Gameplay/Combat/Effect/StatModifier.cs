using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in ModifyStatEffect.cs for an individual 
[System.Serializable]
public class StatModifier
{
    //Enum for which stat this modifier changes
    public StatName modifiedStat = StatName.Health;

    //Amount that the modified stat is changed
    public float amountToChange = 1;
}