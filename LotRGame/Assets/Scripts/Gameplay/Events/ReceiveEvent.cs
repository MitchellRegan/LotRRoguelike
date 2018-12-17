using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReceiveEvent : MonoBehaviour
{
    //Num of the event dispatched from the EventManager that triggers the UnityEvents
    public byte eventNumToListenFor;
    //List of UnityEvents triggered from the Event num To Listen For
    public List<UnityEvent> eventsOnReceive;
    //Delegate event that listens for the correct event num
    private DelegateEvent<EVTData> customListener;



    //Setting the event delegate to trigger
    private void Awake()
    {
        this.customListener = new DelegateEvent<EVTData>(this.EventTriggered);
    }


    //Tells the EventManager to listen for our event num
    private void OnEnable()
    {
        EventManager.StartListening(this.eventNumToListenFor, this.customListener);
    }


    //Tells the EventManager to stop listening for our event num
    private void OnDisable()
    {
        EventManager.StopListening(this.eventNumToListenFor, this.customListener);
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
