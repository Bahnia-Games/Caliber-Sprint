using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Assets.Git.Scripts.Gameplay;
using Assets.Git.Scripts.Misc;

namespace Assets.Git.Scripts.Menu
{
    public class SingleplayerMenu : MonoBehaviour
    {
        [SerializeField] private MasterMiscController mmc;

        /// <summary>
        /// Essentially, element 0 is null
        /// Everything else counts up the index, 
        /// see summary below
        /// </summary>
        public Sprite[] chapterThumbnails;

        /// <summary>
        /// Chapter Summary (alpha):
        /// 0: Test scene 1
        /// 1: Chloe Firing Range
        /// 2: art cover 1
        /// 3: art cover  1
        /// 4: shader test 1
        /// 5: nul
        /// 6: nul
        /// 7: nul
        /// 8: nul
        /// 9: nul
        /// 
        /// Post alpha : (Due to spoilers, chapter names not listed)
        /// 0: Chapter 0: placeholder
        /// 1: Chapter 1: placeholder
        /// 2: Chapter 2: placeholder
        /// 3: Chapter 3: placeholder
        /// 4: Chapter 4: placeholder
        /// 5: Chapter 5: placeholder
        /// 6: Chapter 6: placeholder
        /// 7: Chapter 7: placeholder
        /// 8: Chapter 8: placeholder
        /// 9: Sandbox
        /// </summary>
        public string[] chapterTexts = new string[10];
        [SerializeField] private TMP_Text chapterText;

        [SerializeField] private Sprite nullSceneImage;
        [SerializeField] private Sprite nullSceneImage2;

        private int currentPage = 0;

        [SerializeField] private GameObject thumbnailContainer;
        private Image currentThumbnail;
        [SerializeField] private GameObject nextThumbnailContainer;
        private Image nextThumbnail;

        public enum PageSwitchType
        {
            left,
            right
        }

        [SerializeField] private Button rightButton;
        [SerializeField] private Button leftButton;

        [SerializeField] private Animator thumbnailAnimator;
        private readonly string nextThumb = "transitionNext";
        private readonly string prevThumb = "transitionPrevious";

        [SerializeField] private Animator checkmarkAnimator;
        private readonly string checkMarkSelect = "checkSelect";
        [SerializeField] private GameObject checkMarkGO;

        [SerializeField] private Animator fadeOutAnimator;
        [SerializeField] private GameObject fadeOutGO;

        private void Awake()
        {
            chapterText.text = chapterTexts[currentPage];

            currentThumbnail = thumbnailContainer.GetComponent<Image>();
            nextThumbnail = nextThumbnailContainer.GetComponent<Image>();
            OnPageUpdate();
            if (chapterThumbnails[0] != null)
                currentThumbnail.sprite = chapterThumbnails[0];
            else
                currentThumbnail.sprite = nullSceneImage;

            rightButton.onClick.AddListener(delegate {
                OnSwitchPage(PageSwitchType.right);
            });
            leftButton.onClick.AddListener(delegate {
                OnSwitchPage(PageSwitchType.left);
            });
        }
        public void OnSwitchPage(PageSwitchType pageSwitchType)
        {
            if (thumbnailAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle")) // this is the key to fixing the gun controller
                switch (pageSwitchType)
                {
                    case PageSwitchType.left:
                        currentPage--;
                        chapterText.text = chapterTexts[currentPage];
                        thumbnailAnimator.Play(prevThumb);
                        OnPageUpdate(); // this is in here so i can make the code look cooler, it wont make a difference.
                        StartCoroutine(SetImage(pageSwitchType));
                        break;
                    case PageSwitchType.right:
                        currentPage++;
                        chapterText.text = chapterTexts[currentPage];
                        thumbnailAnimator.Play(nextThumb);
                        OnPageUpdate();
                        StartCoroutine(SetImage(pageSwitchType));
                        break;
                }
        }
        private void OnPageUpdate()
        {
            if (currentPage == 0)
                leftButton.gameObject.SetActive(false);
            if (currentPage == chapterTexts.Length - 1)
                rightButton.gameObject.SetActive(false);
            if (currentPage > 0)
                leftButton.gameObject.SetActive(true);
            if (currentPage < chapterTexts.Length - 1)
                rightButton.gameObject.SetActive(true);
        }

        private IEnumerator SetImage(PageSwitchType pageSwitchType)
        {
            if (chapterThumbnails[0] == null)
            {
                Debug.LogError("No chapter thumbnails! @SinglePlayerMenu.cs");
                yield return null;
            }

            if (chapterThumbnails[currentPage] != null)
                nextThumbnail.sprite = chapterThumbnails[currentPage];
            else
            {
                currentThumbnail.sprite = nullSceneImage;
                nextThumbnail.sprite = nullSceneImage2;
            }

            switch (pageSwitchType)
            {
                case PageSwitchType.right:
                    currentThumbnail.sprite = chapterThumbnails[currentPage - 1];
                    break;
                case PageSwitchType.left:
                    currentThumbnail.sprite = chapterThumbnails[currentPage + 1];
                    break;
            }

            yield return new WaitForSeconds(0.5f); // GetCurrentAnimatorStateInfo(0).length gives 0.5f but it wont yield for that amount of time????
            currentThumbnail.sprite = chapterThumbnails[currentPage];
        }

        public void ChapterOnClick() => StartCoroutine(_ChapterOnClick());

        private IEnumerator _ChapterOnClick() // bug here is minor, and for testing purposes is not important to fix. In the final game, it will not exist
        {
            checkmarkAnimator.Play(checkMarkSelect);
            yield return new WaitForSeconds(checkmarkAnimator.GetCurrentAnimatorStateInfo(0).length);
            checkMarkGO.SetActive(true);
            fadeOutAnimator.Play("fadeOut");
            yield return new WaitForSeconds(fadeOutAnimator.GetCurrentAnimatorStateInfo(0).length);
            Debug.Log(currentPage);
            switch (currentPage)
            {
                case 0:
                    mmc.LoadScene(MasterMiscController.Scenes.PreAlphaTest, true);
                    break;
                case 1:
                    mmc.LoadScene(MasterMiscController.Scenes.FiringRangeTest, true);
                    break;
                case 2:
                    mmc.LoadScene(MasterMiscController.Scenes.artcover, true);
                    break;
                case 3:
                    mmc.LoadScene(MasterMiscController.Scenes.artcover2, true);

                    break;
                case 4:
                    mmc.LoadScene(MasterMiscController.Scenes.ShaderTest, true);
                    break;
            }
        }
    }
}
