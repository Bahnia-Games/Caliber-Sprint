using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncer : MonoBehaviour
{
    public float bias;
    public float timeStep;
    public float timeToBeat;
    public float resetSmoothTime;

    private float previousAudioValue;
    private float _audioValue;
    private float _timer;

    protected bool isBeat;

    void Update()
    {
        OnUpdate();
    }

    public virtual void OnUpdate()
    {
        previousAudioValue = _audioValue;
        _audioValue = AudioSpectrum.spectrumValue;

        if (previousAudioValue > bias && _audioValue <= bias)
            if (_timer > timeStep)
                OnBeat();

        if (previousAudioValue <= bias && _audioValue > bias)
            if (_timer > timeStep)
                OnBeat();

        _timer += Time.deltaTime;
    }

    public virtual void OnBeat()
    {
        _timer = 0;
        isBeat = true;
    }
}
