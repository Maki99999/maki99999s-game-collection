using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscordIntegration : MonoBehaviour
{
    public static Discord.Discord discord;
    static Discord.ActivityManager activityManager;
    static bool alreadyRunning = false;

    void Awake()
    {
        if (alreadyRunning)
        {
            Destroy(this);
            return;
        }
        alreadyRunning = true;

        discord = new Discord.Discord(600379630427570226, (System.UInt64)Discord.CreateFlags.Default);
        activityManager = discord.GetActivityManager();
        Discord.Activity activity = new Discord.Activity
        {
            Details = "Main Menu",
            Assets =
            {
                 LargeImage = "icon",
                 LargeText = "Mein cooles Logo",
            },
        };
        activityManager.UpdateActivity(activity, (res) => { });

        DontDestroyOnLoad(this);
    }

    void Update()
    {
        discord.RunCallbacks();
    }

    public static void UpdateActivity(string details)
    {
        Discord.Activity activity = new Discord.Activity
        {
            Details = details,
            Assets =
            {
                 LargeImage = "icon",
                 LargeText = "Mein cooles Logo",
            },
        };
        activityManager.UpdateActivity(activity, (res) => { });
    }

    private void OnApplicationQuit()
    {
        activityManager.ClearActivity((res) => { });
    }
}