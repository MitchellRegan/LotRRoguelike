using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    //The origin location for this object that we can offset from
    private Vector3 origin;

    //The maximum offset that this object can be set from the origin
    public float maxOffsetRadius = 10;
    //Bools that determine which axis we offset
    public bool offsetX = true;
    public bool offsetY = true;

    //The current percent of the max offset for this current shake
    [Range(0,1)]
    private float offsetPercent = 0;

    //Float for the total duration of the current screen shake
    private float totalShakeTime = 1;
    //Float for the current amount of time we've spent shaking
    private float currentShakeTime = 0;

    //Interpolator that we use to alter our offset over the shake time
    private Interpolator ourInterp;

    //Our delegate event that's used to trigger the shake
    private DelegateEvent<EVTData> startShakeEVT;



	// Use this for initialization
	private void Awake ()
    {
        //Setting our origin to this object's local position so we can offset from the parent
        this.origin = this.transform.localPosition;

        //Initializing our interpolator
        this.ourInterp = new Interpolator();

        //Initializing the DelegateEvents for the Event Manager
        this.startShakeEVT = new DelegateEvent<EVTData>(this.StartScreenShake);
	}
	

    //Function called when this component is enabled
    private void OnEnable()
    {
        EventManager.StartListening(ScreenShakeEVT.eventName, this.startShakeEVT);
    }


    //Function called when this component is disabled
    private void OnDisable()
    {
        EventManager.StopListening(ScreenShakeEVT.eventName, this.startShakeEVT);
    }


    //Function called externally to trigger a screen shake
    public void StartScreenShake(EVTData data_)
    {
        //If the time or percent given aren't above 0, OR the shake percent given is less that our current, nothing happens
        if(data_.screenShake.screenShakeDuration <= 0 || data_.screenShake.screenShakePower <= 0 || data_.screenShake.screenShakePower <= this.offsetPercent)
        {
            return;
        }

        //Setting our offset percent
        this.offsetPercent = data_.screenShake.screenShakePower;
        //Setting our shake time
        this.totalShakeTime = data_.screenShake.screenShakeDuration;
        //Resetting our current shake time to the total so we can subtract from it
        this.currentShakeTime = this.totalShakeTime;

        //Resetting our interpolator's time to the total shake time and ease curve given
        this.ourInterp.ResetTime();
        this.ourInterp.SetDuration(this.totalShakeTime);
        this.ourInterp.ease = data_.screenShake.screenShakeCurve;
    }


	// Update is called once per frame
	private void Update ()
    {
		//If we aren't currently shaking, nothing happens
        if(this.currentShakeTime <= 0)
        {
            return;
        }

        //Decreasing the current shake time
        this.currentShakeTime -= Time.deltaTime;

        //If the current shake time is below 0, we can't let it go below that
        if(this.currentShakeTime <= 0)
        {
            this.currentShakeTime = 0;
            this.transform.localPosition = this.origin;
            return;
        }

        //Setting our interpolator's time to the current shake time
        this.ourInterp.ResetTime();
        this.ourInterp.AddTime(this.currentShakeTime);

        //Finding the percent of the radius we can offset
        float offsetDist = this.maxOffsetRadius * this.ourInterp.GetProgress();

        //Finding a random angle to offset at (in radians)
        float radAngle = Random.Range(0, 359) * Mathf.Deg2Rad;

        //Finding the x and y offsets using the radian angle and the offset distance
        float oX = this.origin.x;
        if (this.offsetX)
        {
            oX += (Mathf.Cos(radAngle) * offsetDist);
        }
        float oY = this.origin.y;
        if (this.offsetY)
        {
            oY += (Mathf.Sin(radAngle) * offsetDist);
        }

        //Setting this object's transform using our new offsets
        this.transform.localPosition = new Vector3(oX, oY, this.origin.z);
	}
}
