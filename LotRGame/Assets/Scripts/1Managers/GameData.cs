using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }


    public void GenerateMap(GameObject landTileRef_, int[] mapData_, Vector3 startPos_, float offsetX_, float offsetY_)
    {
        Debug.Log("GameData.Generate 1");
        //Transitions to the empty gameplay level before generating anything
        GetComponent<EventManager>().SendGoToLevel("GamePlay", false, 0);

        //Sets the length of the arrays that hold all of the land tiles in the object manager
        GetComponent<ObjManager>().InitializeLandTileList(mapData_[0], mapData_[1]);

        //Temp vars that hold the xyz position that each tile will spawn in
        float xPos = 0;
        float yPos = 0;
        float zPos = 0;

        for (int col = 0; col < mapData_[0]; ++col)
        {
            for (int row = 0; row < mapData_[1]; ++row)
            {
                xPos = startPos_.x + (col * offsetX_);
                yPos = startPos_.y - (row * offsetY_);

                //Offsets the tile's y position on every other column because hexes
                if (col % 2 != 0)
                {
                    yPos = yPos - (offsetY_ / 2);
                }

                //Instantiates each tile at the xyz pos designated
                Vector3 tileLoc = new Vector3(xPos, yPos, zPos);
                GameObject newTile = Instantiate(landTileRef_, tileLoc, Quaternion.identity) as GameObject;

                //Adds each tile to the object manager so that we can reference it later
                newTile.GetComponent<LandTileLogic>().SetLandTileLogic(gameObject, LandTileLogic.LandTypes.Plains);
                GetComponent<ObjManager>().SetTileAtPosition(col, row, newTile);
            }
        }
        

        //Calls Events "GeneratePathPoints" and "ConnectPathPoints"
        GetComponent<EventManager>().SendGeneratePathPoints();
        GetComponent<EventManager>().SendConnectPathPoints();
    }
}
