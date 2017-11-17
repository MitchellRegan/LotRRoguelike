using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadSaveGameMenu : MonoBehaviour
{
    //The global reference to this script
    public static LoadSaveGameMenu globalReference;

    //The reference to the default LoadSavePanel.cs object that we duplicate
    public LoadSavePanel originalPanel;

    //The list of LoadSavePanel objects for each save file
    private List<LoadSavePanel> loadSavePanels;

    //The reference to the scroll view's content rect transform that is scaled
    public RectTransform scrollContentTransform;

    //The buffer of units from the top and bottom of the scroll view
    public float topBottomScrollHeightBuffer = 7;
    //The buffer of units between panels
    public float bufferBetweenPanels = 2;


	//Function called when this object is created
    private void Awake()
    {
        //Making sure there's only one global reference for this script
        if(LoadSaveGameMenu.globalReference == null)
        {
            LoadSaveGameMenu.globalReference = this;
        }
        else
        {
            Destroy(this);
        }

        //Initializing our list of loadSavePanels
        this.loadSavePanels = new List<LoadSavePanel>();
        //Adding our original panel to our last so it's always index 0
        this.loadSavePanels.Add(this.originalPanel);
    }


    //Function called when this object is enabled
    private void OnEnable()
    {
        //Looping through all of our LoadSavePanels and deleting their objects unless they're the original
        for(int p = 1; p < this.loadSavePanels.Count; ++p)
        {
            //Deleting the object for the current panel
            Destroy(this.loadSavePanels[p].gameObject);
            this.loadSavePanels[p] = null;
        }
        //Resetting the list of load save panels so only the original is left
        this.loadSavePanels = new List<LoadSavePanel>() { this.originalPanel };

        //Int to hold the index for the folder we're adding to our save panels
        int saveFolderIndex = -1;
        //Finding all of the save folders in our directory
        foreach(string saveFolderPath in Directory.GetDirectories(Application.persistentDataPath + "/Zein/SaveFiles/"))
        {
            //Making sure the save directory has a TileGrid save
            if(File.Exists(saveFolderPath + "/TileGrid.txt"))
            {
                //Making sure the save directory has a PlayerProgress save
                if(File.Exists(saveFolderPath + "/PlayerProgress.txt"))
                {
                    saveFolderIndex += 1;

                    //Variable to hold the current load save panel for this directory
                    LoadSavePanel directoryPanel;

                    //If the index is 0, it's the original panel
                    if(saveFolderIndex == 0)
                    {
                        //Making the original panel visible
                        this.loadSavePanels[0].gameObject.SetActive(true);
                        directoryPanel = this.loadSavePanels[0];
                    }
                    //If the index is greater than 0, we make a new panel
                    else
                    {
                        //Creating a new instance of the original panel
                        GameObject newPanelObj = GameObject.Instantiate(this.originalPanel.gameObject);
                        //Setting the panel's parent transform to the same as the original's
                        newPanelObj.transform.SetParent(this.originalPanel.transform.transform.parent);
                        directoryPanel = newPanelObj.GetComponent<LoadSavePanel>();
                    }

                    //Setting the index for the panel
                    directoryPanel.panelIndex = saveFolderIndex;

                    //Setting the directory path for the panel
                    string filePathSubstring = Application.persistentDataPath + "/Zein/SaveFiles/";
                    string folderName = saveFolderPath.Remove(0, filePathSubstring.Length);
                    directoryPanel.fileDirectory = saveFolderPath;

                    //Setting the save folder name
                    directoryPanel.fileNameText.text = folderName;

                    //Setting the last date modified text
                    System.DateTime fileModifiedDate = File.GetLastWriteTime(saveFolderPath + "/PlayerProgress.txt");
                    directoryPanel.lastPlayedText.text = "" + fileModifiedDate.Day.ToString() + " / " + fileModifiedDate.Month.ToString() + " / " + 
                                                            fileModifiedDate.Year.ToString() + ", " + fileModifiedDate.TimeOfDay.Hours.ToString() + ":" + 
                                                            fileModifiedDate.TimeOfDay.Minutes.ToString();

                    //Adding the panel to our list of loadSavePanels
                    this.loadSavePanels.Add(directoryPanel);
                }
            }
        }

        //If we don't have any valid directories, we hide the original panel
        if(saveFolderIndex == -1)
        {
            this.loadSavePanels[0].gameObject.SetActive(false);
        }

        //Resizing the scroll view content so all of the panels are visible
        this.ResizeScrollView();
    }


    //Function called from OnEnable to resize the scroll view content size
    private void ResizeScrollView()
    {
        //Float to hold the total height that the scroll view should be
        float scrollViewHeight = 0;

        //Adding the top/bottom buffer twice (once for the top and once for the bottom)
        scrollViewHeight += (2 * this.topBottomScrollHeightBuffer);

        //Adding the height of the panel multiplied by the number of panels
        float panelHeight = this.originalPanel.GetComponent<RectTransform>().rect.height;
        scrollViewHeight += ((this.loadSavePanels.Count - 1) * panelHeight);

        //Adding the buffer between panels multiplied by the number of panels - 1
        scrollViewHeight += ((this.loadSavePanels.Count - 1) * this.bufferBetweenPanels);

        //Setting the scroll view height
        this.scrollContentTransform.sizeDelta = new Vector2(0, scrollViewHeight);
    }
}
