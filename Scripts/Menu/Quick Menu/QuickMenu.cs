using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Git.Scripts.Player;

namespace Assets.Git.Scripts.Menu.Quick_Menu
{
    public class QuickMenu : MonoBehaviour // NOTE this script should not exist in non-controllable areas such as the main menu, and credits
    {
        //[SerializeField] private Canvas canvas;
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
            {
                if (!menuOpen)
                    EscapeOnClick();
                else
                    ResumeOnClick();
            }
        }

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
        /// 
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
        /// Do not call. Button delegate only (NO REFERENCES)
        /// </summary>
        public void SaveOnClick()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Do not call. Button delegate only (NO REFERENCES)
        /// </summary>
        public void QuitOnClick()
        {
            masterFade.GetComponent<Animator>().Play(masterFadeOut);

        }

        /// <summary>
        /// Do not call. Button delegate only (NO REFERENCES)
        /// </summary>
        public void SaveAndQuitOnClick()
        {
            throw new NotImplementedException();
        }
    }
}