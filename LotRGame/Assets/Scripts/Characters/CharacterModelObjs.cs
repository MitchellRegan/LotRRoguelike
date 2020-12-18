using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModelObjs : MonoBehaviour
{
    //The model that's spawned in
    public GameObject charModel;

    //The index for which character model is being used
    public int bodyModelIndex = 0;
    //The index for which decal model is being used
    public int decalModelIndex = 0;

    //The color for this character's skin
    public Color skinColor;
    //The color for this character's decal
    public Color decalColor;
}
