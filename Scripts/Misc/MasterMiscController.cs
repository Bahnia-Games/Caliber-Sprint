using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterMiscController : MonoBehaviour
{
    public static bool drpEnabled;
    private void Start()
    {
        Debug.developerConsoleVisible = true;
        EnvironmentController.environmentSoundType = EnvironmentController.SoundType.closed;
    }
}
