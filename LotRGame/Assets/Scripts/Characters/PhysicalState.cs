using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PhysicalState : MonoBehaviour
{
    //This characters health
    public int currentHealth = 20;
    //The maximum amount of health this character can have
    public int maxHealth = 20;

    //The type of health progression curve that determines how much health they get from LevelUpManager.cs
    public HealthCurveTypes startingHealthCurve = HealthCurveTypes.Average;


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
    
    //Array to hold this character's health curve value for each level up
    [HideInInspector]
    public int[] healthCurveLevels = new int[4] { 0, 0, 0, 0 };

    //Floats to track the highest percent of health, food, water, and sleep over the past 24 hours
    [HideInInspector]
    public float highestHealthPercent = 0;
    [HideInInspector]
    public float highestFoodPercent = 0;
    [HideInInspector]
    public float highestWaterPercent = 0;
    [HideInInspector]
    public float highestSleepPercent = 0;

    //Lists of percentages for health, food, water, and sleep over the past few days to influence the character's health curve
    [HideInInspector]
    public List<float> trackingHealthPercents;
    [HideInInspector]
    public List<float> trackingFoodPercents;
    [HideInInspector]
    public List<float> trackingWaterPercents;
    [HideInInspector]
    public List<float> trackingSleepPercents;
    //The number of days that we track percentages
    private int daysToTrackPercentages = 7;



    //Function called when this character is created
    private void Awake()
    {
        //Setting the stats to the maximum amount 
        this.currentFood = this.maxFood;
        this.currentWater = this.maxWater;
        this.currentSleep = this.maxSleep;

        //Initializing our arrays
        healthCurveLevels = new int[4] {0,0,0,0};
        this.trackingHealthPercents = new List<float>();
        this.trackingFoodPercents = new List<float>();
        this.trackingWaterPercents = new List<float>();
        this.trackingSleepPercents = new List<float>();
    }


    //Function called externally when time is advanced. This is used to lower this character's physical state over time
    public void OnTimeAdvanced(int timePassed_)
    {
        //Finding the percentage of a day that's passed
        float daysPassed = (timePassed_ * 1f) / 24f;

        //Finding the highest health, food, water, and sleep percents for the day
        this.FindHighestPercents();

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
        

        //If the time passed will move us to the next day
        if(TimePanelUI.globalReference.timeOfDay + timePassed_ >= 25)
        {
            //We track the health, food, water, and sleep percentages for the last day
            this.TrackPercentages();
        }
    }


    //Function called from OnTimeAdvance to find the highest percentage of health, food, water, and sleep for the day
    private void FindHighestPercents()
    {
        //Floats to find the current character percentage values for health, food, water, and sleep
        float hp = (this.currentHealth * 1f) / (this.maxHealth * 1f);
        float fd = this.currentFood / this.maxFood;
        float wt = this.currentWater / this.maxWater;
        float sl = this.currentSleep / this.maxSleep;

        //Checking to see if the current percentages are higher than the current highest for the day
        if(hp > this.highestHealthPercent)
        {
            this.highestHealthPercent = hp;
        }
        if(fd > this.highestFoodPercent)
        {
            this.highestFoodPercent = fd;
        }
        if(wt > this.highestWaterPercent)
        {
            this.highestWaterPercent = wt;
        }
        if(sl > this.highestSleepPercent)
        {
            this.highestSleepPercent = sl;
        }
    }


    //Function called from OnTimeAdvance to track the health, food, water, and sleep percentages for the past day
    private void TrackPercentages()
    {
        //Adding the highest percents to the end of each list
        this.trackingHealthPercents.Add(this.highestHealthPercent);
        this.trackingFoodPercents.Add(this.highestFoodPercent);
        this.trackingWaterPercents.Add(this.highestWaterPercent);
        this.trackingSleepPercents.Add(this.highestSleepPercent);

        //Making sure the lists are only tracking the maximum number of days needed
        if(this.trackingHealthPercents.Count > this.daysToTrackPercentages)
        {
            this.trackingHealthPercents.RemoveAt(0);
        }
        if(this.trackingFoodPercents.Count > this.daysToTrackPercentages)
        {
            this.trackingFoodPercents.RemoveAt(0);
        }
        if(this.trackingWaterPercents.Count > this.daysToTrackPercentages)
        {
            this.trackingWaterPercents.RemoveAt(0);
        }
        if(this.trackingSleepPercents.Count > this.daysToTrackPercentages)
        {
            this.trackingSleepPercents.RemoveAt(0);
        }

        Debug.Log("Day Passed. Max HP%: " + this.highestHealthPercent + ", Max Food%: " + this.highestFoodPercent +
            ", Max Water%: " + this.highestWaterPercent + ", Max Sleep%: " + this.highestSleepPercent);

        //Resetting the highest percentages for health, food, water, and sleep for the day
        this.highestHealthPercent = 0;
        this.highestFoodPercent = 0;
        this.highestWaterPercent = 0;
        this.highestSleepPercent = 0;
    }


    //Function called externally to eat the given piece of food (or water)
    public void EatFood(Food foodToEat_)
    {
        //Adding this food's hunger and thirst restored to this character's food and water
        this.currentFood += foodToEat_.hungerRestored;
        this.currentWater += foodToEat_.thirstRestored;

        //Making sure that the food and water don't exceed the max amounts
        if(this.currentFood > this.maxFood)
        {
            this.currentFood = this.maxFood;
        }
        if(this.currentWater > this.maxWater)
        {
            this.currentWater = this.maxWater;
        }
    }


    //Function called externally to make this character sleep
    public void Sleep()
    {
        
    }


    //Function called externally to deal damage to this character
    public void DamageCharacter(int damageTaken_)
    {
        //Making sure the amount dealt isn't negative, because healing should only happen in HealCharacter
        if(damageTaken_ <= 0)
        {
            return;
        }

        this.currentHealth -= damageTaken_;

        //If this character's health drops to 0, they are dead
        if(this.currentHealth <= 0)
        {
            this.currentHealth = 0;

            //Creating a character death event to tell other scripts about this death
            EVTData deathEVTData = new EVTData();
            deathEVTData.characterDeath = new CharacterDeathEVT(this.GetComponent<Character>());

            //Dispatching the event to the EventManager
            EventManager.TriggerEvent(CharacterDeathEVT.eventNum, deathEVTData);
        }
    }


    //Function called externally to heal damage on this character
    public void HealCharacter(int healthRestored_)
    {
        //Making sure the amount dealt isn't negative, because damage should only happen in DamageCharacter
        if(healthRestored_ < 0)
        {
            return;
        }

        this.currentHealth += healthRestored_;

        //Making sure the health doesn't exceed the maximum
        if(this.currentHealth > this.maxHealth)
        {
            this.currentHealth = this.maxHealth;
        }
    }


    //Function called externally from PartyCreator.cs to generate this character's starting hit dice
    //They get a few levels worth of max health rolls and 1 random health roll on top
    public void GenerateStartingHitDice()
    {
        //Int to hold the amount of health for this character's starting health curve
        int hitDie = 0;

        //Int to hold the random die roll based on the starting health curve
        int hitRoll = 0;

        //Switch for this character's starting health curve to get the max roll value
        switch(this.startingHealthCurve)
        {
            case HealthCurveTypes.Strong:
                hitDie = 12;
                hitRoll = Random.Range(1, 13);
                //Rolling again to see if the second roll is higher
                int strongRoll2 = Random.Range(1, 13);
                if (strongRoll2 > hitRoll)
                {
                    hitRoll = strongRoll2;
                }
                break;

            case HealthCurveTypes.Sturdy:
                hitDie = 12;
                hitRoll = Random.Range(1, 13);
                break;

            case HealthCurveTypes.Healthy:
                hitDie = 10;
                hitRoll = Random.Range(1, 11);
                break;

            case HealthCurveTypes.Average:
                hitDie = 8;
                hitRoll = Random.Range(1, 9);
                break;

            case HealthCurveTypes.Weak:
                hitDie = 6;
                hitRoll = Random.Range(1, 7);
                break;

            case HealthCurveTypes.Sickly:
                hitDie = 4;
                hitRoll = Random.Range(1, 5);
                break;

            case HealthCurveTypes.Feeble:
                hitDie = 4;
                hitRoll = Random.Range(1, 5);
                //Rolling again to see if the second roll is lower
                int feebleRoll2 = Random.Range(1, 5);
                if(feebleRoll2 < hitRoll)
                {
                    hitRoll = feebleRoll2;
                }
                break;
        }

        //Looping through the character's perks to see if they have any health boost perks
        int perkBonus = 0;
        foreach (Perk charPerk in this.GetComponent<Character>().charPerks.allPerks)
        {
            //If the current perk is a health boost perk, then we can give the character more health
            if (charPerk.GetType() == typeof(HealthBoostPerk))
            {
                HealthBoostPerk hpBoostPerk = charPerk.GetComponent<HealthBoostPerk>();
                //Adding the amount of bonus health to give
                perkBonus += hpBoostPerk.GetHealthBoostAmount();
            }
        }

        //Adding the perk bonus to the total hit die
        hitDie += perkBonus;

        //Multiplying the total amount by the number of starting dice allocated by the PartyCreator.cs
        hitDie = hitDie * PartyCreator.startingHitDice;

        //Adding the random roll value to the total hit die value as well as the perk bonus again
        hitDie += (hitRoll + perkBonus);

        //Setting this character's health to this hit die roll
        this.maxHealth = hitDie;
        this.currentHealth = hitDie;
    }
}
