using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBarUI : MonoBehaviour
{
    //Object to enable when we're loading
    public GameObject loadingUIObject;

    //Reference to the loading bar that fills up
    public Slider loadingBar;

    //Reference to the background slider that moves so we can tell if the game freezes
    public Slider backgroundSlider;

    //The speed that we move the background slider
    public float sliderSpeed = 0.02f;
    //Bool for the direction of the background slider's movement
    private bool backgroundSliderAdd = true;

    //The number of total updates that it takes to fill the loading bar. The amount is set from ToggleLoad
    private int totalUpdates = 0;
    private int currentUpdates = 0;

    //The amount of time that we pause the coroutine after getting a load update
    private float coroutinePauseTime = 1f;

    //Delegate for the function to activate and deactivate the loading bar
    private DelegateEvent<EVTData> toggleLoadEVT;



    //Function called when this object is created
    private void Awake()
    {
        //Resetting the loading bar value
        this.loadingBar.maxValue = 1;
        this.loadingBar.value = 0;

        //Resetting the background slider
        this.backgroundSlider.maxValue = 1;
        this.backgroundSlider.value = 0;

        //Making sure the UI is off by default
        this.loadingUIObject.SetActive(false);

        this.toggleLoadEVT = new DelegateEvent<EVTData>(this.ToggleLoad);
    }


    //Function called when this component is enabled
    private void OnEnable()
    {
        EventManager.StartListening(LoadDataEVT.eventNum, this.toggleLoadEVT);
    }


    //Function called when this component is disabled
    private void OnDisable()
    {
        EventManager.StopListening(LoadDataEVT.eventNum, this.toggleLoadEVT);
    }


    //Function called from the toggleLoadEVT delegate
    private void ToggleLoad(EVTData data_)
    {
        //Making sure the load data is valid
        if(data_ == null || data_.loadData == null)
        {
            return;
        }

        //If the load is starting
        if(data_.loadData.startingLoad)
        {
            //Enabling our loading bar and resetting the value to 0
            this.loadingUIObject.SetActive(true);
            this.loadingBar.value = 0;

            //Setting the total number of load updates
            this.totalUpdates = data_.loadData.totalLoadUpdates;
            this.currentUpdates = 0;
        }
        //If we're still loading
        else
        {
            //Increasing our number of current updates by 1
            this.currentUpdates += 1;

            //Setting the loading bar slider to update the bar
            this.loadingBar.value = (this.currentUpdates * 1f) / (this.totalUpdates * 1f);

            //If we've hit the last update, we disable the loading bar
            if (this.currentUpdates >= this.totalUpdates)
            {
                this.loadingUIObject.SetActive(false);
            }
        }
    }


    //Function called every frame
    private void Update()
    {
        //If the loading bar isn't active, nothing happens
        if(this.loadingUIObject.activeInHierarchy)
        {
            //If we're adding to the background slider's value
            if(this.backgroundSliderAdd)
            {
                this.backgroundSlider.value += this.sliderSpeed;

                //If the slider reached the far end, we reverse direction
                if(this.backgroundSlider.value >= 1)
                {
                    this.backgroundSliderAdd = false;
                }
            }
            //If we're subtracting from the background slider's value
            else
            {
                this.backgroundSlider.value -= this.sliderSpeed;

                //If the slider reached the far end, we reverse direction
                if(this.backgroundSlider.value <= 0)
                {
                    this.backgroundSliderAdd = true;
                }
            }
        }
    }
}
