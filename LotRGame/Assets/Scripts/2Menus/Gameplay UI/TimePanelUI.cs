using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimePanelUI : MonoBehaviour
{
    //The number of days this current adventure has taken
    public int daysTaken = 0;
    //The time of day it is
    public int timeOfDay = 12;

    //The amount of game time that is elapsed before the game advances on its own
    public float gameTimeBeforeHoursAdvance = 120;
    private float currentTimer = 0;

    //The amount of hours that are elapsed when the time is advanced
    public int timeAdvanced = 6;

    //The text field that displays the days elapsed
    public Text daysElapsedText;
    //The text field that displays the time of day
    public Text timeOfDayText;

    

	// Update is called once per frame
	private void Update ()
    {
        //Advancing the current timer
        this.currentTimer += Time.deltaTime;

        //If the current timer is greater than the max time, time is advanced and the current timer is reset
        if(this.currentTimer >= this.gameTimeBeforeHoursAdvance)
        {
            this.AdvanceTime();
        }

        //Setting the text fields to display the correct time
        this.daysElapsedText.text = "Days Elapsed: " + this.daysTaken;
        if (this.timeOfDay <= 12)
        {
            this.timeOfDayText.text = "Time: " + this.timeOfDay + "AM";
        }
        else
        {
            this.timeOfDayText.text = "Time: " + (this.timeOfDay - 12) + "PM";
        }
	}


    //Function called externally to jump the timer forward the number of hours given
    public void AdvanceTime()
    {
        this.timeOfDay += this.timeAdvanced;

        //If the hours are over 24, the day is advanced
        if(this.timeOfDay >= 24)
        {
            this.daysTaken += 1;
            this.timeOfDay -= 24;

            //Resetting the current game timer so that players have the maximum amount of time before the next jump
            this.currentTimer = 0;
        }
    }
}
