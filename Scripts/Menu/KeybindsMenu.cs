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
            SetText(SetJumpButton, InputHandler.Inputs.Jump);
            SetText(SetCrouchButton, InputHandler.Inputs.Crouch);
            SetText(SetFireGrappleButton, InputHandler.Inputs.FireGrapple);
            SetText(SetFireButton, InputHandler.Inputs.Fire);
            SetText(SetAimButton, InputHandler.Inputs.Aim);
            SetText(SetQuickSwitchButton, InputHandler.Inputs.QuickSwitch);
            SetText(SetUnEquipButton, InputHandler.Inputs.UnEquip);
            SetText(SetQuickMeleeButton, InputHandler.Inputs.QuickMelee);
            SetText(SetThrowGrenadeButton, InputHandler.Inputs.ThrowGrenade);

            SetJumpButton.GetComponentInChildren<TMP_Text>().text = InputHandler.JumpKC.ToString();
            SetJumpButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.Jump;
                StartCoroutine(AllowSet(SetJumpButton, currentControl));
            });

            SetCrouchButton.GetComponentInChildren<TMP_Text>().text = InputHandler.CrouchKC.ToString();
            SetCrouchButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.Crouch;
                StartCoroutine(AllowSet(SetCrouchButton, currentControl));
            });

            SetFireGrappleButton.GetComponentInChildren<TMP_Text>().text = InputHandler.FireGrappleKC.ToString();
            SetFireGrappleButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.FireGrapple;
                StartCoroutine(AllowSet(SetFireGrappleButton, currentControl));
            });

            SetFireButton.GetComponentInChildren<TMP_Text>().text = InputHandler.FireKC.ToString();
            SetFireButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.Fire;
                StartCoroutine(AllowSet(SetAimButton, currentControl));
            });

            SetAimButton.GetComponentInChildren<TMP_Text>().text = InputHandler.AimKC.ToString();
            SetAimButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.Aim;
                StartCoroutine(AllowSet(SetAimButton, currentControl));
            });

            SetQuickSwitchButton.GetComponentInChildren<TMP_Text>().text = InputHandler.QuickSwitchKC.ToString();
            SetQuickSwitchButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.QuickSwitch;
                StartCoroutine(AllowSet(SetQuickSwitchButton, currentControl));
            });

            SetUnEquipButton.GetComponentInChildren<TMP_Text>().text = InputHandler.UnEquipKC.ToString();
            SetUnEquipButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.UnEquip;
                StartCoroutine(AllowSet(SetUnEquipButton, currentControl));
            });

            SetQuickMeleeButton.GetComponentInChildren<TMP_Text>().text = InputHandler.QuickMeleeKC.ToString();
            SetQuickMeleeButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.QuickMelee;
                StartCoroutine(AllowSet(SetQuickMeleeButton, currentControl));
            });

            SetThrowGrenadeButton.GetComponentInChildren<TMP_Text>().text = InputHandler.ThrowGrenadeKC.ToString();
            SetThrowGrenadeButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.ThrowGrenade;
                StartCoroutine(AllowSet(SetThrowGrenadeButton, currentControl));
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
            TextMeshProUGUI txt = button.gameObject.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = "Press any key.";
            yield return new WaitForSeconds(controlSetTime);
            checkinFlag = false;
            txt.text = control.ToString();
        }

        private void SetText(Button button, InputHandler.Inputs control) => button.gameObject.GetComponentInChildren<TMP_Text>().text = control.ToString();

    }
}
