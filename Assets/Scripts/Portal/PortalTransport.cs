using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PortalTransport : MonoBehaviour
{


    [SerializeField]
    public Material[] materials;

    public Transform device;

    bool wasInFront;
    bool inOtherWorld;

    bool hasCollided;

    bool validateEntry;


    // Start is called before the first frame update
    void Start()
    {
        SetMaterials(false);
    }


    private bool GetIsInFront()
    {
        Vector3 worldPos = device.position + device.forward * Camera.main.nearClipPlane;
        Vector3 pos = transform.InverseTransformPoint(worldPos);
        return pos.z >= 0 ? true : false;
    }


    /// <summary>
    /// Sets the materials.
    /// </summary>
    /// <param name="fullRender">If set to <c>true</c> full render.</param>
    void SetMaterials(bool fullRender)
    {
        var stencilTest = fullRender ? CompareFunction.NotEqual : CompareFunction.Equal;
        Shader.SetGlobalInt("_StencilTest", (int)stencilTest);

        //foreach (var mat in materials)
        //{
        //    mat.SetInt("_StencilTest", (int)stencilTest);
        //}
    }

    /// <summary>
    /// Ons the trigger enter.
    /// </summary>
    /// <param name="other">Other.</param>
    private void OnTriggerEnter(Collider other)
    {
        //not the portal door
        if (other.transform != device)
            return;

        if (device.transform.position.z < transform.position.z)
        {
            Debug.Log("RARO correct enter");


        }
        else
        {
            Debug.Log("RARO incorrect entry");
        }

        Debug.Log("RARO enter trigger");
        wasInFront = GetIsInFront();
        hasCollided = true;


    }


    /// <summary>
    /// Ons the trigger stay.
    /// </summary>
    /// <param name="other">Other.</param>
    private void OnTriggerExit(Collider other)
    {
        //not the portal door
        if (other.transform != device)
            return;

        hasCollided = false;
    }


    /// <summary>
    /// Whiles the camera colliding.
    /// </summary>
    private void WhileCameraColliding()
    {

        if (!hasCollided)
            return;

        bool isInFront = GetIsInFront();

        if ((isInFront && !wasInFront) || (wasInFront && !isInFront))
        {
            inOtherWorld = !inOtherWorld;
            SetMaterials(inOtherWorld);
        }
        wasInFront = isInFront;

        Debug.Log("RARO enter trigger");
    }

    /// <summary>
    /// when we are working on the scene we want to be able to
    /// see everything not just through the portal
    /// </summary>
    private void OnDestroy()
    {
        SetMaterials(true);
    }


    // Update is called once per frame
    void Update()
    {
        WhileCameraColliding();
    }
}
