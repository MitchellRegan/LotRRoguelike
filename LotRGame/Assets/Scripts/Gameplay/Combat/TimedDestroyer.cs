using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroyer : MonoBehaviour
{
    //The amount of time in seconds that this object will be alive. When the time is up, it destroys this game object
    public float timeUntilDestroyed = 1;



	// Update is called once per frame
	private void Update ()
    {
        //Subtracting the time that's passed since the last frame from our current time
        this.timeUntilDestroyed -= Time.deltaTime;

        //If our current time is up, we destroy this game object
        if(this.timeUntilDestroyed <= 0)
        {
            Destroy(this.gameObject);
        }
	}
}
