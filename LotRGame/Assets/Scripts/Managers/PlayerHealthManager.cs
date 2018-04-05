using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    //Enum for the different health progressions
    public enum healthProgressionTypes
    {
        Strong,
        Sturdy,
        Healthy,
        Average,
        Weak,
        Sickly,
        Feeble
    };

    //The number of days that need to pass before player health is increased
    public int daysBeforeHPIncrease = 7;

    //The number of health increases it takes to reach the maximum
    public int healthIncreasesUntilMax = 100;

    //The list of different health progression curves
    public List<HealthCurve> healthCurveTypes;

    //Delegate function that's tied to the time advance event so we can track when days pass
    private DelegateEvent<EVTData> trackTimePassageEVT;

    //The current number of days that have passed so we can track when a new day ticks over
    private int currentDaysPassed = 0;
    


	//Function called when this object is created
    private void Awake()
    {
        //Initializes the Delegate Event for the Event Manager
        this.trackTimePassageEVT = new DelegateEvent<EVTData>(this.trackTimePassageEVT);
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


    //Function calledfrom the trackTimePassageEVT to track if it's time to increase player health
    private void TrackTimePassage(EVTData data_)
    {
        //Checking to see if this time advancement goes to the next day
        if (this.currentDaysPassed != TimePanelUI.globalReference.daysTaken)
        {
            //Updating the current number of days passed
            this.currentDaysPassed = TimePanelUI.globalReference.daysTaken;

            //If the current number of days that have passed is divisible by the number that need to pass before health increase, we increase player health
            if (TimePanelUI.globalReference.daysTaken % this.daysBeforeHPIncrease == 0)
            {
                //Looping through each player character to give them more health
                for(int p = 0; p < PartyGroup.group1.charactersInParty.Count; ++p)
                {
                    //If the current character slot isn't null, we add health
                    if (PartyGroup.group1.charactersInParty[p] != null)
                    {
                        //Finding the amount of health based on the character's health curve
                        int healthToGiveCharacter = this.GetHealthToAdd(PartyGroup.group1.charactersInParty[p].charPhysState.hPCurveType);

                        //Adding the health to the character's current and maximum values
                        PartyGroup.group1.charactersInParty[p].charPhysState.maxHealth += healthToGiveCharacter;
                        PartyGroup.group1.charactersInParty[p].charPhysState.currentHealth += healthToGiveCharacter;
                    }
                }
            }
        }
    }


    //Function called from TrackTimePassage to get the amount of health a character will gain
    private int GetHealthToAdd(healthProgressionTypes type_)
    {
        //The amount of health to add that is returned
        int healthToAdd = 0;

        //The health curve that we use based on the progression type
        HealthCurve curveToUse = this.healthCurveTypes[0];

        //Looping through all of our health curves until we find one that matches the designated type
        for(int l = 0; l < this.healthCurveTypes.Count; ++l)
        {
            //If we find a match, we set it as the curve to use and break the loop
            if(this.healthCurveTypes[l].curveType == type_)
            {
                curveToUse = this.healthCurveTypes[l];
                break;
            }
        }

        //Finding the difference in health between the min and max for this curve
        int minMaxCurve = curveToUse.maxHealthGiven - curveToUse.minHealthGiven;
        //Getting the value along the curve to multiply based on the number of days passed
        float percent = (1f * this.currentDaysPassed) / (1f * this.healthIncreasesUntilMax);
        percent = curveToUse.curveBetweenMinMax.Evaluate(percent);

        //Adding the amount based on our curve progress and the minimum health to give
        healthToAdd += Mathf.RoundToInt(minMaxCurve * percent);
        healthToAdd += curveToUse.minHealthGiven;

        //Looping through for each die roll for random amount of health
        for(int d = 0; d < curveToUse.diceRolled; ++d)
        {
            //Adding the die roll value to the amount of health to add based on the sides on the die
            healthToAdd += Random.Range(1, curveToUse.numberOfDiceSides + 1);
        }

        //returning the total health
        return healthToAdd;
    }
}


//Class used in PlayerHealthManager.cs for each of the different health curves
[System.Serializable]
public class HealthCurve
{
    //The type of progression curve tied to this
    public PlayerHealthManager.healthProgressionTypes curveType = PlayerHealthManager.healthProgressionTypes.Average;

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
}