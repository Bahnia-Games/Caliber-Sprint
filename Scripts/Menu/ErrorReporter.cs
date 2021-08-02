﻿using UnityEngine;
using TMPro;
using Assets.Git.Scripts.Misc;

namespace Assets.Git.Scripts.Menu
{
    #pragma warning disable CS0649
    public class ErrorReporter : MonoBehaviour // attatch to mmc
    {
        [SerializeField] private GameObject errorPopupGO;
        [SerializeField] private TMP_Text errorTitle;
        [SerializeField] private TMP_Text errorText;
        [SerializeField] private TMP_Text buttonUGUI;

        private static bool inGameFlag = false;

        private ActionType currentActionType;

        public enum ActionType
        {
            ok,
            quit,
            saveAndQuit
        }

        public void ReportError(string title, string message = "", ActionType actionType = ActionType.ok, bool inGame = false)
        {
            errorPopupGO.SetActive(true);
            errorTitle.text = title;
            errorText.text = message;
            currentActionType = actionType;

            inGameFlag = inGame;
            if (inGameFlag)
            { Time.timeScale = 0; Cursor.lockState = CursorLockMode.None; }

            switch (actionType)
            {
                case ActionType.ok:
                    buttonUGUI.text = "OK";
                    break;
                case ActionType.quit:
                    buttonUGUI.text = "QUIT";
                    break;
                case ActionType.saveAndQuit:
                    buttonUGUI.text = "SAVE AND QUIT";
                    break;
            }
        }
        /// <summary>
        /// Do not call. Button delegate only (NO REFERENCES)
        /// </summary>
        public void ErrorButtonOnClick()
        {
            switch (currentActionType)
            {
                case ActionType.ok:
                    errorPopupGO.SetActive(false);
                    if (inGameFlag)
                    { Time.timeScale = 1; Cursor.lockState = CursorLockMode.Locked; }
                    break;
                case ActionType.quit:
                    errorPopupGO.SetActive(false);
                    break;
                case ActionType.saveAndQuit:
                    errorPopupGO.SetActive(false);
                    QuitEventArgs args = new QuitEventArgs();
                    args.quitReason = QuitEventArgs.QuitReason.request;
                    args.Requester = "ErrorReporter.cs ErrorButonOnClick()";
                    GetComponent<MasterMiscController>().InvokeApplicationQuitRequest(this, args);
                    break;
            }
        }
    }
}
