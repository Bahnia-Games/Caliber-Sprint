using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public static float mouseSensitivity = 100f;
    public float fov = 60;

    public Transform playerBody;
    public Rigidbody rb;

    private float xRotation = 0;

    public Camera camera;

    public Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        camera.fieldOfView = fov;
    }

    // Update is called once per frame
    void Update()
    {
        dir.x = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //dir.y = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //xRotation -= dir.y;
        //xRotation = Mathf.Clamp(xRotation, -90, 90);

        //transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * dir.x);

        //rb.rotation = Quaternion.Euler(dir.x, rb.rotation.y, rb.rotation.z);
        

        
    }
}
