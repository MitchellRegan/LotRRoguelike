using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySelectorButton : MonoBehaviour
{
    //Function called externally from UI buttons
    public void SetGameDifficulty(int difficultyLevel_)
    {
        GameData.globalReference.SetGameDifficulty(difficultyLevel_);
    }
}
