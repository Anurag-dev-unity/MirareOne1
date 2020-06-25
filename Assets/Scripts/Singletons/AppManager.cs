using GoogleARCore;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.iOS;

public class AppManager : Singleton<AppManager>
{
    #region privateVariables
    private const string prefabFileName = "PrefabData";
    private const string objFileName = "ObjectData";
    private const string texUrlDroid = "https://firebasestorage.googleapis.com/v0/b/mirare-one.appspot.com/o/Android%20Textures%2Ftexfiledroid?alt=media&token=9101e342-914c-42c4-a521-3be138ed45f8";
    private const string texFileName = "TextureData";
    private const string texUrliOS = "https://firebasestorage.googleapis.com/v0/b/mirare-one.appspot.com/o/iOS%20Textures%2Ftexfileios?alt=media&token=54cf3f1c-d09a-4c23-af64-d1468118e1f6";

    private const string OnRaycastExitStr = "OnRaycastExit";
    private const string OnRaycastEnterStr = "OnRaycastEnter";
    private const string textureSubFolder = "AssetData/Textures";
    private const string objSubFolder = "AssetData/Objects";
    private bool textureReadComplete = false;
    private IList<Object> texList = new List<Object>();

    private Material[] originalMaterials;
    private Material[] ghostMaterials;
    private GameObject prevSelectedObj;

    //[SerializeField]
    //private Material ghostMaterial;

    [SerializeField]
    private Material selectionMaterial;

    [SerializeField]
    public GameObject selectionMarker;

    private Touch touch;
    private Vector2 touchPosition;
    private Quaternion rotationY;
    private float rotateSpeedModifier = 1.1f;

    private Vector2 startTouch, endTouch, moveTouch, tempTouch;
    private bool twoFingerHoldDrag;

    private Ray ray;
    private RaycastHit hit;

    private bool firstRotate;
    private bool firstScale;

    private bool showUserTut;
    private float disableTimer = 0;


    public bool selectionMarkerScaleSet = false;
    private Collider m_Collider;
    private Vector3 m_ColSize;
    private Vector3 oldSelectionMarkerPos = Vector3.zero;
    private bool selectionMarkerPosChange;
    private bool selectedObjChanged = false;
    private GameObject oldSelectedObj = null;
    #endregion


    #region publicVariables
    public bool trackingState;
    public GameObject selectedObj;
    public GameObject objToSpawn;
    public Camera arCamera;
    public Camera mainCamera;
    public LayerMask interactableFilter;
    public Text debugText;
    public string objToSpawnFile = string.Empty;
    public string filePath = string.Empty;
    public string objInfo = string.Empty;
    public GraphicRaycaster canvasGraphicRaycaster;
    public string ObjSubFolder { get { return objSubFolder; } }
    public bool TextureReadComplete { get { return textureReadComplete; } private set {; } }
    public IList<Object> TexList { get { return texList; } private set {; } }
    public string TexUrlDroid { get { return texUrlDroid; } private set {; } }
    public string TexUrliOS { get { return texUrliOS; } private set {; } }
    public string ObjectDataPath { get; set; }
    public string TextureDataPath { get; set; }
    public bool disablePlane;

    public bool doubleTapPlacement = false;

    [HideInInspector]
    public ARCoreSession session;

    [HideInInspector]
    public UnityARCameraManager unityARcamera;

    public float maxRayDistance = 1000.0f;
    public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer
    public List<Vector3> boundaryPolygonPoints = new List<Vector3>();
    public int doOnce;

    public bool portalMode;
    public bool portalIsPlaced;

    public List<GameObject> kitDetectedPlanes = new List<GameObject>();

    public Material ghostMaterial;
    public Material errorMaterial;
    #endregion

    #region privateFunctions
    /// <summary>
    /// Awake this instance.
    /// </summary>
    private void Awake()
    {
        //#if UNITY_EDITOR
        //Debug.unityLogger.logEnabled = false;
        //#else
        //Debug.unityLogger.logEnabled = true;
        //#endif

        Debug.unityLogger.logEnabled = true;


        if (PlatformIsAndroid())
        {
            session = GameObject.Find("ARCore Device").GetComponent<ARCoreSession>();
            session.SessionConfig.PlaneFindingMode = DetectedPlaneFindingMode.Disabled;
            session.OnEnable();
        }
        else if (PlatformIsIPhone())
        {
            unityARcamera = GameObject.Find("ARCameraManager").GetComponent<UnityARCameraManager>();
            if (unityARcamera != null)
            {
                unityARcamera.planeDetectionOFF();
                //unityARcamera.planeDetectionON();
            }
        }
    }

