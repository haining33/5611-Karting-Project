using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookAround : MonoBehaviour
{
    float rotationX = 0f;
    float rotationY = 0f;

    public float sensitivity = 15f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rotationY += Input.GetAxis("Mouse X") * sensitivity;
            //rotationX += Input.GetAxis("Mouse Y") * sensitivity * -1;z
            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }
}
