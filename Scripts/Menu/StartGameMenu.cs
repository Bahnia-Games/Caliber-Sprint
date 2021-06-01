using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Git.Scripts.Gameplay;
using System.Collections;
using UnityEngine.UI;
using Assets.Git.Scripts.Misc;

namespace Assets.Git.Scripts.Menu
{
    public class StartGameMenu : MonoBehaviour
    {
        [SerializeField] MasterMiscController masterMiscController;
        [SerializeField] Animator checkmarkAnimator;
        [SerializeField] Animator fadeOutAnimator;
        private readonly string checkMarkSelect = "checkSelect";
        [SerializeField] private GameObject checkMarkGO;

        [SerializeField] private Button continueGameButton;

        private GameplayManager gameplayManager;

        public void Start()
        {
            if (masterMiscController != null)
                gameplayManager = masterMiscController.gameplayManager;

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
            new GameplayManager().DeleteSave();
            new GameplayManager().DeleteChecksum();

            masterMiscController.LoadScene(MasterMiscController.Scenes.ShaderTest, true);
        }

        public void LoadGameOnClick()
        {
            
        }
    }
}
