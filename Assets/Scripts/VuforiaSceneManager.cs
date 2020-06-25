using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VuforiaSceneManager : MonoBehaviour
{

    public GameObject tutorialBg;
    public GameObject tutorialStep1;
    public int doOnce;
    public Text guideText;
    public GameObject instruction;

    // Start is called before the first frame update
    void Start()
    {

        ////first run on the devices
        //doOnce = PlayerPrefs.GetInt("once", 0);

        //doOnce = 1;

        //if (doOnce == 0)
        //    ToggleTutorial(true);
        //else
            //ToggleTutorial(false);
            

    }


    private void ToggleTutorial(bool show)
    {
        //Step 1
        //Point to AR/MR button to move to the next scene if true
        tutorialBg.SetActive(show);
        tutorialStep1.SetActive(show);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Moves to markerless.
    /// </summary>
    public void MoveToMarkerless(string optionName)
    {
        Debug.Log("RARO Moving to Markerless");

        instruction.SetActive(false);

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
        instruction.SetActive(false);
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
        instruction.SetActive(true);
    }


}
