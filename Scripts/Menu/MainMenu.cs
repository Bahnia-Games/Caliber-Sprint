﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// NOTES:
    /// Buttons do not like static methods
    /// </summary>
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

        // chapters

        // multiplayer maps
    }

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
        Settings

    }

    public void LoadScene(Scenes sceneName)
    {
        string _sceneName = sceneName.ToString();
        foreach (char _char in _sceneName)
        {
            if(_char == '_')
            {
                string[] __sceneName = new string[2]; // this will hold split data
                __sceneName = _sceneName.Split('_'); // split at underscore
                _sceneName = __sceneName[0] + ' ' + __sceneName[1]; // add a space to the string
            }
                
        }
        Debug.Log("Attempting to load scene: " + _sceneName);
        // loading screen???
        //SceneManager.LoadSceneAsync(_sceneName); // async for loading screen
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.LogWarning("Application Has Quit!");
    }
}
