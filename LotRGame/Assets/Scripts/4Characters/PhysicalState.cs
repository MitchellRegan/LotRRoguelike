using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalState : MonoBehaviour
{
    //This characters health
    public int currentHealth = 100;
    //The maximum amount of health this character can have
    public int maxHealth = 100;


    //If false, this character doesn't require food to survive
    public bool requiresFood = true;
    //The number of days this character has gone without food
    public float daysWithoutFood = 0;
    //The number of days at which this character starts losing health from hunger
    public float daysBeforeStarving = 5;


    //If false, this character doesn't require water to survive
    public bool requiresWater = true;
    //The number of days this character has gone without water
    public float daysWithoutWater = 0;
    //The number of days at which this character starts losing health from thirst
    public float daysBeforeDehydrated = 3;


    //If false, this character doesn't require sleep
    public bool requiresSleep = true;
    //The number of days this character has gone without sleep
    public float daysWithoutSleep = 0;
    //The number of days at which this character starts losing health from lack of sleep
    public float daysBeforeFatalInsomnia = 5;


    //The energy this character has based on their health, hunger, thirst, and sleep
    public int currentEnergy = 100;
    //The maximum amount of energy this character can have
    public int maxEnergy = 100;
}
