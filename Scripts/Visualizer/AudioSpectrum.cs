using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpectrum : MonoBehaviour
{
    private float[] _audioSpectrum;

    public float spectrumMultiplier = 100;
    
    public static float spectrumValue { get; private set; }

    void Start()
    {
        _audioSpectrum = new float[256];
    }

    void Update()
    {
        AudioListener.GetSpectrumData(_audioSpectrum, 0, FFTWindow.Hamming);

        if (_audioSpectrum != null && _audioSpectrum.Length > 0)
            spectrumValue = _audioSpectrum[0] * spectrumMultiplier;
    }
}
