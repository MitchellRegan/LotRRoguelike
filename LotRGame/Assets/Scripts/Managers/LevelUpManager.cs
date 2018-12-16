using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    //Static reference to this manager
    public static LevelUpManager globalReference;

    //The current character level
    [Range(1, 100)]
    [HideInInspector]
    int characterLevel = 1;

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
        EventManager.StartListening("Advance Time", this.trackTimePassageEVT);
    }


    //Function called when this component is disabled
    private void OnDisable()
    {
        EventManager.StopListening("Advance Time", this.trackTimePassageEVT);
    }


    //Function called from the trackTimePassageEVT (invoked from TimePanelUI) to track when players level up
    private void TrackTimePassage(EVTData data_)
    {
        //If it's time to level up
        if(this.IsItTimeToLevelUp())
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
    private bool IsItTimeToLevelUp()
    {
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
        float totalDays = TimePanelUI.globalReference.daysTaken;

        //Subtracting the first few hours since the game doesn't start at midnight and level ups happen at noon
        /*if (TimePanelUI.globalReference.startingTimeOfDay < 12)
        {
            totalDays -= ((12 - TimePanelUI.globalReference.startingTimeOfDay) * 1.0f) / 24.0f;
        }
        else if(TimePanelUI.globalReference.startingTimeOfDay > 12)
        {
            totalDays += ((TimePanelUI.globalReference.startingTimeOfDay - 12) * 1.0f) / 24.0f;
        }*/

        //Adding the fraction of a day based on the hours passed
        //If the time of day is before noon, we add the 12 hours from the previous day from noon to midnight
        if (TimePanelUI.globalReference.timeOfDay < 12)
        {
            totalDays += ((TimePanelUI.globalReference.timeOfDay - 12) * 1.0f) / 24.0f;
        }
        //If the time of day is after noon, we only add all of the hours after 12 since noon is when we level
        else if(TimePanelUI.globalReference.timeOfDay > 12)
        {
            totalDays += ((TimePanelUI.globalReference.timeOfDay - 12) * 1.0f) / 24.0f;
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
                    totalDays -= levelCurves[t].daysToLevel * (levelCurves[t].maxLevelForCurve - 1);
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
                //If we are in the first level curve, we disregard the first level
                else if(t == 0)
                {
                    levelsInCurve -= 1;
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
        //Int to hold the current character's health curve
        int healthCurve = character_.charPhysState.healthCurveStagesSum;

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
                healthCurve += hpBoostPerk.healthStageBoost;

                //Adding the amount of bonus health to give
                bonusHealthAdded += hpBoostPerk.GetHealthBoostAmount();
            }
        }

        //Making sure the health curve is within reasonable bounds (between 1(feeble) and 7(strong))
        if(healthCurve > 7)
        {
            healthCurve = 7;
        }
        else if(healthCurve < 1)
        {
            healthCurve = 1;
        }

        //Returning the health cuve and bonus health values
        int[] healthCurveAndBonus = new int[2] { healthCurve, bonusHealthAdded };
        return healthCurveAndBonus;
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
                    Debug.Log(character_.firstName + " " + character_.lastName + ", INVALID!!!!!!");
                    break;
            }
        }

        //Dividing the health sum by the number of available curves to get the average
        hpTotal = Mathf.RoundToInt((hpTotal * 1.0f) / (numberToDivideBy * 1.0f));

        //Adding any bonus health to this total
        hpTotal += bonusHealth_;
        
        Debug.Log(character_.firstName + " " + character_.lastName + ", Health to add: " + hpTotal + ", Current Max HP: " + character_.charPhysState.maxHealth);

        //Increasing the character's max health and current health by the new hp amount
        character_.charPhysState.maxHealth += hpTotal;
        character_.charPhysState.currentHealth += hpTotal;

        Debug.Log(character_.firstName + " " + character_.lastName + ", New Max HP: " + character_.charPhysState.maxHealth);

        //Clearing the health curves for the past 4 levels
        character_.charPhysState.healthCurveLevels[0] = 0;
        character_.charPhysState.healthCurveLevels[1] = 0;
        character_.charPhysState.healthCurveLevels[2] = 0;
        character_.charPhysState.healthCurveLevels[3] = 0;
    }
}

[System.Serializable]
public class LevelUpCurve
{
    //The name of this curve
    public string name = "";
    //The number of days between each level
    public float daysToLevel = 1;
    //The max level this curve applies to
    [Range(1, 100)]
    public int maxLevelForCurve = 1;
}