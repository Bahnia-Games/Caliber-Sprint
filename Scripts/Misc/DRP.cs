using UnityEngine;
using DiscordPresence;
using System;

public class DRP : MonoBehaviour
{

    [SerializeField]
    private string detail;
    [SerializeField]
    private string state;
    private long elapsedTime;

    private void Start()
    {
        elapsedTime = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        PresenceManager.UpdatePresence(detail, state, elapsedTime);
    }
    public void UpdateState(string _state, string _detail = null)
    {
        if (_detail == null)
        {
            PresenceManager.UpdatePresence(detail, _state);
        } else if (_detail != null)
        {
            PresenceManager.UpdatePresence(_detail, _state, elapsedTime);
        }
    }

}
