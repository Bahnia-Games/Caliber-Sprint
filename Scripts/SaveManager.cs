using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Git.Scripts
{
    class SaveManager : MonoBehaviour
    {
        public static void Save() => PlayerPrefs.Save();

        public static void AddData(string dataName, object data) // coming up with this genuinly hurt my brain :(
        {
            Type type = data.GetType();
            if (type == typeof(int))
                PlayerPrefs.SetInt(dataName, (int)data);
            else if (type == typeof(float))
                PlayerPrefs.SetFloat(dataName, (float)data);
            else if (type == typeof(string))
                PlayerPrefs.SetString(dataName, (string)data);
            else
            {
                try
                {
                    throw new ArgumentException();
                } catch (Exception ex)
                {
                    Debug.LogError("A class has tried to save an invalid data type to PlayerPrefs! @SaveManager L11");
                    Debug.Log(ex.ToString());
                }
            }
        }
    }
}