    /// <summary>
    /// Start this instance.
    /// </summary>
    private void Start()
    {
        string path = System.IO.Path.Combine(Application.persistentDataPath, objSubFolder);
        string texPath = System.IO.Path.Combine(Application.persistentDataPath, textureSubFolder);
        Debug.Log(path);
        if (PlatformIsAndroid() || PlatformIsWindows())
        {
            if (System.IO.Directory.Exists(texPath))
            {
                LoadTextureFromMemory(texPath);
            }
            else
            {
                Debug.Log("RARO downloading icons from web");
                WebRequest.Instance.DownloadAssetBundle(texUrlDroid, texFileName, textureSubFolder);
            }
        }
        else if (PlatformIsIPhone() || PlatformIsMac())
        {
            texPath = texPath.Replace("/", System.IO.Path.DirectorySeparatorChar.ToString());
            if (System.IO.Directory.Exists(texPath))
            {
                LoadTextureFromMemory(texPath);
            }
            else
            {
                Debug.Log("RARO downloading icons from web");
                WebRequest.Instance.DownloadAssetBundle(texUrliOS, texFileName, textureSubFolder);
            }
            path = path.Replace("/", System.IO.Path.DirectorySeparatorChar.ToString());
        }
        if (System.IO.Directory.Exists(path))
        {
            string[] filePaths = System.IO.Directory.GetFiles(path);
            for (int i = 0; i < filePaths.Length; i++)
            {
                Debug.Log(filePaths[i]);
                ObjectPooler.Instance.Init(filePaths[i]);
            }
        }



        //first run on the devices
        doOnce = PlayerPrefs.GetInt("once", 0);
        //doOnce = 1;

        if (doOnce == 0)
        {
            UIController.Instance.ToggleMainMenu();
            UIController.Instance.ToggleMarkerlessMRPage();
            //UIController.Instance.ToggleSofaPage();
            UIController.Instance.ToggleFurniturePage();
        }
        else
        {
            FinishTutorials();
            //UIController.Instance.ToggleMainMenu();
        }
    }
       

    /// <summary>
    /// Update this instance.
    /// </summary>
    private void Update()
    {

        //debugText.text = "Spawned Count -  " + UIController.Instance.spawnedObjects.childCount;

        if (kitDetectedPlanes.Count != 0)
        {
            //for (int i = 0; i < kitDetectedPlanes.Count; i++)
            //{
            //    debugText.text = kitDetectedPlanes[i].GetComponent<BoxCollider>().bounds.size.ToString();
                //kitDetectedPlanes[i].GetComponent<BoxCollider>().bounds.center;
            //}
        }
        else
        {
            //debugText.text = "no planes";
        }


        CheckForInternet();
        LoadTextures();
        CheckForObject();
        CheckForPlaneSize();

        /*
        if (trackingState)
        {
            Debug.Log("RARO TRACKING STATE - " + true);
            if (objToSpawn != null)
            {
                //TurnGhost(true, objToSpawn);
            }
            trackingState = false;
        }
        */

        FollowCamera();

        CheckIfObjDownloaded();

        if (selectedObj != null)
        {
            SelectionTouch();

            /*
            //------------------------------

            selectionMarker.transform.position = selectedObj.transform.position;
            selectionMarker.transform.rotation = selectedObj.transform.rotation;
            selectionMarker.transform.parent = selectedObj.transform;

            if (oldSelectedObj != selectedObj)
            {
                selectedObjChanged = true;
                oldSelectedObj = selectedObj;

            }

            //if (selectionMarker.transform.position != oldSelectionMarkerPos)
            //{
            //    oldSelectionMarkerPos = selectionMarker.transform.position;
            //    selectionMarker.transform.localScale = new Vector3(1, 1, 1);
            //    selectionMarkerPosChange = true;
            //}

            if (!selectionMarkerScaleSet || selectedObjChanged)
            {


                selectionMarker.transform.localScale = new Vector3(1, 1, 1);

                selectionMarker.transform.localScale = new Vector3(selectionMarker.transform.localScale.x *
                                                                   selectedObj.GetComponent<InteractableObject>().colliderSize *
                                                                   selectedObj.transform.localScale.x,
                                                                   selectionMarker.transform.localScale.y,
                                                                   selectionMarker.transform.localScale.z *
                                                                   selectedObj.GetComponent<InteractableObject>().colliderSize *
                                                                   selectedObj.transform.localScale.z);




                if (!selectionMarker.activeInHierarchy)
                {
                    selectionMarker.SetActive(true);
                }

                selectionMarkerScaleSet = true;
                selectedObjChanged = false;

            }




            // --------------------------------------
            */

        }
        else
        {
            //selectionMarker.transform.parent = null;
            //selectionMarker.transform.localScale = new Vector3(1, 1, 1);
            //selectionMarkerScaleSet = false;

            //selectionMarker.SetActive(false);
        }

        UserTutorial();
        GestureTutorial();

        //portal
        PortalPlacement();

    }


