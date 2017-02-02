using UnityEngine;
using System.Collections;

public class TESTGenerateMap : MonoBehaviour
{
    public GameObject LandTileRef = null;
    public Vector3 StartPos = new Vector3(0,0,0);
    public float XOffset = 46;
    public float YOffset = 52;
    private GameObject GameManager;


	// Use this for initialization
	void Start ()
    {
        GameManager = GameObject.Find("GameManager");
	}
	

    //Funciton called in the CreateMapGenerate.cs script
	public void Generate(int[] mapInfo_)
    {
        if(LandTileRef == null)
        {
            Debug.LogError("ERROR: TESTGenerateMap.Generate, LandTileRef is null");
            return;
        }
        GameManager.GetComponent<GameData>().GenerateMap(LandTileRef, mapInfo_, StartPos, XOffset, YOffset);
        /*GameManager.GetComponent<ObjManager>().InitializeLandTileList(mapInfo_[0], mapInfo_[1]);

        float xPos = 0;
        float yPos = 0;
        float zPos = 0;
        
        for(int col = 0; col < mapInfo_[0]; ++col)
        {
            for(int row = 0; row < mapInfo_[1]; ++row)
            {
                xPos = StartPos.x + (col * XOffset);
                yPos = StartPos.y - (row * YOffset);

                if (col % 2 != 0)
                {
                    yPos = yPos - (YOffset / 2);
                }


                Vector3 tileLoc = new Vector3(xPos, yPos, zPos);
                GameObject newTile = Instantiate(LandTileRef, tileLoc, Quaternion.identity) as GameObject;

                newTile.GetComponent<LandTileLogic>().SetLandTileLogic(GameManager, LandTileLogic.LandTypes.Plains);
                GameManager.GetComponent<ObjManager>().SetTileAtPosition(col, row, newTile);
            }
        }

        //this.GetComponentInParent<Canvas>().enabled = false;

        //Calls Events "GeneratePathPoints" and "ConnectPathPoints"
        GameManager.GetComponent<EventManager>().SendGeneratePathPoints();
        GameManager.GetComponent<EventManager>().SendConnectPathPoints();*/
    }
}
