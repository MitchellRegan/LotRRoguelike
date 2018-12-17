using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispatchEvents : MonoBehaviour
{
    //List of event nums that can be dispatched to the Event Manager
    public List<byte> eventNumsToDispatch;


    //Public function called externally. Dispatches a blank event to the Event Manager using the num of the index given
    public void DispatchEvent(int nameIndex_)
    {
        Debug.Log(this.gameObject.name + " Dispatch Events");
        //Making sure that the index is valid
        if (this.eventNumsToDispatch.Count < nameIndex_ && nameIndex_ >= 0)
        {
            Debug.Log("Dispatch worked?");
            EventManager.TriggerEvent(this.eventNumsToDispatch[nameIndex_]);
        }
    }


    //Public function called externally. Dispatches multiple blank events to the Event Manager using all event nums
    public void DispatchAllEvents()
    {
        foreach (byte num in this.eventNumsToDispatch)
        {
            EventManager.TriggerEvent(num);
        }
    }
}
