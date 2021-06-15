using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Assets.Git.Scripts.Player.Inputs;
using System.Collections;

namespace Assets.Git.Scripts.Menu
{// 6/13/21 - 6/14/21 were terrible days, yet had great productivity. keep going dude
#pragma warning disable CS0642
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

        private KeyCode checkInKey = KeyCode.None;
        private bool checkinFlag = false;
        private Coroutine checkinCoroutineAllowSet;
        private TextMeshProUGUI checkinTextLabel;

        private InputHandler.Inputs currentControl;
        private void Start()
        {
            checkinCoroutineAllowSet = StartCoroutine(Init());
            SetJumpButton.GetComponentInChildren<TMP_Text>().text = InputHandler.JumpKC.ToString();
            SetJumpButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.Jump;
                checkinTextLabel = BtToUI(SetJumpButton);
                checkinCoroutineAllowSet = StartCoroutine(AllowSet(SetJumpButton, InputHandler.JumpKC));
            });

            SetCrouchButton.GetComponentInChildren<TMP_Text>().text = InputHandler.CrouchKC.ToString();
            SetCrouchButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.Crouch;
                checkinTextLabel = BtToUI(SetCrouchButton);
                checkinCoroutineAllowSet = StartCoroutine(AllowSet(SetCrouchButton, InputHandler.CrouchKC));
            });

            SetFireGrappleButton.GetComponentInChildren<TMP_Text>().text = InputHandler.FireGrappleKC.ToString();
            SetFireGrappleButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.FireGrapple;
                checkinTextLabel = BtToUI(SetFireGrappleButton);
                checkinCoroutineAllowSet = StartCoroutine(AllowSet(SetFireGrappleButton, InputHandler.FireGrappleKC));
            });

            SetFireButton.GetComponentInChildren<TMP_Text>().text = InputHandler.FireKC.ToString();
            SetFireButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.Fire;
                checkinTextLabel = BtToUI(SetFireButton);
                checkinCoroutineAllowSet = StartCoroutine(AllowSet(SetAimButton, InputHandler.FireKC));
            });

            SetAimButton.GetComponentInChildren<TMP_Text>().text = InputHandler.AimKC.ToString();
            SetAimButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.Aim;
                checkinTextLabel = BtToUI(SetAimButton);
                checkinCoroutineAllowSet = StartCoroutine(AllowSet(SetAimButton, InputHandler.AimKC));
            });

            SetQuickSwitchButton.GetComponentInChildren<TMP_Text>().text = InputHandler.QuickSwitchKC.ToString();
            SetQuickSwitchButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.QuickSwitch;
                checkinTextLabel = BtToUI(SetQuickSwitchButton);
                checkinCoroutineAllowSet = StartCoroutine(AllowSet(SetQuickSwitchButton, InputHandler.QuickSwitchKC));
            });

            SetUnEquipButton.GetComponentInChildren<TMP_Text>().text = InputHandler.UnEquipKC.ToString();
            SetUnEquipButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.UnEquip;
                checkinTextLabel = BtToUI(SetUnEquipButton);
                checkinCoroutineAllowSet = StartCoroutine(AllowSet(SetUnEquipButton, InputHandler.UnEquipKC));
            });

            SetQuickMeleeButton.GetComponentInChildren<TMP_Text>().text = InputHandler.QuickMeleeKC.ToString();
            SetQuickMeleeButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.QuickMelee;
                checkinTextLabel = BtToUI(SetQuickMeleeButton);
                checkinCoroutineAllowSet = StartCoroutine(AllowSet(SetQuickMeleeButton, InputHandler.QuickMeleeKC));
            });

            SetThrowGrenadeButton.GetComponentInChildren<TMP_Text>().text = InputHandler.ThrowGrenadeKC.ToString();
            SetThrowGrenadeButton.onClick.AddListener(delegate
            {
                currentControl = InputHandler.Inputs.ThrowGrenade;
                checkinTextLabel = BtToUI(SetThrowGrenadeButton);
                checkinCoroutineAllowSet = StartCoroutine(AllowSet(SetThrowGrenadeButton, InputHandler.ThrowGrenadeKC));
            });
        }

        private void Update() // not breaking
        {
            if (checkinFlag)
                foreach (KeyCode cKey in Enum.GetValues(typeof(KeyCode)))
                    if (Input.GetKey(cKey))
                    {
                        InputHandler.SetControl(currentControl, cKey);
                        checkInKey = cKey;
                        StopCoroutine(checkinCoroutineAllowSet);
                        break;
                    }
                    else
                        ;
            else
                checkInKey = KeyCode.None;
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
            FinalizeAllowSet(txt);
            checkinCoroutineAllowSet = null;
        }

        private void FinalizeAllowSet(TextMeshProUGUI txt)
        {
            if (checkInKey == KeyCode.None)
                txt.text = "nul";
            else
                txt.text = checkInKey.ToString();
        }

        private TextMeshProUGUI BtToUI(Button b) { return b.GetComponent<TextMeshProUGUI>(); }

        private void SetText(Button button, KeyCode control) => button.gameObject.GetComponentInChildren<TMP_Text>().text = control.ToString();

        internal void OnSaveClicked() => InputHandler.Save();

    }
}
