using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    /*########## MOUSE EVENTS ##########*/
    //Left click
    public delegate void LeftMouseEvent();
    public static event LeftMouseEvent OnLeftMouseDown;
    public static event LeftMouseEvent OnLeftMouseUp;
    public static event LeftMouseEvent OnLeftMouse;
    //Right Click
    public delegate void RightMouseEvent();
    public static event RightMouseEvent OnRightMouseDown;
    public static event RightMouseEvent OnRightMouseUp;
    public static event RightMouseEvent OnRightMouse;
    //Middle click
    public delegate void MiddleMouseEvent();
    public static event MiddleMouseEvent OnMiddleMouseDown;
    public static event MiddleMouseEvent OnMiddleMouseUp;
    public static event MiddleMouseEvent OnMiddleMouse;

    /*########## CAMERA EVENTS ##########*/
    public delegate void CameraEvent(GameObject targetCam1, GameObject targetCam2);
    public static event CameraEvent EnableCamera;
    public static event CameraEvent DisableCamera;
    public static event CameraEvent ChangeCamera;

    /*########## CUSTOM EVENTS ##########*/
    public delegate void TransitionEvent();
    public static event TransitionEvent GoToLevel;

    public delegate void MapEvent();
    public static event MapEvent GeneratePathPoints;
    public static event MapEvent ConnectPathPoints;

    public delegate void GameEvent();
    public static event GameEvent TimePass;



    // Update is called once per frame
    void Update()
    {
        //MOUSE DOWN EVENTS
        //Left Mouse Down
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (OnLeftMouseDown != null)
            {
                OnLeftMouseDown();
            }
        }
        //Right Mouse Down
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (OnRightMouseDown != null)
            {
                OnRightMouseDown();
            }
        }
        //Middle Mouse Down
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            if (OnMiddleMouseDown != null)
            {
                OnMiddleMouseDown();
            }
        }


        //MOUSE UP EVENTS
        //Left Mouse Up
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (OnLeftMouseUp != null)
            {
                OnLeftMouseUp();
            }
        }
        //Right Mouse Up
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            if (OnRightMouseUp != null)
            {
                OnRightMouseUp();
            }
        }
        //Left Mouse Up
        if (Input.GetKeyUp(KeyCode.Mouse2))
        {
            if (OnMiddleMouseUp != null)
            {
                OnMiddleMouseUp();
            }
        }


        //MOUSE HELD EVENTS
        //Left Mouse Held
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (OnLeftMouse != null)
            {
                OnLeftMouse();
            }
        }
        //Right Mouse Held
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (OnRightMouse != null)
            {
                OnRightMouse();
            }
        }
        //Left Mouse Held
        if (Input.GetKey(KeyCode.Mouse2))
        {
            if (OnMiddleMouse != null)
            {
                OnMiddleMouse();
            }
        }
    }


	public void SendGoToLevel(string LevelName_, bool ShouldFade_, float FadeTime_)
	{
		//Sends out the GoToLevel event
		if (GoToLevel != null) 
		{
			GoToLevel ();
        }

		//Loads the level with the given name
		Application.LoadLevel (LevelName_);
    }


	//Sends out the TimePass event
	public void SendTimePass()
	{
        if (TimePass != null)
        {
            TimePass();
        }
	}


	//Sends out the GeneratePathPoints event. SHOULD ONLY BE CALLED ONCE
	public void SendGeneratePathPoints()
	{
        if(GeneratePathPoints != null)
        {
            GeneratePathPoints();
        }
	}

	//Sends out the ConnectPathPoints event. SHOULD ONLY BE CALLED ONCE
	public void SendConnectPathPoints()
    {
        if (ConnectPathPoints != null)
        {
            ConnectPathPoints();
        }
    }
}
