using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSavePanel : MonoBehaviour
{
    //The index of this panel
    [HideInInspector]
    public int panelIndex = 0;
    //The file path for this save directory
    [HideInInspector]
    public string fileDirectory = "";

    //The text reference for this panel's file name
    public Text fileNameText;
    //The text reference for this panel's last played date
    public Text lastPlayedText;
    //The button reference for this panel's Play Game
    public Button playGameFileButton;
    //The button reference for this panel's Delete Save
    public Button deleteSaveButton;


	//Function called externally to load this panel's save file
    public void LoadFile()
    {
        //Setting this file directory as the one to load
        GameData.globalReference.LoadSaveFile(this.fileDirectory);
    }


    //Function called externally to delete this panel's save file
    public void DeleteFile()
    {
        //Prompting the LoadSaveManager to delete this file
        LoadSaveGameMenu.globalReference.PromptFileDeletion(this.panelIndex);
    }
}
