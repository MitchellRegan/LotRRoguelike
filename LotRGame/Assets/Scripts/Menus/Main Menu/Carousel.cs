using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Carousel : MonoBehaviour
{
    //Float for how long (in sec) it takes to transition to the next item
    public float transitionTime = 0.2f;
    private float currentTransitionTime = 0;

    //References to the buttons that rotate this carousel so we can disable them
    public Button nextButton;
    public Button prevButton;

    [Space(8)]

    //The list of UI elements that will be rotated
    public List<RectTransform> carouselItems;

    [Space(8)]

    //The offset for the center of the carousel rotation
    public float centerOffset = 100;
    public float width = 300;
    public float depth = 50;
    public AnimationCurve depthScale;

    //The index of the currently selected carousel item
    private int currentIndex = 0;

    //enum for if we're currently transitioning to a different element
    private enum CarouselState { Stopped, Next, Previous };
    //Enum for our current state
    private CarouselState state = CarouselState.Stopped;

    //The list of positions that carousel items stop at
    private Vector3[] itemPositions;



    // Start is called before the first frame update
    private void Awake()
    {
        this.InitializeItemPositions();
        this.UpdateItemPos(1 - (this.currentTransitionTime / this.transitionTime), CarouselState.Next);
    }


    // Update is called once per frame
    private void Update()
    {
        if(this.state == CarouselState.Stopped)
        {
            return;
        }
        else if(this.state == CarouselState.Next)
        {
            this.currentTransitionTime -= Time.deltaTime;
            if(this.currentTransitionTime < 0)
            {
                this.currentTransitionTime = 0;
                this.state = CarouselState.Stopped;
                this.nextButton.interactable = true;
                this.prevButton.interactable = true;
            }

            this.UpdateItemPos(1 - (this.currentTransitionTime / this.transitionTime), CarouselState.Next);
        }
        else if(this.state == CarouselState.Previous)
        {
            this.currentTransitionTime -= Time.deltaTime;
            if (this.currentTransitionTime < 0)
            {
                this.currentTransitionTime = 0;
                this.state = CarouselState.Stopped;
                this.nextButton.interactable = true;
                this.prevButton.interactable = true;
            }

            this.UpdateItemPos(1 - (this.currentTransitionTime / this.transitionTime), CarouselState.Previous);
        }
    }


    //Method called externally to rotate to the next carousel element
    public void Next()
    {
        this.currentIndex++;
        if(this.currentIndex >= this.carouselItems.Count)
        {
            this.currentIndex = 0;
        }

        this.currentTransitionTime = this.transitionTime;
        this.nextButton.interactable = false;
        this.prevButton.interactable = false;
        this.state = CarouselState.Next;
    }


    //Method called externally to rotate to the previous carousel element
    public void Previous()
    {
        this.currentIndex--;
        if(this.currentIndex < 0)
        {
            this.currentIndex = this.carouselItems.Count - 1;
        }

        this.currentTransitionTime = this.transitionTime;
        this.nextButton.interactable = false;
        this.prevButton.interactable = false;
        this.state = CarouselState.Previous;
    }


    //Method called from Start to initialize the starting position array
    private void InitializeItemPositions()
    {
        this.itemPositions = new Vector3[this.carouselItems.Count];
        
        //Finding the different position bounds of the carousel
        RectTransform ourRT = this.GetComponent<RectTransform>();
        Vector3 pos = ourRT.position;
        Vector3 center = pos + new Vector3(0, this.centerOffset, this.depth / 2);
        Vector3 opposite = pos + new Vector3(0, this.centerOffset * 2, this.depth);
        Vector3 leftBound = center - new Vector3(this.width / 2, 0, 0);
        Vector3 rightBound = center + new Vector3(this.width / 2, 0, 0);

        //Looping through each element in the CarouselItems list to show their position in rotation
        for (int i = 0; i < this.carouselItems.Count; i++)
        {
            //Getting the depth
            float depth = (i * 1.0f) / (this.carouselItems.Count * 0.5f);
            if (depth > 1)
            {
                depth -= 1;
                depth = 1 - depth;
            }
            depth = depth * (opposite.z - pos.z) + pos.z;

            //Getting width
            float width = (i * 1.0f) / (this.carouselItems.Count * 1.0f);
            if (width < 0.25f)
            {
                width = (width * 4) * (rightBound.x - pos.x);
                width += pos.x;
            }
            else if (width < 0.75f)
            {
                width = ((width - 0.25f) * 2) * (1 - (rightBound.x - leftBound.x));
                width += rightBound.x;
            }
            else
            {
                width = ((width - 0.75f) * 4) * (pos.x - leftBound.x);
                width += leftBound.x;
            }

            //Getting height
            float height = (i * 1.0f) / (this.carouselItems.Count * 0.5f);
            if (height > 1)
            {
                height -= 1;
                height = 1 - height;
            }
            height = height * (opposite.y - pos.y) + pos.y;

            //Setting the local position of the item based on where it is in the rotation
            Vector3 displayPos = new Vector3(width, height, depth);
            this.itemPositions[i] = displayPos;
        }
    }


    //Method called from Update to shift the positions of all carousel items
    private void UpdateItemPos(float interpPercent_, CarouselState direction_)
    {
        //Looping through each element in the carousel
        for(int i = 0; i < this.carouselItems.Count; i++)
        {
            //Finding the position that this item is moving toward based on our currently selected index
            int currIndex = i + this.currentIndex;
            if(currIndex >= this.carouselItems.Count)
            {
                currIndex -= this.carouselItems.Count;
            }

            //Finding the position that this item is moving away from based on our direction
            int prevIndex = currIndex;
            if(direction_ == CarouselState.Next)
            {
                prevIndex--;
                if(prevIndex < 0)
                {
                    prevIndex = this.carouselItems.Count - 1;
                }
            }
            else if(direction_ == CarouselState.Previous)
            {
                prevIndex++;
                if(prevIndex >= this.carouselItems.Count)
                {
                    prevIndex = 0;
                }
            }

            //Finding the new position based on the interp percent between the two positions
            Vector3 currPos = this.itemPositions[currIndex];
            Vector3 prevPos = this.itemPositions[prevIndex];
            Vector3 newPos = prevPos + ((currPos - prevPos) * interpPercent_);

            this.carouselItems[i].localPosition = newPos;
            
            //Finding the scale based on the depth of the item
            float scalePercent = 1 - (newPos.z / this.depth);
            scalePercent = this.depthScale.Evaluate(scalePercent);
            this.carouselItems[i].localScale = new Vector3(1, 1, 1) * scalePercent;
        }
    }


    //Method called in editor to display the carousel path
    public void OnDrawGizmos()
    {
        RectTransform ourRT = this.GetComponent<RectTransform>();

        Gizmos.color = Color.green;
        Vector3 pos = ourRT.position;
        Vector3 center = pos + new Vector3(0, this.centerOffset, this.depth / 2);
        Vector3 opposite = pos + new Vector3(0, this.centerOffset * 2, this.depth);
        Gizmos.DrawLine(pos, opposite);

        Gizmos.color = Color.red;
        Vector3 leftBound = center - new Vector3(this.width / 2, 0, 0);
        Vector3 rightBound = center + new Vector3(this.width / 2, 0, 0);
        Gizmos.DrawLine(leftBound, rightBound);

        //Looping through each element in the CarouselItems list to show their position in rotation
        for(int i = 0; i < this.carouselItems.Count; i++)
        {
            //Getting the depth
            float depth = (i * 1.0f) / (this.carouselItems.Count * 0.5f);
            if(depth > 1)
            {
                depth -= 1;
                depth = 1 - depth;
            }
            depth = depth * (opposite.z - pos.z) + pos.z;

            //Getting width
            float width = (i * 1.0f) / (this.carouselItems.Count * 1.0f);
            if(width < 0.25f)
            {
                width = (width * 4) * (rightBound.x - pos.x);
                width += pos.x;
            }
            else if(width < 0.75f)
            {
                width = ((width - 0.25f) * 2) * (1 - (rightBound.x - leftBound.x));
                width += rightBound.x;
            }
            else
            {
                width = ((width - 0.75f) * 4) * (pos.x - leftBound.x);
                width += leftBound.x;
            }

            //Getting height
            float height = (i * 1.0f) / (this.carouselItems.Count * 0.5f);
            if(height > 1)
            {
                height -= 1;
                height = 1 - height;
            }
            height = height * (opposite.y - pos.y) + pos.y;

            Vector3 displayPos = new Vector3(width, height, depth);



            //Finding the scale based on the depth of the item
            float scalePercent = 1 - (depth / (opposite.z));
            scalePercent = this.depthScale.Evaluate(scalePercent);

            float percent = (i * 1.0f) / (this.carouselItems.Count * 1.0f);
            Gizmos.color = new Color(percent, percent, percent, 1);
            Gizmos.DrawSphere(displayPos, 10 * scalePercent);
        }
    }
}
