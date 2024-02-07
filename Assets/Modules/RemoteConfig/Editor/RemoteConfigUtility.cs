using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig.Editor;
using System.Linq;
using Unity.RemoteConfig;
using Newtonsoft.Json.Linq;
using System;
using Newtonsoft.Json;
using UnityEditor;

#if UNITY_EDITOR
public static class RemoteConfigUtility
{
    public static void AddSetting(this RemoteConfigDataStore store, string key, object value)
    {
        JToken entry = store.rsKeyList.FirstOrDefault(t => t["rs"]["key"].Value<string>() == key);
        ConfigEntry newEntry = new ConfigEntry(key, value);

        if (entry == null)
        {
            Debug.Log("Adding new entry");
            store.AddSetting(JObject.FromObject(newEntry));
        }
        else
        {
            Debug.Log("Updating entry");
            newEntry.metadata.entityId = entry["metadata"]["entityId"].Value<string>() ?? "";
            store.UpdateSetting(entry as JObject, JObject.FromObject(newEntry));
        }
    }

    [Serializable]
    public class ConfigEntry
    {
        public ConfigEntry(string key, object value)
        {
            rs.key = key;
            rs.value = value;
        }
        public MetaData metadata = new MetaData();
        public ConfigValue rs = new ConfigValue();

        [Serializable]
        public class MetaData
        {
            public string entityId = string.Empty;
        }

        [Serializable]
        public class ConfigValue
        {
            public string key;
            public string type = "json";
            public object value;
        }
    }
}

#endif