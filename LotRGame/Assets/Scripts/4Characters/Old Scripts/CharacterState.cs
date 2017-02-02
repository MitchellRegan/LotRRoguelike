using UnityEngine;
using System.Collections;

public class CharacterState : MonoBehaviour
{
    private bool IsDead;
    private int MaxHealth;
    private int CurrentHealth;

    //Biological state of the character in numerical form
    private int SpeedValue;
    private int HungerValue;
    private int ThirstValue;
    private int HoursAwake;
    private bool IsAsleep;

    //Biological state of the character in enum form so that the exact numbers are hidden
    public enum HungerState {Starving, Ravenous, Hungry, Satisfied, Full, DoesntEat};
    private HungerState Hunger = HungerState.Full;
    
    public enum ThirstState {Dehydrated, Parched, Thirsty, Satisfied, Full, DoesntDrink};
    private ThirstState Thirst = ThirstState.Full;

    public enum SleepState {Asleep, Exhausted, Tired, Awake, Alert, DoesntSleep};
    private SleepState Sleep = SleepState.Alert;



	// Use this for initialization
	void Start ()
    {
        //Starts off as "not dead" which is good, and sets the health
        IsDead = false;
        SetRaceStats();
	}
	

	// Update is called once per frame
	void Update ()
    {
        //Updates Hunger enum
        if (Hunger != HungerState.DoesntEat)
        {
            //90% - 100% Full
            if(HungerValue >= 90)
            {
                Hunger = HungerState.Full;
            }
            //60% - 89% Satisfied
            else if(HungerValue >= 60)
            {
                Hunger = HungerState.Satisfied;
            }
            //40% - 59% Hungry
            else if(HungerValue >= 40)
            {
                Hunger = HungerState.Hungry;
            }
            //15% = 39% Ravenous
            else if(HungerValue >= 15)
            {
                Hunger = HungerState.Ravenous;
            }
            //0% - 14% Starving
            else
            {
                Hunger = HungerState.Starving;
            }
        }


        //Updates Thirst enum
        if (Thirst != ThirstState.DoesntDrink)
        {
            //90% - 100% Full
            if (HungerValue >= 90)
            {
                Thirst = ThirstState.Full;
            }
            //60% - 89% Satisfied
            else if (HungerValue >= 60)
            {
                Thirst = ThirstState.Satisfied;
            }
            //40% - 59% Thirsty
            else if (HungerValue >= 40)
            {
                Thirst = ThirstState.Thirsty;
            }
            //15% = 39% Parched
            else if (HungerValue >= 15)
            {
                Thirst = ThirstState.Parched;
            }
            //0% - 14% Dehydrated
            else
            {
                Thirst = ThirstState.Dehydrated;
            }
        }


        //Updates Sleep enum
        if (Sleep != SleepState.DoesntSleep)
        {
            //Asleep if....Asleep. Makes sense.
            if(IsAsleep)
            {
                Sleep = SleepState.Asleep;
            }
            //0 - 6 Hours Alert
            else if (HoursAwake >= 90)
            {
                Sleep = SleepState.Alert;
            }
            //7 - 12 Hours Awake
            else if (HoursAwake >= 60)
            {
                Sleep = SleepState.Awake;
            }
            //13 - 18 Hours Tired
            else if (HoursAwake >= 40)
            {
                Sleep = SleepState.Tired;
            }
            //19+ Hours Exhausted
            else if (HoursAwake >= 15)
            {
                Sleep = SleepState.Exhausted;
            }
        }
    }


    //Function called whenever the "TimePass" Event is sent out. Represents 6 hours passing
    void TimePass()
    {
        //Reduces the HoursAsleep if sleeping
        if(IsAsleep)
        {
            //Reduces only by 5 so that characters can't just nap once per day. Tiredness should accumulate if not being well rested.
            HoursAwake -= 5;

            //Makes sure that HoursAwake can't go below Min 0 and automatically wakes up.
            if(HoursAwake <= 0)
            {
                HoursAwake = 0;
                IsAsleep = false;
            }
        }
        //If not asleep (and requires sleep), the HoursAwake is increased
        else if(Sleep != SleepState.DoesntSleep)
        {
            HoursAwake += 6;
        }
    }


    //Increases the character's HungerValue by a food's HungerValue 
    public void Eat(int amountEaten_)
    {
        HungerValue += amountEaten_;

        //Prevents the HungerValue from going over the Max 100
        if(HungerValue > 100)
        {
            HungerValue = 100;
        }
    }


    //Increases the character's ThirstValue by a drink's ThirstValue
    public void Drink(int amountDrunk)
    {
        ThirstValue += amountDrunk;

        //Prevents the ThirstValue from going over the Max 100
        if(ThirstValue > 100)
        {
            ThirstValue = 100;
        }
    }


    //Puts the character to sleep so they can reduce their HoursAwake
    public void GoToSleep()
    {
        //Can't go to sleep if already asleep
        if(IsAsleep)
        {
            return;
        }
        IsAsleep = true;
    }

    //Wakes the character from sleeping so they can take other actions
    public void WakeUp()
    {
        //Can't wake up if already awake
        if(!IsAsleep)
        {
            return;
        }
        IsAsleep = false;
    }


    //Function called on creation to set the character's max HP based on their race
    private void SetRaceStats()
    {
        //Finding Max Health
        switch(this.GetComponent<CharacterCore>().Race)
        {
            case CharacterCore.CharacterRace.Human:

                break;
            case CharacterCore.CharacterRace.Elf:

                break;
            case CharacterCore.CharacterRace.Dwarf:

                break;
            case CharacterCore.CharacterRace.Orc:

                break;
            case CharacterCore.CharacterRace.HalfMan:

                break;
            case CharacterCore.CharacterRace.GillFolk:

                break;
            case CharacterCore.CharacterRace.ScaleSkin:

                break;
        }
        CurrentHealth = MaxHealth;


        //Setting Hunger
        //if(this.GetComponent<CharacterCore>().Race == CharacterCore.CharacterRace.)
        //{
        Hunger = HungerState.DoesntEat;
        //}
        //else
        //{
        HungerValue = 70;
        //}


        //Setting Thirst
        if(this.GetComponent<CharacterCore>().Race == CharacterCore.CharacterRace.ScaleSkin)
        {
            //Scale Skin don't need to drink.
            Thirst = ThirstState.DoesntDrink;
        }
        else if(this.GetComponent<CharacterCore>().Race == CharacterCore.CharacterRace.GillFolk)
        {
            //Gill Folk live in water, so they start at max hydration
            ThirstValue = 100;
        }
        else
        {
            //All other races start at an alright amount of thirst
            ThirstValue = 70;
        }


        //Setting Sleep
        if(this.GetComponent<CharacterCore>().Race == CharacterCore.CharacterRace.Elf)
        {
            //Elves don't need to sleep
            Sleep = SleepState.DoesntSleep;
        }
        else
        {
            //Sets HoursAwake to 2 so that players will encounter the need to double sleep soon so they learn it
            HoursAwake = 2;
        }
    }
}
