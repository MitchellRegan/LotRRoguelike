using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueAndLoadGameButton : MonoBehaviour
{
    //Bool for if this is the continue button, which is disabled if there's no existing save file
    public bool isContinueButton = false;



    //Function called when this object is enabled
    private void OnEnable()
    {
        //Enables or disables the continue button depending on if a save file exists
        if (this.isContinueButton)
        {
            this.GetComponent<UnityEngine.UI.Button>().interactable = SaveLoadManager.globalReference.DoesSaveFileExist();
        }
    }


    //Function called externally from UI Buttons to continue the current game
    public void ContinueSaveGame()
    {
        GameData.globalReference.ContinueGame();
    }


    //Function called externally from UI Buttons to start a new game
    public void StartNewGame()
    {
        GameData.globalReference.StartNewGame();
    }
}
