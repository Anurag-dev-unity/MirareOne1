using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GoogleARCore;

public class ARController : MonoBehaviour
{
    //list of planes ar core detected in the current frame
    private List<TrackedPlane> m_newTrackedPlanes = new List<TrackedPlane>();

    public GameObject gridPrefab;
    public GameObject portal;
    public GameObject arCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //check ARCore session status
        if (Session.Status != SessionStatus.Tracking)
        {
            return;
        }

        //fill the list of frames
        Session.GetTrackables<TrackedPlane>(m_newTrackedPlanes, TrackableQueryFilter.New);

        //Instantiate a grid for each tracked plane
        for (int i = 0; i < m_newTrackedPlanes.Count; i++)
        {
            GameObject grid = Instantiate(gridPrefab, Vector3.zero, Quaternion.identity, transform);

            //set position of the grid and modify vertices of attached mesh
            grid.GetComponent<GridVisualizer>().Initialize(m_newTrackedPlanes[i]);
        }

        //check if the user has touched the screen
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        //Let's now check if the user has touched any of the tracked planes
        TrackableHit hit;
        if (Frame.Raycast(touch.position.x, touch.position.y, TrackableHitFlags.PlaneWithinPolygon, out hit))
        {
            //let's now place the portal on top of the tracked plane that we touched

            //enable the portal
            portal.SetActive(true);

            //create a new anchor
            Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose);

            //set the position of portal to be same as hit position
            portal.transform.position = hit.Pose.position;
            portal.transform.rotation = hit.Pose.rotation;

            //we want portal to face the camera
            Vector3 cameraPosition = arCamera.transform.position;

            //the portal should only rotate around the y-axis
            cameraPosition.y = hit.Pose.position.y;

            //rotate the portal to face the camera
            portal.transform.LookAt(cameraPosition, portal.transform.up);

            //AR Core will keep understanding the world and update the anchors
            //accordingly hence we need to attach our portal to the anchor
            portal.transform.parent = anchor.transform;
                  

        }

    }
}


