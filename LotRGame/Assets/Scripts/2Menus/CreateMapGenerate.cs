using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

public class CreateMapGenerate : MonoBehaviour
{
    //Name and Location of saved XML
    private string FileName;
    private string FileLocation;

    //Reference to this object's Text child
    public GameObject TextChild = null;
    private UnityEngine.UI.Text TextChildGUI = null;

    //Reference to this object's Debug Text child
    public GameObject DebugTextChild = null;
    private UnityEngine.UI.Text DebugTextChildGUI = null;

    //References to all Land percentages
    public GameObject Ocean;
    public GameObject Plains;
    public GameObject Forrest;
    public GameObject Swamp;
    public GameObject Desert;
    public GameObject Mountain;
    public GameObject Volcano;

    //References to all Special Tiles
    public GameObject Villages;
    public GameObject EnemyCamps;
    public GameObject ExplorePoints;

    //References to the Row and Column sizes
    public GameObject Row;
    public GameObject Column;

    private int TotalLand = 0;

    private int[] MapInfo;


	// Use this for initialization
	void Start ()
    {
        TextChildGUI = TextChild.GetComponent<UnityEngine.UI.Text>();
        DebugTextChildGUI = DebugTextChild.GetComponent<UnityEngine.UI.Text>();

        MapInfo = new int[12];
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Gets the sum of all tile percentages
        TotalLand = Ocean.GetComponent<CreateMapButtons>().GetTilePercentage() +
                    Plains.GetComponent<CreateMapButtons>().GetTilePercentage() +
                    Forrest.GetComponent<CreateMapButtons>().GetTilePercentage() +
                    Swamp.GetComponent<CreateMapButtons>().GetTilePercentage() +
                    Desert.GetComponent<CreateMapButtons>().GetTilePercentage() +
                    Mountain.GetComponent<CreateMapButtons>().GetTilePercentage() +
                    Volcano.GetComponent<CreateMapButtons>().GetTilePercentage();

        TextChildGUI.text = TotalLand.ToString() + "%";

        if (TotalLand > 100)
        {
            DebugTextChildGUI.text = "Too many Land Tiles. Total % of Tiles must be 100";
        }
        else if(TotalLand < 100)
        {
            DebugTextChildGUI.text = "Too few Land Tiles. Total % of Tiles must be 100";
        }
        else
        {
            DebugTextChildGUI.text = "";
        }
    }


    public void GenerateMap()
    {
        if(TotalLand != 100)
        {
            return;
        }

        //Store total dimensions of the map
        MapInfo[0] = Column.GetComponent<CreateMapButtons>().GetTilePercentage();
        MapInfo[1] = Row.GetComponent<CreateMapButtons>().GetTilePercentage();

        //Temp var to hold the total number of tiles the map will have
        var TotalTiles = MapInfo[0] * MapInfo[1];

        //Store the actual number of each type of tile that the map will have
        MapInfo[2] = (Plains.GetComponent<CreateMapButtons>().GetTilePercentage()) * TotalTiles / 100;
        MapInfo[3] = (Forrest.GetComponent<CreateMapButtons>().GetTilePercentage()) * TotalTiles / 100;
        MapInfo[4] = (Swamp.GetComponent<CreateMapButtons>().GetTilePercentage()) * TotalTiles / 100;
        MapInfo[5] = (Desert.GetComponent<CreateMapButtons>().GetTilePercentage()) * TotalTiles / 100;
        MapInfo[6] = (Mountain.GetComponent<CreateMapButtons>().GetTilePercentage()) * TotalTiles / 100;
        MapInfo[7] = (Volcano.GetComponent<CreateMapButtons>().GetTilePercentage()) * TotalTiles / 100;

        //Ocean tiles take up the number of tiles left over
        MapInfo[8] = TotalTiles - MapInfo[2] - MapInfo[3] - MapInfo[4] - MapInfo[5] - MapInfo[6] - MapInfo[7];

        //Stores the Special tiles
        MapInfo[9] = Villages.GetComponent<CreateMapButtons>().GetTilePercentage();
        MapInfo[10] = EnemyCamps.GetComponent<CreateMapButtons>().GetTilePercentage();
        MapInfo[11] = ExplorePoints.GetComponent<CreateMapButtons>().GetTilePercentage();

        //Debug.Log(TotalTiles);
        string tileString = MapInfo[2].ToString() + " Plains, " +
                            MapInfo[3].ToString() + " Forrest, " +
                            MapInfo[4].ToString() + " Swamp, " +
                            MapInfo[5].ToString() + " Desert, " +
                            MapInfo[6].ToString() + " Mountain, " +
                            MapInfo[7].ToString() + " Volcano, " +
                            MapInfo[8].ToString() + " Ocean";
        //Debug.Log(tileString);

        this.GetComponent<TESTGenerateMap>().Generate(MapInfo);
    }
}
