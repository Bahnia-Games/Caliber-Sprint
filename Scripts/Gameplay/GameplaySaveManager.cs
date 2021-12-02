using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
//using System.Security.Cryptography;
using ACH_1_Demonstrator;
using UnityEngine;

namespace Assets.Git.Scripts.Gameplay
{
    /// <summary>
    /// Do not access directly.
    /// </summary>
    public class GameplaySaveManager // it would be funny if i switched this to ACH-1 istead of SHA-1 later on...... Just gotta get it working first lmao
    {

        private string dataPath;
        private string hashPath;

        private bool secondAttempt = false;

        private BinaryFormatter binaryFromatter = new BinaryFormatter();

        //public string dataPath;
        //internal string hashPath;

        public enum DataState
        {
            ok,
            notfound,
            corrupt,
            busy
        }

        public GameplaySaveManager(string dataPath, string hashPath)
        {
            //Debug.Log("Initializing a GameplaySaveManager instance with the data path: " + dataPath + " , and hash path : " + hashPath);
            this.dataPath = dataPath;
            this.hashPath = hashPath;
        }

        public (PlayerData data, DataState state) Load()
        {
            try
            {
                if (!File.Exists(dataPath) || !File.Exists(hashPath))
                    return (null, DataState.notfound);

                FileStream dataStream = new FileStream(dataPath, FileMode.Open);
                PlayerData data = (PlayerData)binaryFromatter.Deserialize(dataStream);
                string computeHash;
                using (ACH1 hasher = new ACH1(ACH1.InitType.stream))
                    computeHash = BitConverter.ToString(hasher.ComputeHash(dataStream));
                dataStream.Close();

                string readHash;
                using (BinaryReader reader = new BinaryReader(File.Open(hashPath, FileMode.Open)))
                    readHash = BitConverter.ToString(reader.ReadBytes(20));

                if (readHash == computeHash)
                    return (data, DataState.ok);
                else
                {
                    Debug.LogError("Checksum invalid (is the checksum or save corrupted?) @GameplaySaveManager.cs PlayerData Load()");
                    return (null, DataState.corrupt);
                }
            } catch (FileNotFoundException ex)
            {
                Debug.LogError("Player data or checksum not found. @GameplaySaveManager.cs PlayerData Load()");
                Debug.LogError(ex.ToString());
                return (null, DataState.notfound);
            }

            catch (Exception ex)
            {
                Debug.LogError("Unable to load player data. @GameplaySaveManager.cs PlayerData Load()");
                Debug.LogError(ex.ToString());
                return (null, DataState.corrupt);
            }
        }

        public void Save(PlayerData data, string dataPath, string hashPath)
        {
            try
            {
                if (File.Exists(dataPath))
                    File.Delete(dataPath);
                if (File.Exists(hashPath))
                    File.Delete(hashPath);

                FileStream dataStream = new FileStream(dataPath, FileMode.Create);
                binaryFromatter.Serialize(dataStream, data);
                using (ACH1 hasher = new ACH1(ACH1.InitType.stream))
                    using (BinaryWriter writer = new BinaryWriter(File.Open(hashPath, FileMode.Create)))
                        writer.Write(hasher.ComputeHash(dataStream));

                dataStream.Close();
                Debug.Log("Successfully saved @GameplaySaveManager.cs Save()");
            } catch (Exception ex)
            {
                if (!secondAttempt)
                {
                    Debug.LogError("Critical error! Could not save player data, making second attempt...");
                    secondAttempt = true;
                    Save(data, dataPath, hashPath);
                    return;
                } else
                {
                    Debug.LogError("Critical error! Could not save player data. @GameplaySaveManager Save()");
                    // error popup lol
                    Debug.LogError(ex.ToString());
                }
                
            }
        }

        /// <summary>
        /// DO NOT CALL THIS DIRECTLY
        /// </summary>
        internal void DeleteSave(string dataPath)
        {
            if (File.Exists(dataPath))
                File.Delete(dataPath);
        }

        /// <summary>
        /// DO NOT CALL THIS DIRECTLY
        /// </summary>
        internal void DeleteHash(string hashPath)
        {
            if (File.Exists(hashPath))
                File.Delete(hashPath);
        }
    }
}
