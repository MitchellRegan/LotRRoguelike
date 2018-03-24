using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class QuestUIPanel : MonoBehaviour
{
    //Index for this panel
    [HideInInspector]
    public int panelIndex = 0;

    //Text reference for the name of the quest for this panel
    public Text questNameText;



	//Function called externally from Buttons to display this quest
    public void DisplayThisQuest()
    {
        //Telling the QuestMenuUI script to display this quest
        QuestMenuUI.globalReference.DisplayQuestDescription(this.panelIndex);
    }
}
