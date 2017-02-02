using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LandTileLogic : MonoBehaviour
{
    //Reference to the PathPoint prefab
    public Object PathPointRef = null;

    //What type of Land tile this is
    public enum LandTypes { Ocean, Plains, Forrest, Swamp, Desert, Mountain, Volcano};
    public LandTypes LandID = LandTypes.Ocean;

    //Reference to this tile's color
    private SpriteRenderer OwnerSprite = null;

    //Amount of Food that the player needs to consume each turn they're in this tile
    private int FoodConsumedPerTurn = 0;
    //Number of turns that the player needs to spend in order to get across this tile.
    public int TravelTime = 0;
    //List of all path points this tile contains
    private GameObject[] PathPointList;

    //Reference to the game object that contains the ObjManager script
    private GameObject GameManager;

    public bool Hilighted = false;
    private bool Targeted = false;




	// Use this for initialization
	public void SetLandTileLogic(GameObject gameManager_, LandTypes tileID)
    {
		GameManager = gameManager_;

        //Sets this tile's ID, color, and stats
        LandID = tileID;

        //Getting a reference to this tile's sprite
        OwnerSprite = GetComponent<SpriteRenderer>();

        //connects the GeneratePointsEvent function to the GeneratePathPoints event in the Event Manager
        EventManager.GeneratePathPoints += GeneratePointsEvent;
	}


    //Function called when the GeneratePathPoints Event is sent
    private void GeneratePointsEvent()
    {
        EventManager.GeneratePathPoints -= GeneratePointsEvent;
        SetColor();
        SetTravelTime();
        GeneratePathPoints(TravelTime);
    }


    //Sets this owner's sprite color based on the Tile ID
    private void SetColor()
    {
        switch(LandID)
        {
            //Yellow Plains
            case LandTypes.Plains:
                OwnerSprite.color = new Vector4(255, 255, 0, 255);
                break;
            
            //Green Forrest
            case LandTypes.Forrest:
                OwnerSprite.color = new Vector4(0, 187, 0, 255);
                break;

            //Turquoise Swamp
            case LandTypes.Swamp:
                OwnerSprite.color = new Vector4(0, 255, 168, 255);
                break;

            //Tan Desert
            case LandTypes.Desert:
                OwnerSprite.color = new Vector4(255, 214, 89, 255);
                break;

            //Brown Mountain
            case LandTypes.Mountain:
                OwnerSprite.color = new Vector4(158, 95, 0, 255);
                break;

            //Red Volcano
            case LandTypes.Volcano:
                OwnerSprite.color = new Vector4(255, 0, 0, 255);
                break;

            //Blue Ocean
            default:
                OwnerSprite.color = new Vector4(0, 160, 255, 255);
                LandID = LandTypes.Ocean;
                break;
        }
    }


    //Calls the correct "Set Stats" function based on the Tile ID
    private void SetTravelTime()
    {
        switch (LandID)
        {
            //Plains
            case LandTypes.Plains:
                //Plains are flat and easy to travel accross, so the travel time is low.
                int rand1 = Random.Range(1, 3);

                //66% chance to have travel time 2, 33% to have travel time 3
                switch (rand1)
                {
                    case 1:
                        TravelTime = 2;
                        break;
                    case 2:
                        TravelTime = 2;
                        break;
                    case 3:
                        TravelTime = 3;
                        break;
                }
                break;


            //Forrest
            case LandTypes.Forrest:
                //Forrests are relatively simple to navigate, but can sometimes take a bit, so the travel time varies from low to moderate.
                int rand2 = Random.Range(1, 2);

                //50% chance travel time 2, 50% chance travel time 4
                switch (rand2)
                {
                    case 1:
                        TravelTime = 2;
                        break;
                    case 2:
                        TravelTime = 4;
                        break;
                }
                break;


            //Swamp
            case LandTypes.Swamp:
                //Swamps are flat, but confusing and waterlogged, so their travel time is moderate.
                int rand3 = Random.Range(1, 4);

                //25% travel time 2, 50% travel time 4, 25% travel time 6
                switch (rand3)
                {
                    case 1:
                        TravelTime = 2;
                        break;
                    case 2:
                        TravelTime = 4;
                        break;
                    case 3:
                        TravelTime = 4;
                        break;
                    case 4:
                        TravelTime = 6;
                        break;
                }
                break;


            //Desert
            case LandTypes.Desert:
                //Deserts can be either fast to cross, or take forever, so the travel time varies from low to high
                int rand4 = Random.Range(1, 5);

                //20% travel time 2, 20% travel time 4, 20% travel time 6, 20% travel time 8, 20% travel time 10
                switch (rand4)
                {
                    case 1:
                        TravelTime = 2;
                        break;
                    case 2:
                        TravelTime = 4;
                        break;
                    case 3:
                        TravelTime = 6;
                        break;
                    case 4:
                        TravelTime = 8;
                        break;
                    case 5:
                        TravelTime = 10;
                        break;
                }
                break;


            //Mountain
            case LandTypes.Mountain:
                //Mountains are very hard to cross so the travel time is very high
                int rand5 = Random.Range(1, 4);

                //25% travel time 6, 50% travel time 8, 25% travel time 10
                switch (rand5)
                {
                    case 1:
                        TravelTime = 6;
                        break;
                    case 2:
                        TravelTime = 8;
                        break;
                    case 3:
                        TravelTime = 8;
                        break;
                    case 4:
                        TravelTime = 10;
                        break;
                }
                break;


            //Volcano
            case LandTypes.Volcano:
                //Volcanos are mountains with fire hazards. The travel time is the highest
                int rand6 = Random.Range(1, 3);

                //66% travel time 8, 33% travel time 10
                switch (rand6)
                {
                    case 1:
                        TravelTime = 8;
                        break;
                    case 2:
                        TravelTime = 8;
                        break;
                    case 3:
                        TravelTime = 10;
                        break;
                }
                break;


            //Ocean
            default:
                //Ocean tiles are basically uncrossable unless the party has a boat.
                int rand7 = Random.Range(1, 3);

                //66% travel time 8, 33% travel time 10
                switch (rand7)
                {
                    case 1:
                        TravelTime = 8;
                        break;
                    case 2:
                        TravelTime = 8;
                        break;
                    case 3:
                        TravelTime = 10;
                        break;
                }
                break;
        }
    }


    //Creates the tile's PathPoint game objects in the correct positions
    private void GeneratePathPoints(int travelTime_)
    {
        PathPointLogic.LandTypes landType = PathPointLogic.LandTypes.Ocean;
        switch(LandID)
        {
            case LandTypes.Plains:
                landType = PathPointLogic.LandTypes.Plains;
                break;
            case LandTypes.Forrest:
                landType = PathPointLogic.LandTypes.Forrest;
                break;
            case LandTypes.Swamp:
                landType = PathPointLogic.LandTypes.Swamp;
                break;
            case LandTypes.Desert:
                landType = PathPointLogic.LandTypes.Desert;
                break;
            case LandTypes.Mountain:
                landType = PathPointLogic.LandTypes.Mountain;
                break;
            case LandTypes.Volcano:
                landType = PathPointLogic.LandTypes.Volcano;
                break;
            default:
                landType = PathPointLogic.LandTypes.Ocean;
                break;
        }


        switch(travelTime_)
        {
            case 2:
                PathPointList = new GameObject[7];
                SetPathPoints2(landType);
                break;
            case 4:
                PathPointList = new GameObject[13];
                SetPathPoints4(landType);
                break;
            case 6:
                PathPointList = new GameObject[19];
                SetPathPoints6(landType);
                break;
            case 8:
                PathPointList = new GameObject[25];
                SetPathPoints8(landType);
                break;
            case 10:
                PathPointList = new GameObject[31];
                SetPathPoints10(landType);
                break;
            //########################################################################## ERROR
            default:
                Debug.Log("ERROR: LandTileLogic.GeneratePathPoints unexpected int");
                break;
        }

    }


    //Finds and returns the PathPoint game object with the given PointID
    public GameObject FindPathPoint(PathPointLogic.PointIDs id_)
    {
        //Loops through all of this tile's path points and finds one with the given ID
        for (int p = 0; p < PathPointList.Length; ++p)
        {
            if(PathPointList[p].GetComponent<PathPointLogic>().PointID == id_)
            {
                return PathPointList[p];
            }
        }

        //If nothing is found, returns null
        return null;
    }


    public void HilightPoints()
    {
        Hilighted = !Hilighted;
    }

    void OnMouseEnter()
    {
        Hilighted = true;
    }

    void OnMouseDown()
    {
        if(Hilighted)
        {
            Targeted = true;
        }
        else
        {
            Targeted = false;
            Hilighted = false;
        }
    }

    void OnMouseExit()
    {
        if (!Targeted)
        {
            Hilighted = false;
        }
    }


    //Function that clears all stored data in this tile
    public void ClearData()
    {
        //Loops through all path points and clears their data
        for(int p = 0; p < PathPointList.Length; ++p)
        {
            PathPointList[p].GetComponent<PathPointLogic>().ClearData();
            PathPointList[p] = null;
        }

        //makes sure that PathPointList is cleared and null
        PathPointList = null;

        //Nulls the reference to this owner's sprite
        OwnerSprite = null;
    }





    private void SetPathPoints2(PathPointLogic.LandTypes landType)
    {
        PathPointList[0] = Instantiate(PathPointRef) as GameObject;
        PathPointList[0].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.Center,
                            transform.position.x, transform.position.y, transform.position.z + 1);

        PathPointList[1] = Instantiate(PathPointRef) as GameObject;
        PathPointList[1].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.A1,
                            transform.position.x, transform.position.y + 19, transform.position.z + 1);

        PathPointList[2] = Instantiate(PathPointRef) as GameObject;
        PathPointList[2].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.B1,
                            transform.position.x + 16, transform.position.y + 9, transform.position.z + 1);

        PathPointList[3] = Instantiate(PathPointRef) as GameObject;
        PathPointList[3].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.C1,
                            transform.position.x + 16, transform.position.y - 9, transform.position.z + 1);

        PathPointList[4] = Instantiate(PathPointRef) as GameObject;
        PathPointList[4].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.D1,
                            transform.position.x, transform.position.y - 19, transform.position.z + 1);

        PathPointList[5] = Instantiate(PathPointRef) as GameObject;
        PathPointList[5].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.E1,
                            transform.position.x - 16, transform.position.y - 9, transform.position.z + 1);

        PathPointList[6] = Instantiate(PathPointRef) as GameObject;
        PathPointList[6].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.F1,
                            transform.position.x - 16, transform.position.y + 9, transform.position.z + 1);
    }

    private void SetPathPoints4(PathPointLogic.LandTypes landType)
    {
        PathPointList[0] = Instantiate(PathPointRef) as GameObject;
        PathPointList[0].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.Center,
                            transform.position.x, transform.position.y, transform.position.z + 1);

        PathPointList[1] = Instantiate(PathPointRef) as GameObject;
        PathPointList[1].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.A1,
                            transform.position.x, transform.position.y + 23, transform.position.z + 1);

        PathPointList[2] = Instantiate(PathPointRef) as GameObject;
        PathPointList[2].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.B1,
                            transform.position.x + 20, transform.position.y + 12, transform.position.z + 1);

        PathPointList[3] = Instantiate(PathPointRef) as GameObject;
        PathPointList[3].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.C1,
                            transform.position.x + 20, transform.position.y - 12, transform.position.z + 1);

        PathPointList[4] = Instantiate(PathPointRef) as GameObject;
        PathPointList[4].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.D1,
                            transform.position.x, transform.position.y - 23, transform.position.z + 1);

        PathPointList[5] = Instantiate(PathPointRef) as GameObject;
        PathPointList[5].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.E1,
                            transform.position.x - 20, transform.position.y - 12, transform.position.z + 1);

        PathPointList[6] = Instantiate(PathPointRef) as GameObject;
        PathPointList[6].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.F1,
                            transform.position.x - 20, transform.position.y + 12, transform.position.z + 1);
