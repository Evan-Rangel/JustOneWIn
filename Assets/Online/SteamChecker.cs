using UnityEngine;
using Steamworks;

public static class SteamChecker
{
#if !DISABLESTEAMWORKS
    public static bool IsSteamAvailable()
    {
        return false;
#if UNITY_EDITOR
        return false;    // nunca usar Steam dentro del Editor
#else
        return SteamManager.Initialized;
#endif
    }
#else
    public static bool IsSteamAvailable() => false;
#endif
}
