using UnityEngine;

namespace Assets.Git.Scripts.Player.Audio
{
    public class AudioHandler
    {
        public static float MasterVolume { get; private set; }
        internal static void Init()
        {
            var data = SaveManager.Load(SaveManager.DataType.dfloat, "masterAudio");
            if (data.status == SaveManager.GetStatus.success)
                MasterVolume = (float)data.data;
            else
            {
                Debug.Log("Unable to load master audio volume. Falling back to default.");
                MasterVolume = 1;
            }
        }
        internal static void Refresh() => AudioListener.volume = MasterVolume;
        internal static void SaveVolume() { SaveManager.AddData("masterAudio", MasterVolume); SaveManager.Save(); }
        internal static void SetVolume(float v) { MasterVolume = v; AudioListener.volume = MasterVolume; }
    }
}