//#################################################################################################################################################
        PathPointList[7] = Instantiate(PathPointRef) as GameObject;
        PathPointList[7].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.A2,
                            transform.position.x, transform.position.y + 11, transform.position.z + 1);

        PathPointList[8] = Instantiate(PathPointRef) as GameObject;
        PathPointList[8].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.B2,
                            transform.position.x + 9, transform.position.y + 6, transform.position.z + 1);

        PathPointList[9] = Instantiate(PathPointRef) as GameObject;
        PathPointList[9].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.C2,
                            transform.position.x + 9, transform.position.y - 6, transform.position.z + 1);

        PathPointList[10] = Instantiate(PathPointRef) as GameObject;
        PathPointList[10].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.D2,
                            transform.position.x, transform.position.y - 11, transform.position.z + 1);

        PathPointList[11] = Instantiate(PathPointRef) as GameObject;
        PathPointList[11].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.E2,
                            transform.position.x - 9, transform.position.y - 6, transform.position.z + 1);

        PathPointList[12] = Instantiate(PathPointRef) as GameObject;
        PathPointList[12].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.F2,
                            transform.position.x - 9, transform.position.y + 6, transform.position.z + 1);
    }

    private void SetPathPoints6(PathPointLogic.LandTypes landType)
    {
        PathPointList[0] = Instantiate(PathPointRef) as GameObject;
        PathPointList[0].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.Center,
                            transform.position.x, transform.position.y, transform.position.z + 1);

        PathPointList[1] = Instantiate(PathPointRef) as GameObject;
        PathPointList[1].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.A1,
                            transform.position.x, transform.position.y + 24, transform.position.z + 1);

        PathPointList[2] = Instantiate(PathPointRef) as GameObject;
        PathPointList[2].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.B1,
                            transform.position.x + 21, transform.position.y + 12, transform.position.z + 1);

        PathPointList[3] = Instantiate(PathPointRef) as GameObject;
        PathPointList[3].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.C1,
                            transform.position.x + 21, transform.position.y - 12, transform.position.z + 1);

        PathPointList[4] = Instantiate(PathPointRef) as GameObject;
        PathPointList[4].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.D1,
                            transform.position.x, transform.position.y - 24, transform.position.z + 1);

        PathPointList[5] = Instantiate(PathPointRef) as GameObject;
        PathPointList[5].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.E1,
                            transform.position.x - 21, transform.position.y - 12, transform.position.z + 1);

        PathPointList[6] = Instantiate(PathPointRef) as GameObject;
        PathPointList[6].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.F1,
                            transform.position.x - 21, transform.position.y + 12, transform.position.z + 1);
        //#################################################################################################################################################
        PathPointList[7] = Instantiate(PathPointRef) as GameObject;
        PathPointList[7].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.A2,
                            transform.position.x, transform.position.y + 16, transform.position.z + 1);

        PathPointList[8] = Instantiate(PathPointRef) as GameObject;
        PathPointList[8].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.B2,
                            transform.position.x + 14, transform.position.y + 8, transform.position.z + 1);

        PathPointList[9] = Instantiate(PathPointRef) as GameObject;
        PathPointList[9].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.C2,
                            transform.position.x + 14, transform.position.y - 8, transform.position.z + 1);

        PathPointList[10] = Instantiate(PathPointRef) as GameObject;
        PathPointList[10].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.D2,
                            transform.position.x, transform.position.y - 16, transform.position.z + 1);

        PathPointList[11] = Instantiate(PathPointRef) as GameObject;
        PathPointList[11].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.E2,
                            transform.position.x - 14, transform.position.y - 8, transform.position.z + 1);

        PathPointList[12] = Instantiate(PathPointRef) as GameObject;
        PathPointList[12].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.F2,
                            transform.position.x - 14, transform.position.y + 8, transform.position.z + 1);
        //#################################################################################################################################################
        PathPointList[13] = Instantiate(PathPointRef) as GameObject;
        PathPointList[13].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.A3,
                            transform.position.x, transform.position.y + 8, transform.position.z + 1);

        PathPointList[14] = Instantiate(PathPointRef) as GameObject;
        PathPointList[14].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.B3,
                            transform.position.x + 7, transform.position.y + 4, transform.position.z + 1);

        PathPointList[15] = Instantiate(PathPointRef) as GameObject;
        PathPointList[15].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.C3,
                            transform.position.x + 7, transform.position.y - 4, transform.position.z + 1);

        PathPointList[16] = Instantiate(PathPointRef) as GameObject;
        PathPointList[16].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.D3,
                            transform.position.x, transform.position.y - 8, transform.position.z + 1);

        PathPointList[17] = Instantiate(PathPointRef) as GameObject;
        PathPointList[17].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.E3,
                            transform.position.x - 7, transform.position.y - 4, transform.position.z + 1);

        PathPointList[18] = Instantiate(PathPointRef) as GameObject;
        PathPointList[18].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.F3,
                            transform.position.x - 7, transform.position.y + 4, transform.position.z + 1);
    }

    private void SetPathPoints8(PathPointLogic.LandTypes landType)
    {
        PathPointList[0] = Instantiate(PathPointRef) as GameObject;
        PathPointList[0].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.Center,
                            transform.position.x, transform.position.y, transform.position.z + 1);
        
        PathPointList[1] = Instantiate(PathPointRef) as GameObject;
        PathPointList[1].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.A1,
                            transform.position.x, transform.position.y + 24, transform.position.z + 1);

        PathPointList[2] = Instantiate(PathPointRef) as GameObject;
        PathPointList[2].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.B1,
                            transform.position.x + 21, transform.position.y + 12, transform.position.z + 1);

        PathPointList[3] = Instantiate(PathPointRef) as GameObject;
        PathPointList[3].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.C1,
                            transform.position.x + 21, transform.position.y - 12, transform.position.z + 1);

        PathPointList[4] = Instantiate(PathPointRef) as GameObject;
        PathPointList[4].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.D1,
                            transform.position.x, transform.position.y - 24, transform.position.z + 1);

        PathPointList[5] = Instantiate(PathPointRef) as GameObject;
        PathPointList[5].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.E1,
                            transform.position.x - 21, transform.position.y - 12, transform.position.z + 1);

        PathPointList[6] = Instantiate(PathPointRef) as GameObject;
        PathPointList[6].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.F1,
                            transform.position.x - 21, transform.position.y + 12, transform.position.z + 1);
        //#################################################################################################################################################
        PathPointList[7] = Instantiate(PathPointRef) as GameObject;
        PathPointList[7].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.A2,
                            transform.position.x, transform.position.y + 18, transform.position.z + 1);

        PathPointList[8] = Instantiate(PathPointRef) as GameObject;
        PathPointList[8].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.B2,
                            transform.position.x + 16, transform.position.y + 9, transform.position.z + 1);

        PathPointList[9] = Instantiate(PathPointRef) as GameObject;
        PathPointList[9].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.C2,
                            transform.position.x + 16, transform.position.y - 9, transform.position.z + 1);

        PathPointList[10] = Instantiate(PathPointRef) as GameObject;
        PathPointList[10].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.D2,
                            transform.position.x, transform.position.y - 18, transform.position.z + 1);

        PathPointList[11] = Instantiate(PathPointRef) as GameObject;
        PathPointList[11].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.E2,
                            transform.position.x - 16, transform.position.y - 9, transform.position.z + 1);

        PathPointList[12] = Instantiate(PathPointRef) as GameObject;
        PathPointList[12].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.F2,
                            transform.position.x - 16, transform.position.y + 9, transform.position.z + 1);
        //#################################################################################################################################################
        PathPointList[13] = Instantiate(PathPointRef) as GameObject;
        PathPointList[13].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.A3,
                            transform.position.x, transform.position.y + 12, transform.position.z + 1);

        PathPointList[14] = Instantiate(PathPointRef) as GameObject;
        PathPointList[14].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.B3,
                            transform.position.x + 10, transform.position.y + 6, transform.position.z + 1);

        PathPointList[15] = Instantiate(PathPointRef) as GameObject;
        PathPointList[15].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.C3,
                            transform.position.x + 10, transform.position.y - 6, transform.position.z + 1);

        PathPointList[16] = Instantiate(PathPointRef) as GameObject;
        PathPointList[16].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.D3,
                            transform.position.x, transform.position.y - 12, transform.position.z + 1);

        PathPointList[17] = Instantiate(PathPointRef) as GameObject;
        PathPointList[17].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.E3,
                            transform.position.x - 10, transform.position.y - 6, transform.position.z + 1);

        PathPointList[18] = Instantiate(PathPointRef) as GameObject;
        PathPointList[18].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.F3,
                            transform.position.x - 10, transform.position.y + 6, transform.position.z + 1);
        //#################################################################################################################################################
        PathPointList[19] = Instantiate(PathPointRef) as GameObject;
        PathPointList[19].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.A4,
                            transform.position.x, transform.position.y + 6, transform.position.z + 1);

        PathPointList[20] = Instantiate(PathPointRef) as GameObject;
        PathPointList[20].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.B4,
                            transform.position.x + 5, transform.position.y + 3, transform.position.z + 1);

        PathPointList[21] = Instantiate(PathPointRef) as GameObject;
        PathPointList[21].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.C4,
                            transform.position.x + 5, transform.position.y - 3, transform.position.z + 1);

        PathPointList[22] = Instantiate(PathPointRef) as GameObject;
        PathPointList[22].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.D4,
                            transform.position.x, transform.position.y - 6, transform.position.z + 1);

        PathPointList[23] = Instantiate(PathPointRef) as GameObject;
        PathPointList[23].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.E4,
                            transform.position.x - 5, transform.position.y - 3, transform.position.z + 1);

        PathPointList[24] = Instantiate(PathPointRef) as GameObject;
        PathPointList[24].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.F4,
                            transform.position.x - 5, transform.position.y + 3, transform.position.z + 1);
    }

    private void SetPathPoints10(PathPointLogic.LandTypes landType)
    {
        PathPointList[0] = Instantiate(PathPointRef) as GameObject;
        PathPointList[0].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.Center,
                            transform.position.x, transform.position.y, transform.position.z + 1);

        PathPointList[1] = Instantiate(PathPointRef) as GameObject;
        PathPointList[1].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.A1,
                            transform.position.x, transform.position.y + 25, transform.position.z + 1);

        PathPointList[2] = Instantiate(PathPointRef) as GameObject;
        PathPointList[2].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.B1,
                            transform.position.x + 22, transform.position.y + 13, transform.position.z + 1);

        PathPointList[3] = Instantiate(PathPointRef) as GameObject;
        PathPointList[3].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.C1,
                            transform.position.x + 22, transform.position.y - 13, transform.position.z + 1);

        PathPointList[4] = Instantiate(PathPointRef) as GameObject;
        PathPointList[4].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.D1,
                            transform.position.x, transform.position.y - 25, transform.position.z + 1);

        PathPointList[5] = Instantiate(PathPointRef) as GameObject;
        PathPointList[5].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.E1,
                            transform.position.x - 22, transform.position.y - 13, transform.position.z + 1);

        PathPointList[6] = Instantiate(PathPointRef) as GameObject;
        PathPointList[6].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.F1,
                            transform.position.x - 22, transform.position.y + 13, transform.position.z + 1);
        //#################################################################################################################################################
        PathPointList[7] = Instantiate(PathPointRef) as GameObject;
        PathPointList[7].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.A2,
                            transform.position.x, transform.position.y + 20, transform.position.z + 1);

        PathPointList[8] = Instantiate(PathPointRef) as GameObject;
        PathPointList[8].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.B2,
                            transform.position.x + 17, transform.position.y + 10, transform.position.z + 1);

        PathPointList[9] = Instantiate(PathPointRef) as GameObject;
        PathPointList[9].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.C2,
                            transform.position.x + 17, transform.position.y - 10, transform.position.z + 1);

        PathPointList[10] = Instantiate(PathPointRef) as GameObject;
        PathPointList[10].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.D2,
                            transform.position.x, transform.position.y - 20, transform.position.z + 1);

        PathPointList[11] = Instantiate(PathPointRef) as GameObject;
        PathPointList[11].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.E2,
                            transform.position.x - 17, transform.position.y - 10, transform.position.z + 1);

        PathPointList[12] = Instantiate(PathPointRef) as GameObject;
        PathPointList[12].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.F2,
                            transform.position.x - 17, transform.position.y + 10, transform.position.z + 1);
        //#################################################################################################################################################
        PathPointList[13] = Instantiate(PathPointRef) as GameObject;
        PathPointList[13].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.A3,
                            transform.position.x, transform.position.y + 15, transform.position.z + 1);

        PathPointList[14] = Instantiate(PathPointRef) as GameObject;
        PathPointList[14].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.B3,
                            transform.position.x + 13, transform.position.y + 8, transform.position.z + 1);

        PathPointList[15] = Instantiate(PathPointRef) as GameObject;
        PathPointList[15].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.C3,
                            transform.position.x + 13, transform.position.y - 8, transform.position.z + 1);

        PathPointList[16] = Instantiate(PathPointRef) as GameObject;
        PathPointList[16].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.D3,
                            transform.position.x, transform.position.y - 15, transform.position.z + 1);

        PathPointList[17] = Instantiate(PathPointRef) as GameObject;
        PathPointList[17].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.E3,
                            transform.position.x - 13, transform.position.y - 8, transform.position.z + 1);

        PathPointList[18] = Instantiate(PathPointRef) as GameObject;
        PathPointList[18].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.F3,
                            transform.position.x - 13, transform.position.y + 8, transform.position.z + 1);
        //#################################################################################################################################################
        PathPointList[19] = Instantiate(PathPointRef) as GameObject;
        PathPointList[19].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.A4,
                            transform.position.x, transform.position.y + 10, transform.position.z + 1);

        PathPointList[20] = Instantiate(PathPointRef) as GameObject;
        PathPointList[20].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.B4,
                            transform.position.x + 9, transform.position.y + 5, transform.position.z + 1);

        PathPointList[21] = Instantiate(PathPointRef) as GameObject;
        PathPointList[21].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.C4,
                            transform.position.x + 9, transform.position.y - 5, transform.position.z + 1);

        PathPointList[22] = Instantiate(PathPointRef) as GameObject;
        PathPointList[22].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.D4,
                            transform.position.x, transform.position.y - 10, transform.position.z + 1);

        PathPointList[23] = Instantiate(PathPointRef) as GameObject;
        PathPointList[23].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.E4,
                            transform.position.x - 9, transform.position.y - 5, transform.position.z + 1);

        PathPointList[24] = Instantiate(PathPointRef) as GameObject;
        PathPointList[24].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.F4,
                            transform.position.x - 9, transform.position.y + 5, transform.position.z + 1);
        //#################################################################################################################################################
        PathPointList[25] = Instantiate(PathPointRef) as GameObject;
        PathPointList[25].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.A4,
                            transform.position.x, transform.position.y + 5, transform.position.z + 1);

        PathPointList[26] = Instantiate(PathPointRef) as GameObject;
        PathPointList[26].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.B4,
                            transform.position.x + 4, transform.position.y + 2, transform.position.z + 1);

        PathPointList[27] = Instantiate(PathPointRef) as GameObject;
        PathPointList[27].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.C4,
                            transform.position.x + 4, transform.position.y - 2, transform.position.z + 1);

        PathPointList[28] = Instantiate(PathPointRef) as GameObject;
        PathPointList[28].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.D4,
                            transform.position.x, transform.position.y - 5, transform.position.z + 1);

        PathPointList[29] = Instantiate(PathPointRef) as GameObject;
        PathPointList[29].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.E4,
                            transform.position.x - 4, transform.position.y - 2, transform.position.z + 1);

        PathPointList[30] = Instantiate(PathPointRef) as GameObject;
        PathPointList[30].GetComponent<PathPointLogic>().SetPathPointLogic(this.gameObject, GameManager, landType, PathPointLogic.PointIDs.F4,
                            transform.position.x - 4, transform.position.y + 2, transform.position.z + 1);
    }
}
