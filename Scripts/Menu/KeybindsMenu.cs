using System;
using UnityEngine;
using Unity;
using TMPro;
using UnityEngine.UI;
using Assets.Git.Scripts.Player.Inputs;
using System.Collections;

namespace Assets.Git.Scripts.Menu
{
    public class KeybindsMenu : MonoBehaviour
    {
        [SerializeField] private int controlSetTime;

        [SerializeField] private Button SetJumpButton;
        [SerializeField] private Button SetCrouchButton;
        [SerializeField] private Button SetFireGrappleButton;
        [SerializeField] private Button SetFireButton;
        [SerializeField] private Button SetAimButton;
        [SerializeField] private Button SetQuickSwitchButton;
        [SerializeField] private Button SetUnEquipButton;
        [SerializeField] private Button SetQuickMeleeButton;
        [SerializeField] private Button SetThrowGrenadeButton;

        bool checkinFlag = false;

        private InputHandler.Inputs currentControl;
        private void Start()
        {
            SetJumpButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.Jump;
                StartCoroutine(AllowSet(SetJumpButton, currentControl));
            });
        }

        private void FixedUpdate()
        {
            if (checkinFlag)
                foreach (KeyCode cKey in Enum.GetValues(typeof(KeyCode)))
                    if (Input.GetKey(cKey))
                        InputHandler.SetControl(currentControl, cKey);
        }

        private IEnumerator AllowSet(Button button, InputHandler.Inputs control)
        {
            checkinFlag = true;
            TMP_Text txt = button.GetComponent<TMP_Text>();
            txt.text = "Press any key.";
            yield return new WaitForSeconds(controlSetTime);
            checkinFlag = false;
            txt.text = control.ToString();
        }

    }
}
