using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;

namespace Assets.Git.Scripts.Menu.Quick_Menu
{
    public class QuickMenu : MonoBehaviour // NOTE this script should not exist in non-controllable areas such as the main menu, and credits
    {
        //[SerializeField] private Canvas canvas;
        [SerializeField] private GameObject pauseMenuContainer;
        [SerializeField] private Animator pauseMenuAnimator;
        private readonly string pauseAnim = "pause";
        private readonly string resumeAnim = "resume"; // idle is just idle
        public void Update() { if (Input.GetKeyDown(KeyCode.Escape)) { EscapeOnClick(); } }
        private void EscapeOnClick()
        {
            Time.timeScale = 0;
            pauseMenuContainer.SetActive(true);
            pauseMenuAnimator.Play(pauseAnim);
        }
        public void ResumeOnClick() => StartCoroutine(_ResumeOnClick());
        private IEnumerator _ResumeOnClick()
        {
            pauseMenuAnimator.Play(resumeAnim);
            yield return new WaitForSeconds(pauseMenuAnimator.GetCurrentAnimatorStateInfo(0).length);
            pauseMenuContainer.SetActive(false);
            Time.timeScale = 1;
        }

        public void SaveOnClick()
        {

        }

        public void QuitOnClick()
        {

        }
    }
}
