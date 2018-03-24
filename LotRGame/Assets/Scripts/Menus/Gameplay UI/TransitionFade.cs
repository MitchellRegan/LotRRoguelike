using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class TransitionFade : MonoBehaviour
{
    //Reference to this object's Image component
    private Image ourImage;

    //Delegate event that this transition is listening for
    private DelegateEvent<EVTData> fadeEVT;

    //The amount of time it takes to fade to black
    private float fadeToBlackTime = 1;
    //The amount of time we'll stay on a black screen
    private float stayOnBlackTime = 1;
    //The amount of time it takes to fade back in
    private float fadeInTime = 1;

    //The current amount of time that's passed for the current fade state
    private float currentTime = 0;

    //The Unity Event that's invoked when the screen goes black
    private UnityEvent eventOnBlack;

    //Bool that determines if we need to be transitioning
    private bool currentlyTransitioning = false;
    //Enum that determines if we're fading out, staying on black, or fading in
    private enum FadeState { FadeIn, Stay, FadeOut };
    private FadeState currentState = FadeState.FadeOut;



    //Function called when this object is created
    private void Awake()
    {
        //Getting the reference to our image component
        this.ourImage = this.GetComponent<Image>();

        //Making sure our image alpha is 0
        this.ourImage.color = new Color(0, 0, 0, 0);

        //Initializes new DelegateEvent for the Event Manager
        this.fadeEVT = new DelegateEvent<EVTData>(this.BeginFade);
    }


    //Function called on the frist frame this object is created
    private void Start()
    {
        EventManager.StartListening(CombatTransitionEVT.eventName, this.fadeEVT);
    }


    //Function called when this object is enabled
    private void Enable()
    {
        EventManager.StartListening(CombatTransitionEVT.eventName, this.fadeEVT);
    }


    //Function called when this object is disabled
    private void Disable()
    {
        EventManager.StopListening(CombatTransitionEVT.eventName, this.fadeEVT);
    }


    //Function called from the EventManager
    private void BeginFade(EVTData data_)
    {
        //Making sure the combat transition class isn't null, or else we stop
        if(data_.combatTransition == null)
        {
            return;
        }

        //Storing all of the fade information
        this.fadeToBlackTime = data_.combatTransition.fadeToBlackTime;
        this.stayOnBlackTime = data_.combatTransition.stayOnBlackTime;
        this.fadeInTime = data_.combatTransition.fadeInTime;
        this.eventOnBlack = data_.combatTransition.eventOnBlack;

        //Starts the transition
        this.currentlyTransitioning = true;
        this.currentState = FadeState.FadeOut;
        this.currentTime = 0;
    }


    //Function called every frame
    private void Update()
    {
        //If we aren't transitioning
        if(!this.currentlyTransitioning)
        {
            return;
        }

        //Increasing the time passed for the current count
        this.currentTime += Time.deltaTime;

        //Based on the current state, the update changes
        switch(this.currentState)
        {
            case FadeState.FadeOut:
                //Setting this image's alpha to a percentage based on the time that's passed
                this.ourImage.color = new Color(0, 0, 0, (this.currentTime / this.fadeToBlackTime));

                //If the current time is greater than the fade out time
                if (this.currentTime >= this.fadeToBlackTime)
                {
                    //Reset the current time to 0
                    this.currentTime = 0;
                    //Sets the current state to stay on a black screen
                    this.currentState = FadeState.Stay;
                    //Calls the event on black event if it's not null
                    if(this.eventOnBlack != null)
                    {
                        this.eventOnBlack.Invoke();
                    }
                }
                break;

            case FadeState.Stay:
                //If the current time is greater than the time we need to stay on black
                if(this.currentTime >= this.stayOnBlackTime)
                {
                    //Reset the current time to 0
                    this.currentTime = 0;
                    //Sets the current state to start fading back in
                    this.currentState = FadeState.FadeIn;
                }
                break;

            case FadeState.FadeIn:
                //Setting this image's alpha to a percentage based on the time that's passed
                this.ourImage.color = new Color(0, 0, 0, 1f -(this.currentTime / this.fadeToBlackTime));

                //If the current time is greater than the fade in time
                if (this.currentTime >= this.fadeInTime)
                {
                    //Reset the current time to 0
                    this.currentTime = 0;
                    //Stops the transition
                    this.currentlyTransitioning = false;
                    this.eventOnBlack = null;
                }
                break;
        }
    }
}
