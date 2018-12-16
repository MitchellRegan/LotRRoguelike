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
        .

        //Looping through each level curve prior to the one we're on now to find the number of days that have passed under them
        for (int t = 0; t < (this.levelCurves.Count - 1); ++t)
        {
            //Subtracting the total number of days that passed under this previous level curve
            totalDays -= levelCurves[t].daysToLevel * levelCurves[t].maxLevelForCurve;
        }
        .
        //Otherwise it isn't time to level up
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
        //Getting the total number of dice to roll
        int strong = 0;
        int sturdy = 0;
        int healthy = 0;
        int average = 0;
        int weak = 0;
        int sickly = 0;
        int feeble = 0;

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
                    ++strong;
                    break;

                case 6://sturdy
                    ++sturdy;
                    break;

                case 5://healthy
                    ++healthy;
                    break;

                case 4://average
                    ++average;
                    break;

                case 3://weak
                    ++weak;
                    break;

                case 2://sickly
                    ++sickly;
                    break;

                case 1://feeble
                    ++feeble;
                    break;

                default://Below 1 or above 7, this causes problems
                        //Not adding any health to the total, and reducing the number to divide by
                    --numberToDivideBy;
                    break;
            }
        }

        //Rolling dice for each strong curve
        if (strong > 0)
        {
            //Rolling 2d12 and taking the higher roll
            int roll1 = Random.Range(1, 13);
            int roll2 = Random.Range(1, 13);

            if (roll1 > roll2)
            {
                hpTotal += (roll1 * strong);
            }
            else
            {
                hpTotal += (roll2 * strong);
            }
        }
        //Rolling dice for each sturdy curve
        if (sturdy > 0)
        {
            hpTotal += (Random.Range(1, 13) * sturdy);
        }
        //Rolling dice for each healthy curve
        if (healthy > 0)
        {
            hpTotal += (Random.Range(1, 11) * healthy);
        }
        //Rolling dice for each average curve
        if (average > 0)
        {
            hpTotal += (Random.Range(1, 9) * average);
        }
        //Rolling dice for each weak curve
        if (weak > 0)
        {
            hpTotal += (Random.Range(1, 7) * weak);
        }
        //Rolling dice for each sickly curve
        if (sickly > 0)
        {
            hpTotal += (Random.Range(1, 5) * sickly);
        }
        //Rolling dice for each feeble curve
        if (feeble > 0)
        {
            //Rolling 2d4 and taking the lower roll
            int roll1 = Random.Range(1, 5);
            int roll2 = Random.Range(1, 5);

            if (roll1 < roll2)
            {
                hpTotal += (roll1 * feeble);
            }
            else
            {
                hpTotal += (roll2 * feeble);
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