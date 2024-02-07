using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class AssetsEditorUtility
{
#if UNITY_EDITOR
  public static Dictionary<Type, List<Object>> AssetCache = new();

  public static T GetAsset<T>(bool invalidateCache = false, params string[] paths)
  {
    return GetAssets<T>(invalidateCache, paths).FirstOrDefault();
  }

  public static Object GetAsset(Type type, bool invalidateCache = false, params string[] paths)
  {
    return GetAssets(type, invalidateCache, paths).FirstOrDefault();
  }

  public static List<T> GetAssets<T>(bool invalidateCache = false, params string[] paths)
  {
    return GetAssets(typeof(T), invalidateCache, paths).OfType<T>().ToList();
  }

  public static IEnumerable<Object> GetAssets(Type type, bool invalidateCache = false, params string[] paths)
  {
    if (type == null) return new List<Object>();

    if (type.IsInterface)
    {
      return GetAssetsWithInterface(type, invalidateCache, paths);
    }

    return GetAssetsOfType(type, invalidateCache, paths);
  }

  private static IEnumerable<Object> GetAssetsOfType(Type type, bool invalidateCache = false, params string[] paths)
  {
    List<Object> data = AssetCache.GetValueOrDefault(type)?.Cast<Object>().ToList();
    if (data == null || invalidateCache)
    {
      data = AssetDatabase
        .FindAssets($"t:{type.Name}", paths)
        .Select(guid => AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), type))
        .ToList();

      List<Object> toSave = data.Cast<Object>().ToList();
      if (AssetCache.ContainsKey(type))
      {
        AssetCache[type] = toSave;
      }
      else
      {
        AssetCache.Add(type, toSave);
      }
    }

    return data;
  }

  private static IEnumerable<Object> GetAssetsWithInterface(Type type, bool invalidateCache = false, params string[] paths)
  {
    List<Object> objects = new List<Object>();
    foreach (Type typesWithInterface in System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(mytype => mytype.GetInterfaces().Contains(type)))
    {
      IEnumerable<Object> otherObjects = GetAssetsOfType(typesWithInterface, invalidateCache, paths);
      objects.AddRange(otherObjects);
    }

    return objects;
  }

  public static void CreateFolderIfDoesntExist(string path)
  {
    if (AssetDatabase.IsValidFolder(path) || !string.IsNullOrWhiteSpace(AssetDatabase.AssetPathToGUID(path, AssetPathToGUIDOptions.OnlyExistingAssets))) return; //if already exists
    string parentFolder = Path.GetDirectoryName(path);
    string folderName = Path.GetFileName(path);
    if (!AssetDatabase.IsValidFolder(parentFolder))
    {
      Debug.LogError($"{parentFolder} is not a valid location");
      return;
    }

    AssetDatabase.CreateFolder(parentFolder, folderName);
  }

  public static void CreateAsset(string path, Object so, bool highlightOnEditor = true)
  {
    CreateFolderIfDoesntExist(Path.GetDirectoryName(path));
    AssetDatabase.CreateAsset(so, path);
    ProjectWindowUtil.ShowCreatedAsset(so);
  }

#endif
}