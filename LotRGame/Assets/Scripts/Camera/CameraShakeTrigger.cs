using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeTrigger : MonoBehaviour
{
    //The amount of time this component waits before triggering the screen shake
    public float timeDelay = 0;
    //The amount of time that the screen will shake once triggered
    public float screenShakeDuration = 1;
    //The percent of the maximum amount the screen can shake
    [Range(0,1)]
    public float screenShakePower = 0.5f;
    //The curve that determines how fast the shake returns to normal
    public EaseType screenShakeCurve = EaseType.SineIn;
	


	// Update is called once per frame
	private void Update ()
    {
        //Subtracting the time passed since the previous frame
        this.timeDelay -= Time.deltaTime;

        //If the time delay is over, we trigger the screen shake
        if (this.timeDelay <= 0)
        {
            //Creating a new EVTData class and giving it the screen shake data
            EVTData shakeEVT = new EVTData();
            shakeEVT.screenShake = new ScreenShakeEVT(this.screenShakeDuration, this.screenShakePower, this.screenShakeCurve);
            //Dispatching the event data to the Event Manager
            EventManager.TriggerEvent(ScreenShakeEVT.eventNum, shakeEVT);
            //Disabling this component so we don't keep triggering the shake
            this.enabled = false;
        }
	}
}
