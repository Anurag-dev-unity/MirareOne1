using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeviceCameraManager : Singleton<DeviceCameraManager>
{

    public int doOnce;
    public Text guideText;

    // Start is called before the first frame update
    void Start()
    {

        Debug.unityLogger.logEnabled = true;

        //first run on the devices
        //doOnce = PlayerPrefs.GetInt("once", 0);

        //doOnce = 0;
        //if (doOnce == 0)
        //    ToggleTutorial(true);
        //else
        //    ToggleTutorial(false);


        //first run on the devices
        doOnce = PlayerPrefs.GetInt("once", 0);
        //doOnce = 1;

        if (doOnce == 0)
        {
            //UIController.Instance.ToggleMainMenu();
        }

        else
        {
            FinishTutorials();
            //UIController.Instance.ToggleMainMenu();
        }


    }

    //private void ToggleTutorial(bool show)
    //{
    //    //Step 1
    //    //Point to AR/MR button to move to the next scene if true
    //    tutorialBg.SetActive(show);
    //    tutorialStep1.SetActive(show);
    //}


    // Update is called once per frame
    void Update()
    {

        UserTutorial();

    }


    /// <summary>
    /// Users the tutorial.
    /// </summary>
    private void UserTutorial()
    {
        //if already ran once
        if (doOnce == 1)
            return;

        Debug.Log("RARO FIRST RUN");

        //Step 1 -
        //Point to the menu buton
        //darken the canvas
        UIControllerCamera.Instance.tutorialBg.SetActive(true);
        UIControllerCamera.Instance.tutorialStep1.SetActive(true);

        //Step 2-
        //Point to markerless MR page
        if (UIControllerCamera.Instance.step1Done)
        {
            UIControllerCamera.Instance.tutorialStep1.SetActive(false);
            UIControllerCamera.Instance.tutorialStep2.SetActive(true);

            //Mask the menu button
            UIControllerCamera.Instance.menuBtn.transform.GetChild(0).gameObject.SetActive(true);
            UIControllerCamera.Instance.menuBtn.GetComponent<Button>().enabled = false;

            //Unmask the markerless AR button
            UIControllerCamera.Instance.markerlessBtn.transform.GetChild(0).gameObject.SetActive(false);
            UIControllerCamera.Instance.markerlessBtn.GetComponent<Button>().enabled = true;
        }

        //Step 3-
        //Point to furniture
        if (UIControllerCamera.Instance.step2Done)
        {
            UIControllerCamera.Instance.tutorialStep2.SetActive(false);
            UIControllerCamera.Instance.tutorialStep3.SetActive(true);

            //Unmask the furniture button
            UIControllerCamera.Instance.furnitureBtn.transform.GetChild(0).gameObject.SetActive(false);
            UIControllerCamera.Instance.furnitureBtn.GetComponent<Button>().enabled = true;
        }

        /*
        //Done tutorials
        if (UIController.Instance.step8Done)
        {

            //lighten the canvas
            UIController.Instance.tutorialBg.SetActive(false);

            UIController.Instance.tutorialStep8.SetActive(false);

            PlayerPrefs.SetInt("once", 1);
            doOnce = PlayerPrefs.GetInt("once", 0);

            //FinishTutorials();

        }
        */

    }


    /// <summary>
    /// Finishes the tutorials.
    /// </summary>
    private void FinishTutorials()
    {
        //lighten the canvas
        UIControllerCamera.Instance.tutorialBg.SetActive(false);

        //unmask and enable all buttons
        UIControllerCamera.Instance.menuBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIControllerCamera.Instance.menuBtn.GetComponent<Button>().enabled = true;

        UIControllerCamera.Instance.markerBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIControllerCamera.Instance.markerBtn.GetComponent<Button>().enabled = true;

        UIControllerCamera.Instance.markerlessBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIControllerCamera.Instance.markerlessBtn.GetComponent<Button>().enabled = true;

        UIControllerCamera.Instance.portalBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIControllerCamera.Instance.portalBtn.GetComponent<Button>().enabled = true;

        UIControllerCamera.Instance.campaignBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIControllerCamera.Instance.campaignBtn.GetComponent<Button>().enabled = true;

        UIControllerCamera.Instance.markerlessTitleBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIControllerCamera.Instance.markerlessTitleBtn.GetComponent<Button>().enabled = true;

        UIControllerCamera.Instance.defenceBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIControllerCamera.Instance.defenceBtn.GetComponent<Button>().enabled = true;

        UIControllerCamera.Instance.educationBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIControllerCamera.Instance.educationBtn.GetComponent<Button>().enabled = true;

        UIControllerCamera.Instance.furnitureBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIControllerCamera.Instance.furnitureBtn.GetComponent<Button>().enabled = true;

        UIControllerCamera.Instance.automobileBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIControllerCamera.Instance.automobileBtn.GetComponent<Button>().enabled = true;

        UIControllerCamera.Instance.markerlessPlusBtn.transform.GetChild(0).gameObject.SetActive(false);
        UIControllerCamera.Instance.markerlessPlusBtn.GetComponent<Button>().enabled = true;

    }


    /// <summary>
    /// Moves to markerless.
    /// </summary>
    public void MoveToMarkerless(string optionName)
    {
        Debug.Log("RARO Moving to Markerless");

        guideText.text = "Loading...";

        //put the option that is to be loaded in the next scene
        PlayerPrefs.SetString("Option", optionName);

        //lighten the canvas
        //tutorialBg.SetActive(false);

        //disable the tutorial
        //tutorialStep1.SetActive(false);

        //for iOS builds, remove the arcore scene
        //SceneManager.LoadSceneAsync(2);
        SceneManager.LoadScene(2);

    }

    /// <summary>
    /// Comings the soon.
    /// </summary>
    public void ComingSoon()
    {
        guideText.text = "Coming Soon";
        StartCoroutine(FlashMessage());
    }

    /// <summary>
    /// Flashs the message.
    /// </summary>
    /// <returns>The message.</returns>
    IEnumerator FlashMessage()
    {
        yield return new WaitForSeconds(2.0f);
        guideText.text = "";
    }

}
