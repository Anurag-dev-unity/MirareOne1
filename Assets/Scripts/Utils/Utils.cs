using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    /// <summary>
    /// get string between [first]strStart and [last]strEnd
    /// </summary>
    /// <param name="strSource">source string</param>
    /// <param name="strStart">first string</param>
    /// <param name="strEnd">last string</param>
    /// <returns>string</returns>
    public static string GetBetween(string strSource, string strStart, string strEnd)
    {
        int start, end;
        if (strSource.Contains(strStart) && strSource.Contains(strEnd))
        {
            start = strSource.IndexOf(strStart, 0) + strStart.Length;
            end = strSource.IndexOf(strEnd, start);
            return strSource.Substring(start, end - start);
        }
        else
        {
            return string.Empty;
        }
    }
    /// <summary>
    /// load object data from file into a list.
    /// </summary>
    /// <typeparam name="T">generic type to loadAllAssets</typeparam>
    /// <param name="path">file location</param>
    /// <returns>list containing object data</returns>
    public static IList<Object> Load<T>(string path) where T : class
    {
        IList<Object> list = new List<Object>();
        AssetBundleCreateRequest bundle = AssetBundle.LoadFromFileAsync(path);
        AssetBundle myLoadedAssetBundle = bundle.assetBundle;

        Object[] objs = myLoadedAssetBundle.LoadAllAssets(typeof(T));
        list = new List<Object>(objs);

        myLoadedAssetBundle.Unload(false);
        AssetBundle.UnloadAllAssetBundles(false);
        return list;
    }
    /// <summary>
    /// save data to file.
    /// </summary>
    /// <param name="data">data to write to file</param>
    /// <param name="path">file location to createDirectory</param>
    public static void Save(byte[] data, string path)
    {
        if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(path)))
        {
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
        }

        try
        {
            System.IO.File.WriteAllBytes(path, data);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Failed To Save Data to: " + path.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }
    }
}