using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationIDList : MonoBehaviour
{
    //The list of locations with the IDTag component so we can keep track of their IDs
    public List<IDTag> cityLocationsList;       //ID 7000-7099
    public List<IDTag> dungeonLocationsList;    //7100-7199
    public List<IDTag> miscLocationsList;       //7200-7299 



	//Function called externally to return a location object reference when given the ID number
    public GameObject GetLocationByIDNum(int numberID_)
    {
        //Looping through each city
        for(int c = 0; c < this.cityLocationsList.Count; ++c)
        {
            //If the current location has the same ID number, we return the object reference
            if(this.cityLocationsList[c].numberID == numberID_)
            {
                return this.cityLocationsList[c].gameObject;
            }
        }

        //Looping through each dungeon
        for (int d = 0; d < this.dungeonLocationsList.Count; ++d)
        {
            //If the current location has the same ID number, we return the object reference
            if (this.dungeonLocationsList[d].numberID == numberID_)
            {
                return this.dungeonLocationsList[d].gameObject;
            }
        }

        //Looping through each misc location
        for (int m = 0; m < this.miscLocationsList.Count; ++m)
        {
            //If the current location has the same ID number, we return the object reference
            if (this.miscLocationsList[m].numberID == numberID_)
            {
                return this.miscLocationsList[m].gameObject;
            }
        }

        //If we make it through the loop then we don't have the location and we return null
        return null;
    }


    //Function called for debugging purposes to make sure there are no problems with any of the location IDs in the list
    public void CheckForInvalidIDs()
    {
        this.CheckList(this.cityLocationsList, "Cities");
        this.CheckList(this.dungeonLocationsList, "Dungeons");
        this.CheckList(this.miscLocationsList, "Misc Locations");
    }


    //Function called from CheckForInvalidIDs to loop through the given weapon list
    private void CheckList(List<IDTag> listToCheck_, string nameOfList_)
    {
        //Looping through all of the locations in our list
        for(int l = 0; l < listToCheck_.Count; ++l)
        {
            //If this slot in the list is empty, we throw a debug
            if(listToCheck_[l] == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: LocationIDList.CheckList " + nameOfList_ + ": Empty slot in location list at index " + l);
            }
            //If this ID has the wrong enum tag, we throw a debug
            else if(listToCheck_[l].objType != IDTag.ObjectType.Location)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: LocationIDList.CheckList " + nameOfList_ + ": Invalid ID type at index " + l);
            }
            //If this object doesn't have the location component, we throw a debug
            else if(listToCheck_[l].GetComponent<MapLocation>() == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: LocationIDList.CheckList " + nameOfList_ + ": Object at index " + l + " doesn't have the MapLocation component");
            }
        }

        //Looping through the list again with nexted for loops to check each ID against all other ID numbers
        for(int x = 0; x < listToCheck_.Count - 1; ++x)
        {
            for(int y = x + 1; y < listToCheck_.Count; ++y)
            {
                //If the ID numbers are the same we need to throw a debug
                if(listToCheck_[x].numberID == listToCheck_[y].numberID)
                {
                    Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: LocationIDList.CheckList " + nameOfList_ + ": Duplicate ID numbers on index " + x + " and " + y);
                }
            }
        }
    }
}
