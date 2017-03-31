using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Food : MonoBehaviour
{
    //The amount of hunger this piece of food restores when eaten
    public float hungerRestored = 1;

    //The number of days this food lasts before it becomes rotten
    public int daysOfFreshness = 10;
    //The current number of days this food has lasted
    public float currentDayCount = 0;

    //The item that this food turns into when it is no longer fresh
    public Item turnsInto;


    //Function called externally to update the amount of time passed
    public void OnTimeAdvanced()
    {
        //Finding out how much time we should age this item
        float daysPassed = (TimePanelUI.globalReference.hoursAdvancedPerUpdate * 1f) / 24f;
        this.currentDayCount += daysPassed;

        //Checking to see if this item is still fresh
        if(this.currentDayCount >= this.daysOfFreshness)
        {
            Debug.Log(this.name + " has expired and needs to turn into " + this.turnsInto.name);
        }
    }
}
