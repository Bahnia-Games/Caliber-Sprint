using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Git.Scripts;

public class MasterMiscController : MonoBehaviour
{
    public static bool drpEnabled;

    [HideInInspector ]public float masterAudio;

    [Header("Menu Stuff")]
    public GameObject canvas;
    private Color canvasImagePreviousColor;
    public GameObject loadingObject;
    public GameObject introSequenceContainer;

    public enum Scenes
    {
        /// <summary>
        /// ok so let me establish rules here
        /// 
        /// 1. A scene in here must be named EXACTLY like the actual scene name.
        /// 2. A scene must have no more than 1 space in the name.
        /// 3. A scene that does have a space must use an underscore instead of a space.
        /// 4. A scene without a space in the name MUST NOT have an underscore in the name.
        /// 
        /// if more rules come up ill add them later...
        /// </summary>


        nul,
        // menus
        PreMenu, // the cool intro stuff
        MainMenu, // main menu

        //misc
        DRPHIDDEN,
        Teaser_Cinemachine,

        // artwork scenes
        artcover,
        artcover2,

        // test scenes

        PreAlphaTest,

        // chapters

        // multiplayer maps
    }

    private void Start() => EnvironmentController.environmentSoundType = EnvironmentController.SoundType.closed;
    private void OnEnable() => SceneManager.sceneLoaded += SetAduio;

    /// <summary>
    /// Argument summary:
    /// -useloading = use loading screen
    /// 
    /// </summary>
    /// <param name="args"></param>
    public void LoadScene(Scenes sceneName, string arg = null)
    {
        bool moveAlong = true;
        if (arg == null)
            moveAlong = false;
        if (arg == "-useloading" && moveAlong)
            loadingObject.SetActive(true);
        else if (arg != null && moveAlong)
            Debug.LogWarning("Invalid arguments! @MasterMiscController.Loadscene(sceneName, args)");

        if (introSequenceContainer != null)
            introSequenceContainer.SetActive(false);

        string _sceneName = sceneName.ToString();
        foreach (char _char in _sceneName)
            if (_char == '_')
            {
                string[] __sceneName = new string[2]; // this will hold split data
                __sceneName = _sceneName.Split('_'); // split at underscore
                _sceneName = __sceneName[0] + ' ' + __sceneName[1]; // add a space to the string
            }
        Debug.Log("Attempting to load scene: " + _sceneName);

        StartCoroutine(LoadAsynchronously(_sceneName));
    }

    private IEnumerator LoadAsynchronously (string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName); // async for loading screen
        while (!operation.isDone) // congrats on being the frst while loop in cs
            yield return new WaitForEndOfFrame();
    }

    private void SetAduio(Scene unused, LoadSceneMode _unused) 
    {
        (object data, SaveManager.GetStatus status) audio = SaveManager.Load(SaveManager.DataType.dfloat, "masterAudio");
        if (audio.status == SaveManager.GetStatus.success) {
            masterAudio = (float)audio.data; AudioListener.volume = masterAudio; 
        } else 
        { 
            masterAudio = 1.0f; AudioListener.volume = masterAudio;
            Debug.LogWarning("Data for master audio could not be found, defaulting to 100% @MasterMiscController SetAudio()");
        }
    }
}
