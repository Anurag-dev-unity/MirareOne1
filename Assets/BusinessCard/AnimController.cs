using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class AnimController : MonoBehaviour
{

    public GameObject Home, ImageOverlayObject, VideoOverlayObject;


    public void BackTrigger()
    {
        Home.SetActive(true);
        ImageOverlayObject.SetActive(false);
        VideoOverlayObject.SetActive(false);
    }

    public void imageTrigger()
    {
        Home.SetActive(false);
        ImageOverlayObject.SetActive(true);
        VideoOverlayObject.SetActive(false);
    }


    public void videoTrigger()
    {
        Home.SetActive(false);
        VideoOverlayObject.SetActive(true);
        ImageOverlayObject.SetActive(false);
    }


    public void eMailTrigger()
    {
        Application.OpenURL("mailto:" + "anup@macle.co.in" + "?subject:" + "test" + "&body:" + "hello");
    }


    public void threeDTrigger()
    {
        Application.OpenURL("https://threejs.org");
    }


    public void phoneCallTrigger()
    {
        Application.OpenURL("tel:[9986642566]");
    }


    public void whatsappTrigger()
    {
        Application.OpenURL("https://wa.me/9986642566");
    }


    public void websiteTrigger()
    {
        Application.OpenURL("http://www.mirareinteracttive.com/");
    }


}
