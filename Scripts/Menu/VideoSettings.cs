using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

namespace Assets.Git.Scripts.Menu
{
    public class VideoSettings : MonoBehaviour // ok so this entire class is dogshit but as much as i tried to simplify it it just did not work. because of this, for anyone considering using the code I insist that you dont.
    {
        #pragma warning disable IDE1006

        [SerializeField] private GameObject CustomButton;

        // ok i find it stupid that I have to add a listener myself
        [SerializeField] private TMP_Dropdown[] dropdowns; // this doesnt fucking work on its own so im splitting it up;

        /// <summary>
        /// Indexes
        /// 0. Quality Presets
        /// 1. Texture Quality
        /// 2. Anisotrophic Textures
        /// ...
        /// </summary>

        private enum TargetSetting
        {
            qualityPreset,
            textureQuality,
            anisotrophicTextureQuality,
            vsycnc,
            realtimeReflections
        }

        /// <summary>
        /// index 0 - very low
        /// index 1 - low
        /// index 2 - medium
        /// index 3 - high
        /// index 4 - very high
        /// index 5 - ultra
        /// index 6 - custom
        /// </summary>
        private TMP_Dropdown qualityPresetsDD;
        private int cQualityPreset;

        private TMP_Dropdown textureQualityDD;
        private int cTextureQuality;

        private TMP_Dropdown anisotrophicTextureQualityDD;
        private int cAnisotrophicTextureQuality;

        private TMP_Dropdown vsyncDD;
        private int cVsync;

        private TMP_Dropdown realtimeReflectionsDD;
        private int cRealtimeReflections;

        #region playerperfs saving stuff
        // nate add stuff here later k thx bye


        #endregion

        private void Start()
        {
            qualityPresetsDD = dropdowns[0];
            textureQualityDD = dropdowns[1];
            anisotrophicTextureQualityDD = dropdowns[2];
            vsyncDD = dropdowns[3];
            realtimeReflectionsDD = dropdowns[4];

            qualityPresetsDD.onValueChanged.AddListener(delegate {
                HandleInputData(TargetSetting.qualityPreset, qualityPresetsDD.value);
                AssignGraphicsQuality();
            });
            textureQualityDD.onValueChanged.AddListener(delegate { 
                HandleInputData(TargetSetting.textureQuality, textureQualityDD.value);
                AssignTextureQuality();
            });
            anisotrophicTextureQualityDD.onValueChanged.AddListener(delegate { 
                HandleInputData(TargetSetting.anisotrophicTextureQuality, anisotrophicTextureQualityDD.value);
                AssignAnisotrophicTextureQuality();
            });
            vsyncDD.onValueChanged.AddListener(delegate {
                HandleInputData(TargetSetting.vsycnc, vsyncDD.value);
                AssignVsync();
            });
            realtimeReflectionsDD.onValueChanged.AddListener(delegate {
                HandleInputData(TargetSetting.realtimeReflections, realtimeReflectionsDD.value);
                AssignRealtimeReflections();
            });

            // restore dropdowns
            LoadPrefs();
            qualityPresetsDD.value = cQualityPreset;
            textureQualityDD.value = cTextureQuality;
            anisotrophicTextureQualityDD.value = cAnisotrophicTextureQuality;
            vsyncDD.value = cVsync;
            realtimeReflectionsDD.value = cRealtimeReflections;

            // preassign settings based on playerprefs
            if (cQualityPreset == 6)
            {
                AssignGraphicsQuality();
                AssignTextureQuality();
                AssignAnisotrophicTextureQuality();
                AssignVsync();
            }
            else
                AssignGraphicsQuality();
        }

        private void HandleInputData(TargetSetting targetSetting, int index)
        { 
            switch (targetSetting)
            {
                case TargetSetting.qualityPreset:
                    cQualityPreset = index;
                    break;
                case TargetSetting.textureQuality:
                    cTextureQuality = index;
                    break;
                case TargetSetting.anisotrophicTextureQuality:
                    cAnisotrophicTextureQuality = index;
                    break;
                case TargetSetting.vsycnc:
                    cVsync = index;
                    break;
                case TargetSetting.realtimeReflections:
                    cRealtimeReflections = index;
                    break;
            }
        }
        /// <summary>
        /// Do not call. Button delegate only (NO REFERENCES)
        /// </summary>
        public void SaveOnClick()
        {
            AssignPlayerPrefs();
            if (cQualityPreset == 6) // force settings on save
            {
                AssignTextureQuality();
                AssignAnisotrophicTextureQuality();
                AssignVsync();
            }
            SaveManager.Save();
        }

        private void AssignPlayerPrefs()
        {
            SaveManager.AddData("qualityPreset", cQualityPreset);
            SaveManager.AddData("textureQuality", cTextureQuality);
            SaveManager.AddData("anisTextureQuality", cAnisotrophicTextureQuality);
            SaveManager.AddData("vSync", cVsync);
            SaveManager.AddData("realtimeReflections", cRealtimeReflections);
        }
        private void LoadPrefs()
        {
            cQualityPreset = (int)SaveManager.Load(SaveManager.DataType.dint, "qualityPreset").data;
            cTextureQuality = (int)SaveManager.Load(SaveManager.DataType.dint, "textureQuality").data;
            cAnisotrophicTextureQuality = (int)SaveManager.Load(SaveManager.DataType.dint, "anisTextureQuality").data;
            cVsync = (int)SaveManager.Load(SaveManager.DataType.dint, "vSync").data;
            cRealtimeReflections = (int)SaveManager.Load(SaveManager.DataType.dint, "realtimeReflections").data;
        }
        private void AssignGraphicsQuality()
        {
            if (cQualityPreset == 6) {// custom preset
                CustomButton.SetActive(true); return; }
            else
                CustomButton.SetActive(false);

            QualitySettings.SetQualityLevel(cQualityPreset);
        }
        private void AssignTextureQuality()
        {
            switch (cTextureQuality) // simplify later
            {
                case 3: // full res
                    QualitySettings.masterTextureLimit = 0; // ful res as per qualitysettings
                    break;
                case 2:
                    QualitySettings.masterTextureLimit = 1;
                    break;
                case 1:
                    QualitySettings.masterTextureLimit = 2;
                    break;
                case 0: // eigth res
                    QualitySettings.masterTextureLimit = 3;
                    break; // eigth res
            }
        }

        private void AssignAnisotrophicTextureQuality()
        {
            switch (cAnisotrophicTextureQuality)
            {
                case 2: 
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                    break;
                case 1:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                    break;
                case 0:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                    break;
            }
        }

        private void AssignVsync()
        {
            switch (cVsync)
            {
                case 0:
                    QualitySettings.vSyncCount = 0;
                    break;
                case 1:
                    QualitySettings.vSyncCount = 2;
                    break;
                case 2:
                    QualitySettings.vSyncCount = 1;
                    break;
            }
        }

        private void AssignRealtimeReflections()
        {
            switch (cRealtimeReflections)
            {
                case 0:
                    QualitySettings.realtimeReflectionProbes = false;
                    break;
                case 1:
                    QualitySettings.realtimeReflectionProbes = true;
                    break;
            }
        }
    }
}
