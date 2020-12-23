using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallisticsTracker : MonoBehaviour
{
    public Transform rangePoint;
    public TextMeshProUGUI zeroText;
    private int zero;
    public TextMeshProUGUI rangeText;
    private float range;
    private bool _enabled;
    private RaycastHit hit;
    [SerializeField] private float scanPhsDel;
    private bool scanFireOnce;
    private bool activeScan;
    private string _s;
    // Start is called before the first frame update
    void Start()
    {
        BalTRK(true);
        zero = 0;
        zeroText.text = zero + "m";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_enabled)
        {
            Physics.Raycast(rangePoint.position, rangePoint.forward, out hit, Mathf.Infinity);
            Debug.DrawRay(rangePoint.position, rangePoint.forward, Color.red);
            if(hit.collider != null)
                StartCoroutine(Track(false));
            if (hit.collider == null)
            {
                Debug.Log("GO null");
                Track(true);
                activeScan = false;
                //scanFireOnce = true;
            }
                
            if (activeScan)
            {
                range = hit.distance;
                int _i = (int)range;
                string _s = _i.ToString();
                rangeText.text = _s + "m";
            }
        }
    }

    private IEnumerator Track(bool isBroken)
    {
        if (isBroken)
        {
            Debug.Log("Breaking...");
            yield break;
        } else
        {
            range = hit.distance;
            int _i = (int)range;
            string _s = _i.ToString();
        }
        if (!isBroken && scanFireOnce)
        {
            rangeText.text = "Scanning.";
            yield return new WaitForSeconds(scanPhsDel);
            rangeText.text = "Scanning..";
            yield return new WaitForSeconds(scanPhsDel);
            rangeText.text = "Scanning...";
            yield return new WaitForSeconds(scanPhsDel);
            rangeText.text = _s + "m";
            scanFireOnce = false;
            activeScan = true;
        }
        
    }

    public void BalTRK(bool enabled)
    {
        _enabled = enabled;
    }
}
