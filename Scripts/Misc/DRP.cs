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
    public static void UpdateState(string _detail)
    {
        Debug.Log("Updating DRP");
        PresenceManager.UpdatePresence(_detail);
    }

}
