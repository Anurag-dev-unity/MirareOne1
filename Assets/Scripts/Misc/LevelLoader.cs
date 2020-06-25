using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : Singleton<LevelLoader>
{
    public GameObject go;     public Animator splashAnim;      private void Update()     {         if (splashAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)         {
             if(Application.platform == RuntimePlatform.Android)             {                 SceneManager.LoadSceneAsync(1);             }             else if(Application.platform == RuntimePlatform.IPhonePlayer)             {
                go.SetActive(false);
                //for iOS builds, remove the arcore scene
                SceneManager.LoadSceneAsync(1);             }         }     }
}
