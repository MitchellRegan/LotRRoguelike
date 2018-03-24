using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSeedGenerator : MonoBehaviour
{
    //String that the user can use to set a seed for the Random functions
    public string seed;


	//Function called on Initialization
    private void Awake()
    {
        //Sets the seed
        this.SetSeed(this.seed);
    }


    //Function called from Awake and can be called externally. Sets the random seed based on the input string
    public void SetSeed(string seed_)
    {
        //Randomizes the seed based on the tick count if no seed was given
        if (seed_ == "")
        {
            Random.InitState((int)System.Environment.TickCount);
            return;
        }

        //Converts the characters in the given string to an int
        int seedValue = 0;
        foreach(char c in seed_)
        {
            seedValue += c.GetHashCode();
        }

        //Sets the state of the Random functions using our value
        Random.InitState(seedValue);
    }
}
