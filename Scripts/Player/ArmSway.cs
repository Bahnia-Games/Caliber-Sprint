using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Git.Scripts.Player
{
    public class ArmSway : MonoBehaviour
    {
        public float swayAmount;
        public float smoothAmount;

        private Vector3 initialPos;

        private void Start() => initialPos = transform.localPosition;

        public void Update()
        {
            float movementX = -Input.GetAxis("Mouse X") * swayAmount;
            float movementY = -Input.GetAxis("Mouse Y") * swayAmount;

            Vector3 finalPos = new Vector3(movementX, movementY);
            transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + initialPos, Time.deltaTime * smoothAmount);
        }
    }
}
