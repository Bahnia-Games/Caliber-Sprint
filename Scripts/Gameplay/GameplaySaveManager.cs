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
    /// <summary>
    /// Do not access directly.
    /// </summary>
    public class GameplaySaveManager
    {
        private bool secondAttempt = false;

        private BinaryFormatter binaryFromatter = new BinaryFormatter();

        //public string dataPath;
        //internal string hashPath;
        private enum State { 
            save,
            load,
            none
        }

        public enum DataState
        {
            ok,
            notfound,
            corrupt,
            busy
        }

        private State state = State.none;

        public (PlayerData data, DataState) Load(string dataPath, string hashPath)
        {
            try
            {
                if (state != State.save || state == State.none)
                {
                    if (!File.Exists(dataPath))
                        return (null, DataState.notfound);


                    state = State.load;
                    FileStream dataStream = new FileStream(dataPath, FileMode.Open);
                    PlayerData data = (PlayerData)binaryFromatter.Deserialize(dataStream);
                    string computeHash;
                    using (SHA1 hasher = SHA1.Create())
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
                }
                else
                {
                    Debug.LogError("Cannot load because savemanager is in another state. @GameplaySaveManager.cs PlayerData Load()");
                    return (null, DataState.busy);
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
                if (state != State.load || state == State.none)
                {
                    state = State.save;
                    if (File.Exists(dataPath))
                        File.Delete(dataPath);
                    if (File.Exists(hashPath))
                        File.Delete(hashPath);

                    FileStream dataStream = new FileStream(dataPath, FileMode.Create);
                    binaryFromatter.Serialize(dataStream, data);
                    using (SHA1 hasher = SHA1.Create())
                    {
                        using (BinaryWriter writer = new BinaryWriter(File.Open(hashPath, FileMode.Create)))
                            writer.Write(hasher.ComputeHash(dataStream));
                    }

                    dataStream.Close();
                    Debug.Log("Successfully saved @GameplaySaveManager.cs Save()");
                }
                else
                {
                    Debug.LogError("Cannot save because savemanager is in another state. @GameplaySaveManager.cs Save()");
                }
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
