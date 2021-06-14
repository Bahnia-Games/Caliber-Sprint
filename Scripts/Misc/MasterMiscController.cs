using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Git.Scripts.Gameplay;
using System;
using Assets.Git.Scripts.Player.Inputs;

namespace Assets.Git.Scripts.Misc
{
    public class MasterMiscController : MonoBehaviour
    {
        public static bool drpEnabled;

        [HideInInspector] public float masterAudio;

        [Header("Menu Stuff")]
        public GameObject canvas;
        public GameObject loadingObject;
        public GameObject introSequenceContainer;

        public GameplayManager gameplayManager; // per scene. make sure none already preexist

        [Header("Misc")]
        [SerializeField] private bool visualDebugEnabled = false;
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

            FiringRangeTest,

            ShaderTest,

            // chapters

            // multiplayer maps
        }

        private void Awake()
        {
            VisualDebugger.visualDebuggerEnabled = visualDebugEnabled;
        }

        private void Start()
        {
            #region general loading

            gameplayManager = new GameplayManager();
            ApplicationQuitRequest += OnApplicationRequestQuit;

            #endregion

            #region scene initialization

            EnvironmentController.environmentSoundType = EnvironmentController.SoundType.closed;

            #endregion

            #region load and handle control scheme

            if ((int)SaveManager.Load(SaveManager.DataType.dint, "LGO").data == 0) // game has never been launched, 1 = launched
            {
                Debug.Log("Game is launching for the first time. Assigning default settings...");
                SaveManager.AddData("LGO", 1);
                InputHandler.AssignDefaults();
                InputHandler.Save();
            }

            InputHandler.Status status = InputHandler.Load();
            if (status == InputHandler.Status.ok)
                Debug.Log("Found control scheme.");
            else if (status == InputHandler.Status.notfound)
            {
                Debug.LogWarning("Control scheme not found, falling back to defaults...");
                InputHandler.AssignDefaults();
            }
            else if (status == InputHandler.Status.noparse)
            {
                Debug.LogWarning("Control scheme could not be parsed correctly, falling back to defaults...");
                InputHandler.AssignDefaults();
            }

            #endregion
        }
        private void OnEnable() => SceneManager.sceneLoaded += SetAduio;

        public void LoadScene(Scenes sceneName, bool useLoading = false)
        {
            bool moveAlong = true;
            if (!useLoading)
                moveAlong = false;
            if (useLoading && moveAlong)
                loadingObject.SetActive(true);

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

        private IEnumerator LoadAsynchronously(string sceneName)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName); // async for loading screen
            while (!operation.isDone) // congrats on being the frst while loop in cs
                yield return new WaitForEndOfFrame();
        }

        private void SetAduio(Scene unused, LoadSceneMode _unused)
        {
            (object data, SaveManager.GetStatus status) audio = SaveManager.Load(SaveManager.DataType.dfloat, "masterAudio");
            if (audio.status == SaveManager.GetStatus.success)
            {
            }
            else
            {
                masterAudio = 1.0f; AudioListener.volume = masterAudio;
                Debug.LogWarning("Data for master audio could not be found, defaulting to 100% @MasterMiscController SetAudio()");
            }
        }

        #region application quit handler

        public static event EventHandler<QuitEventArgs> ApplicationQuitRequest;
        public void InvokeApplicationQuitRequest(object sender, QuitEventArgs args) => ApplicationQuitRequest.Invoke(sender, args);
        private void OnApplicationRequestQuit(object sender, QuitEventArgs args)
        {
            // stuff
            Debug.LogWarning("Quit request recieved.");
            StartCoroutine(ApplicationQuitAtEndOfFrame());
        }

        private IEnumerator ApplicationQuitAtEndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            Debug.LogWarning("Quitting...");
            Application.Quit();
        }

        #endregion
    }
}