using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTag : MonoBehaviour
{
    //Enum for all of the different types of monsters
    public enum EnemyTypes
    {
        Humanoid,
        Beast,
        Insect,
        Undead,
        Specter,
        Plant,
        Aquatic,
        Flying,
        WaterElemental,
        FireElemental,
        EarthElemental,
        WindElemental,
        Dragon
    }

    //List for all of this enemy's types, since they can be multiple at once
    public List<EnemyTypes> typeList;
}
