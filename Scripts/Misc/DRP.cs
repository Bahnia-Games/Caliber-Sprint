using UnityEngine;
using DiscordPresence;

public class DRP : MonoBehaviour
{
    [SerializeField]
    private string detail;
    [SerializeField]
    private string state;

    private void Start()
    {
        PresenceManager.UpdatePresence(detail, state);
    }
}
