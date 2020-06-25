using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;

public class WebRequest : Singleton<WebRequest>
{
    private string str = string.Empty;
    public UnityWebRequest uwr;

    public void DownloadAssetBundle(string url, string fileName, string subFolder)
    {
        StartCoroutine(DownloadAsset(url, fileName, subFolder));
    }
    public void DownloadObjectInfo(string fileName, string campaignModel, string brandName, string subFolder, GameObject currentObj)
    {
        StartCoroutine(Info(fileName, campaignModel, brandName, subFolder, currentObj));
    }

    #region privateMethods
    /// <summary>
    /// download texture from web.
    /// save texture to file.
    /// </summary>
    /// <param name="url">texture url</param>
    /// <param name="image">image to set</param>
    /// <returns>unityWebRequestAsyncOperation</returns>
    private IEnumerator GetTexture(string url, Image image, string name)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        Debug.Log("RARO textureUrl-> " + url);
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                Debug.LogError(uwr.error);
            }
            else
            {
                try
                {
                    Debug.Log(" RARO webrequest-> " + uwr.isDone);
                    Debug.Log("RARO downloadHandler-> " + uwr.downloadHandler.isDone);
                    var texture = ((DownloadHandlerTexture)uwr.downloadHandler).texture;
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                    image.sprite = sprite;

                    string tempPath = Path.Combine(Application.persistentDataPath, "AssetData", name + ".txt");
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        tempPath = tempPath.Replace("/", Path.DirectorySeparatorChar.ToString());
                        Debug.Log("RARO tempPathMac-> " + tempPath);
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        tempPath = tempPath.Replace("/", "\\");
                        Debug.Log("RARO tempPath-> " + tempPath);
                    }
                    else if (Application.platform == RuntimePlatform.Android)
                    {
                        tempPath = Path.Combine(Application.persistentDataPath, "AssetData", name + ".txt");
                    }
                    Utils.Save(uwr.downloadHandler.data, tempPath);
                }
                catch (System.InvalidOperationException e)
                {
                    throw e;
                }
            }
        }
        sw.Stop();
        Debug.Log("RARO textureDownloaded in " + sw.Elapsed.Seconds);
    }
    /// <summary>
    /// download asset data from web.
    /// </summary>
    /// <param name="url">assetBundle url</param>
    /// <returns>unityWebRequestAsyncOperation</returns>
    private IEnumerator DownloadAsset(string url, string fileName, string subFolder, GameObject currentObj = null)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        using (uwr = UnityWebRequest.Get(url))
        {
            yield return uwr.SendWebRequest();
            //print(uwr.responseCode);
            //AppManager.Instance.myText.text = uwr.responseCode.ToString();
            if (uwr == null)
                yield break;
            if (uwr.isNetworkError)
            {
                Debug.LogError("RARO Error while downloading data-> " + uwr.error);
                uwr.Abort();
                UIController.Instance.downloadingObj.SetActive(false);
                AppManager.Instance.objToSpawnFile = string.Empty;
                AppManager.Instance.canvasGraphicRaycaster.enabled = true;
                AppManager.Instance.trackingState = false;
                yield break;
            }
            else
            {
                string tempPath = Path.Combine(Application.persistentDataPath, subFolder, fileName + ".unity3d");
                if (AppManager.Instance.PlatformIsIPhone() || AppManager.Instance.PlatformIsMac())
                {
                    if (url == AppManager.Instance.TexUrliOS)
                    {
                        AppManager.Instance.TextureDataPath = tempPath.Replace("/", Path.DirectorySeparatorChar.ToString());
                        Utils.Save(uwr.downloadHandler.data, AppManager.Instance.TextureDataPath);
                        Debug.Log("RARO texDataPathMac-> " + AppManager.Instance.TextureDataPath);
                    }
                    else
                    {
                        AppManager.Instance.ObjectDataPath = tempPath.Replace("/", Path.DirectorySeparatorChar.ToString());
                        Utils.Save(uwr.downloadHandler.data, AppManager.Instance.ObjectDataPath);
                        ObjectPooler.Instance.Init(AppManager.Instance.ObjectDataPath);
                    }
                }
                else if (AppManager.Instance.PlatformIsWindows())
                {
                    if (url == AppManager.Instance.TexUrlDroid || url == AppManager.Instance.TexUrliOS)
                    {
                        AppManager.Instance.TextureDataPath = tempPath.Replace("/", "\\");
                        Utils.Save(uwr.downloadHandler.data, AppManager.Instance.TextureDataPath);
                        Debug.Log("RARO texDataPathWindows-> " + AppManager.Instance.TextureDataPath);
                    }
                    else
                    {
                        AppManager.Instance.ObjectDataPath = tempPath.Replace("/", "\\");
                        Utils.Save(uwr.downloadHandler.data, AppManager.Instance.ObjectDataPath);
                        Debug.Log("RARO objPath-> " + AppManager.Instance.ObjectDataPath);
                        ObjectPooler.Instance.Init(AppManager.Instance.ObjectDataPath);
                    }
                }
                else if (AppManager.Instance.PlatformIsAndroid())
                {
                    if (url == AppManager.Instance.TexUrlDroid)
                    {
                        AppManager.Instance.TextureDataPath = tempPath;
                        Utils.Save(uwr.downloadHandler.data, AppManager.Instance.TextureDataPath);
                    }
                    else
                    {
                        AppManager.Instance.ObjectDataPath = tempPath;
                        Utils.Save(uwr.downloadHandler.data, AppManager.Instance.ObjectDataPath);
                        ObjectPooler.Instance.Init(AppManager.Instance.ObjectDataPath);
                    }
                }
            }
            print(uwr.responseCode);
        }   //unityWebRequest disposed here.
        sw.Stop();
        Debug.Log("RARO assetDownloaded in " + sw.Elapsed.Seconds + ", " + fileName);
    }
    /// <summary>
    /// get android and iOS bundle url
    /// </summary>
    /// <param name="fileName">save file name of object</param>
    /// <param name="campaignModel">object campaign category</param>
    /// <param name="brandName">object brandName</param>
    /// <param name="subFolder">folder to save object</param>
    /// <param name="currentObj">selected button</param>
    /// <returns>unityWebRequestAsyncOperation</returns>
    private IEnumerator Info(string fileName, string campaignModel, string brandName, string subFolder, GameObject currentObj)
    {
        WWWForm form = new WWWForm();
        form.AddField("camp_model", campaignModel);
        form.AddField("brand_name", brandName);
        string url = "https://mirareinteracttive.com/backend/dashboard/uploads/package/";

        using (UnityWebRequest uwr = UnityWebRequest.Post("http://mirareinteracttive.com/backend/api/allcamp.php", form))
        {
            yield return uwr.SendWebRequest();
            print(uwr.responseCode);
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
                uwr.Abort();
                UIController.Instance.downloadingObj.SetActive(false);
                AppManager.Instance.objToSpawnFile = string.Empty;
                AppManager.Instance.canvasGraphicRaycaster.enabled = true;
                AppManager.Instance.trackingState = false;
                yield break;
            }
            else
            {
                string data = uwr.downloadHandler.text;
                //Debug.Log(data);
                string strStart = string.Empty;
                if (Application.platform == RuntimePlatform.Android || RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    strStart = "add_link";
                else if (Application.platform == RuntimePlatform.IPhonePlayer || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    strStart = "ios_link";
                data = Utils.GetBetween(data, strStart, ",");
                //Debug.Log(data);
                data = data.Replace(":", "").Replace('"', ' ').Trim();
                //Debug.Log(data);
                url += data;
                Debug.Log(url);
            }
        }  //unityWebRequest disposed here.
        form = null;
        System.GC.Collect();
        StartCoroutine(DownloadAsset(url, fileName, subFolder, currentObj));
    }
    #endregion
}