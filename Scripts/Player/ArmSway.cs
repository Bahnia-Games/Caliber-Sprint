using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Git.Scripts.Player
{
    public class ArmSway : MonoBehaviour
    {
        [SerializeField] private float swayAmount;
        [SerializeField] private float smoothAmount;

        private Vector3 initialPos;

        private void Start() => initialPos = transform.localPosition;

        public void Update() // broken atm, no idea why
        {
            float mx = -Input.GetAxis("Mouse X") * swayAmount;
            float my = -Input.GetAxis("Mouse Y") * swayAmount;
            Vector3 finalPos = new Vector3(mx, my);
            transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + initialPos, Time.deltaTime * smoothAmount);
        }
    }
}
