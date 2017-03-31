using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispatchEvents : MonoBehaviour
{
    //List of event names that can be dispatched to the Event Manager
    public List<string> eventNamesToDispatch;


    //Public function called externally. Dispatches a blank event to the Event Manager using the name of the index given
    public void DispatchEvent(int nameIndex_)
    {
        Debug.Log(this.gameObject.name + " Dispatch Events");
        //Making sure that the index is valid
        if (this.eventNamesToDispatch.Count < nameIndex_ && nameIndex_ >= 0)
        {
            Debug.Log("Dispatch worked?");
            EventManager.TriggerEvent(this.eventNamesToDispatch[nameIndex_]);
        }
    }


    //Public function called externally. Dispatches multiple blank events to the Event Manager using all event names
    public void DispatchAllEvents()
    {
        foreach (string name in this.eventNamesToDispatch)
        {
            EventManager.TriggerEvent(name);
        }
    }
}
