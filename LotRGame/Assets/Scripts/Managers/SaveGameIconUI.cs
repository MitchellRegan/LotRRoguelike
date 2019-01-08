using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameIconUI : MonoBehaviour
{
    //Send event from SaveLoadManager to turn this on
    //Track the amount of time that passes
    //Rotate spinny thing
    //Send event from SaveLoadManager to indicate the save is done
    //If the time passed is less than X sec, keep spinning until X sec has passed

    //Object to enable when we're saving
    public GameObject iconObject;

    //Object to rotate so we can tell if the game freezes
    public RectTransform objectToRotate;
    //The rotation speed of the rotation object
    public float rotationSpeed = 2f;

    //The minimum amount of time this icon is on screen
    public float minIconTime = 0.5f;
    //Counter for the current amount of time this icon has been on screen
    private float currentIconTime = 0;
    //Bool for if we need to wait for the save icon minimum time to finish
    private bool waitForMinTime = false;

    //Delegate for the function to activate and deactivate the save icon
    private DelegateEvent<EVTData> toggleSaveEVT;



    //Function called when this object is created
    private void Awake()
    {
        //Making sure the icon is off by default
        this.iconObject.SetActive(false);

        this.toggleSaveEVT = new DelegateEvent<EVTData>(this.ToggleSaveIcon);
    }


    //Function called when this component is enabled
    private void OnEnable()
    {
        EventManager.StartListening(SaveDataEVT.eventNum, this.toggleSaveEVT);
    }


    //Function called when this component is disabled
    private void OnDisable()
    {
        EventManager.StopListening(SaveDataEVT.eventNum, this.toggleSaveEVT);
    }


    //Function called from the toggleSaveEVT delegate
    private void ToggleSaveIcon(EVTData data_)
    {
        //If the save is starting
        if(data_.saveData.startingSave)
        {
            //Enabling our icon object
            this.iconObject.SetActive(true);
        }
        //If the save is done
        else
        {
            //If the current icon timer has been on screen for at least the minimum time
            if(this.currentIconTime >= this.minIconTime)
            {
                //Disabling our icon object and resetting the timer
                this.iconObject.SetActive(false);
                this.currentIconTime = 0;
            }
            //If the icon hasn't been on screen for the minimum amount of time
            else
            {
                //We wait for the minimum amount of time before disabling the icon
                this.waitForMinTime = true;
            }
        }
    }


    //Function called every frame
    private void Update()
    {
        //If the icon isn't active, nothing happens
        if(this.iconObject.activeInHierarchy)
        {
            //Increasing our current timer
            this.currentIconTime += Time.deltaTime;

            //Spinning our rotator object
            this.objectToRotate.transform.localEulerAngles += new Vector3(0, 0, this.rotationSpeed);

            //If we're waiting for the minimum time on screen, we check the current time
            if(this.waitForMinTime)
            {
                //If the current timer is above the minimum time
                if(this.currentIconTime >= this.minIconTime)
                {
                    //We reset the timer and disable our icon
                    this.currentIconTime = 0;
                    this.waitForMinTime = false;
                    this.iconObject.SetActive(false);
                }
            }
        }
    }
}
