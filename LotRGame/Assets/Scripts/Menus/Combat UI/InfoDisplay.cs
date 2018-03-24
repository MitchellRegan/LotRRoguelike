using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoDisplay : MonoBehaviour
{
    //The reference to the component that displays our text
    public Text textRef;

    //The state for how we update
    private enum TransitionState { Hide, TransitionIn, Show, TransitionOut };
    private TransitionState currentState = TransitionState.Hide;

    //The amount of time it takes for this to transition to and from the display position
    public float transitionTime = 0.25f;
    //The amount of time we display the info before transitioning out
    private float timeToDisplay = 1;
    //Float that tracks the current time for the current transition
    private float currentTime = 0;

    //The position where we start the transition in
    public Vector3 transitionStartPos;
    //The position where we hold on screen when displaying info
    private Vector3 transitionStayPos;
    //The position where we end the transition out
    public Vector3 transitionOutPos;


    //Function called the first frame this object is active
    private void Start()
    {
        this.transitionStayPos = transform.localPosition;
        this.transitionStartPos += this.transitionStayPos;
        this.transitionOutPos += this.transitionStayPos;

        //Setting our transform to the start position 
        this.transform.localPosition = this.transitionStartPos;

        //Setting our starting state to "Hide"
        this.currentState = TransitionState.Hide;
    }

    //Function called externally from CombatManager.cs to start this info display
    public void StartInfoDisplay(string infoText_, float timeToDisplay_)
    {
        //Sets the current state to start transitioning in
        this.currentState = TransitionState.TransitionIn;
        //Sets the time to display the info
        this.timeToDisplay = timeToDisplay_;
        //Resets our timer
        this.currentTime = 0;
        //Sets our position to the transition start pos
        this.transform.localPosition = this.transitionStartPos;
        //Setting our text component to display the info text
        this.textRef.text = infoText_;
    }

	
	// Update is called once per frame
	void Update ()
    {
        //Updating based on our state
		switch(this.currentState)
        {
            //If we're staying hidden, nothing happens
            case TransitionState.Hide:
                return;

            //Moving into position
            case TransitionState.TransitionIn:
                //Increasing the current time
                this.currentTime += Time.deltaTime;
                //If the current time has reached the total transition time, we need to switch states to "Show"
                if(this.currentTime >= this.transitionTime)
                {
                    this.currentState = TransitionState.Show;
                    this.currentTime = 0;
                    this.transform.localPosition = this.transitionStayPos;
                }
                //Otherwise, we're still transitioning
                else
                {
                    //Setting our position between the start pos and the stay pos based on the percent of time passed
                    float transInPercent = this.currentTime / this.transitionTime;
                    this.transform.localPosition = ((this.transitionStayPos - this.transitionStartPos) * transInPercent) + this.transitionStartPos;
                }
                break;

            case TransitionState.Show:
                //Increasing the current time
                this.currentTime += Time.deltaTime;
                //If the current time has reached the total time to display, we need to switch states to "Transition Out"
                if(this.currentTime >= this.timeToDisplay)
                {
                    this.currentState = TransitionState.TransitionOut;
                    this.currentTime = 0;
                }
                break;

            case TransitionState.TransitionOut:
                //Increasing the current time
                this.currentTime += Time.deltaTime;
                //If the current time has reached the total transition time, we need to switch states to "Hide"
                if (this.currentTime >= this.transitionTime)
                {
                    this.currentState = TransitionState.Hide;
                    this.currentTime = 0;
                    this.transform.localPosition = this.transitionOutPos;
                }
                //Otherwise, we're still transitioning
                else
                {
                    //Setting our position between the stay pos and the out pos based on the percent of time passed
                    float transInPercent = this.currentTime / this.transitionTime;
                    this.transform.localPosition = ((this.transitionOutPos - this.transitionStayPos) * transInPercent) + this.transitionStayPos;
                }
                break;
        }
	}
}
