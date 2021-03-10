using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Git.Scripts
{
    class SaveManager : MonoBehaviour
    {
        public static void Save() => PlayerPrefs.Save();
    }
}
