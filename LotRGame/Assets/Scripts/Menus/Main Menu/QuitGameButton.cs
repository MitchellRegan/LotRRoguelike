using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameButton : MonoBehaviour
{
    //Function called externally from UI Buttons
    public void QuitGame()
    {
        GameData.globalReference.QuitGame();
    }
}
