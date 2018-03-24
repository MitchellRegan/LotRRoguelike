using UnityEngine;
using UnityEngine.Events; //For events
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic; //For dictionary

//The delegate that we use to call all of our events
public delegate void DelegateEvent<T>(T data_) where T : EVTData;

public class EventManager : MonoBehaviour
{
    //The dictionary where we hold all of our events and their name tags
    private Dictionary<string, List<DelegateEvent<EVTData>>> EventDictionary;
    //A reference to this event manager that can be accessed anywhere
    public static EventManager EVTManager;


    //Initializes this event manager and the event dictionary
    void Awake()
    {
        //If there isn't already a static reference to this event manager, this instance becomes the static reference
        if (EVTManager == null)
        {
            EVTManager = GetComponent<EventManager>();
        }
        else
        {
            //Debug.LogError("ERROR: Manager_EventManager.Awake, there is already a static instance of EVTManager");
        }

        //Initializes a new dictionary to hold all events
        if (EventDictionary == null)
        {
            EventDictionary = new Dictionary<string, List<DelegateEvent<EVTData>>>();
        }
    }



    //Adds the given UnityAction to the dictionary of events under the given event name
    public static void StartListening(string evtName_, DelegateEvent<EVTData> evtListener_)
    {
        List<DelegateEvent<EVTData>> startListeningDelegate = null;

        //Checks to see if our entry for the event dictionary is found. If so, adds the listener to the event
        if (EVTManager.EventDictionary.TryGetValue(evtName_, out startListeningDelegate))
        {
            startListeningDelegate.Add(evtListener_);
        }
        //If an existing entry isn't found, a new entry is created and added to the dictionary
        else
        {
            startListeningDelegate = new List<DelegateEvent<EVTData>>();
            startListeningDelegate.Add(evtListener_);
            EVTManager.EventDictionary.Add(evtName_, startListeningDelegate);
        }
    }



    //Removes the given UnityAction from the dictionary of events with the given event name
    public static void StopListening(string evtName_, DelegateEvent<EVTData> evtListener_)
    {
        if (EVTManager == null)
            return;

        List<DelegateEvent<EVTData>> stopListeningDelegate = null;

        //Checks to see if our entry for the event dictionary is found. If so, removes the listener from the event
        if (EVTManager.EventDictionary.TryGetValue(evtName_, out stopListeningDelegate))
        {
            stopListeningDelegate.Remove(evtListener_);
        }
        //If an existing entry isn't found, nothing happens
    }



    //Invokes the event with the given name, calling all functions attached to the event
    public static void TriggerEvent(string evtName_, EVTData dataPassed_ = null)
    {
        List<DelegateEvent<EVTData>> triggerDelegate = null;

        //Null event data can't be sent, so we send an empty data event instead
        if (dataPassed_ == null)
        {
            dataPassed_ = new EVTData();
        }

        //Checks to see if our entry for the event dictionary is found. If so, invokes the event to call all functions attached to it
        if (EVTManager.EventDictionary.TryGetValue(evtName_, out triggerDelegate))
        {
            foreach (DelegateEvent<EVTData> evt_ in triggerDelegate)
            {
                evt_(dataPassed_);
            }
        }
    }
}