    /// <summary>
    /// Checks for internet.
    /// </summary>
    private void CheckForInternet()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //Change the Text
            //myText.text = "Not Reachable.";
        }
        else
        {
            if (objInfo != string.Empty)
            {
                //print("internet back");
                string[] s = objInfo.Split(',');
                string objName = s[0];
                string url = s[1];
                UIController.Instance.checkNetTextObj.SetActive(false);
                string path = System.IO.Path.Combine(Application.persistentDataPath, AppManager.Instance.ObjSubFolder, objName + ".unity3d");
                AppManager.Instance.filePath = path;
                AppManager.Instance.boundaryPolygonPoints.Clear();
                if (AppManager.Instance.objToSpawnFile == string.Empty)
                    AppManager.Instance.objToSpawnFile = objName;
                if (!System.IO.File.Exists(path))
                {
                    UIController.Instance.downloadingObj.SetActive(true);
                    UIController.Instance.cancelDownloadObj.SetActive(true);
                    AppManager.Instance.canvasGraphicRaycaster.enabled = false;
                    WebRequest.Instance.DownloadAssetBundle(url, objName, AppManager.Instance.ObjSubFolder);
                    objInfo = string.Empty;
                    //break;
                }
            }
        }
    }

    /// <summary>
    /// Checks if object downloaded.
    /// </summary>
    private void CheckIfObjDownloaded()
    {
        //myText.text = objToSpawnFile;
        if(objToSpawnFile != string.Empty)
        {
            if(ObjectPooler.Instance.poolDictionary.ContainsKey(objToSpawnFile))
            {
                UIController.Instance.downloadingObj.SetActive(false);
                UIController.Instance.cancelDownloadObj.SetActive(false);
                disablePlane = false;
                objToSpawn = ObjectPooler.Instance.GetFromPool(objToSpawnFile, UIController.Instance.spawnedObjects);
                trackingState = true;

                if (PlatformIsAndroid())
                {
                    if (objToSpawn != null && boundaryPolygonPoints.Count == 0)
                        UIController.Instance.scanPlane.SetActive(true);

                }
                else if (PlatformIsIPhone())
                {
                    //check for plane generated here
                    if (objToSpawn != null && kitDetectedPlanes.Count == 0)
                    {
                        UIController.Instance.scanPlane.SetActive(true);
                    }
                }

                Debug.Log("RARO obj downloaded and created");
                canvasGraphicRaycaster.enabled = true;
                objToSpawnFile = string.Empty;

            }
        }
    }


    /// <summary>
    /// Users the tutorial.
    /// </summary>
    private void UserTutorial()
    {
        //if already ran once
        if (doOnce == 1)
            return;

        //Debug.Log("RARO FIRST RUN");

        //Step 1 -
        //Point to the AR/MR buton
        //darken the canvas
        UIController.Instance.tutorialBg.SetActive(true);

        //Step 4-
        //Point to sub category sofa on furniture page
        UIController.Instance.tutorialStep4.SetActive(true);

            //Unmask the sofa button
        UIController.Instance.sofaBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIController.Instance.sofaBtn.GetComponent<Button>().enabled = true;


        //Step 5-
        //Point to sofa1 on sofa page
        if (UIController.Instance.step4Done)
        {
        UIController.Instance.tutorialStep4.SetActive(false);
        UIController.Instance.tutorialStep5.SetActive(true);
            UIController.Instance.tutorialStep7.SetActive(false);

            //Unmask the sofa button
            UIController.Instance.sofa1Btn.transform.GetChild(0).gameObject.SetActive(false);
            UIController.Instance.sofa1Btn.GetComponent<Button>().enabled = true;

        }


        //Step 6-
        //Scan
        if (UIController.Instance.step5Done)
        {
            //brighten the canvas
            UIController.Instance.tutorialBg.SetActive(false);

            UIController.Instance.tutorialStep5.SetActive(false);
            //UIController.Instance.tutorialStep6.SetActive(true);
        }


        //Step 7-
        //Tap to place object
        if (UIController.Instance.step6Done)
        {
            UIController.Instance.tutorialStep6.SetActive(false);
            UIController.Instance.tutorialStep7.SetActive(true);
            UIController.Instance.tutorialStep5.SetActive(false);
        }

        //Step 8-
        //Show gesture controls
        if (UIController.Instance.step7Done)
        {
            UIController.Instance.tutorialStep5.SetActive(false);
            UIController.Instance.tutorialStep7.SetActive(false);
            UIController.Instance.tutorialStep8.SetActive(true);
        }

        /*//Step 9-
        //Activate plane
        if (UIController.Instance.step8Done)
        {

            UIController.Instance.tutorialStep8.SetActive(false);
            UIController.Instance.tutorialStep9.SetActive(true);
        }

        //Step 10-
        //Reposition Object
        if (UIController.Instance.step8Done)
        {
            UIController.Instance.tutorialStep8.SetActive(false);
            UIController.Instance.tutorialStep10.SetActive(true);
        }*/



        //Step 11-
        //Show done
        if (UIController.Instance.step8Done)
        {

            //StartCoroutine(Step10());
            UIController.Instance.tutorialBg.SetActive(true);

            UIController.Instance.tutorialStep8.SetActive(false);
            UIController.Instance.tutorialStep11.SetActive(true);

            //UnMask the AR button
            UIController.Instance.arBtn.transform.GetChild(0).gameObject.SetActive(false);
            UIController.Instance.arBtn.GetComponent<Button>().enabled = true;
            UIController.Instance.menuBtnImage.sprite = UIController.Instance.tickSprite;
            

        }


        //----------------------------------
        /*
        //Step 11-
        //Show done
        if (UIController.Instance.step11Done)
        {
            //darken the canvas
            UIController.Instance.tutorialBg.SetActive(true);

            UIController.Instance.tutorialStep10.SetActive(false);
            UIController.Instance.tutorialStep11.SetActive(true);

            //UnMask the AR button
            UIController.Instance.arBtn.transform.GetChild(0).gameObject.SetActive(false);
                UIController.Instance.arBtn.GetComponent<Button>().enabled = true;

         }

        */


        //Done tutorials
         if (UIController.Instance.step11Done)
        {

            //lighten the canvas
            UIController.Instance.tutorialBg.SetActive(false);

            UIController.Instance.tutorialStep11.SetActive(false);

            PlayerPrefs.SetInt("once", 1);
            doOnce = PlayerPrefs.GetInt("once", 0);

            FinishTutorials();

        }

    }


    /*IEnumerator Step10()
    {
        yield return new WaitForSeconds(3.0f);
        //darken the canvas
        UIController.Instance.tutorialBg.SetActive(true);

        UIController.Instance.tutorialStep10.SetActive(false);
        UIController.Instance.tutorialStep11.SetActive(true);

        //UnMask the AR button
        UIController.Instance.arBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIController.Instance.arBtn.GetComponent<Button>().enabled = true;
    }*/


    /// <summary>
    /// Finishs the tutorials.
    /// </summary>
    private void FinishTutorials()
    {

        //lighten the canvas
        UIController.Instance.tutorialBg.SetActive(false);

        //unmask and enable all buttons
        UIController.Instance.arBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIController.Instance.arBtn.GetComponent<Button>().enabled = true;

        UIController.Instance.deleteBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIController.Instance.deleteBtn.GetComponent<Button>().enabled = true;

        UIController.Instance.furnitureTitleBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIController.Instance.furnitureTitleBtn.GetComponent<Button>().enabled = true;

        UIController.Instance.sofaBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIController.Instance.sofaBtn.GetComponent<Button>().enabled = true;

        UIController.Instance.chairBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIController.Instance.chairBtn.GetComponent<Button>().enabled = true;

        UIController.Instance.tableBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIController.Instance.tableBtn.GetComponent<Button>().enabled = true;

        UIController.Instance.bedBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIController.Instance.bedBtn.GetComponent<Button>().enabled = true;

        UIController.Instance.plusBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIController.Instance.plusBtn.GetComponent<Button>().enabled = true;

        UIController.Instance.sofaTitleBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIController.Instance.sofaTitleBtn.GetComponent<Button>().enabled = true;

        UIController.Instance.sofaSofaBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIController.Instance.sofaSofaBtn.GetComponent<Button>().enabled = true;

        UIController.Instance.sofa1Btn.transform.GetChild(0).gameObject.SetActive(false);
        UIController.Instance.sofa1Btn.GetComponent<Button>().enabled = true;

        UIController.Instance.sofa2Btn.transform.GetChild(0).gameObject.SetActive(false);
        UIController.Instance.sofa2Btn.GetComponent<Button>().enabled = true;

        UIController.Instance.sofa3Btn.transform.GetChild(0).gameObject.SetActive(false);
        UIController.Instance.sofa3Btn.GetComponent<Button>().enabled = true;

        UIController.Instance.sofaPlusBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIController.Instance.sofaPlusBtn.GetComponent<Button>().enabled = true;

}


    /// <summary>
    /// Gestures the tutorial.
    /// </summary>
    private void GestureTutorial()
    {
        if (UIController.Instance.step6Done)
        {
            if (Input.touches.Length > 0)
            {
                //Debug.Log("RARO show gestures");
                //check if it's the first run
                if (AppManager.Instance.doOnce == 0)
                {
                    UIController.Instance.step7Done = true;
                    UIController.Instance.pinchToScale.SetActive(true);
                    UIController.Instance.swipeToRotate.SetActive(true);
                }
            }
        }

        if (UIController.Instance.step7Done)
        {
            if (firstRotate)
            {
                disableTimer += Time.deltaTime;

                //check if it's the first run
                if (AppManager.Instance.doOnce == 0 && disableTimer >= 3.0f)
                {
                    //StopCoroutine(UserRotateTrial());
                    UIController.Instance.pinchToScale.SetActive(false);
                    UIController.Instance.swipeToRotate.SetActive(false);

                    UIController.Instance.step8Done = true;
                }
            }
        }
    }

    /// <summary>
    /// Users the rotate trial.
    /// </summary>
    /// <returns>The rotate trial.</returns>
    IEnumerator UserRotateTrial()
    {
        yield return new WaitForSeconds(3.0f);
        UIController.Instance.step7Done = true;
    }



    /// <summary>
    /// Toggles the user tutorial.
    /// </summary>
    private void ToggleGestureTutorial()
    {
        if (doOnce != 0)
        {
            disableTimer += Time.deltaTime;
        }

        if (Input.touches.Length > 0)// && !IsPointerOverUIObject())
        {
            if (Input.touches.Length == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began && !IsPointerOverUIObject() && doOnce != 0 && disableTimer >= 3.0f)
                {
                    UIController.Instance.pinchToScale.SetActive(false);
                    UIController.Instance.swipeToRotate.SetActive(false);
                    //UIController.Instance.dragToMove.SetActive(false);
                    TouchManager.Instance.touchPhaseBegan = false;

                    //check if it's the first run
                    //if (AppManager.Instance.doOnce == 0)
                    //{
                    //    UIController.Instance.step6Done = true;
                    //}
                }
            }
        }

      
        if (selectedObj != null && doOnce == 0 && objToSpawn == null)
        {
            PlayerPrefs.SetInt("once", 1);
            doOnce = PlayerPrefs.GetInt("once", 0);
            UIController.Instance.pinchToScale.SetActive(true);
            UIController.Instance.swipeToRotate.SetActive(true);
            //UIController.Instance.dragToMove.SetActive(true);
        }
    }


    /// <summary>
    /// 
    /// </summary>

    private void SelectionTouch()
    {
        //if single touch is registered
        if (Input.touches.Length == 1)
        {
            if (IsPointerOverUIObject())
                return;

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startTouch = Input.GetTouch(0).position;
                //Debug.Log("RARO TOUCH BEGAN - " + startTouch);
                tempTouch = startTouch;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                moveTouch = Input.GetTouch(0).position;
                //Debug.Log("RARO TOUCH MOVED - " + startTouch);
                if (moveTouch.x > tempTouch.x)
                {
                    selectedObj.transform.Rotate(0, -100 * Time.deltaTime, 0);
                }
                else
                {
                    selectedObj.transform.Rotate(0, 100 * Time.deltaTime, 0);
                }

                tempTouch = moveTouch;

                firstRotate = true;

            }

            else if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
            {
                endTouch = Input.GetTouch(0).position;
                //Debug.Log("RARO TOUCH ENDED - " + endTouch);

                if (endTouch == startTouch)
                {
                    Debug.Log("RARO TAP - " + startTouch + " " + endTouch);

                    //tap to activate plane detections
                    if (doubleTapPlacement)
                    {
                        Debug.Log("RARO - try to reposition");
                        //var touchPoint = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
                        ray = Camera.main.ScreenPointToRay(endTouch);
                        //we'll try to hit one of the plane collider gameobjects that were generated by the plugin
                        //effectively similar to calling HitTest with ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent
                        if (Physics.Raycast(ray, out hit, maxRayDistance, collisionLayer))
                        {
                            //we're going to get the position from the contact point
                            selectedObj.transform.position = hit.point;
                            Debug.Log("RARO - new position " + selectedObj.transform.position);

                            //check if it's the first run
                            if (AppManager.Instance.doOnce == 0)
                            {
                                //step1Done = true;
                                if (UIController.Instance.step9Done)
                                {
                                    UIController.Instance.step10Done = true;
                                }
                            }

                            //and the rotation from the transform of the plane collider
                            //selectedObj.transform.rotation = hit.transform.rotation;
                        }
                    }

                    else
                    {
                        if (!FingerOnObject(endTouch) && selectedObj != null)
                        {

                            unityARcamera.planeDetectionON();
                            doubleTapPlacement = true;
                            Debug.Log("RARO - try to restart detection");
                            UIController.Instance.scanPlane.SetActive(true);
                            //check if it's the first run
                            if (AppManager.Instance.doOnce == 0)
                            {
                                //step1Done = true;
                                if (UIController.Instance.step8Done)
                                {
                                    UIController.Instance.step9Done = true;
                               }
                            }
                        }



                    }

                }
            }
        }


        /*
        //two touches registered
        else if (Input.touches.Length == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            if (FingerOnObject(touchZero.position) && FingerOnObject(touchOne.position))
            {
                //sselectedObj.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                if (touchZero.phase == TouchPhase.Moved && touchOne.phase == TouchPhase.Moved)
                {

                    //start the dragging
                    selectedObj.GetComponent<InteractableObject>().isDragging = true;

                    if (PlatformIsIPhone())
                    {
                        //var touchPoint = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
                        ray = Camera.main.ScreenPointToRay(touchZero.position);
                        //we'll try to hit one of the plane collider gameobjects that were generated by the plugin
                        //effectively similar to calling HitTest with ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent
                        if (Physics.Raycast(ray, out hit, maxRayDistance, collisionLayer))
                        {
                            //we're going to get the position from the contact point
                            selectedObj.transform.position = hit.point;
                            //and the rotation from the transform of the plane collider
                            selectedObj.transform.rotation = hit.transform.rotation;
                        }

                    }
                    if (PlatformIsAndroid())
                    {
                        Ray ray = arCamera.ScreenPointToRay(touchZero.position);
                        TrackableHit hit;
                        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon | TrackableHitFlags.FeaturePointWithSurfaceNormal;
                        if (Frame.Raycast(ray.origin, ray.direction, out hit, 1000f, raycastFilter))
                        {
                            if ((hit.Trackable is DetectedPlane) &&
                                Vector3.Dot(arCamera.transform.position - hit.Pose.position, hit.Pose.rotation * Vector3.up) < 0)
                            {
                                Debug.Log("Hit at back of the current DetectedPlane");
                            }
                            else
                            {
                                objToSpawn.transform.position = hit.Pose.position;
                                objToSpawn.transform.rotation = hit.Pose.rotation;
                                var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                                objToSpawn.transform.parent = anchor.transform;
                            }
                        }

                    }


                }
                else if (touchZero.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Canceled ||
                         touchOne.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Canceled)
                {
                    selectedObj.GetComponent<InteractableObject>().isDragging = false;
                    //selectedObj.GetComponent<LeanScale>().enabled = true;
                }



            }

        }
        */
        //-------------------------------

    }

    /// <summary>
    /// Rotates the y.
    /// </summary>
    /// <returns>The y.</returns>
    /// <param name="target">Target.</param>
    /// <param name="current">Current.</param>
    private static Quaternion RotateY(Vector3 target, Vector3 current)
    {
        Vector3 dir = (target - current).normalized;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        lookRot.eulerAngles = new Vector3(0, lookRot.eulerAngles.y, 0);
        return lookRot;
    }

    /// <summary>
    /// Portals the enable.
    /// </summary>
    public void PortalEnable()
    {
        //enable the portal
        portalMode = true;

        //enable the plane detection
        if (PlatformIsIPhone())
        {
            unityARcamera.planeDetectionON();

            //check for plane generated here
            //if (kitDetectedPlanes.Count == 0)
            //{
            //    UIController.Instance.scanPlane.SetActive(true);
            //}
        }

        if (PlatformIsAndroid())
        {
            session.SessionConfig.PlaneFindingMode = DetectedPlaneFindingMode.Horizontal;
            session.OnEnable();
        }

        Debug.Log("RARO PLANE DEETECTION oN");

        //UIController.Instance.portal.SetActive(true);

        Debug.Log("RARO PORTAL ENABLE - "+ portalMode);
    }



    /// <summary>
    /// Portals the placement.
    /// </summary>
    private void PortalPlacement()
    {
        if (!portalMode || portalIsPlaced)
            return;

        Debug.Log("RARO PORTAL PLACEMENT");

        //iphone
        if (PlatformIsIPhone())
        {
            //check for plane generated here
            if (kitDetectedPlanes.Count == 0)
            {
                UIController.Instance.scanPlane.SetActive(true);
            }
            else
            {
                UIController.Instance.scanPlane.SetActive(false);
                //Activate the portal and move it with the camera
                UIController.Instance.portal.SetActive(true);

                var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
                Ray ray = Camera.current.ScreenPointToRay(screenCenter);
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Debug.Log("RARO PORTAL repositioned");

                if (Physics.Raycast(ray, out hit, maxRayDistance, collisionLayer))
                {
                    //we're going to get the position from the contact point
                    UIController.Instance.portal.transform.position = hit.point;
                    //portal faces the camera
                    UIController.Instance.portal.transform.rotation = RotateY(mainCamera.transform.position,
                                                                              UIController.Instance.portal.transform.position);

                }

                if (Input.touches.Length > 0 && !IsPointerOverUIObject())
                {
                    if (Input.touches.Length == 1)
                    {
                        if (Input.GetTouch(0).phase == TouchPhase.Began)
                        {
                            portalIsPlaced = true;
                            unityARcamera.planeDetectionOFF();
                        }
                    }
                }

            }

        }


        //android
        if (PlatformIsAndroid())
        {
            var screenCenter = arCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
            Ray ray = arCamera.ScreenPointToRay(screenCenter);

            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon | TrackableHitFlags.FeaturePointWithSurfaceNormal;
            if (Frame.Raycast(ray.origin, ray.direction, out hit, 1000f, raycastFilter))
            {

                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(arCamera.transform.position - hit.Pose.position, hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("RARO Hit at back of the current DetectedPlane");
                    DetectedPlane plane = (DetectedPlane)hit.Trackable;
                    plane.GetBoundaryPolygon(boundaryPolygonPoints);
                }

            }
            if (boundaryPolygonPoints.Count < 5)
            {
                UIController.Instance.scanPlane.SetActive(true);
            }
            else
            {
                UIController.Instance.scanPlane.SetActive(false);
                //Activate the portal and move it with the camera
                UIController.Instance.portal.SetActive(true);

                UIController.Instance.portal.transform.position = hit.Pose.position;
                var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                UIController.Instance.portal.transform.parent = anchor.transform;
                UIController.Instance.portal.transform.rotation = RotateY(arCamera.transform.position,
                                                                          UIController.Instance.portal.transform.position);


            }

            //on touch when the portal is not yet placed
            if (Input.touches.Length > 0 && !IsPointerOverUIObject())
            {
                if (Input.touches.Length == 1)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        portalIsPlaced = true;

                        session.SessionConfig.PlaneFindingMode = DetectedPlaneFindingMode.Disabled;
                        session.OnEnable();
                        disablePlane = true;
                    }
                }
            }
        }
    }


   /// <summary>
    /// Follows the camera.
    /// </summary>
    private void FollowCamera()
    {
        if (PlatformIsAndroid())
        {
            if (objToSpawn != null)
            {
                //session.SessionConfig.EnablePlaneFinding = true;
                session.SessionConfig.PlaneFindingMode = DetectedPlaneFindingMode.Horizontal;
                session.OnEnable();
                var screenCenter = arCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
                Ray ray = arCamera.ScreenPointToRay(screenCenter);

                if (Input.touches.Length > 0 && !IsPointerOverUIObject())
                {
                    if (Input.touches.Length == 1)
                    {
                        if (Input.GetTouch(0).phase == TouchPhase.Began)
                        {
                            if(objToSpawn.GetComponent<Renderer>().materials[0].color != new Color(1, 0.4f, 0.4f, 0.3f))
                            {
                                if (UIController.Instance.scanPlane.activeInHierarchy)
                                    return;
                                selectedObj = objToSpawn;
                                InteractableObject obj = selectedObj.GetComponent<InteractableObject>();
                                obj.isSelected = true;
                                obj.isPlaced = true;
                                //TurnGhost(false, objToSpawn);
                                objToSpawn = null;
                                UIController.Instance.menuBtnImage.sprite = UIController.Instance.tickSprite;
                                
                                session.SessionConfig.PlaneFindingMode = DetectedPlaneFindingMode.Disabled;
                                session.OnEnable();
                                disablePlane = true;
                                TouchManager.Instance.touchPhaseBegan = false;
                                return;
                            }
                        }
                    }
                }


                TrackableHit hit;
                TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon | TrackableHitFlags.FeaturePointWithSurfaceNormal;
                if (Frame.Raycast(ray.origin, ray.direction, out hit, 1000f, raycastFilter))
                {
                    if ((hit.Trackable is DetectedPlane) &&
                        Vector3.Dot(arCamera.transform.position - hit.Pose.position, hit.Pose.rotation * Vector3.up) < 0)
                    {
                        Debug.Log("RARO Hit at back of the current DetectedPlane");
                    }
                    else
                    {
                        objToSpawn.transform.position = hit.Pose.position;
                        //objToSpawn.transform.rotation = hit.Pose.rotation;
                        var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                        objToSpawn.transform.parent = anchor.transform;
                        objToSpawn.transform.rotation = RotateY(arCamera.transform.position, objToSpawn.transform.position);
                    }
                }
            }
            if (objToSpawn == null && selectedObj == null)
            {
                session.SessionConfig.PlaneFindingMode = DetectedPlaneFindingMode.Disabled;
                session.OnEnable();
            }
        }

        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (objToSpawn != null)
            {
                unityARcamera.planeDetectionON();

                //move the object on the AR plane if it is in the ghost state
                if (Camera.current == null)
                    return;
                //shoot from the center of the screen
                var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
                ray = Camera.current.ScreenPointToRay(screenCenter);

                if (Input.touches.Length > 0 && !IsPointerOverUIObject())
                {
                    if (Input.touches.Length == 1)
                    {
                        if (Input.GetTouch(0).phase == TouchPhase.Began)
                        {
                            //if (objToSpawn.GetComponent<Renderer>().materials[0].color != new Color(1, 0.4f, 0.4f, 0.3f))
                            if(objToSpawn.GetComponent<Renderer>().materials[0] != errorMaterial)
                            {
                                if (UIController.Instance.scanPlane.activeInHierarchy)
                                    return;
                                selectedObj = objToSpawn;
                                InteractableObject obj = selectedObj.GetComponent<InteractableObject>();
                                obj.isSelected = true;
                                obj.isPlaced = true;
                                //TurnGhost(false, objToSpawn);
                                objToSpawn = null;
                                TouchManager.Instance.touchPhaseBegan = false;
                                UIController.Instance.menuBtnImage.sprite = UIController.Instance.tickSprite;
                                

                                unityARcamera.planeDetectionOFF();  //--RARO now
                                return;
                            }
                        }
                    }
                }


                //we'll try to hit one of the plane collider gameobjects that were generated by the plugin
                //effectively similar to calling HitTest with ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent
                if (Physics.Raycast(ray, out hit, maxRayDistance, collisionLayer))
                {
                    //we're going to get the position from the contact point
                    if (objToSpawn.gameObject.name.Contains("brain") ||
                        objToSpawn.gameObject.name.Contains("kidney") ||
                        objToSpawn.gameObject.name.Contains("lung") ||
                        objToSpawn.gameObject.name.Contains("liver") ||
                        objToSpawn.gameObject.name.Contains("intestine") ||
                        objToSpawn.gameObject.name.Contains("heart") ||
                        objToSpawn.gameObject.name.Contains("skeleton"))

                    {
                        objToSpawn.transform.position = hit.point + new Vector3(0, 0.25f, 0);
                    }
                    else
                        objToSpawn.transform.position = hit.point;
                    // ------

                    //objToSpawn.transform.position = hit.point; //+ new Vector3(0, 0.5f, 0);

                    //Debug.Log(string.Format("x:{0:0.######} y:{1:0.######} z:{2:0.######}", m_HitTransform.position.x, m_HitTransform.position.y, m_HitTransform.position.z));

                    //and the rotation from the transform of the plane collider
                    //objToSpawn.transform.rotation = hit.transform.rotation;

                    //--------------------------------------
                    objToSpawn.transform.rotation = RotateY(mainCamera.transform.position, objToSpawn.transform.position);
                    
                    //myText.text = "Touchoutside - " + TouchManager.Instance.touchPhaseBegan;

                    //--------------------------------------

                }
 

            }

            //if (objToSpawn == null && selectedObj == null)
            //{
            //    if (unityARcamera != null)
            //    {
            //       unityARcamera.planeDetectionOFF(); //--RARO now
            //    }
            //}

        }
    }

    /// <summary>
    /// Checks the size of the for plane.
    /// </summary>
    private void CheckForPlaneSize()
    {

        if (!UIController.Instance.scanPlane.activeInHierarchy)
            return;

        if (PlatformIsAndroid())
        {
            var screenCenter = arCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
            Ray ray = arCamera.ScreenPointToRay(screenCenter);
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon | TrackableHitFlags.FeaturePointWithSurfaceNormal;
            if (Frame.Raycast(ray.origin, ray.direction, out hit, 1000f, raycastFilter))
            {
                if (hit.Trackable is DetectedPlane)
                {
                    DetectedPlane plane = (DetectedPlane)hit.Trackable;
                    plane.GetBoundaryPolygon(boundaryPolygonPoints);
                }
            }
            if (boundaryPolygonPoints.Count >= 5)
            {
                DisableScanPlaneAnim();
            }
        }
        else if (PlatformIsIPhone() || PlatformIsMac())
        {

            if (objToSpawn != null || doubleTapPlacement)
            {
                if (kitDetectedPlanes.Count != 0 && UIController.Instance.scanPlane.activeInHierarchy)
                {
                    Debug.Log("RARO plane detected");
                    //DisableScanPlaneAnim();
                    Invoke("DisableScanPlaneAnim", 2);
                }
            }

            /*
            if (objToSpawn != null && kitDetectedPlanes.Count != 0)
            {
                Debug.Log("RARO plane detected");
                DisableScanPlaneAnim();
            }

            if (doubleTapPlacement && kitDetectedPlanes.Count != 0)
            {
                Debug.Log("RARO plane detected");
                DisableScanPlaneAnim();
            }
            */
            //Invoke("DisableScanPlaneAnim", 2);
        }

    }

    /// <summary>
    /// Disables the scan plane animation.
    /// </summary>
    private void DisableScanPlaneAnim()
    {
        if (objToSpawn != null)
        {
            UIController.Instance.scanPlane.SetActive(false);
            objToSpawn.SetActive(true);
        }
        if (doubleTapPlacement)
        {
            UIController.Instance.scanPlane.SetActive(false);
        }

        //Tutorial disable the scan plane animation
        //check if it's the first run
        if (AppManager.Instance.doOnce == 0)
        {
            UIController.Instance.step6Done = true;
        }

    }

    /// <summary>
    /// Loads the texture from memory.
    /// </summary>
    /// <param name="texPath">Tex path.</param>
    private void LoadTextureFromMemory(string texPath)
    {
        Debug.Log("RARO loading icons from memory");
        string[] filePaths = System.IO.Directory.GetFiles(texPath);
        for (int i = 0; i < filePaths.Length; i++)
        {
            texList = new List<Object>();
            texList = Utils.Load<Texture2D>(filePaths[i]);
        }
    }



    /// <summary>
    /// Loads the textures.
    /// </summary>
    private void LoadTextures()
    {
        if (TextureDataPath != string.Empty && !textureReadComplete)
        {
            if (System.IO.File.Exists(TextureDataPath))
            {
                texList = new List<Object>();
                texList = Utils.Load<Texture2D>(TextureDataPath);
                for (int i = 0; i < texList.Count; i++)
                {
                    Debug.Log(texList[i]);
                }
                textureReadComplete = true;
            }
        }
    }

    /// <summary>
    /// Checks for object.
    /// </summary>
    public void CheckForObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 1000f, interactableFilter) && !IsPointerOverUIObject() && objToSpawn == null)
        {
            if(selectedObj != null)
            {
                InteractableObject interactableObject = selectedObj.GetComponent<InteractableObject>();
                interactableObject.isSelected = false;
            }
            selectedObj = hitInfo.collider.gameObject;
            if (PlatformIsAndroid())
            {
                //session.SessionConfig.EnablePlaneFinding = true;
                session.SessionConfig.PlaneFindingMode = DetectedPlaneFindingMode.Horizontal;
                session.OnEnable();
            }
            
            InteractableObject obj = selectedObj.GetComponent<InteractableObject>();
            obj.isSelected = true;
            UIController.Instance.menuBtnImage.sprite = UIController.Instance.tickSprite;
            
            //Debug.Log("RARO hitting obj");
        }
    }


    /// <summary>
    /// Fingers the on object.
    /// </summary>
    /// <returns><c>true</c>, if on object was fingered, <c>false</c> otherwise.</returns>
    /// <param name="screenPos">Screen position.</param>
    private bool FingerOnObject(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 1000f, interactableFilter) && !IsPointerOverUIObject())
            return true;

        else
            return false;
    }


    /// <summary>
    /// Ises the pointer over UIO bject.
    /// </summary>
    /// <returns><c>true</c>, if pointer over UIO bject was ised, <c>false</c> otherwise.</returns>
    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    /// <summary>
    /// Platforms the is android.
    /// </summary>
    /// <returns><c>true</c>, if is android was platformed, <c>false</c> otherwise.</returns>
    public bool PlatformIsAndroid()
    {
        return Application.platform == RuntimePlatform.Android;
    }

    /// <summary>
    /// Platforms the is windows.
    /// </summary>
    /// <returns><c>true</c>, if is windows was platformed, <c>false</c> otherwise.</returns>
    public bool PlatformIsWindows()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    }

    /// <summary>
    /// Platforms the is IP hone.
    /// </summary>
    /// <returns><c>true</c>, if is IP hone was platformed, <c>false</c> otherwise.</returns>
    public bool PlatformIsIPhone()
    {
        return Application.platform == RuntimePlatform.IPhonePlayer;
    }

    /// <summary>
    /// Platforms the is mac.
    /// </summary>
    /// <returns><c>true</c>, if is mac was platformed, <c>false</c> otherwise.</returns>
    public bool PlatformIsMac()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    }
    #endregion

}
