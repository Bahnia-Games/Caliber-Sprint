using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSoundController : MonoBehaviour
{
    #region global variables

    private AudioSource audioSource;
    [SerializeField]
    private Camera camera;
    [Range(0.0f, 1.0f)]
    public float volume;

    public enum SoundType
    {
        open,
        closed
    }

    public enum Sound
    {
        fire,
        insertMag,
        removeMag,
        charge,
        hammer
    }

    public SoundType currentSoundTypeBasedOnEnvironment;
    #endregion

    #region audio clips
    [Header("Audio Clips")]

    [SerializeField]
    private AudioClip fireOpen;
    [SerializeField]
    private AudioClip fireClosed;
    [SerializeField]
    private AudioClip insertMagOpen;
    [SerializeField]
    private AudioClip insertMagClosed;
    [SerializeField]
    private AudioClip removeMagOpen;
    [SerializeField]
    private AudioClip removeMagClosed;
    [SerializeField]
    private AudioClip chargeOpen;
    [SerializeField]
    private AudioClip chargeClosed;
    [SerializeField]
    private AudioClip hammerOpen;
    [SerializeField]
    private AudioClip hammerClosed;

    #endregion
    private void Start()
    {
        if (EnvironmentController.environmentSoundType == EnvironmentController.SoundType.open)
            currentSoundTypeBasedOnEnvironment = SoundType.open;
        if (EnvironmentController.environmentSoundType == EnvironmentController.SoundType.closed)
            currentSoundTypeBasedOnEnvironment = SoundType.closed;
    }

    public void PlaySound(Sound sound, float volumeModifier = 0)
    {
        GameObject audioSourceHolder = Instantiate(new GameObject(), camera.transform);
        audioSourceHolder.transform.position = Vector3.zero;
        audioSource = audioSourceHolder.AddComponent<AudioSource>();
        

        if (volumeModifier != 0)
            audioSource.volume = volumeModifier;

        if (currentSoundTypeBasedOnEnvironment == SoundType.open)
        {
            if (sound == Sound.fire)
                audioSource.clip = fireOpen;
            if (sound == Sound.insertMag)
                audioSource.clip = insertMagOpen;
            if (sound == Sound.removeMag)
                audioSource.clip = removeMagOpen;
            if (sound == Sound.charge)
                audioSource.clip = chargeOpen;
            if (sound == Sound.hammer)
                audioSource.clip = hammerOpen;
        }
        if (currentSoundTypeBasedOnEnvironment == SoundType.closed)
        {
            if (sound == Sound.fire)
                audioSource.clip = fireClosed;
            if (sound == Sound.insertMag)
                audioSource.clip = insertMagClosed;
            if (sound == Sound.removeMag)
                audioSource.clip = removeMagClosed;
            if (sound == Sound.charge)
                audioSource.clip = chargeClosed;
            if (sound == Sound.hammer)
                audioSource.clip = hammerClosed;
        }

        if (audioSource.clip == null)
            return;
        audioSource.Play();
        Destroy(audioSourceHolder, 3);
    }
}
