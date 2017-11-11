using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    //Static reference to this camera's rotation so the UI compas can rotate
    public static float directionFacing = 0;

    //The amount that's rotated along the x and y axis per frame
    public float xRotationSpeed = 6;
    public float yRotationSpeed = 8;

    //If true the x rotation is inverted
    public bool invertX = false;
    //If true the y rotation is inverted
    public bool invertY = false;

    //The minimum and maximum angles that the X value can be rotated between
    public Vector2 xRotMinMax = new Vector2(-45, 80);

    //The button that's held to activate the mouse rotation in this script
    public KeyCode activateButton = KeyCode.LeftAlt;

    //If the Left Mouse button has to be down to rotate
    public bool leftMouseActivate = false;
    //If the Right Mouse button has to be down to rotate
    public bool rightMouseActivate = false;



    // Update is called once per frame
    private void Update()
    {
        //Setting the static rotation variable to this camera's Y rotation
        directionFacing = (-1 * this.transform.eulerAngles.y) + 90;
        if(directionFacing < 0)
        {
            directionFacing += 360;
        }

        //Only rotates when the activate button is down and the correct mouse buttons are down
        if (Input.GetKey(this.activateButton) && this.leftMouseActivate == Input.GetMouseButton(0) && this.rightMouseActivate == Input.GetMouseButton(1))
        {
            //Finding the total movement of degrees the camera should be rotated based on the mouse movement
            float xRot = Input.GetAxis("Mouse Y") * -this.xRotationSpeed;
            float yRot = Input.GetAxis("Mouse X") * -this.yRotationSpeed;

            //Checking if we need to invert the X rotation
            if(this.invertX)
            {
                xRot *= -1;
            }

            //Checking if we need to invert the Y rotation
            if(this.invertY)
            {
                yRot *= -1;
            }

            //If the current rotation is between 0 and 90 degrees
            if(this.transform.localEulerAngles.x >= 0 && this.transform.localEulerAngles.x < 90)
            {
                //If adding to the x rotation would put it over the max X rotation, it stops at the max
                if (this.transform.localEulerAngles.x + xRot > this.xRotMinMax.y)
                {
                    xRot = this.xRotMinMax.y - this.transform.localEulerAngles.x;
                }
                //If the min x rotation is at or above 0 and rotating would put it below the min, it stops at the min
                else if(this.xRotMinMax.x >= 0 && xRot + this.transform.localEulerAngles.x < this.xRotMinMax.x)
                {
                    xRot = this.xRotMinMax.x - this.transform.localEulerAngles.x;
                }
            }
            //Otherwise, the rotation went below 0 and looped back around past 360
            else
            {
                //If adding to the x rotation would put it under the min x rotation, it stops at the min
                if(xRot + this.transform.localEulerAngles.x < 360 + this.xRotMinMax.x)
                {
                    xRot = (360 + this.xRotMinMax.x) - this.transform.localEulerAngles.x;
                }
            }

            //Applying the rotation to this object's transform
            this.transform.localEulerAngles += new Vector3(xRot, yRot, 0);
        }
    }
}
