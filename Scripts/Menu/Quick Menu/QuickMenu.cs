using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Git.Scripts.Player;
using Assets.Git.Scripts.Gameplay;

namespace Assets.Git.Scripts.Menu.Quick_Menu
{
    public class QuickMenu : MonoBehaviour // NOTE this script should not exist in non-controllable areas such as the main menu, and credits
    {
        //[SerializeField] private Canvas canvas;
        [SerializeField] private Misc.MasterMiscController mmc;
        private ErrorReporter errorReporter;
        [SerializeField] private GameObject pauseMenuContainer;
        [SerializeField] private Animator pauseMenuAnimator;
        private readonly string pauseAnim = "FadeIn";
        private readonly string resumeAnim = "FadeOut"; // idle is just idle

        [SerializeField] private Image masterFade;
        private readonly string masterFadeOut = "FadeOutMaster";
        [SerializeField] private GameObject loadingObject;

        private bool menuOpen;

        // for debugging only
        private void Start()
        {
            errorReporter = mmc.GetComponent<ErrorReporter>();
            if (pauseMenuContainer.activeInHierarchy)
            {
                menuOpen = true;
                EscapeOnClick();
            }
            else
                menuOpen = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                if (!menuOpen)
                    EscapeOnClick();
                else
                    ResumeOnClick();
        }

        /// <summary>
        /// Do not call. Button delegate only (NO REFERENCES)
        /// </summary>
        private void EscapeOnClick() => StartCoroutine(_EscapeOnClick());
        private IEnumerator _EscapeOnClick()
        {
            PlayerLookV2.ToggleLooking(false);
            menuOpen = true;
            pauseMenuContainer.SetActive(true);
            pauseMenuAnimator.Play(pauseAnim);
            for (int i = 0; i < 20; i++) // one of those cases where getting state doesnt wanna work for no reason...
                yield return new WaitForEndOfFrame(); // anim is 20 frames :/
            Time.timeScale = 0;
        }

        /// <summary>
        /// Do not call. Button delegate only (NO REFERENCES)
        /// </summary>
        public void ResumeOnClick() => StartCoroutine(_ResumeOnClick());
        private IEnumerator _ResumeOnClick()
        {
            Time.timeScale = 1;
            pauseMenuAnimator.Play(resumeAnim);
            yield return new WaitForSeconds(pauseMenuAnimator.GetCurrentAnimatorStateInfo(0).length);
            pauseMenuContainer.SetActive(false);
            PlayerLookV2.ToggleLooking(true);
            menuOpen = false;
        }

        /// <summary>
        /// Button delegate. Do not call externally.
        /// </summary>
        public void SaveOnClick()
        {
            if (mmc.TryGetComponent(out GameplayManager gm))
                gm.WriteData();
            else
                errorReporter.ReportError("Unable to save.", "GameplayManager not attatched to master miscellaneous controller.", ErrorReporter.ActionType.ok);
        }

        /// <summary>
        /// Button delegate. Do not call externally.
        /// </summary>
        public void QuitOnClick()
        {
            masterFade.GetComponent<Animator>().Play(masterFadeOut);
            mmc.LoadScene(Misc.MasterMiscController.Scenes.MainMenu, true);
        }

        /// <summary>
        /// Do not call. Button delegate only (NO REFERENCES)
        /// </summary>
        public void SaveAndQuitOnClick()
        {
            SaveOnClick();
            QuitOnClick();
        }
    }
}