using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Assets.Git.Scripts.Player.Inputs;
using System.Collections;

namespace Assets.Git.Scripts.Menu
{// 6/13/21 - 6/14/21 were terrible days, yet had great productivity. keep going dude
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
            StartCoroutine(Init());
            SetJumpButton.GetComponentInChildren<TMP_Text>().text = InputHandler.JumpKC.ToString();
            SetJumpButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.Jump;
                StartCoroutine(AllowSet(SetJumpButton, InputHandler.JumpKC));
            });

            SetCrouchButton.GetComponentInChildren<TMP_Text>().text = InputHandler.CrouchKC.ToString();
            SetCrouchButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.Crouch;
                StartCoroutine(AllowSet(SetCrouchButton, InputHandler.CrouchKC));
            });

            SetFireGrappleButton.GetComponentInChildren<TMP_Text>().text = InputHandler.FireGrappleKC.ToString();
            SetFireGrappleButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.FireGrapple;
                StartCoroutine(AllowSet(SetFireGrappleButton, InputHandler.FireGrappleKC));
            });

            SetFireButton.GetComponentInChildren<TMP_Text>().text = InputHandler.FireKC.ToString();
            SetFireButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.Fire;
                StartCoroutine(AllowSet(SetAimButton, InputHandler.FireKC));
            });

            SetAimButton.GetComponentInChildren<TMP_Text>().text = InputHandler.AimKC.ToString();
            SetAimButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.Aim;
                StartCoroutine(AllowSet(SetAimButton, InputHandler.AimKC));
            });

            SetQuickSwitchButton.GetComponentInChildren<TMP_Text>().text = InputHandler.QuickSwitchKC.ToString();
            SetQuickSwitchButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.QuickSwitch;
                StartCoroutine(AllowSet(SetQuickSwitchButton, InputHandler.QuickSwitchKC));
            });

            SetUnEquipButton.GetComponentInChildren<TMP_Text>().text = InputHandler.UnEquipKC.ToString();
            SetUnEquipButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.UnEquip;
                StartCoroutine(AllowSet(SetUnEquipButton, InputHandler.UnEquipKC));
            });

            SetQuickMeleeButton.GetComponentInChildren<TMP_Text>().text = InputHandler.QuickMeleeKC.ToString();
            SetQuickMeleeButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.QuickMelee;
                StartCoroutine(AllowSet(SetQuickMeleeButton, InputHandler.QuickMeleeKC));
            });

            SetThrowGrenadeButton.GetComponentInChildren<TMP_Text>().text = InputHandler.ThrowGrenadeKC.ToString();
            SetThrowGrenadeButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.ThrowGrenade;
                StartCoroutine(AllowSet(SetThrowGrenadeButton, InputHandler.ThrowGrenadeKC));
            });
        }

        private void Update()
        {
            if (checkinFlag)
                foreach (KeyCode cKey in Enum.GetValues(typeof(KeyCode)))
                    if (Input.GetKey(cKey))
                        InputHandler.SetControl(currentControl, cKey);
        }
        private IEnumerator Init()
        {
            yield return new WaitForEndOfFrame();
            SetText(SetJumpButton, InputHandler.JumpKC);
            SetText(SetCrouchButton, InputHandler.CrouchKC);
            SetText(SetFireGrappleButton, InputHandler.FireGrappleKC);
            SetText(SetFireButton, InputHandler.FireKC);
            SetText(SetAimButton, InputHandler.AimKC);
            SetText(SetQuickSwitchButton, InputHandler.QuickSwitchKC);
            SetText(SetUnEquipButton, InputHandler.UnEquipKC);
            SetText(SetQuickMeleeButton, InputHandler.QuickMeleeKC);
            SetText(SetThrowGrenadeButton, InputHandler.ThrowGrenadeKC);
        }

        private IEnumerator AllowSet(Button button, KeyCode control)
        {
            checkinFlag = true;
            TextMeshProUGUI txt = button.gameObject.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = "Press any key.";
            yield return new WaitForSeconds(controlSetTime);
            checkinFlag = false;
            txt.text = control.ToString();
        }

        private void SetText(Button button, KeyCode control) => button.gameObject.GetComponentInChildren<TMP_Text>().text = control.ToString();

    }
}
