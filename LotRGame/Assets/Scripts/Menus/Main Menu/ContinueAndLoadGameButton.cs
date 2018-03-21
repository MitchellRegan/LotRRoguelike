using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueAndLoadGameButton : MonoBehaviour
{
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
