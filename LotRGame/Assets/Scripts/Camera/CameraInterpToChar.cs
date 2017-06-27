using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInterpToChar : MonoBehaviour
{
    //Reference to the group that this camera interpolates to
    private PartyGroup groupToFollow;

    //The percent of distance that's covered each frame
    [Range(0.01f, 0.99f)]
    public float interpDist = 0.9f;

    //Vec3 to hold the difference in position betwee this camera and the group to follow
    private Vector3 distDiff;



	// Function called on the first frame of gameplay
	private void Start ()
    {
        //Sets the groupToFollow as the selected party group in the character manager
        this.transform.position = CharacterManager.globalReference.selectedGroup.transform.position;
        this.ChangeGroupToFollow();
	}
	

	// Update is called once per frame
	private void Update ()
    {
        //Finding the difference in position between this camera and the group to follow
        this.distDiff = this.groupToFollow.transform.position - this.transform.position;
        //Multiplying the distance difference by the interp percent to get the distance to move
        this.distDiff *= this.interpDist;
        //Adding the distance difference to this camera's position
        this.transform.position += this.distDiff;
	}


    //Public function called from the Character List UI in the Gameplay scene to set a new character to interpolate to
    public void ChangeGroupToFollow()
    {
        this.groupToFollow = CharacterManager.globalReference.selectedGroup;
    }
}
