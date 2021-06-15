using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Git.Scripts;
using TMPro;
using UnityEngine.UI;

namespace Assets.Git.Scripts.Menu
{
    public class AudioSettings : MonoBehaviour
    {
        #pragma warning disable IDE0049
        private float currentMasterVolume;
        [SerializeField] private TMP_Text masterVolumePercentageText;
        [SerializeField] private Slider masterVolumeSlider;
        private (object data, SaveManager.GetStatus status) loadedMasterVolume;

        private void Awake()
        {
            if ((loadedMasterVolume = SaveManager.Load(SaveManager.DataType.dfloat, "masterAudio")).status == SaveManager.GetStatus.success)
            {
                currentMasterVolume = (float)loadedMasterVolume.data;
                masterVolumeSlider.value = currentMasterVolume;
            }
            else
                masterVolumeSlider.value = 1;
        }

        /// <summary>
        /// Do not call. Button delegate only (NO REFERENCES)
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            currentMasterVolume = volume;
            float _val = volume * 100;
            int _volume = Int32.Parse(_val.ToString().Split('.')[0]);
            masterVolumePercentageText.text = _volume + "%";
        }
        /// <summary>
        /// Do not call. Button delegate only (NO REFERENCES)
        /// </summary>
        public void OnSaveClicked()
        {
            SaveManager.AddData("masterAudio", currentMasterVolume);
            SaveManager.Save();
        }
    }
}
