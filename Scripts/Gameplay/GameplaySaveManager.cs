using Assets.Git.Scripts.Gameplay;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Security.Cryptography;

namespace Assets.Git.Scripts.Gameplay
{
    public class GameplaySaveManager // switch to reference only
    {
        ///<summary>
        /// DO NOT ACCESS THIS CLASS DIRECTLY OR I WILL SHOOT YOU
        /// </summary>

        private string checksum;
        private int checksumMod;

        private bool secondAttempt = false;

        private BinaryFormatter binaryFromatter = new BinaryFormatter();

        private string dataPath = Application.persistentDataPath + "/main.csplayerdata";

        private System.Random random = new System.Random();

        private enum State { 
            save,
            load,
            none
        }

        private State state = State.none;

        public PlayerData Load()
        {
            try
            {
                if (state != State.save || state == State.none)
                {
                    state = State.load;
                    (object data, SaveManager.GetStatus status) checksumModLoad = SaveManager.Load(SaveManager.DataType.dint, "checksumMod");

                    if (checksumModLoad.status == SaveManager.GetStatus.notfound)
                    {
                        Debug.LogError("Unable to get checksum randomizer @GameplaySaveManager.cs PlayerData Load()");
                        return null;
                    }
                    checksumMod = (int)checksumModLoad.data;

                    FileStream dataStream = new FileStream(dataPath, FileMode.Open);

                    string sum;
                    using (var md5 = MD5.Create())
                    {
                        var hash = md5.ComputeHash(dataStream);
                        sum = BitConverter.ToString(hash).Replace("-", checksumMod.ToString());
                    }
                    if (sum != checksum)
                    {
                        Debug.LogError("Checksum for save data invalid! @GameplaySaveManager.cs PlayerData Load()");
                        return null;
                    }

                    PlayerData data = (PlayerData)binaryFromatter.Deserialize(dataStream);
                    dataStream.Close();
                    state = State.none;

                    return data;
                } else
                {
                    Debug.LogError("@GameplaySaveManager PlayerData Load() cannot load because savemanager is in another state.");
                    return null;
                }
            } catch (Exception ex)
            {
                Debug.LogError("Unable to load player data. @GameplaySaveManager.cs PlayerData Load()");
                Debug.LogError(ex.ToString());
                return null;
            }
        }

        public void Save(PlayerData data)
        {
            try
            {
                if (state != State.load || state == State.none)
                {
                    state = State.save;
                    if (File.Exists(dataPath))
                        File.Delete(dataPath);

                    FileStream dataStream = new FileStream(dataPath, FileMode.Create);
                    binaryFromatter.Serialize(dataStream, data);

                    checksum = GetChecksum(dataStream);
                    checksumMod = 0;
                    dataStream.Close();
                    state = State.none;
                } else
                {
                    Debug.LogError("@GameplaySaveManager Save() cannot save because savemanager is in another state.");
                }
            } catch (Exception ex)
            {
                if (!secondAttempt)
                {
                    Debug.LogError("Critical error! Could not save player data, making second attempt...");
                    secondAttempt = true;
                    Save(data);
                    return;
                } else
                {
                    Debug.LogError("Critical error! Could not save player data. @GameplaySaveManager Save()");
                    // error popup lol
                    Debug.LogError(ex.ToString());
                }
                
            }
        }

        private string GetChecksum(FileStream stream)
        {
            try
            {
                using (var md5 = MD5.Create())
                {
                    checksumMod = random.Next(0, 100);
                    SaveManager.AddData("checksumMod", checksumMod);
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", checksumMod.ToString());
                }
            } catch (Exception ex)
            {
                Debug.LogError("Could not calculate checksum. @GameplaySaveManager string GetChecksum()");
                Debug.LogError(ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// DO NOT CALL THIS DIRECTLY
        /// </summary>
        public void DeleteSave()
        {
            if (File.Exists(dataPath))
                File.Delete(dataPath);
        }
    }
}
