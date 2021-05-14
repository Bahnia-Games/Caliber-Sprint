using Assets.Git.Scripts;
using UnityEngine;
using Assets.Git.Scripts.Misc;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// NOTES:
    /// Buttons do not like static methods
    /// </summary>

    [SerializeField] private GameObject miscScriptContainer;
    public enum MainMenuTransitions { 
        /// <summary>
        /// Rules:
        /// 1. The name of the enum MUST be the name of the animation state.
        /// 2. NO SPACES!!!!!!!!!!!!!!
        /// 3. It is always TO, not FROM.
        /// </summary>
        Main, // main screen of the main menu
        Singleplayer, // the singleplayer menu
        Multiplayer,
        Settings,
        CustomVideoSettings

    }

    public void Save() => SaveManager.Save();

    public void QuitGame()
    {
        QuitEventArgs args = new QuitEventArgs();
        args.quitReason = QuitEventArgs.QuitReason.request;
        args.Requester = "MainMenu.cs QuitGame()";
        GetComponent<MasterMiscController>().InvokeApplicationQuitRequest(this, args); // bruh for some reason i cant invoke outside mmc.
    }
}
