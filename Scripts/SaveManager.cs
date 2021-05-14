using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Git.Scripts
{
    /// <summary>
    /// This is for playerprefs
    /// </summary>
    public class SaveManager : MonoBehaviour
    {
        
        public enum DataType
        {
            dstring,
            dint,
            dfloat
        }
        public enum GetStatus
        {
            success,
            notfound
        }
        public static void Save() => PlayerPrefs.Save();

        public static (object data, GetStatus status) Load(DataType dataType, string key)
        {
            object data = null;
            if(dataType == DataType.dstring) 
                data = PlayerPrefs.GetString(key);
            if (dataType == DataType.dint)
                data = PlayerPrefs.GetInt(key);
            if (dataType == DataType.dfloat)
                data = PlayerPrefs.GetFloat(key);

            if (data == null) { 
                Debug.LogError("Critical error! Potential data/key mismatch. @SaveManager.Load(dataType, key)"); return (null, GetStatus.notfound); }
                
            return (data, GetStatus.success);
        }

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
                try
                {
                    throw new ArgumentException();
                }
                catch (Exception ex)
                {
                    Debug.LogError("A class has tried to save an invalid data type to PlayerPrefs! @SaveManager.AddData(dataName, data)");
                    Debug.Log(ex.ToString());
                }
        }
    }
}
