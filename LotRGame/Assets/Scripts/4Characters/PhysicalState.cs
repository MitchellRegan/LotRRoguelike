﻿using System.Collections;
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
    public float currentFood = 0;
    //The number of days at which this character starts losing health from hunger
    public float maxFood = 5;


    //If false, this character doesn't require water to survive
    public bool requiresWater = true;
    //The number of days this character has gone without water
    public float currentWater = 0;
    //The number of days at which this character starts losing health from thirst
    public float maxWater = 3;


    //If false, this character doesn't require sleep
    public bool requiresSleep = true;
    //The number of days this character has gone without sleep
    public float currentSleep = 0;
    //The number of days at which this character starts losing health from lack of sleep
    public float maxSleep = 5;


    //The energy this character has based on their health, hunger, thirst, and sleep
    public float currentEnergy = 1;
    //The maximum amount of energy this character can have
    public float maxEnergy = 1;



    //Function called when this character is created
    private void Awake()
    {
        //Setting the stats to the maximum amount 
        this.currentFood = this.maxFood;
        this.currentWater = this.maxWater;
        this.currentSleep = this.maxSleep;
    }


    //Function called externally when time is advanced. This is used to lower this character's physical state over time
    public void OnTimeAdvanced()
    {
        //Finding the percentage of a day that's passed
        float daysPassed = (TimePanelUI.globalReference.hoursAdvancedPerUpdate * 1f) / 24f;

        //Doesn't lower hunger if this character doesn't eat
        if (this.requiresFood)
        {
            this.currentFood -= daysPassed;

            //Keeping the days without food from being negative
            if (this.currentFood < 0)
            {
                this.currentFood = 0;
            }
        }

        //Doesn't lower thirst if this character doesn't drink
        if (this.requiresWater)
        {
            this.currentWater -= daysPassed;

            //Keeping the days without water below the maximum
            if (this.currentWater < 0)
            {
                this.currentWater = 0;
            }
        }

        //Doesn't lower sleep if this character doesn't sleep
        if (this.requiresSleep)
        {
            this.currentSleep -= daysPassed;

            //Keeping the days without sleep below the maximum
            if (this.currentSleep < 0)
            {
                this.currentSleep = 0;
            }
        }

        //Finding this character's energy level
        this.CalculateEnergyLevel();
    }


    //Function called from OnTimeAdvance to calculate this character's energy level
    private void CalculateEnergyLevel()
    {
        //Floats that determine how much of the energy percentage bar each category fills
        float maxEnergyFromFood = 0.2f;
        float maxEnergyFromWater = 0.35f;
        float maxEnergyFromSleep = 0.45f;

        //Floats that determine the percentage of each category that need to be dropped below before it start's reducing energy
        float foodSafePercent = 0.65f;
        float waterSafePercent = 0.75f;
        float sleepSafePercent = 0.8f;

        //Variable to hold the current energy value
        float newEnergyValue = 0;

        //If this character doesn't require a category, it's importance is split with the others
        if (!this.requiresFood)
        {
            maxEnergyFromFood = 0;
            waterSafePercent = 0.4f;
            maxEnergyFromSleep = 0.6f;
        }
        else if (!this.requiresWater)
        {
            maxEnergyFromFood = 0.3f;
            waterSafePercent = 0;
            maxEnergyFromSleep = 0.7f;
        }
        else if (!this.requiresSleep)
        {
            maxEnergyFromFood = 0.4f;
            waterSafePercent = 0.6f;
            maxEnergyFromSleep = 0;
        }

        //If the current food level is below the safe percentage, only a portion of its energy is added
        if ((this.currentFood / this.maxFood) < foodSafePercent)
        {
            float energyFromFood = this.currentFood / (this.maxFood * foodSafePercent);
            newEnergyValue += energyFromFood * maxEnergyFromFood;
        }
        //If the current food level is at or above the safe percentage, all of its energy is added
        else
        {
            newEnergyValue += maxEnergyFromFood;
        }

        //If the current water level is below the safe percentage, only a portion of its energy is added
        if ((this.currentWater / this.maxWater) < waterSafePercent)
        {
            float energyFromWater = this.currentWater / (this.maxWater * waterSafePercent);
            newEnergyValue += energyFromWater * maxEnergyFromWater;
        }
        //If the current water level is at or above the safe percentage, all of its energy is added
        else
        {
            newEnergyValue += maxEnergyFromWater;
        }

        //If the current water level is below the safe percentage, only a portion of its energy is added
        if ((this.currentSleep / this.maxSleep) < sleepSafePercent)
        {
            float energyFromSleep = this.currentSleep / (this.maxSleep * sleepSafePercent);
            newEnergyValue += energyFromSleep * maxEnergyFromSleep;
        }
        //If the current water level is at or above the safe percentage, all of its energy is added
        else
        {
            newEnergyValue += maxEnergyFromSleep;
        }

        //Setting this character's energy to the new value calculated
        this.currentEnergy = newEnergyValue;
    }


    //Function called externally to eat the given piece of food (or water)
    public void EatFood(Food foodToEat_)
    {

    }
}
