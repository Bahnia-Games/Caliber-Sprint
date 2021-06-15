using Assets.Git.Scripts.Misc;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Assets.Git.Scripts.Menu
{
    public class IntroSequenceController : MonoBehaviour // for some odd reason, this cant be one line, so sad. Goodbye meme script :Sadde:
    { 
        //causes nonfatal missing reference exception
        [SerializeField] private Animator animator;
        [SerializeField] private string introClip;
        [SerializeField] private float sequenceTime=1.0f;
        [SerializeField] private MasterMiscController mmc;
        private void OnEnable() => SceneManager.sceneLoaded += OnLoadScene;
        private void OnLoadScene(Scene unused,LoadSceneMode _unused) => StartCoroutine(WaitAndPlay());
        private IEnumerator WaitAndPlay()
        {
            Debug.Log("Starting Caliber Sprint, Auchefansen! Ignore the next missing reference exception @IntroSequenceController.cs Cocoutine WaitAndPlay() L:16");
            yield return new WaitForEndOfFrame();animator.Play(introClip);
            yield return new WaitForSeconds(sequenceTime);
            mmc.LoadScene(MasterMiscController.Scenes.MainMenu, true);
        }
    }
}