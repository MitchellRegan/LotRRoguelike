using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    //Reference to the image that will be fading
    public Image fadeImage;
    //Reference to a game object to display a loading icon
    public GameObject loadIcon;

    //Delegate event that this transition is listening for
    private DelegateEvent<EVTData> transitionEVT;

    //The name of the scene to transition to
    private string newSceneName = "";

    //The amount of time it takes to fade to black
    private float transitionTime = 0;
    //The current amount of time that's passed for the current fade state
    private float currentTime = 0;
    //The amount of time that we stay on a black screen
    private float blackScreenTime = 0;

    //Int for which stage of the transition we're in
    int tState = 0; //0 - Off, 1 - Fade Out, 2 - Stay On Black, 3 - Stay on Black, 4 - Fade In



    //Function called when this object is created
    private void Awake()
    {
        this.transitionTime = 0;
        this.currentTime = 0;
        this.blackScreenTime = 0;

        //Setting our state to OFF
        this.tState = 0;

        //Making sure our image alpha is 0 and the loading icons are invisible
        this.fadeImage.color = new Color(0, 0, 0, 0);
        this.fadeImage.raycastTarget = false;
        this.loadIcon.SetActive(false);

        //Initializes the delegate event for the Event Manager
        this.transitionEVT = new DelegateEvent<EVTData>(this.BeginTransition);
    }


    //Function called on the first frame this object is created
    private void Start()
    {
        EventManager.StartListening(SceneTransitionEVT.eventNum, this.transitionEVT);
    }


    //Function called when this object is enabled
    private void Enable()
    {
        EventManager.StartListening(SceneTransitionEVT.eventNum, this.transitionEVT);
    }


    //Function called when this object is disabled
    private void Disable()
    {
        EventManager.StopListening(SceneTransitionEVT.eventNum, this.transitionEVT);
    }


    //Function called from the EventManager
    private void BeginTransition(EVTData data_)
    {
        //Using the image to block input
        this.fadeImage.raycastTarget = true;
        //Getting the transition settings from the event data
        this.transitionTime = data_.sceneTransition.transitionTime;
        this.newSceneName = data_.sceneTransition.newSceneName;
        this.currentTime = 0;
        this.blackScreenTime = data_.sceneTransition.stayOnBlackTime;
        //Setting the state to begin fading out
        this.tState = 1;
    }


    //Function called every frame
    private void Update()
    {
        if(this.tState == 1)//Fade Out
        {
            this.currentTime += Time.deltaTime;

            //Going to the next scene when the time is up
            if(this.currentTime >= this.transitionTime)
            {
                //Turning on the load icon
                this.loadIcon.SetActive(true);
                //Making sure the screen is completely black
                this.fadeImage.color = new Color(0, 0, 0, 1);
                //Resetting the timer and going to state 2
                this.currentTime = this.blackScreenTime;
                this.tState = 2;
                return;
            }

            //Getting the amount of alpha for the fade image based on the timer
            float alphaPercent = this.currentTime / this.transitionTime;
            this.fadeImage.color = new Color(0, 0, 0, alphaPercent);
        }
        else if(this.tState == 2)//Black before transition
        {
            this.currentTime -= Time.deltaTime;

            //Scene transition when the timer is up
            if(this.currentTime < 0)
            {
                this.currentTime = this.blackScreenTime;
                this.tState = 3;

                GameData.globalReference.GetComponent<GoToLevel>().LoadLevelByName(this.newSceneName);
            }
        }
        else if(this.tState == 3)//Black after transition
        {
            this.currentTime -= Time.deltaTime;

            //Fade back in when the timer is up
            if (this.currentTime < 0)
            {
                this.currentTime = this.transitionTime;
                this.tState = 4;
                //Turning off the load icon
                this.loadIcon.SetActive(false);
            }
        }
        else if(this.tState == 4)//Fade in
        {
            this.currentTime -= Time.deltaTime;

            //Turning off the transition when the time is up
            if(this.currentTime < 0)
            {
                //Setting our state to OFF
                this.tState = 0;

                //Making sure our image alpha is 0 and the loading icons are invisible
                this.fadeImage.color = new Color(0, 0, 0, 0);
                this.fadeImage.raycastTarget = false;
                return;
            }

            //Getting the amount of alpha for the fade image based on the timer
            float alphaPercent = this.currentTime / this.transitionTime;
            this.fadeImage.color = new Color(0, 0, 0, alphaPercent);
        }
    }
}
