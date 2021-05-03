using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Git.Scripts.Gameplay;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Git.Scripts.Menu
{
    public class StartGameMenu : MonoBehaviour
    {
        [SerializeField] GameplayManager gameplayManager;
        [SerializeField] MasterMiscController masterMiscController;
        [SerializeField] Animator checkmarkAnimator;
        [SerializeField] Animator fadeOutAnimator;
        private readonly string checkMarkSelect = "checkSelect";
        [SerializeField] private GameObject checkMarkGO;

        [SerializeField] private Button continueGameButton;

        public void Start()
        {
            if (GameplayManager.playerData == null)
                continueGameButton.interactable = false;
        }

        public void NewGameOnClick() => StartCoroutine(_NewGameOnClick());

        private IEnumerator _NewGameOnClick()
        {
            checkmarkAnimator.Play(checkMarkSelect);
            yield return new WaitForSeconds(checkmarkAnimator.GetCurrentAnimatorStateInfo(0).length);
            checkMarkGO.SetActive(true);
            fadeOutAnimator.Play("fadeOut");
            yield return new WaitForSeconds(fadeOutAnimator.GetCurrentAnimatorStateInfo(0).length);
            gameplayManager.DeleteSave();

            masterMiscController.LoadScene(MasterMiscController.Scenes.PreAlphaTest, true);
        }

        public void LoadGameOnClick()
        {
            
        }
    }
}
