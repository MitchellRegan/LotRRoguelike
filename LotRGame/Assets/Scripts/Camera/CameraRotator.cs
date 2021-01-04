using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [Tooltip("The number of Euler degrees rotated along the Y-axis each update")]
    public float rotationSpeed = 2;



    // Update is called once per frame
    private void Update()
    {
        this.transform.localEulerAngles += new Vector3(0, this.rotationSpeed, 0);
    }
}
