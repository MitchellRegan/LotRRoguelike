using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TimePanelUI : MonoBehaviour
{
    //Static reference to this component for everything that uses time advancements
    public static TimePanelUI globalReference;

    //The number of days this current adventure has taken
    public int daysTaken = 0;
    //The time of day it is
    public int timeOfDay = 12;

    //The amount of game time that is elapsed before the game advances on its own
    public float gameTimeBeforeHoursAdvance = 120;
    private float currentTimer = 0;

    //The amount of hours that are elapsed when the time is advanced
    public int hoursAdvancedPerUpdate = 6;

    //The text field that displays the days elapsed
    public Text daysElapsedText;
    //The text field that displays the time of day
    public Text timeOfDayText;

    //The UnityEvent that's dispatched when time is advanced
    public UnityEvent onTimeAdvancedEvent;



    //Function called when this object is initialized
    private void Awake()
    {
        //Making sure there's only one static reference to this component
        if(globalReference != null)
        {
            this.enabled = false;
        }
        else
        {
            globalReference = this;
        }
    }


	// Update is called once per frame
	private void Update ()
    {
        //Advancing the current timer
        this.currentTimer += Time.deltaTime;

        //If the current timer is greater than the max time, time is advanced and the current timer is reset
        if(this.currentTimer >= this.gameTimeBeforeHoursAdvance)
        {
            this.AdvanceTime(this.hoursAdvancedPerUpdate);
        }

        //Setting the text fields to display the correct time
        this.daysElapsedText.text = "Days Elapsed: " + this.daysTaken;
        if (this.timeOfDay < 12)
        {
            if (this.timeOfDay == 0)
            {
                this.timeOfDayText.text = "Time: 12AM";
            }
            else
            {
                this.timeOfDayText.text = "Time: " + this.timeOfDay + "AM";
            }
        }
        else if(this.timeOfDay == 12)
        {
            this.timeOfDayText.text = "Time: 12PM";
        }
        else
        {
            this.timeOfDayText.text = "Time: " + (this.timeOfDay - 12) + "PM";
        }
	}


    //Function called externally to jump the timer forward the number of hours given
    public void AdvanceTime(int hoursToAdvance_)
    {
        //If the hours to advance are less than 1, nothing happens. THERE IS NO TIME TRAVEL HERE
        if(hoursToAdvance_ < 1)
        {
            return;
        }

        this.timeOfDay += hoursToAdvance_;

        //If the hours are over 24, the day is advanced
        if(this.timeOfDay >= 24)
        {
            this.daysTaken += 1;
            this.timeOfDay -= 24;

            //Resetting the current game timer so that players have the maximum amount of time before the next jump
            this.currentTimer = 0;
        }

        //Calling the unity event
        this.onTimeAdvancedEvent.Invoke();
    }
}
