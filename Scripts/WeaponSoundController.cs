using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSoundController : MonoBehaviour
{
    #region global variables

    public AudioSource audioSource;
    [Range(0.0f, 1.0f)]
    public float volume;

    #endregion

    #region audio clips

    public AudioClip fire;
    public AudioClip insertMag;
    public AudioClip removeMag;
    public AudioClip charge;

    #endregion
    public void PlaySound(AudioClip audioClip, float volumeModifier = 0)
    {
        if (audioClip == null)
            return;

        if (volumeModifier != 0)
            audioSource.volume = volumeModifier;

        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
