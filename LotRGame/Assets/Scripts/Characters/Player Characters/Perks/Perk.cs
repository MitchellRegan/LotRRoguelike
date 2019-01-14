using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(IDTag))]
public class Perk : MonoBehaviour
{
    //The prefab object reference that this perk is an instance of
    [HideInInspector]
    public GameObject perkPrefabRoot;

    //The displayed name of this perk in the character UI
    public string perkNameID;

    //The description of this perk in the character UI
    public string perkDescription;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
