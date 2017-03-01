using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GoToLevel : MonoBehaviour
{
    //Function called externally to load levels based on their name
    public void LoadLevelByName(string levelName_)
    {
        SceneManager.LoadScene(levelName_);
    }


    //Function called externally to load levels based on their level index
    public void LoadLevelByIndex(int levelIndex_)
    {
        SceneManager.LoadScene(levelIndex_);
    }
}
