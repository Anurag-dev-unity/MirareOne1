using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour
{

    private bool camAvailable;
    private WebCamTexture backCam;
    private Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;

    /// <summary>
    /// Start this instance.
    /// </summary>
    private void Start()
    {
        defaultBackground = background.texture;
        //looking for camera devices on our device
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            //no camera available
            Debug.Log("no camera detected");
            camAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                //back camera is available
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if (backCam == null)
        {
            Debug.Log("unable to find back camera");
            return;
        }

        backCam.Play();
        background.texture = backCam;

        camAvailable = true;

    }

    /// <summary>
    /// Update this instance.
    /// </summary>
    private void Update()
    {
        if (!camAvailable)
        {
            return;
        }

        float ratio = (float)backCam.width / (float)backCam.height;
        fit.aspectRatio = ratio;

        float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }

}
