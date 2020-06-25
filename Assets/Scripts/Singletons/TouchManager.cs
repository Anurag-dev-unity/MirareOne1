using GoogleARCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchManager : Singleton<TouchManager>
{

    #region publicVariables
    public bool oneFingerTouch;
    public bool twoFingerTouch;
    public bool touchTap;
    public bool touchHoldToMove;
    public bool pinchOut;
    public bool pinchIn;
    public Vector2 startTouch;
    public Vector2 endTouch;
    public Camera FirstPersonCamera;
    public GameObject cubePrefab;
    private const float k_ModelRotation = 180.0f;
    public bool touchPhaseBegan;

    #endregion publicVariables

    #region privateVariables
    private bool onTouchHold = false;


    private Vector3 currentAngle;
    private Ray ray;
    private RaycastHit hitObject;
    private Quaternion targetAngle;
    private Vector3 targetScale;
    private float rotationSpeed = 0.2f;
    private bool firstTap = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //cubePrefab = Instantiate(cubePrefab, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        touchTap = false;
        CheckForTouch();
    }

    public void CheckForTouch()
    {
        #region touchGestures
        //------------------------------------------------------
        //lets check for inputs over non ui portions of screen
        if (Input.touches.Length > 0 && !IsPointerOverUIObject())
        {
            //if single touch is registered
            if (Input.touches.Length == 1)
            {

                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    startTouch = Input.GetTouch(0).position;
                    Debug.Log("RARO TOUCH BEGAN - " + startTouch);
                    touchPhaseBegan = true;
                }

                    

                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    touchHoldToMove = true;
                    Debug.Log("RARO TOUCH MOVED - " + startTouch);
                }

                else if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
                {
                    endTouch = Input.GetTouch(0).position;
                    Debug.Log("RARO TOUCH ENDED - " + endTouch);
                    touchHoldToMove = false;


                    if (endTouch == startTouch)
                    {
                        Debug.Log("RARO TAP - " + startTouch + " " + endTouch);
                        touchTap = true;
                    }
                    else
                    {
                        touchTap = false;
                    }
                }
            }
            //two touches registered
            else if (Input.touches.Length == 2)
            {
                twoFingerTouch = true;

                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPosition = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPosition = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPosition - touchOnePrevPosition).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;


                if (currentMagnitude > prevMagnitude)
                {
                    Debug.Log("RARO 2 touch PINCH OUT");
                    pinchOut = true;


                    //scale up the object by 0.5
                    //targetScale = selectedObj.transform.localScale + new Vector3(0.5f, 0.5f, 0.5f);
                    //selectedObj.transform.localScale = Vector3.Slerp(selectedObj.transform.localScale, targetScale, 0.1f);
                }
                else
                {
                    Debug.Log("RARO 2 touch PINCH IN");
                    pinchIn = true;
                }
            }
        }

        #endregion touchGestures

    }

    /// <summary>
    /// Is the pointer over any UI object or somewhere else on the screen.
    /// </summary>
    /// <returns><c>true</c>, if pointer over UI Object was registered, <c>false</c> otherwise.</returns>
    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

}
