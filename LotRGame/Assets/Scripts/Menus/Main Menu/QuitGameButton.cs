using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameButton : MonoBehaviour
{
    //Countdown timer to make sure we save player progress before quitting
    private float countDown = 0.25f;
    private bool quitting = false;



    //Function called externally from UI Buttons
    public void QuitGame()
    {
        SaveLoadManager.globalReference.SavePlayerProgress();
        this.quitting = true;
    }


    //Function called every frame
    private void Update()
    {
        if(this.quitting)
        {
            this.countDown -= Time.deltaTime;

            if(this.countDown < 0)
            {
                GameData.globalReference.QuitGame();
            }
        }
    }
}
