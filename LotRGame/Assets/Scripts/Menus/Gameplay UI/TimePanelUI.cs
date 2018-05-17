using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TimePanelUI : MonoBehaviour
{
    //Static reference to this component for everything that uses time advancements
    public static TimePanelUI globalReference;
    //Reference to the button that is used to advance time so we can disable it
    public Button advanceTimeButton;

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

    //Bool that determines if we're currently transitioning time
    private bool isTimePassing = false;
    //The time that it will be when we're done transitioning time and the time when the transition started
    private int timeAfterTransition = 0;
    private int timeBeforeTransition = 0;

    //The amount of real time that it takes to transition time 1 hour
    public float timeItTakesToTransition1Hour = 0.1f;
    //The current time it's taken during a transition
    private float currentTransitionTime = 0;

    [Space(8)]

    //The object that we rotate to make the sun and moon change positions in the sky
    public Transform globalLightObject;

    //The light reference for the sun
    public Light sunLightObj;
    //The color gradient of the sun as it moves through the day
    public Gradient sunColors;

    //The light reference for the moon
    public Light moonLightObj;
    //The color gradient of the moon as it moves through the night
    public Gradient moonColors;

    [Space(8)]

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

            //Setting the rotation of the lights
            this.SetLightPositions(this.timeOfDay * 1f);
        }
    }


    //Function called externally for other scripts to check if time is currently changing
    public bool IsTimePassing()
    {
        return this.isTimePassing;
    }


	// Update is called once per frame
	private void Update ()
    {
        //If the game time isn't currently passing
        if (!this.isTimePassing)
        {
            //Advancing the current timer
            this.currentTimer += Time.deltaTime;

            //If the current timer is greater than the max time, time is advanced and the current timer is reset
            if (this.currentTimer >= this.gameTimeBeforeHoursAdvance)
            {
                this.AdvanceTime(this.hoursAdvancedPerUpdate);
            }
        }
        //If the game time is passing
        else
        {
            //Adding to the current transition time
            this.currentTransitionTime += Time.deltaTime;

            //Finding the total transition time (real world time to transition 1 hour times the hours passed)
            float totalTransitionTime = this.timeItTakesToTransition1Hour * (this.timeAfterTransition - this.timeBeforeTransition);

            //Finding the percent that we are through the transition and multiplying it by the amount of in-game time that needs to pass
            int newTime = this.timeAfterTransition - this.timeBeforeTransition;
            newTime = Mathf.RoundToInt((this.currentTransitionTime / totalTransitionTime) * newTime);
            newTime += this.timeBeforeTransition;
            this.timeOfDay = newTime;

            //If the time of day passes beyond 24 hours, we have to correct for it
            if(this.timeOfDay > 24)
            {
                //Looping all of the time variables back around 0
                this.timeOfDay -= 24;
                this.timeBeforeTransition -= 24;
                this.timeAfterTransition -= 24;

                //Moving to the next day
                this.daysTaken += 1;
            }

            //Setting the daylight rotation and colors
            float daylightTime = this.timeAfterTransition - this.timeBeforeTransition;
            daylightTime = (this.currentTransitionTime / totalTransitionTime) * daylightTime;
            daylightTime += this.timeBeforeTransition;
            this.SetLightPositions(daylightTime);

            //If the current transition time is at the total transition time, we stop
            if (this.currentTransitionTime >= totalTransitionTime)
            {
                this.currentTransitionTime = 0;
                this.isTimePassing = false;
                this.advanceTimeButton.interactable = true;
            }
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

        //Resetting the current game timer so that players have the maximum amount of time before the next advance
        this.currentTimer = 0;

        //Setting the time of day to the current time after the advance
        this.timeBeforeTransition = this.timeOfDay;
        this.timeAfterTransition = this.timeOfDay + hoursToAdvance_;

        //Starting the time transition
        this.isTimePassing = true;

        //Resetting the current transition time value
        this.currentTransitionTime = 0;

        //Disabling the advance time button so it can't be pressed during the transition
        this.advanceTimeButton.interactable = false;

        //Calling the unity event
        this.onTimeAdvancedEvent.Invoke();
    }


    //Function called from Awake and Update to change the position and color of the lights
    private void SetLightPositions(float daylightTime_)
    {
        //Getting the percent of the day that's passed
        float dayPercent = (daylightTime_ / 24f);

        //Setting the sun's color based on the day percent
        this.sunLightObj.color = this.sunColors.Evaluate(dayPercent);
        //Setting the moon's color based on the day percent
        this.moonLightObj.color = this.moonColors.Evaluate(dayPercent);

        //Setting the light euler rotation
        this.globalLightObject.eulerAngles = new Vector3(0, 0, dayPercent * 360);

        //Setting the lights to be at the same location as the player party
        this.globalLightObject.position = PartyGroup.group1.transform.position;
    }
}
