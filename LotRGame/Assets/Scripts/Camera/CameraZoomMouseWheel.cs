using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraZoomMouseWheel : MonoBehaviour
{
    //The minimum and maximum Z positions in local space
    public Vector2 zPosMinMax = new Vector2(5, 50);

    //The amount that this camera zooms in/out when the mouse scroll wheel is turned
    public Vector2 scrollSpeedMinMax = new Vector2(100, 250);


    
    //Function called every frame
    private void Update()
    {
        //Raycasting from the mouse to find out what's under it
        List<RaycastResult> results = new List<RaycastResult>();
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;
        EventSystem.current.RaycastAll(pointerData, results);

        //Looping through all of the objects hit by the raycast
        if(results.Count > 0)
        {
            foreach(RaycastResult rayResult in results)
            {
                //If we find a scroll rectangle under the mouse's current position, we prevent the player from zooming
                if(rayResult.gameObject.GetComponent<ScrollRect>())
                {
                    return;
                }
            }
        }

        //Finding the correct amount to zoom in based on the camera's current distance
        float distPercent = 1 - (this.transform.localPosition.z - this.zPosMinMax.x) / (this.zPosMinMax.y - this.zPosMinMax.x);
        float scrollSpeed = (distPercent * (this.scrollSpeedMinMax.y - this.scrollSpeedMinMax.x)) + this.scrollSpeedMinMax.x;

        //Moves this camera in and out based on the input from the mouse scroll wheel
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        this.transform.localPosition += new Vector3(0, 0, scrollDelta);

        //Makes sure the camera stays within the min/max distances
        if(this.transform.localPosition.z < this.zPosMinMax.x)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x,
                                                        this.transform.localPosition.y,
                                                        this.zPosMinMax.x);
        }
        else if(this.transform.localPosition.z > this.zPosMinMax.y)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x,
                                                        this.transform.localPosition.y,
                                                        this.zPosMinMax.y);
        }
    }
}
