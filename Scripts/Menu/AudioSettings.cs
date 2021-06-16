using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Git.Scripts.Player.Audio;
using TMPro;
using UnityEngine.UI;

namespace Assets.Git.Scripts.Menu
{
    public class AudioSettings : MonoBehaviour
    {
        #pragma warning disable IDE0049

        [SerializeField] private TMP_Text masterVolumePercentageText;
        [SerializeField] private Slider masterVolumeSlider;

        private float currentMasterVolume;

        private void Awake() => masterVolumeSlider.value = AudioHandler.MasterVolume;

        /// <summary>
        /// Do not call. Slider delegate only (NO REFERENCES)
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            currentMasterVolume = volume;
            float _val = volume * 100;
            int _volume = Int32.Parse(_val.ToString().Split('.')[0]);
            masterVolumePercentageText.text = _volume + "%";
            AudioHandler.SetVolume(currentMasterVolume);
        }
        /// <summary>
        /// Do not call. Button delegate only (NO REFERENCES)
        /// </summary>
        public void OnSaveClicked() => AudioHandler.SaveVolume();
    }
}
