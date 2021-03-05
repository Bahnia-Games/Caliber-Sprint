using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualDebugger : MonoBehaviour
{
    [SerializeField] private static Transform[] debugSlot = new Transform[12];
    [SerializeField] private static Text[] debugText = new Text[12];
    public static bool visualDebuggerEnabled = false;

    void Awake()
    {
        if (visualDebuggerEnabled)
        {
            for (int i = 1; i <= 12; i++)
            {
                string _name = "debug slot (" + i + ")";
                Debug.Log(i - 1);
                GameObject _go = GameObject.Find(_name);
                debugSlot[i - 1] = _go.GetComponent<Transform>();
            }
        }
    }

    public static void InitLog(string variableName, int index)
    {
        if (visualDebuggerEnabled)
        {
            try
            {
                debugText[index - 1] = debugSlot[index - 1].gameObject.GetComponent<Text>();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        } else if (!visualDebuggerEnabled)
            Debug.LogWarning("Unable to assign variable to visual debugger, it is currently disabled on this scene.");
    }

    public static void Log(string variableName, int index, object data)
    {
        if (visualDebuggerEnabled)
            debugText[index - 1].text = variableName + ": " + data.ToString();
        else if (!visualDebuggerEnabled)
            Debug.LogWarning("Unable to assign variable to visual debugger, it is currently disabled on this scene.");
        
    }
}
