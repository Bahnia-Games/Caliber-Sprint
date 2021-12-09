using Assets.Git.Scripts.Misc;
using Discord;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using D = Discord;

public class MasterDRPController : MonoBehaviour
{

    private static readonly long token = 786595721121103904;
    private static readonly string csIcon = "gameicon";

    internal D.Discord MasterDiscord { get; private set; }
    internal ActivityManager MasterActivityManager { get; private set; }

    //private MasterMiscController mmc;
    //public MasterDRPController(MasterMiscController mmc) => this.mmc = mmc;
    void Awake() => OnRequestUse();
    private void Start() => OnRequestUse();
    private void OnRequestUse()
    {
        MasterDiscord = new D.Discord(token, 0);
        MasterActivityManager = MasterDiscord.GetActivityManager();
        MasterDiscord.SetLogHook(LogLevel.Error, LogErrors);
        MasterMiscController.ApplicationQuitRequest += OnShutdown;
    }

    private void Update() => MasterDiscord.RunCallbacks();

    internal void Initialize(Activity? initActivity = null) // forced nullable moment 
    {
        if (initActivity == null)
            initActivity = new Activity
            {
                State = $"Initializing scene {SceneManager.GetActiveScene()}.",
                Details = "",
                Assets = new ActivityAssets
                {
                    LargeImage = csIcon,
                    LargeText = "Caliber Sprint Tech Demo"
                }
            };
        UpdatePresence((Activity)initActivity); // epic nullable fail
    }

    internal void UpdatePresence(Activity activity) => MasterActivityManager.UpdateActivity(activity, (res) =>
    {
        if (res != Result.Ok)
            try {
                throw new Exception($"Callback response erorr, could not update presence. {res.ToString()}." +
              $"@MasterDRPController.cs UpdatePresence(Activity)"); }
            catch (Exception e) { Debug.LogError(e.ToString()); }
    });

    internal void OnShutdown(object u = null, QuitEventArgs au = null)
    {
        Debug.LogWarning("Discord services shutting down.");
        MasterActivityManager.ClearActivity((res) => { }); // no callback nescassary here
        MasterActivityManager = null;
        MasterDiscord.Dispose();
    }

    internal void ResetPresence() { OnShutdown(); Start(); }

    private void LogErrors(LogLevel lvl, string msg) => Debug.LogError($"Discord:{lvl} - {msg}");
}
