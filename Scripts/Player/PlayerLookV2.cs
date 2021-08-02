using UnityEngine;

namespace Assets.Git.Scripts.Player
{
    public class PlayerLookV2 : MonoBehaviour // rename to PlayerLook for initial beta release, attatch to camera container
    {
        [Header("Cameras")]
        [SerializeField] Camera playerCam;
        [SerializeField] Camera weaponCam;
        [SerializeField] internal float sensitivity;
        [SerializeField] internal int cameraFov;
        private const int weaponCameraFov = 70;
        private float rx, ry;

        private static bool enabledFlag = true;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            playerCam.fieldOfView = cameraFov;
            weaponCam.fieldOfView = weaponCameraFov;
        }
        private void Update()
        {
            if (enabledFlag)
            {
                rx += -Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
                ry += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
                rx = Mathf.Clamp(rx, -89, 89);
                transform.rotation = Quaternion.Euler(rx, ry, 0);
            }
        }

        /// <summary>
        /// Enable and disable first person camera movement. Unlocks cursor.
        /// </summary>
        /// <param name="look"></param>
        internal static void ToggleLooking(bool look)
        {
            enabledFlag = look;
            if (look)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }
    }
}
