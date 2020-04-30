using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    //Static reference to this manager
    public static LevelUpManager globalReference;

    //The current character level
    [Range(0, 100)]
    [HideInInspector]
    public int characterLevel = 0;

    //The percentage values for modifications to character health curves based on their physical state
    //From 0 to the first float is a -1 to the curve, first to second floats is +0, and second float to 1 is +1
    public Vector2 healthPercentCurveModRange;
    public Vector2 foodPercentCurveModRange;
    public Vector2 waterPercentCurveModRange;
    public Vector2 sleepPercentCurveModRange;

    [Space(8)]

    //The different curves for level progressions
    public List<LevelUpCurve> levelCurves;

    //Delegate function that's tied to the time advance event so we can track when days pass
    private DelegateEvent<EVTData> trackTimePassageEVT;



    //Function called when this object is created
    private void Awake()
    {
        //Setting the static reference
        if (globalReference == null)
        {
            globalReference = this;
        }
        //If a static reference already exists, this component is destroyed
        else
        {
            Destroy(this);
        }

        //Initializes the Delegate Event for the Event Manager
        this.trackTimePassageEVT = new DelegateEvent<EVTData>(this.TrackTimePassage);
    }


    //Function called when this component is enabled
    private void OnEnable()
    {
        EventManager.StartListening(TimePassedEVT.eventNum, this.trackTimePassageEVT);
    }


    //Function called when this component is disabled
    private void OnDisable()
    {
        EventManager.StopListening(TimePassedEVT.eventNum, this.trackTimePassageEVT);
    }


    //Function called from the trackTimePassageEVT (invoked from TimePanelUI) to track when players level up
    private void TrackTimePassage(EVTData data_)
    {
        //If it's time to level up
        if(this.IsItTimeToLevelUp(data_.timePassed))
        {
            //Increasing the player character levels
            this.characterLevel += 1;

            Debug.Log("LevelUpManager.TrackTimePassage: Current Character Level: " + this.characterLevel);

            //Looping through each player character to level up
            for(int c = 0; c < PartyGroup.group1.charactersInParty.Count; ++c)
            {
                if (PartyGroup.group1.charactersInParty[c] != null)
                {
                    //Finding the character's current health curve and bonus health
                    int[] currentCurve = new int[2];
                    currentCurve = this.FindCharacterHealthCurve(PartyGroup.group1.charactersInParty[c]);
                    
                    //Setting the character's curve for this level
                    PartyGroup.group1.charactersInParty[c].charPhysState.healthCurveLevels[this.characterLevel % 4] = currentCurve[0];

                    //If this level was a health boost level, we increase their level
                    if (this.characterLevel % 4 == 0)
                    {
                        Debug.Log("LevelUpManager.TrackTimePassage: Health Boost!");
                        this.BoostCharacterHealth(PartyGroup.group1.charactersInParty[c], currentCurve[1]);
                    }
                }
            }
        }
    }


    //Function called from TrackTimePassage to find out if it's time to level up the player characters
    private bool IsItTimeToLevelUp(TimePassedEVT data_)
    {
        //Making sure the TimePassedEVT data given isn't null
        if(data_ == null)
        {
            return false;
        }

        //Looping through each level up curve to find the one that applies to the current character levels
        int currentCurveIndex = 0;
        for(int i = 0; i < this.levelCurves.Count; ++i)
        {
            //Checking to see if the current player character level is within the level curve for this index
            if(this.characterLevel <= this.levelCurves[i].maxLevelForCurve)
            {
                //Setting the index for the current level curve we're on and breaking the loop
                currentCurveIndex = i;
                break;
            }
        }

        //Getting the total amount of in-game time that has passed for this playthrough
        float totalDays = data_.days;
        float timeOfDay = data_.timeOfDay + data_.timePassed;
        //If the time of day goes past midnight we correct the time
        if(timeOfDay >= 24)
        {
            totalDays += 1;
            timeOfDay -= 24;
        }

        //Adding the fraction of a day based on the hours passed
        //If the time of day is before noon, we add the 12 hours from the previous day from noon to midnight
        if (timeOfDay < 12)
        {
            totalDays += ((timeOfDay - 12) * 1.0f) / 24.0f;
        }
        //If the time of day is after noon, we only add all of the hours after 12 since noon is when we level
        else if(timeOfDay > 12)
        {
            totalDays += ((timeOfDay - 12) * 1.0f) / 24.0f;
        }

        //Looping through each level curve prior to the one we're on now to find the number of days that have passed under them
        for (int t = 0; t < (this.levelCurves.Count - 1); ++t)
        {
            //If the current level is greater than the max level for the current level curve we subtract all of the time spent under that curve
            if (this.characterLevel > this.levelCurves[t].maxLevelForCurve)
            {
                //If we're looking at the first curve, we ignore the first level since characters start at lvl 1 not 0
                if (t == 0)
                {
                    //Subtracting the total number of days that passed under this previous level curve
                    totalDays -= levelCurves[t].daysToLevel * (levelCurves[t].maxLevelForCurve);
                }
                else
                {
                    //Subtracting the total number of days that passed under this previous level curve
                    totalDays -= levelCurves[t].daysToLevel * levelCurves[t].maxLevelForCurve;
                }
            }
            //When we find the level curve that we're currently in
            else
            {
                //Finding the number of levels we've already spent during this level curve
                int levelsInCurve = this.characterLevel;
                //If we're not in the first level curve we subtract the number of levels since we moved from the previous curve
                if (t > 0)
                {
                    levelsInCurve -= levelCurves[t - 1].maxLevelForCurve;
                }

                //Subtracting the total number of days that passed under this curve
                totalDays -= levelsInCurve * levelCurves[t].daysToLevel;

                //If the number of days remaining is enough to level up for this curve
                if (totalDays >= levelCurves[t].daysToLevel)
                {
                    //We return true to say it's time to level up
                    return true;
                }
                //Otherwise it isn't time to level up
                else
                {
                    return false;
                }
            }
        }

        //If somehow the code gets to this point (which it shouldn't) we return false
        return false;
    }


    //Function called from TrackTimePassage to calculate a given character's current health curve and any bonus health to add
    private int[] FindCharacterHealthCurve(Character character_)
    {
        //Getting the modifier for this character's health curve from their physical state
        int physStateMod = this.FindHealthCurveModFromPhysState(character_.charPhysState);

        //Int to hold the amount of bonus health added
        int bonusHealthAdded = 0;

        //Looping through the character's perks to see if they have any health boost perks
        foreach (Perk charPerk in character_.charPerks.allPerks)
        {
            //If the current perk is a health boost perk, then we can give the player character more health
            if (charPerk.GetType() == typeof(HealthBoostPerk))
            {
                HealthBoostPerk hpBoostPerk = charPerk.GetComponent<HealthBoostPerk>();
                //Increasing the health curve
                physStateMod += hpBoostPerk.healthStageBoost;

                //Adding the amount of bonus health to give
                bonusHealthAdded += hpBoostPerk.GetHealthBoostAmount();
            }
        }

        //Making sure the health curve is within reasonable bounds (between 1(feeble) and 7(strong))
        if(physStateMod > 7)
        {
            physStateMod = 7;
        }
        else if(physStateMod < 1)
        {
            physStateMod = 1;
        }

        //Returning the health cuve and bonus health values
        int[] healthCurveAndBonus = new int[2] { physStateMod, bonusHealthAdded };
        return healthCurveAndBonus;
    }


    //Finding the total health curve modifier for a given character based on their physical state
    private int FindHealthCurveModFromPhysState(PhysicalState state_)
    {
        //The total modifier returned
        int curveMod = 0;

        //Finding the starting health curve
        switch(state_.startingHealthCurve)
        {
            case HealthCurveTypes.Strong:
                curveMod += 7;
                break;

            case HealthCurveTypes.Sturdy:
                curveMod += 6;
                break;

            case HealthCurveTypes.Healthy:
                curveMod += 5;
                break;

            case HealthCurveTypes.Average:
                curveMod += 4;
                break;

            case HealthCurveTypes.Weak:
                curveMod += 3;
                break;

            case HealthCurveTypes.Sickly:
                curveMod += 2;
                break;

            case HealthCurveTypes.Feeble:
                curveMod += 1;
                break;
        }

        //Finding the average health percent for this character
        float hpAvg = 0;
        foreach (float hpP in state_.trackingHealthPercents)
        {
            hpAvg += hpP;
        }
        hpAvg = hpAvg / state_.trackingHealthPercents.Count;

        //Finding the average food percent for this character
        float fdAvg = 0;
        foreach(float fdP in state_.trackingFoodPercents)
        {
            fdAvg += fdP;
        }
        fdAvg = fdAvg / state_.trackingFoodPercents.Count;

        //Finding the average water percent for this character
        float wtAvg = 0;
        foreach(float wtP in state_.trackingWaterPercents)
        {
            wtAvg += wtP;
        }
        wtAvg = wtAvg / state_.trackingWaterPercents.Count;

        //Finding the average sleep percent for this character
        float slAvg = 0;
        foreach(float slP in state_.trackingSleepPercents)
        {
            slAvg += slP;
        }
        slAvg = slAvg / state_.trackingSleepPercents.Count;


        //If the Health average is too low, the modifier goes down
        if(hpAvg < this.healthPercentCurveModRange.x)
        {
            curveMod -= 1;
        }
        //If the Health average is high enough, the modifier goes up
        else if(hpAvg >= this.healthPercentCurveModRange.y)
        {
            curveMod += 1;
        }

        //If the Food average is too low, the modifier goes down
        if(fdAvg < this.foodPercentCurveModRange.x)
        {
            curveMod -= 1;
        }
        //If the Food average is high enough, the modifier goes up
        else if(fdAvg >= this.foodPercentCurveModRange.y)
        {
            curveMod += 1;
        }

        //If the Water average is too low, the modifier goes down
        if(wtAvg < this.waterPercentCurveModRange.x)
        {
            curveMod -= 1;
        }
        //If the Water average is high enough, the modifier goes up
        else if(wtAvg >= this.waterPercentCurveModRange.y)
        {
            curveMod += 1;
        }

        //If the Sleep average is too low, the modifier goes down
        if(slAvg < this.sleepPercentCurveModRange.x)
        {
            curveMod -= 1;
        }
        //If the Sleep average is high enough, the modifier goes up
        else if(slAvg >= this.sleepPercentCurveModRange.y)
        {
            curveMod += 1;
        }

        return curveMod;
    }


    //Function called from TrackTimePassage to increase a given character's max health
    private void BoostCharacterHealth(Character character_, int bonusHealth_)
    {
        //The total health value
        int hpTotal = 0;

        //The amount to divide the health value by to get the average
        int numberToDivideBy = 4;

        //Looping through each health curve for the character's last 4 levels
        for (int l = 0; l < character_.charPhysState.healthCurveLevels.Length; ++l)
        {
            switch (character_.charPhysState.healthCurveLevels[l])
            {
                case 7://strong
                    //Rolling 2d12 and taking the higher roll
                    int strongRoll1 = Random.Range(1, 13);
                    int strongRoll2 = Random.Range(1, 13);

                    if (strongRoll1 > strongRoll2)
                    {
                        hpTotal += strongRoll1;
                    }
                    else
                    {
                        hpTotal += strongRoll2;
                    }
                    Debug.Log(character_.firstName + " " + character_.lastName + ", Strong");
                    break;

                case 6://sturdy
                    hpTotal += Random.Range(1, 13);
                    Debug.Log(character_.firstName + " " + character_.lastName + ", Sturdy");
                    break;

                case 5://healthy
                    hpTotal += Random.Range(1, 11);
                    Debug.Log(character_.firstName + " " + character_.lastName + ", Healthy");
                    break;

                case 4://average
                    hpTotal += Random.Range(1, 9);
                    Debug.Log(character_.firstName + " " + character_.lastName + ", Average");
                    break;

                case 3://weak
                    hpTotal += Random.Range(1, 7);
                    Debug.Log(character_.firstName + " " + character_.lastName + ", Weak");
                    break;

                case 2://sickly
                    hpTotal += Random.Range(1, 5);
                    Debug.Log(character_.firstName + " " + character_.lastName + ", Sickly");
                    break;

                case 1://feeble
                    //Rolling 2d4 and taking the lower roll
                    int feebleRoll1 = Random.Range(1, 5);
                    int feebleRoll2 = Random.Range(1, 5);

                    if (feebleRoll1 < feebleRoll2)
                    {
                        hpTotal += feebleRoll1;
                    }
                    else
                    {
                        hpTotal += feebleRoll2;
                    }
                    Debug.Log(character_.firstName + " " + character_.lastName + ", Feeble");
                    break;

                default://Below 1 or above 7, this causes problems
                    //Not adding any health to the total, and reducing the number to divide by
                    --numberToDivideBy;
                    break;
            }
        }

        //Dividing the health sum by the number of available curves to get the average
        hpTotal = Mathf.RoundToInt((hpTotal * 1.0f) / (numberToDivideBy * 1.0f));

        //Adding any bonus health to this total
        hpTotal += bonusHealth_;
        
        //Increasing the character's max health and current health by the new hp amount
        character_.charPhysState.maxHealth += hpTotal;
        character_.charPhysState.currentHealth += hpTotal;

        //Clearing the health curves for the past 4 levels
        character_.charPhysState.healthCurveLevels[0] = 0;
        character_.charPhysState.healthCurveLevels[1] = 0;
        character_.charPhysState.healthCurveLevels[2] = 0;
        character_.charPhysState.healthCurveLevels[3] = 0;
    }
}
