using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompasUI : MonoBehaviour
{
	// Update is called once per frame
	private void Update ()
    {
        this.transform.eulerAngles = new Vector3(0, 0, OrbitCamera.directionFacing);
	}
}
