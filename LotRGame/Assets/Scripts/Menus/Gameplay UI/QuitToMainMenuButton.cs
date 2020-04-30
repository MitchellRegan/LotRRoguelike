using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitToMainMenuButton : MonoBehaviour
{
    //Function called externally from UI buttons
    public void QuitToMainMenu(string mainMenuName_)
    {
        //Save player progress and transition to the MainMenu.unity scene
        SaveLoadManager.globalReference.SavePlayerProgress();

        EVTData transitionEVT = new EVTData();
        transitionEVT.sceneTransition = new SceneTransitionEVT(mainMenuName_, 0.5f, 1);
        EventManager.TriggerEvent(SceneTransitionEVT.eventNum, transitionEVT);
    }
}
