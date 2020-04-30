using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    //The tile that this was cast at
    private Vector3 castersTilePos;
    //The tile that the target is on
    private Vector3 targetTilePos;

    //The amount of time it takes to interpolate to the target tile
    [Range(0, 3)]
    public float travelTime = 1;
    //Float that tracks the total time the interpolation has been happening
    private float currentTime = 0;

    //Offsets from where this projectile starts and ends
    public Vector3 startTileOffset = new Vector3(0, 50, 0);
    public Vector3 endTileOffset = new Vector3(0, 50, 0);

    //If we arc this projectile, this is the amount of distance up above the midpoint it travels
    public float arcHeightOffset = 0;

    //Bool that determines if we rotate to the direction of travel
    public bool rotateToFaceTarget = true;

    //The type of interpolation this projectile follows along the path to the target
    public ProjectilePath pathType = ProjectilePath.Straight;

    //Bool that needs to be triggered in order to set this projectile to travel along the path
    private bool isTraveling = false;

    //Creating an interpolator for the projectile's Y velocity if we need to arc
    private Interpolator yVelocityInterp;



    //Function called externally from AttackAction.cs or SpellAction.cs when this object is created. Tells this projectile to start traveling
    public void StartTravelPath(Vector3 casterTilePos_, Vector3 targetTilePos_)
    {
        //Sets the start and end position of this projectile's path
        if (this.pathType == ProjectilePath.Straight || this.pathType == ProjectilePath.Arc || this.pathType == ProjectilePath.Instant)
        {
            this.castersTilePos = casterTilePos_;
            this.targetTilePos = targetTilePos_;
        }
        //If the path type is inverted, the positions are reversed
        else
        {
            this.castersTilePos = targetTilePos_;
            this.targetTilePos = casterTilePos_;
        }

        //Adding the offsets to the start and end
        this.castersTilePos += this.startTileOffset;
        this.targetTilePos += this.endTileOffset;

        //If we need to arc this projectile, we create a new interpolator
        if(this.pathType == ProjectilePath.Arc || this.pathType == ProjectilePath.ReversedArc)
        {
            this.yVelocityInterp = new Interpolator(EaseType.CubeOut, this.travelTime/2);
        }

        //Setting our position to the caster tile position
        this.transform.position = new Vector3(this.castersTilePos.x, this.castersTilePos.y, 20);

        //If we need to rotate, then we do
        if(this.rotateToFaceTarget)
        {
            float newAngle = Mathf.Atan2(this.targetTilePos.y - this.castersTilePos.y, this.targetTilePos.x - this.castersTilePos.x) * Mathf.Rad2Deg;
            this.transform.eulerAngles = new Vector3(0, 0, newAngle);
        }

        //Now we allow the Update function to do its work
        this.isTraveling = true;
    }


    //Function called every frame
    private void Update()
    {
        //If we're not traveling, nothing happens
        if(!this.isTraveling)
        {
            return;
        }

        //Finding the difference in X and Y coordinates from the target and caster tiles
        float xDiff = this.targetTilePos.x - this.castersTilePos.x;
        float yDiff = this.targetTilePos.y - this.castersTilePos.y;

        //Based on our path type, our interpolation changes
        switch(this.pathType)
        {
            case ProjectilePath.Straight://If we travel in a straight line to the target
                //Finding the percentage of the x difference based on the percent of time passed
                xDiff = xDiff * (this.currentTime / this.travelTime);
                //Adding the x position from the caster tile (start)
                xDiff += this.castersTilePos.x;
                //Finding the percentage of the y difference based on the percent of time passed
                yDiff = yDiff * (this.currentTime / this.travelTime);
                //Adding the y position from the caster tile (start)
                yDiff += this.castersTilePos.y;
                //Setting our new position using the x and y differences
                this.transform.position = new Vector3(xDiff, yDiff, 20);
                break;

            case ProjectilePath.ReversedStraight://If we travel in a straight line to the caster
                //Finding the percentage of the x difference based on the percent of time passed
                xDiff = xDiff * (this.currentTime / this.travelTime);
                //Adding the x position from the target tile (start)
                xDiff += this.castersTilePos.x;
                //Finding the percentage of the y difference based on the percent of time passed
                yDiff = yDiff * (this.currentTime / this.travelTime);
                //Adding the y position from the target tile (start)
                yDiff += this.castersTilePos.y;
                //Setting our new position using the x and y differences
                this.transform.position = new Vector3(xDiff, yDiff, 20);
                break;

            case ProjectilePath.Arc://If we lob this projectile at the target
                //Finding the percentage of the x difference based on the percent of time passed
                xDiff = xDiff * (this.currentTime / this.travelTime);
                //Adding the x position from the caster tile (start)
                xDiff += this.castersTilePos.x;
                //Finding the Y midpoint, which is the top of the arc
                float yMidpoint = this.castersTilePos.y + (yDiff / 2) + this.arcHeightOffset;

                //If we're in the first half of the travel time, we arc up
                if(this.currentTime < this.travelTime / 2)
                {
                    //Setting the interpolator's time to the amount that's passed
                    this.yVelocityInterp.ResetTime();
                    this.yVelocityInterp.AddTime(this.currentTime);
                    //Finding the difference in height from the caster tile to the midpoint
                    yDiff = yMidpoint - this.castersTilePos.y;
                    //Finding the percent of the height difference that we've interpolated
                    yDiff *= this.yVelocityInterp.GetProgress();
                    //Adding the y position from the caster tile (start)
                    yDiff += this.castersTilePos.y;
                }
                //If we're in the second half of the travel time, we arc down
                else
                {
                    //Setting the ease type for the interpolator to sine in for a downward arc
                    this.yVelocityInterp.ease = EaseType.CubeIn;
                    //Setting the interpolator's time to the amount that's passed for the second half
                    this.yVelocityInterp.ResetTime();
                    this.yVelocityInterp.AddTime(this.currentTime - (this.travelTime / 2));
                    //Finding the difference in height from the midpoint to the target tile
                    yDiff = this.targetTilePos.y - yMidpoint;
                    //Finding the percent of the height difference that we've interpolated
                    yDiff *= this.yVelocityInterp.GetProgress();
                    //Adding the y position from the midpoint
                    yDiff += yMidpoint;
                }

                //If we need to rotate to face the new position
                if(this.rotateToFaceTarget)
                {
                    float newAngle = Mathf.Atan2(yDiff - this.transform.position.y, xDiff - this.transform.position.x) * Mathf.Rad2Deg;
                    this.transform.eulerAngles = new Vector3(0, 0, newAngle);
                }
                //Setting our new position using the x and y differences
                this.transform.position = new Vector3(xDiff, yDiff, 20);
                break;

            case ProjectilePath.ReversedArc://If we lob this projectile at the caster
                //Finding the percentage of the x difference based on the percent of time passed
                xDiff = xDiff * (this.currentTime / this.travelTime);
                //Adding the x position from the caster tile (start)
                xDiff += this.castersTilePos.x;
                //Finding the Y midpoint, which is the top of the arc
                float yReverseMidpoint = this.castersTilePos.y + (yDiff / 2) + this.arcHeightOffset;

                //If we're in the first half of the travel time, we arc up
                if (this.currentTime < this.travelTime / 2)
                {
                    //Setting the interpolator's time to the amount that's passed
                    this.yVelocityInterp.ResetTime();
                    this.yVelocityInterp.AddTime(this.currentTime);
                    //Finding the difference in height from the caster tile to the midpoint
                    yDiff = yReverseMidpoint - this.castersTilePos.y;
                    //Finding the percent of the height difference that we've interpolated
                    yDiff *= this.yVelocityInterp.GetPercent();
                    //Adding the y position from the caster tile (start)
                    yDiff += this.castersTilePos.y;
                }
                //If we're in the second half of the travel time, we arc down
                else
                {
                    //Setting the ease type for the interpolator to sine in for a downward arc
                    this.yVelocityInterp.ease = EaseType.SineIn;
                    //Setting the interpolator's time to the amount that's passed for the second half
                    this.yVelocityInterp.ResetTime();
                    this.yVelocityInterp.AddTime(this.currentTime - (this.travelTime / 2));
                    //Finding the difference in height from the midpoint to the target tile
                    yDiff = this.targetTilePos.y - yReverseMidpoint;
                    //Finding the percent of the height difference that we've interpolated
                    yDiff *= this.yVelocityInterp.GetPercent();
                    //Adding the y position from the midpoint
                    yDiff += yReverseMidpoint;
                }

                //If we need to rotate to face the new position
                if (this.rotateToFaceTarget)
                {
                    float newAngle = Mathf.Atan2(yDiff - this.transform.position.y, xDiff - this.transform.position.x) * Mathf.Rad2Deg;
                    this.transform.eulerAngles = new Vector3(0, 0, newAngle);
                }
                //Setting our new position using the x and y differences
                this.transform.position = new Vector3(xDiff, yDiff, 20);
                break;
        }

        //Once we're done moving, we advance the time forward
        this.currentTime += Time.deltaTime;


        //If the current time becomes greater than the total travel time, we cap it at the total
        if(this.currentTime > this.travelTime)
        {
            this.currentTime = this.travelTime;
        }
    }
}
