using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimedActivator : MonoBehaviour
{
    //Time in seconds before this component calls the Unity Event
    public float timeBeforeEvent = 1;
    private float currentTime = 0;

    //The Unity Event that's activated once our timer is up
    public UnityEvent timedEvent;



	// Update is called once per frame
	private void Update ()
    {
        //As long as our current time is under the time set before the event happens, we add the delta time
		if(currentTime < this.timeBeforeEvent)
        {
            currentTime += Time.deltaTime;

            //If the current time reaches the set time, we call our Unity Event and disable this component
            if(currentTime >= this.timeBeforeEvent)
            {
                this.timedEvent.Invoke();
                this.enabled = false;
            }
        }
	}


    //Function called externally to reset this component's timer
    public void ResetTimer()
    {
        //Makes sure this component is turned on
        this.enabled = true;
        //Sets our current time to 0
        this.currentTime = 0;
    }
}
