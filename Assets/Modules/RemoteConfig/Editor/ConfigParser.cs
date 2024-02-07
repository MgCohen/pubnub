using Newtonsoft.Json.Linq;
using Pubnub.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.RemoteConfig.Editor;
using UnityEditor;
using UnityEngine;

public class ConfigParser : EditorWindow
{
    public RemoteConfigDataStore store;
    public NewsCollection news;
    public LeaderboardConfigWrapper leaderboard;

    [MenuItem("Window/Game/Parser")]
    public static void OpenWindow()
    {
        ConfigParser wnd = GetWindow<ConfigParser>();
        wnd.titleContent = new GUIContent("ConfigParser");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Parse News Collection"))
        {
            try
            {
                store.AddSetting("News", news.News);
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
        }

        if(GUILayout.Button("Parse Leaderboard Config"))
        {
            try
            {
                store.AddSetting("Leaderboard", leaderboard.Config);
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
        }
    }

}
