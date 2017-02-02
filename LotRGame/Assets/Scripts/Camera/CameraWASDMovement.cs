using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWASDMovement : MonoBehaviour
{
    //Reference to the object where the player camera pivots
    public Transform cameraPivot;
    //Movement speed of this object
    public float moveSpeed = 50;
    //Maximum movement ranges from the origin that the player camera can move
    public Vector2 maxXZRanges = new Vector2(500, 500);



    // Update is called once per frame
    private void Update()
    {
        //Velocities to calculate and add to the current veloctiy
        float upDownVelocity = 0;
        float leftRightVelocity = 0;

        //Moving Up
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            upDownVelocity += this.moveSpeed;
        }
        //Moving Down
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            upDownVelocity -= this.moveSpeed;
        }

        //Moving Left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            leftRightVelocity -= moveSpeed;
        }
        //Moving Right
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            leftRightVelocity += this.moveSpeed;
        }

        //Adjusts the new velocities to take rotation into account
        float xSpeed = upDownVelocity * Mathf.Sin(this.cameraPivot.eulerAngles.y * Mathf.Deg2Rad);
        float ySpeed = upDownVelocity * Mathf.Cos(this.cameraPivot.eulerAngles.y * Mathf.Deg2Rad);

        xSpeed += leftRightVelocity * Mathf.Sin((this.cameraPivot.eulerAngles.y + 90) * Mathf.Deg2Rad);
        ySpeed += leftRightVelocity * Mathf.Cos((this.cameraPivot.eulerAngles.y + 90) * Mathf.Deg2Rad);


        //Prevents the x coord from going beyond the max ranges
        if (this.transform.localPosition.x + xSpeed > this.maxXZRanges.x)
        {
            xSpeed = this.maxXZRanges.x - this.transform.localPosition.x;
        }
        else if (this.transform.localPosition.x + xSpeed < -this.maxXZRanges.x)
        {
            xSpeed = this.maxXZRanges.x + this.transform.localPosition.x;
        }


        //Prevents the z coord from going beyond the max ranges
        if (this.transform.localPosition.y + ySpeed > this.maxXZRanges.y)
        {
            ySpeed = this.maxXZRanges.y - this.transform.localPosition.y;
        }
        else if (this.transform.localPosition.y + ySpeed < -this.maxXZRanges.y)
        {
            ySpeed = this.maxXZRanges.y + this.transform.localPosition.y;
        }

        //Updates the position of this object using the final velocities
        this.transform.localPosition += new Vector3(xSpeed, ySpeed, 0);
    }
}
