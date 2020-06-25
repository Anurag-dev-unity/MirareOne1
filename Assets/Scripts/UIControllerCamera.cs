using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIControllerCamera : Singleton<UIControllerCamera>
{
    #region publicVariables
    public RectTransform panelRect;

    public GameObject mainMenu;

    public GameObject markerARPage;
    public GameObject campaignPage;

    public GameObject portalPage;
    public GameObject plusPortalPage;

    public GameObject markerlessMRPage;
    public GameObject plusMarkerlessPage;

    public Sprite[] sprites;
    public Image menuBtnImage;
    public ScrollRect panelScrollRect;
    public GameObject scrollbarObj;
    public Scrollbar scrollbar;

    public Text guideText;


    public GameObject menuBtn;
    public GameObject markerBtn;
    public GameObject markerlessBtn;
    public GameObject portalBtn;
    public GameObject campaignBtn;

    public GameObject markerlessTitleBtn;
    public GameObject defenceBtn;
    public GameObject educationBtn;
    public GameObject furnitureBtn;
    public GameObject automobileBtn;
    public GameObject markerlessPlusBtn;

    public GameObject tutorialBg;
    public GameObject tutorialStep1;
    public GameObject tutorialStep2;
    public GameObject tutorialStep3;

    public bool step1Done;
    public bool step2Done;
    public bool step3Done;

    #endregion

    #region privateVariables
    private RectTransform mainMenuRect;

    private RectTransform markerARPageRect;
    private RectTransform campaignPageRect;

    private RectTransform portalPageRect;
    private RectTransform plusPortalPageRect;

    private RectTransform markerlessMRPageRect;
    private RectTransform plusMarkerlessPageRect;

    private bool openMainMenu = false;

    private bool openMarkerARPage = false;
    private bool openCampaignPage = false;

    private bool openPortalPage = false;
    private bool openPlusPortalPage = false;

    private bool openMarkerlessMRPage = false;
    private bool openPlusMarkerlessMRPage = false;

    private float lerpSpeed = 5000.0f;
    private GameObject currentPage;
    private GameObject backPage;
    //private Vector2 targetVector;
    private bool menuIconsSet = false;

    private Image furniturePlusImage;
    private Image healthcarePlusImage;
    private Image sofaPlusImage;

    private Image portalPlusImage;
    private Image markerlessPlusImage;


    private List<Image> menuIcons;
    #endregion

    #region enums
    /// <summary>
    /// Direction.
    /// </summary>
    private enum Direction
    {
        Null,
        BT,     // Bottom to top
        TB,     // Top to Bottom
        RL      // Right to Left
    }
    #endregion

    /// <summary>
    /// Start this instance.
    /// </summary>
    private void Start()
    {
        menuIcons = new List<Image>();
        for (int i = 0; i < mainMenu.transform.childCount; i++)
        {
            menuIcons.Add(mainMenu.transform.GetChild(i).GetComponent<Image>());
        }

        mainMenuRect = mainMenu.GetComponent<RectTransform>();

        markerARPageRect = markerARPage.GetComponent<RectTransform>();
        campaignPageRect = campaignPage.GetComponent<RectTransform>();

        portalPageRect = portalPage.GetComponent<RectTransform>();
        plusPortalPageRect = plusPortalPage.GetComponent<RectTransform>();

        portalPlusImage = portalPage.transform.GetChild(portalPage.transform.childCount - 1).GetComponent<Image>();

        markerlessMRPageRect = markerlessMRPage.GetComponent<RectTransform>();
        plusMarkerlessPageRect = plusMarkerlessPage.GetComponent<RectTransform>();

        markerlessPlusImage = markerlessMRPage.transform.GetChild(markerlessMRPage.transform.childCount - 1).GetComponent<Image>();
        //myVariable = PlayerPrefs.GetFloat("Player Score");
    }

    /// <summary>
    /// Update this instance.
    /// </summary>
    private void Update()
    {
        TogglePages();
        ToggleScrollbar();
    }

    #region privateMethods
    /// <summary>
    /// Toggles the pages.
    /// </summary>
    private void TogglePages()
    {
        MainMenu();
        MarkerARPage();
        CampaignPage();
        PortalPage();
        MarkerlessMRPage();
    }


    /// <summary>
    /// Main Menu page.
    /// </summary>
    private void MainMenu()
    {
        if (!panelScrollRect.enabled)
            MoveObj(openMainMenu, mainMenu, mainMenuRect, Direction.BT, panelScrollRect, new Vector2(0, (panelRect.rect.height - mainMenuRect.rect.height) / 2f));
        else if (!openMainMenu)
        {
            if (Vector2.Distance(mainMenuRect.anchoredPosition, new Vector2(0, -mainMenuRect.rect.height)) > 1)
                MoveObj(openMainMenu, mainMenu, mainMenuRect, Direction.BT);
        }
    }

    /// <summary>
    /// Marker AR page.
    /// </summary>
    private void MarkerARPage()
    {
        if (!panelScrollRect.enabled)
            MoveObj(openMarkerARPage, markerARPage, markerARPageRect, Direction.BT, panelScrollRect, new Vector2(0, (panelRect.rect.height - markerARPageRect.rect.height) / 2f));
        else if (!openMarkerARPage)
        {
            if (Vector2.Distance(markerARPageRect.anchoredPosition, new Vector2(0, -markerARPageRect.rect.height)) > 1)
                MoveObj(openMarkerARPage, markerARPage, markerARPageRect, Direction.BT);
        }

        //MoveObj(openPlusHealthcarePage, plusHealthcarePage, plusHealthcarePageRect, Direction.RL);
    }


    /// <summary>
    /// Campaign Page.
    /// </summary>
    private void CampaignPage()
    {
        if (!panelScrollRect.enabled)
            MoveObj(openCampaignPage, campaignPage, campaignPageRect, Direction.BT, panelScrollRect, new Vector2(0, (panelRect.rect.height - campaignPageRect.rect.height) / 2f));
        else if (!openCampaignPage)
        {
            if (Vector2.Distance(campaignPageRect.anchoredPosition, new Vector2(0, -campaignPageRect.rect.height)) > 1)
                MoveObj(openCampaignPage, campaignPage, campaignPageRect, Direction.BT);
        }

        //MoveObj(openPlusHealthcarePage, plusHealthcarePage, plusHealthcarePageRect, Direction.RL);
    }


    /// <summary>
    /// Portal Page.
    /// </summary>
    private void PortalPage()
    {
        if (!panelScrollRect.enabled)
            MoveObj(openPortalPage, portalPage, portalPageRect, Direction.BT, panelScrollRect, new Vector2(0, (panelRect.rect.height - portalPageRect.rect.height) / 2f));
        else if (!openPortalPage)
        {
            if (Vector2.Distance(portalPageRect.anchoredPosition, new Vector2(0, -portalPageRect.rect.height)) > 1)
                MoveObj(openPortalPage, portalPage, portalPageRect, Direction.BT);
        }

        MoveObj(openPlusPortalPage, plusPortalPage, plusPortalPageRect, Direction.RL);
    }


    /// <summary>
    /// Markerless MR Page.
    /// </summary>
    private void MarkerlessMRPage()
    {
        if (!panelScrollRect.enabled)
            MoveObj(openMarkerlessMRPage, markerlessMRPage, markerlessMRPageRect, Direction.BT, panelScrollRect, new Vector2(0, (panelRect.rect.height - markerlessMRPageRect.rect.height) / 2f));
        else if (!openMarkerlessMRPage)
        {
            if (Vector2.Distance(markerlessMRPageRect.anchoredPosition, new Vector2(0, -markerlessMRPageRect.rect.height)) > 1)
                MoveObj(openMarkerlessMRPage, markerlessMRPage, markerlessMRPageRect, Direction.BT);
        }

        MoveObj(openPlusMarkerlessMRPage, plusMarkerlessPage, plusMarkerlessPageRect, Direction.RL);
    }


    /// <summary>
    /// Toggles the scrollbar.
    /// </summary>
    private void ToggleScrollbar()
    {
        if (panelScrollRect.velocity == Vector2.zero)
        {
            scrollbarObj.SetActive(false);
            panelScrollRect.verticalScrollbar = null;
        }
        else
        {
            panelScrollRect.verticalScrollbar = scrollbar;
            panelScrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
            scrollbarObj.SetActive(true);
        }
    }


    /// <summary>
    /// Moves the object.
    /// </summary>
    /// <param name="b">If set to <c>true</c> b.</param>
    /// <param name="go">Go.</param>
    /// <param name="rt">Rt.</param>
    /// <param name="dir">Dir.</param>
    /// <param name="sr">Sr.</param>
    /// <param name="target">Target.</param>
    private void MoveObj(bool b, GameObject go, RectTransform rt, Direction dir = Direction.Null, ScrollRect sr = null, Vector2 target = default)
    {
        if (b && (dir == Direction.BT || dir == Direction.TB))
        {
            go.SetActive(true);
            rt.anchoredPosition = Vector2.MoveTowards(rt.anchoredPosition, target, lerpSpeed * Time.deltaTime);
            if (Vector2.Distance(rt.anchoredPosition, target) <= 1)
            {
                //Debug.Log(rt.anchoredPosition);
                rt.anchoredPosition = target;
                if (sr != null)
                {
                    if (sr.content == null)
                    {
                        sr.enabled = !sr.enabled;
                        sr.content = rt;
                        sr.verticalScrollbar = scrollbar;
                        sr.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
                    }
                }
            }
        }
        else if (b && dir == Direction.RL)
        {
            go.SetActive(true);
            rt.anchoredPosition = Vector2.MoveTowards(rt.anchoredPosition, target, lerpSpeed * Time.deltaTime);
            if (Vector2.Distance(rt.anchoredPosition, target) <= 1)
            {
                rt.anchoredPosition = target;
                if (sr != null)
                {
                    if (sr.content == null)
                    {
                        sr.enabled = !sr.enabled;
                        sr.content = rt;
                    }
                }
            }
        }
        else if (!b && dir == Direction.BT)
        {
            rt.anchoredPosition = Vector2.MoveTowards(rt.anchoredPosition, new Vector2(0, -rt.rect.height), lerpSpeed * Time.deltaTime);
            if (rt.anchoredPosition.y <= -rt.rect.height)
            {
                rt.anchoredPosition = new Vector2(0, -rt.rect.height);
                go.SetActive(false);
                //myScrollRect.content = rt;
            }
        }
        else if (!b && dir == Direction.TB)
        {
            rt.anchoredPosition = Vector2.MoveTowards(rt.anchoredPosition, new Vector2(0, rt.rect.height), lerpSpeed * Time.deltaTime);
            if (rt.anchoredPosition.y >= rt.rect.height)
                go.SetActive(false);
        }
        else if (!b && dir == Direction.RL)
        {
            rt.anchoredPosition = Vector2.MoveTowards(rt.anchoredPosition, new Vector2(rt.rect.width, target.y), lerpSpeed * Time.deltaTime);
            if (sr != null)
            {
                if (sr.content != null)
                {
                    sr.enabled = !sr.enabled;
                    sr.content = null;
                }
            }
            if (rt.anchoredPosition.x >= rt.rect.width)
                go.SetActive(false);
        }
    }

    /// <summary>
    /// Swaps the sprite.
    /// </summary>
    /// <param name="b">If set to <c>true</c> b.</param>
    /// <param name="image">Image.</param>
    /// <param name="sprite1">Sprite1.</param>
    /// <param name="sprite2">Sprite2.</param>
    private void SwapSprite(bool b, Image image, Sprite sprite1, Sprite sprite2)
    {
        if (b)
            image.sprite = sprite1;
        else
            image.sprite = sprite2;
    }
    #endregion

    #region publicMethods
    /// <summary>
    /// Toggles the main menu.
    /// </summary>
    public void ToggleMainMenu()
    {

        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex == 1) //if it is the device camera scene
        {
            //Tutorial Step 1
            //check if it's the first run
            if (DeviceCameraManager.Instance.doOnce == 0)
            {
                step1Done = true;
            }
        }


        //page1BtnImage.sprite = sprites[1];

        openMainMenu = !openMainMenu;

        if (!openMainMenu)
        {
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
            //if no object is spawned or  placed, go back to vuforia scene
            Debug.Log("RARO PAGE DOWN");

            //if (spawnedObjects.childCount == 0)
            //{
            //    Invoke("MoveToVuforia", 0.5f);
            //}

        }


        if (currentPage == markerARPage)
        {
            openMarkerARPage = false;
            //openPlusFurniturePage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }

        if (currentPage == campaignPage)
        {
            openCampaignPage = false;
            //openPlusFurniturePage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }

        if (currentPage == portalPage)
        {
            openPortalPage = false;
            openPlusPortalPage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }

        if (currentPage == markerlessMRPage)
        {
            openMarkerlessMRPage = false;
            openPlusMarkerlessMRPage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }

        SwapSprite(openMainMenu, menuBtnImage, sprites[1], sprites[0]);
    }

    /// <summary>
    /// Moves to vuforia.
    /// </summary>
    public void MoveToVuforia()
    {
        guideText.text = "Loading...";
        SceneManager.LoadScene(3);
    }

    /// <summary>
    /// Toggles the healthcare page.
    /// </summary>
    public void ToggleMarkerARPage()
    {
        openMainMenu = !openMainMenu;
        openMarkerARPage = !openMarkerARPage;
        if (!openMainMenu)
        {
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }

        currentPage = markerARPage;
        backPage = mainMenu;
    }


    /// <summary>
    /// Toggles the campaign page.
    /// </summary>
    public void ToggleCampaignPage()
    {
        openMainMenu = !openMainMenu;
        openCampaignPage = !openCampaignPage;
        if (!openMainMenu)
        {
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }

        currentPage = campaignPage;
        backPage = mainMenu;
    }


    /// <summary>
    /// Toggles the portal page.
    /// </summary>
    public void TogglePortalPage()
    {
        openMainMenu = !openMainMenu;
        openPortalPage = !openPortalPage;
        if (!openMainMenu)
        {
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }

        currentPage = portalPage;
        backPage = mainMenu;
    }

    /// <summary>
    /// Toggles the plus portal page.
    /// </summary>
    public void TogglePlusPortalPage()
    {
        openPlusPortalPage = !openPlusPortalPage;

        SwapSprite(openPlusPortalPage, portalPlusImage, sprites[3], sprites[2]);
    }



    /// <summary>
    /// Toggles the markerless MR page.
    /// </summary>
    public void ToggleMarkerlessMRPage()
    {

        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex == 1) //if it is the device camera scene
        {
            //Tutorial Step2
            //check if it's the first run
            if (DeviceCameraManager.Instance.doOnce == 0)
            {
                step2Done = true;
            }
        }


        openMainMenu = !openMainMenu;
        openMarkerlessMRPage = !openMarkerlessMRPage;
        if (!openMainMenu)
        {
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }

        currentPage = markerlessMRPage;
        backPage = mainMenu;
    }



    /// <summary>
    /// Toggles the plus portal page.
    /// </summary>
    public void TogglePlusMarkerlessMRPage()
    {
        openPlusMarkerlessMRPage = !openPlusMarkerlessMRPage;

        SwapSprite(openPlusMarkerlessMRPage, markerlessPlusImage, sprites[3], sprites[2]);
    }

    /// <summary>
    /// Backs the page.
    /// </summary>
    public void BackPage()
    {
        panelScrollRect.content = null;
        panelScrollRect.enabled = false;

        if (currentPage == markerARPage)
        {
            openMainMenu = !openMainMenu;
            openMarkerARPage = !openMarkerARPage;

            //if (!openHealthcarePage)
            //    openPlusHealthcarePage = false;

            //SwapSprite(openPlusHealthcarePage, healthcarePlusImage, sprites[1], sprites[0]);
            currentPage = backPage;
        }

        if (currentPage == campaignPage)
        {
            openMainMenu = !openMainMenu;
            openCampaignPage = !openCampaignPage;

            //if (!openHealthcarePage)
            //    openPlusHealthcarePage = false;

            //SwapSprite(openPlusHealthcarePage, healthcarePlusImage, sprites[1], sprites[0]);
            currentPage = backPage;
        }

        if (currentPage == portalPage)
        {
            openMainMenu = !openMainMenu;
            openPortalPage = !openPortalPage;

            if (!openPortalPage)
                openPlusPortalPage = false;

            SwapSprite(openPlusPortalPage, portalPlusImage, sprites[3], sprites[2]);
            currentPage = backPage;
        }

        if (currentPage == markerlessMRPage)
        {
            openMainMenu = !openMainMenu;
            openMarkerlessMRPage = !openMarkerlessMRPage;

            if (!openMarkerlessMRPage)
                openPlusMarkerlessMRPage = false;

            SwapSprite(openPlusMarkerlessMRPage, markerlessPlusImage, sprites[3], sprites[2]);
            currentPage = backPage;
        }

    }


    #endregion
}
