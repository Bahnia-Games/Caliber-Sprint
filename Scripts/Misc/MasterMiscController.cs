using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterMiscController : MonoBehaviour
{
    public static bool drpEnabled;
    public bool visualDebugEnabled;
    public GameObject visualDebugContainer;
    private void Start()
    {
        Debug.developerConsoleVisible = true;
        EnvironmentController.environmentSoundType = EnvironmentController.SoundType.closed;
    }
    public void Awake()
    {
        if (visualDebugEnabled)
        {
            visualDebugContainer.SetActive(true);
            VisualDebugger.visualDebuggerEnabled = true;
        }
    }
}
