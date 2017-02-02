using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class PathPointLogic : MonoBehaviour
{
    //Reference to this point's parent tile
    private GameObject ParentTile;

    //Reference to the game object with the ObjManager script
    private GameObject GameManager;

    //Reference to this Point's sprite
    private SpriteRenderer OwnerSprite;

    //What type of Land tile this path point is on
    public enum LandTypes { Ocean, Plains, Forrest, Swamp, Desert, Mountain, Volcano };
    public LandTypes LandID = LandTypes.Ocean;

    //What path point ID this point is on the owner's tile
    public enum PointIDs { Center, A1, B1, C1, D1, E1, F1,
                                   A2, A3, A4, A5,
                                   B2, B3, B4, B5,
                                   C2, C3, C4, C5,
                                   D2, D3, D4, D5,
                                   E2, E3, E4, E5,
                                   F2, F3, F4, F5}
    public PointIDs PointID = PointIDs.Center;

    //Amount of food that can be gathered on this path point
    public int FoodToGather = 0;

    //The type of water that can be gathered on this point
    public enum WaterTypes { None, FreshWater, SaltWater, SwampWater, FoulWater };
    public WaterTypes Water = WaterTypes.None;

    //List that holds all connected path points
    public GameObject[] ConnectedPoints;

    //Hilights all connected points if active
    private bool IsActive;
    private bool IsHilighted;



    // Use this for initialization
    public void SetPathPointLogic(GameObject parentTile_, GameObject gameManager_, LandTypes tileID_, PointIDs pointID_, float xPos_, float yPos_, float zPos_)
    {
        ParentTile = parentTile_;
        GameManager = gameManager_;
        OwnerSprite = this.GetComponent<SpriteRenderer>();
        LandID = tileID_;
        PointID = pointID_;
        transform.position = new Vector3(xPos_, yPos_, zPos_);
        IsActive = false;
        IsHilighted = false;

        //Sets the length of the ConnectedPoints list
        if(PointID == PointIDs.Center)
        {
            ConnectedPoints = new GameObject[6];
        }
        else
        {
            ConnectedPoints = new GameObject[4];
        }


        //Connects this path point to the TimePass and ConnectPathPoints events
        EventManager.TimePass += this.TimePass;
        EventManager.ConnectPathPoints += this.FindConnectedPoints;
	}


    void Update()
    {
        if(ParentTile.GetComponent<LandTileLogic>().Hilighted || IsActive || IsHilighted)
        {
            OwnerSprite.enabled = true;
        }
        else
        {
            OwnerSprite.enabled = false;
        }
    }

    
    //Function called when the "TimePass" Event is sent out.
    public void TimePass()
    {
        
    }


    //Hilights all connected points when clicked
    public void Activate()
    {
        IsActive = !IsActive;

        if(!IsActive)
        {
            for(int p = 0; p < ConnectedPoints.GetLength(0); ++p)
            {
                if(ConnectedPoints[p] != null)
                {
                    ConnectedPoints[p].GetComponent<PathPointLogic>().Hilight(IsActive);
                }
            }
        }
    }

    //Changes this path point's color when hilighted
    public void Hilight(bool isHilighted_)
    {
        IsHilighted = isHilighted_;

        SpriteRenderer ownerSprite = this.GetComponent<SpriteRenderer>();

        if(IsHilighted)
        {
            ownerSprite.color = new Color(255, 255, 255, 255);
        }
        else
        {
            ownerSprite.color = new Color(255, 0, 0, 255);
        }
    }


    //Finds all path points that are connected to this one
    private void FindConnectedPoints()
	{
        //Debug.Log("PathPointLogic.FindConnected 1: " + PointID);
		//Disconnects this path point from the event, because it should only be called once
		EventManager.ConnectPathPoints -= FindConnectedPoints;
        
        switch (PointID)
        {
            //######################################################## Center
            case PointIDs.Center:
                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 2:
                        ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A1);
                        ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B1);
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C1);
                        ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D1);
                        ConnectedPoints[4] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E1);
                        ConnectedPoints[5] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F1);
                        break;
                    case 4:
                        ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A2);
                        ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B2);
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C2);
                        ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D2);
                        ConnectedPoints[4] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E2);
                        ConnectedPoints[5] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F2);
                        break;
                    case 6:
                        ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A3);
                        ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B3);
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C3);
                        ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D3);
                        ConnectedPoints[4] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E3);
                        ConnectedPoints[5] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F3);
                        break;
                    case 8:
                        ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A4);
                        ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B4);
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C4);
                        ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D4);
                        ConnectedPoints[4] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E4);
                        ConnectedPoints[5] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F4);
                        break;
                    case 10:
                        ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A5);
                        ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B5);
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C5);
                        ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D5);
                        ConnectedPoints[4] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E5);
                        ConnectedPoints[5] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F5);
                        break;
                }
                break;
            //######################################################## A1
            case PointIDs.A1:
                int[] parentPosA = GameManager.GetComponent<ObjManager>().FindTilePosition(ParentTile);

                if(parentPosA[1] != 0 || parentPosA[1] != 1)
                {
                    GameObject tileAbove = GameManager.GetComponent<ObjManager>().FindTileAtPosition(parentPosA[0], parentPosA[1] - 1);
                    
                    if (tileAbove != null)
                    {
                        ConnectedPoints[0] = tileAbove.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D1);
                    }
                }
                

                switch(ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 2:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center);//Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A2);//Down
                        break;
                }

                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B1);//Right
                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F1);//Left
                break;
            //######################################################## B1
            case PointIDs.B1:
                int[] parentPosB = GameManager.GetComponent<ObjManager>().FindTilePosition(ParentTile);

                //Checking the offset of the tiles next to the parent, since it shifts
                if(parentPosB[0] % 2 == 0)
                {
                    GameObject tileUpRight = GameManager.GetComponent<ObjManager>().FindTileAtPosition(parentPosB[0] + 1, parentPosB[1] - 1);
                    if(tileUpRight != null)
                    {
                        ConnectedPoints[0] = tileUpRight.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E1);
                    }
                    else
                    {
                        ConnectedPoints[0] = null;
                    }
                }
                else
                {
                    GameObject tileUpRight = GameManager.GetComponent<ObjManager>().FindTileAtPosition(parentPosB[0] + 1, parentPosB[1]);
                    if (tileUpRight != null)
                    {
                        ConnectedPoints[0] = tileUpRight.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E1);
                    }
                    else
                    {
                        ConnectedPoints[0] = null;
                    }
                }


                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 2:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center);//Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B2);//Down
                        break;
                }

                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C1);//Right
                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A1);//Left
                break;
            //######################################################## C1
            case PointIDs.C1:
                int[] parentPosC = GameManager.GetComponent<ObjManager>().FindTilePosition(ParentTile);

                //Checking the offset of the tiles next to the parent, since it shifts
                if (parentPosC[0] % 2 == 0)
                {
                    GameObject tileUpRight = GameManager.GetComponent<ObjManager>().FindTileAtPosition(parentPosC[0] + 1, parentPosC[1]);
                    if (tileUpRight != null)
                    {
                        ConnectedPoints[0] = tileUpRight.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F1);
                    }
                    else
                    {
                        ConnectedPoints[0] = null;
                    }
                }
                else
                {
                    GameObject tileUpRight = GameManager.GetComponent<ObjManager>().FindTileAtPosition(parentPosC[0] + 1, parentPosC[1] + 1);
                    if (tileUpRight != null)
                    {
                        ConnectedPoints[0] = tileUpRight.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F1);
                    }
                    else
                    {
                        ConnectedPoints[0] = null;
                    }
                }


                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 2:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center);//Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C2);//Down
                        break;
                }

                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D1);//Right
                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B1);//Left
                break;
            //######################################################## D1
            case PointIDs.D1:
                int[] parentPosD = GameManager.GetComponent<ObjManager>().FindTilePosition(ParentTile);

                if (GameManager.GetComponent<ObjManager>().FindTileAtPosition(parentPosD[0], parentPosD[1]) != null)
                {
                    GameObject tileAbove = GameManager.GetComponent<ObjManager>().FindTileAtPosition(parentPosD[0], parentPosD[1] + 1);
                    tileAbove.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A1);
                }
                else
                {
                    ConnectedPoints[0] = null;
                }

                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E1);//Right

                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 2:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center);//Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D2);//Down
                        break;
                }

                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C1);//Left
                break;
            //######################################################## E1
            case PointIDs.E1:
                int[] parentPosE = GameManager.GetComponent<ObjManager>().FindTilePosition(ParentTile);

                //Checking the offset of the tiles next to the parent, since it shifts
                if (parentPosE[0] % 2 == 0)
                {
                    GameObject tileUpRight = GameManager.GetComponent<ObjManager>().FindTileAtPosition(parentPosE[0] - 1, parentPosE[1]);
                    if (tileUpRight != null)
                    {
                        ConnectedPoints[0] = tileUpRight.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B1);
                    }
                    else
                    {
                        ConnectedPoints[0] = null;
                    }
                }
                else
                {
                    GameObject tileUpRight = GameManager.GetComponent<ObjManager>().FindTileAtPosition(parentPosE[0] - 1, parentPosE[1] + 1);
                    if (tileUpRight != null)
                    {
                        ConnectedPoints[0] = tileUpRight.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B1);
                    }
                    else
                    {
                        ConnectedPoints[0] = null;
                    }
                }


                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 2:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center);//Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E2);//Down
                        break;
                }

                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F1);//Right
                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D1);//Left
                break;
            //######################################################## F1
            case PointIDs.F1:
                int[] parentPosF = GameManager.GetComponent<ObjManager>().FindTilePosition(ParentTile);

                //Checking the offset of the tiles next to the parent, since it shifts
                if (parentPosF[0] % 2 == 0)
                {
                    GameObject tileUpRight = GameManager.GetComponent<ObjManager>().FindTileAtPosition(parentPosF[0] - 1, parentPosF[1] - 1);

                    if (tileUpRight != null)
                    {
                        ConnectedPoints[0] = tileUpRight.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C1);
                    }
                    else
                    {
                        ConnectedPoints[0] = null;
                    }
                }
                else
                {
                    GameObject tileUpRight = GameManager.GetComponent<ObjManager>().FindTileAtPosition(parentPosF[0] - 1, parentPosF[1]);
                    if (tileUpRight != null)
                    {
                        ConnectedPoints[0] = tileUpRight.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C1);
                    }
                    else
                    {
                        ConnectedPoints[0] = null;
                    }
                }


                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 2:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center);//Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F2);//Down
                        break;
                }

                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A1);//Right
                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E1);//Left
                break;
            //######################################################## A2 ##
            case PointIDs.A2:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A1); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B2); //Right

                switch(ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 4:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A3); //Down
                        break;
                }

                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F2); //Left
                break;
            //######################################################## A3
            case PointIDs.A3:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A2); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B3); //Right

                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 6:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A4); //Down
                        break;
                }

                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F3); //Left
                break;
            //######################################################## A4
            case PointIDs.A4:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A3); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B4); //Right

                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 8:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A5); //Down
                        break;
                }

                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F4); //Left
                break;
            //######################################################## A5
            case PointIDs.A5:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A4); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B5); //Right
                ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F5); //Left
                break;
            //######################################################## B2 ##
            case PointIDs.B2:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B1); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C2); //Right

                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 4:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B3); //Down
                        break;
                }

                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A2); //Left
                break;
            //######################################################## B3
            case PointIDs.B3:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B2); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C3); //Right

                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 6:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B4); //Down
                        break;
                }

                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A3); //Left
                break;
            //######################################################## B4
            case PointIDs.B4:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B3); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C4); //Right

                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 8:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B5); //Down
                        break;
                }

                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A4); //Left
                break;
            //######################################################## B5
            case PointIDs.B5:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B4); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C5); //Right
                ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A5); //Left
                break;
            //######################################################## C2 ##
            case PointIDs.C2:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C1); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D2); //Right

                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 4:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C3); //Down
                        break;
                }

                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B2); //Left
                break;
            //######################################################## C3
            case PointIDs.C3:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C2); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D3); //Right

                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 6:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C4); //Down
                        break;
                }

                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B3); //Left
                break;
            //######################################################## C4
            case PointIDs.C4:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C3); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D4); //Right

                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 8:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C5); //Down
                        break;
                }

                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B4); //Left
                break;
            //######################################################## C5
            case PointIDs.C5:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C4); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D5); //Right
                ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.B5); //Left
                break;
            //######################################################## D2 ##
            case PointIDs.D2:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D1); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E2); //Right

                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 4:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D3); //Down
                        break;
                }

                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C2); //Left
                break;
            //######################################################## D3
            case PointIDs.D3:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D2); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E3); //Right

                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 6:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D4); //Down
                        break;
                }

                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C3); //Left
                break;
            //######################################################## D4
            case PointIDs.D4:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D3); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E4); //Right

                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 8:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D5); //Down
                        break;
                }

                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C4); //Left
                break;
            //######################################################## D5
            case PointIDs.D5:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D4); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E5); //Right
                ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.C5); //Left
                break;
            //######################################################## E2 ##
            case PointIDs.E2:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E1); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F2); //Right

                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 4:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E3); //Down
                        break;
                }

                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D2); //Left
                break;
            //######################################################## E3
            case PointIDs.E3:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E2); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F3); //Right

                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 6:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E4); //Down
                        break;
                }

                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D3); //Left
                break;
            //######################################################## E4
            case PointIDs.E4:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E3); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F4); //Right

                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 8:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E5); //Down
                        break;
                }

                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D4); //Left
                break;
            //######################################################## E5
            case PointIDs.E5:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E4); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F5); //Right
                ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.D5); //Left
                break;
            //######################################################## F2 ##
            case PointIDs.F2:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F1); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A2); //Right

                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 4:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F3); //Down
                        break;
                }

                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E2); //Left
                break;
            //######################################################## F3
            case PointIDs.F3:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F2); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A3); //Right

                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 6:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F4); //Down
                        break;
                }

                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E3); //Left
                break;
            //######################################################## F4
            case PointIDs.F4:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F3); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A4); //Right

                switch (ParentTile.GetComponent<LandTileLogic>().TravelTime)
                {
                    case 8:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                        break;
                    default:
                        ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F5); //Down
                        break;
                }

                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E4); //Left
                break;
            //######################################################## F5
            case PointIDs.F5:
                ConnectedPoints[0] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.F4); //Up
                ConnectedPoints[1] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.A5); //Right
                ConnectedPoints[2] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.Center); //Down
                ConnectedPoints[3] = ParentTile.GetComponent<LandTileLogic>().FindPathPoint(PointIDs.E5); //Left
                break;
        }
        //Debug.Log("PathPointLogic.FindConnected 2: " + PointID);
    }


    //Function called to clear all stored data
    public void ClearData()
    {
        //Loops through and nulls all points in the ConnectedPoints list
        for (int p = 0; p < ConnectedPoints.GetLength(0); ++p)
        {
            ConnectedPoints[p] = null;
        }

        //Makes sure that ConnectedPoints list is clear and null
        ConnectedPoints = null;
    }
}
