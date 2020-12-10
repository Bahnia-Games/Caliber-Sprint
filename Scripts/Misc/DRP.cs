using UnityEngine;
using DiscordPresence;
using System;

public class DRP : MonoBehaviour
{
    [SerializeField]
    private string detail;
    [SerializeField]
    private string state;
    [SerializeField]
    long elapsedTime;

    private void Start()
    {
        PresenceManager.UpdatePresence(detail, state, elapsedTime);
    }
}
