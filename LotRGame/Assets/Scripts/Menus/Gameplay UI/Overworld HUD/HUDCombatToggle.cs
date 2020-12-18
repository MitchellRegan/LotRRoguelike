using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDCombatToggle : MonoBehaviour
{
    //Reference to the HUD object that will be enabled/disabled
    public Canvas hudObject;
    
    //Delegate event to listen for events to start and end combat
    private DelegateEvent<EVTData> combatTransitionListener;



    //Function called when this object is created to assign our delegate event
    private void Awake()
    {
        this.combatTransitionListener = new DelegateEvent<EVTData>(this.CombatTransitioning);
    }


    //Function called when this component is enabled
    private void OnEnable()
    {
        EventManager.StartListening(CombatTransitionEVT.eventNum, this.combatTransitionListener);
    }


    //Function called when this component is disabled
    private void OnDisable()
    {
        EventManager.StopListening(CombatTransitionEVT.eventNum, this.combatTransitionListener);
    }


    //Function called from the combatTransitionListener delegate from CombatTransitionEVT events
    private void CombatTransitioning(EVTData data_)
    {
        //Making sure the combat transition event data isn't null
        if (data_.combatTransition != null)
        {
            //Setting the isInCombat bool based on if combat is starting or ending
            this.hudObject.enabled = !data_.combatTransition.startingCombat;
            TimePanelUI.globalReference.SetTimePaused(data_.combatTransition.startingCombat);
        }
    }
}
