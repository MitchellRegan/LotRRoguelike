using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReceiveEvent : MonoBehaviour
{
    //Name of the event dispatched from the EventManager that triggers the UnityEvents
    public string eventNameToListenFor;
    //List of UnityEvents triggered from the Event Name To Listen For
    public List<UnityEvent> eventsOnReceive;
    //Delegate event that listens for the correct event name
    private DelegateEvent<EVTData> customListener;



    //Setting the event delegate to trigger
    private void Awake()
    {
        this.customListener = new DelegateEvent<EVTData>(this.EventTriggered);
    }


    //Tells the EventManager to listen for our event name
    private void OnEnable()
    {
        EventManager.StartListening(this.eventNameToListenFor, this.customListener);
    }


    //Tells the EventManager to stop listening for our event name
    private void OnDisable()
    {
        EventManager.StopListening(this.eventNameToListenFor, this.customListener);
    }


    //Private function called from the EventManager using our delegate. Triggers all UnityEvents
    private void EventTriggered(EVTData data_)
    {
        //Invokes all UnityEvents 
        foreach (UnityEvent evt in this.eventsOnReceive)
        {
            evt.Invoke();
        }
    }
}
