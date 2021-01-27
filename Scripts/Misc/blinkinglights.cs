using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blinkinglights : MonoBehaviour
{
    public enum BlinkType
    {
        steady,
        flashtwice
    }

    public BlinkType blinkType;

    public float blinkDelaySteady;
    public float blinkDelayFlash;

    public Light _light;

    private bool flashReady;

    public void Awake()
    {
        _light.enabled = false;
        flashReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (flashReady)
            StartCoroutine(Blink(blinkType));
    }

    private IEnumerator Blink(BlinkType blinkType)
    {
        flashReady = false;
        if (blinkType == BlinkType.flashtwice)
        {
            _light.enabled = true;
            yield return new WaitForSeconds(blinkDelayFlash);
            _light.enabled = false;
            yield return new WaitForSeconds(blinkDelayFlash);
            _light.enabled = true;
            yield return new WaitForSeconds(blinkDelayFlash);
            _light.enabled = false;
            yield return new WaitForSeconds(blinkDelaySteady);
            flashReady = true;
        }
        if (blinkType == BlinkType.steady)
        {
            _light.enabled = true;
            yield return new WaitForSeconds(blinkDelaySteady);
            _light.enabled = false;
            yield return new WaitForSeconds(blinkDelaySteady);
            flashReady = true;
        }
    }
}
