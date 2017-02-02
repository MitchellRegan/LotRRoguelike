using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Food : MonoBehaviour
{
    //The amount of hunger this piece of food restores when eaten
    public int hungerRestored = 1;

    //The number of days this food lasts before it becomes rotten
    public int daysOfFreshness = 10;
    //The current number of days this food has lasted
    public int currentDayCount = 0;

    //The item that this food turns into when it is no longer fresh
    public Item turnsInto;
}
