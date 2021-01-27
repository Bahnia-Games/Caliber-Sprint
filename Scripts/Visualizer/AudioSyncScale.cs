using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncScale : AudioSyncer
{
    public Vector3 beatScale;
    public Vector3 resetScale;

    private IEnumerator MoveToScale(Vector3 _target)
    {
        Vector3 _current = transform.localScale;
        Vector3 _initial = _current;
        float timer = 0;

        while (_current != _target)
        {
            _current = Vector3.Lerp(_initial, _target, timer / timeToBeat);
            timer += Time.deltaTime;

            transform.localScale = _current;
            
            yield return null;
        }
        isBeat = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (isBeat)
            return;

        transform.localScale = Vector3.Lerp(transform.localScale, resetScale, resetSmoothTime * Time.deltaTime);
    }

    public override void OnBeat()
    {
        base.OnBeat();

        StopCoroutine("MoveToScale");
        StartCoroutine("MoveToScale", beatScale);
    }
}
