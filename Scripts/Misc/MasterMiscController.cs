using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterMiscController : MonoBehaviour
{
    public static bool drpEnabled;
    private void Start()
    {
        EnvironmentController.environmentSoundType = EnvironmentController.SoundType.closed;
    }
    public void Awake()
    {

    }
}
