using Assets.Git.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// NOTES:
    /// Buttons do not like static methods
    /// </summary>

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
        Application.Quit();
        Debug.LogWarning("Application Has Quit!");
    }
}
