using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotCam : MonoBehaviour
{

    public Camera camera;
    public Vector3 dir;
    float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dir.y = Input.GetAxis("Mouse Y") * PlayerLook.mouseSensitivity * Time.deltaTime;
        xRotation -= dir.y;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}
