// stale
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Git.Scripts
{
    public class PlayerLook : MonoBehaviour
    {

        [SerializeField] private float mouseSensitivity = 100f;
        public float fov = 60;

        [SerializeField] private Transform playerBody;
        [SerializeField] public Transform arms;

        private float xRotation = 0;

        [SerializeField] private Camera camera;

        [SerializeField] private GameObject cameraContainer;

        // Start is called before the first frame update
        void Start()
        {
            camera.renderingPath = RenderingPath.DeferredLighting;

            Cursor.lockState = CursorLockMode.Locked;
            camera.fieldOfView = fov;
        }

        // Update is called once per frame
        void Update()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90, 90);

            cameraContainer.transform.localRotation = Quaternion.Euler(xRotation, xRotation, 0);
            arms.transform.rotation = cameraContainer.transform.rotation;

            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}