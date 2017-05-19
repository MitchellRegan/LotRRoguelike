using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInterpToChar : MonoBehaviour
{
    //Reference to the character that this camera interpolates to
    private Character characterToFollow;

    //The percent of distance that's covered each frame
    [Range(0.01f, 0.99f)]
    public float interpDist = 0.9f;

    //Vec3 to hold the difference in position betwee this camera and the character to follow
    private Vector3 distDiff;



	// Function called on the first frame of gameplay
	private void Start ()
    {
        //Sets the characterToFollow as the character in slot 1
        this.characterToFollow = CharacterManager.globalReference.playerParty[0];
	}
	

	// Update is called once per frame
	private void Update ()
    {
        //Finding the difference in position between this camera and the character to follow
        this.distDiff = this.characterToFollow.transform.position - this.transform.position;
        //Multiplying the distance difference by the interp percent to get the distance to move
        this.distDiff *= this.interpDist;
        //Adding the distance difference to this camera's position
        this.transform.position += this.distDiff;
	}


    //Public function called from the Character List UI in the Gameplay scene to set a new character to interpolate to
    public void SetCharToFollow(Character newCharToFollow_)
    {
        this.characterToFollow = newCharToFollow_;
    }
}
