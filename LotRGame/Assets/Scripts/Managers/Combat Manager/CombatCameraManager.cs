using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCameraManager : MonoBehaviour
{
    [Tooltip("Camera root object that rotates around the battlefield while no actions are being taken")]
    public GameObject orbitCamObject;
    [Tooltip("Camera that displays all combat tiles so the player can select a tile for their selected action")]
    public Camera tileSelectCam;
    [Tooltip("Camera that looks head-on at the acting character")]
    public Camera actingCharHeadOnCam;
    [Tooltip("Camera that looks at the acting character from behind")]
    public Camera actingCharBehindCam;

    [Tooltip("Camera that's used to interpolate between the acting camera and the camera that will be activated next")]
    public Camera interpCam;
    [Tooltip("The amount of time in seconds for the interp camera to interpolate")]
    public float interpTime = 0.5f;
    //The current counter for how long we've been interpolating
    private float currentInterpTime = 0;
    //Bool for if we are currently interpolating the camera
    private bool isCameraInterpolating = false;
    //Reference to the camera that the interpCam started at
    private Camera interpCamStart = null;
    //Reference to the camera that the interpCam is interpolating to
    private Camera interpCamEnd = null;



    /// <summary>Method called by CombatManager.cs to initialize our cameras for the start of combat</summary>
    public void ResetForCombatStart()
    {
        //Making sure the only enabled camera is the orbit cam for now
        this.orbitCamObject.SetActive(true);
        this.actingCharHeadOnCam.enabled = false;
        this.actingCharBehindCam.enabled = false;
        this.interpCam.enabled = false;
        this.tileSelectCam.enabled = true;

        //Setting the orbit camera's position to the center of the combat characters
        this.RecenterOrbitCamera();
    }


    // Update is called once per frame
    private void Update()
    {
        //Interpolating the interpCam if needed
        if (this.isCameraInterpolating)
        {
            this.currentInterpTime += Time.deltaTime;

            //If the interpolation isn't finished yet
            if(this.currentInterpTime < this.interpTime)
            {
                float percent = this.currentInterpTime / this.interpTime;
                
                //Setting the camera's position based on a percentage of the difference in distance between the start and end cams
                this.interpCam.transform.position = this.interpCamEnd.transform.position - this.interpCamStart.transform.position;
                this.interpCam.transform.position *= percent;
                this.interpCam.transform.position += this.interpCamStart.transform.position;

                //Rotating the camera between the rotations of the start and end cams
                this.interpCam.transform.rotation = Quaternion.Lerp(this.interpCamStart.transform.rotation, this.interpCamEnd.transform.rotation, percent);

                //Changing the camera's FOV
                this.interpCam.fieldOfView = (this.interpCamEnd.fieldOfView - this.interpCamStart.fieldOfView) * percent;
                this.interpCam.fieldOfView += this.interpCamStart.fieldOfView;
            }
            //If the interpolation is finished
            else
            {
                //The interp camera is disabled and the target camera is enabled
                this.interpCam.enabled = false;
                this.interpCamEnd.enabled = true;
                this.isCameraInterpolating = false;
            }
        }
        //If the player is selecting their action
        else if(CombatManager.globalReference.currentState == CombatState.PlayerInput && CombatActionPanelUI.globalReference.selectedAction != null)
        {
            this.tileSelectCam.enabled = true;
        }
    }


    ///<summary>Method called to center the orbit camera root at the center position of all combat characters</summary>
    private void RecenterOrbitCamera()
    {
        int numChars = 0;
        Vector3 posSum = new Vector3();

        foreach (GameObject p in CombatManager.globalReference.characterHandler.playerModels)
        {
            numChars++;
            posSum += p.transform.position;
        }
        foreach (GameObject e in CombatManager.globalReference.characterHandler.enemyModels)
        {
            numChars++;
            posSum += e.transform.position;
        }

        this.orbitCamObject.transform.SetPositionAndRotation(posSum / numChars, this.orbitCamObject.transform.rotation);
    }


    ///<summary>Method called from CombatManager.SetWaitTime to focus the camera on the acting character</summary>
    public void FocusCamOnActingChar()
    {
        //Interpolating from the active camera to the camera facing the acting character
        this.InterpolateCamera(Camera.current, this.actingCharHeadOnCam);

        //Making sure the only enabled camera is the interpcam for now
        this.actingCharHeadOnCam.enabled = false;
        this.actingCharBehindCam.enabled = false;
        this.tileSelectCam.enabled = false;
        this.orbitCamObject.SetActive(false);
    }


    ///<summary>Method called to activate our interp camera and tell it where to go</summary>
    ///<param name="startCam_">Where the interp camera begins the interpolation</param>
    ///<param name="endCam_">Where the interp camera ends up by the end of the interpolation</param>
    private void InterpolateCamera(Camera startCam_, Camera endCam_)
    {
        //If either of the given cameras are null references then nothing happens
        if(startCam_ == null || endCam_ == null)
        {
            return;
        }

        //Setting the interpCam to copy the location, rotation, and FOV of the start cam
        this.interpCam.transform.position = startCam_.transform.position;
        this.interpCam.transform.rotation = startCam_.transform.rotation;
        this.interpCam.fieldOfView = startCam_.fieldOfView;

        //Making sure the start cam is turned off and the interp cam is turned on so the transition is seamless
        startCam_.enabled = false;
        this.interpCam.enabled = true;

        //Resetting the interpolation time and target variables so our update function can perform the interpolation
        this.interpCamStart = startCam_;
        this.interpCamEnd = endCam_;
        this.currentInterpTime = 0;
        this.isCameraInterpolating = true;
    }
}
