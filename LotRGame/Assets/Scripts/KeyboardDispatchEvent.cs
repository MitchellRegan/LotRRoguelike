using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyboardDispatchEvent : MonoBehaviour
{
    //The keyboard buttons that trigger events with the corresponding list index
    public List<KeyCode> keysThatTriggerEvents;
    //The unity events that are triggered when the button with the same index is pressed
    public List<UnityEvent> unityEventsToTrigger;


    // Update is called once per frame
    private void Update()
    {
        //Loops through and finds if any of the keys we're looking for are pressed
        for (int i = 0; i < this.keysThatTriggerEvents.Count; ++i)
        {
            if (Input.GetKey(this.keysThatTriggerEvents[i]))
            {
                this.DispatchEvent(i);
            }
        }
    }


    //Public event that's called from Update and can be called Externally. 
    public void DispatchEvent(int index_)
    {
        //Only dispatches the Unity Event if there's a cooresponding event
        if (index_ < this.unityEventsToTrigger.Count)
        {
            this.unityEventsToTrigger[index_].Invoke();
        }
    }
}
