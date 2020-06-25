using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using Lean.Touch;
using GoogleARCore;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class UIController : Singleton<UIController>
{
    #region publicVariables
    public RectTransform panelRect;

    public GameObject mainMenu;

    public GameObject markerARPage;

    public GameObject campaignPage;

    public GameObject markerlessMRPage;
    public GameObject plusMarkerlessPage;

    public GameObject healthcarePage;
    public GameObject plusHealthcarePage;

    public GameObject furniturePage;
    public GameObject plusFurniturePage;

    public GameObject sofaPage;
    public GameObject plusSofaPage;

    public GameObject chairPage;
    public GameObject plusChairPage;

    public GameObject tablePage;
    public GameObject plusTablePage;

    public GameObject bedPage;
    public GameObject plusBedPage;

    public GameObject portalPage;
    public GameObject plusPortalPage;

    public GameObject selectionMenu;
    public GameObject colorsPage;

    public Sprite[] sprites;

    public Image menuBtnImage;

    public Transform spawnedObjects;



    public ScrollRect panelScrollRect;
    public GameObject scrollbarObj;
    public Scrollbar scrollbar;
    public ScrollRect myScrollRect2;

    public GameObject deleteBtn;

    public GameObject pinchToScale;
    public GameObject scanPlane;
    public GameObject swipeToRotate;
    public GameObject downloadingObj;

    public Sprite tickSprite;

    public GameObject cancelDownloadObj;
    public GameObject checkNetTextObj;

    public GameObject tutorialBg;
    //public GameObject tutorialStep1;
    //public GameObject tutorialStep2;
    //public GameObject tutorialStep3;
    public GameObject tutorialStep4;
    public GameObject tutorialStep5;
    public GameObject tutorialStep6;
    public GameObject tutorialStep7;
    public GameObject tutorialStep8;
    public GameObject tutorialStep9;
    public GameObject tutorialStep10;
    public GameObject tutorialStep11;

    public GameObject arBtn;
    public GameObject Tickbutton;
    public GameObject automobileBtn;
    public GameObject furnitureBtn;
    public GameObject portalBtn;

    public GameObject healthcareBtn;
    public GameObject defenceBtn;
    public GameObject educationBtn;


    public GameObject furnitureTitleBtn;
    public GameObject sofaBtn;
    public GameObject chairBtn;
    public GameObject tableBtn;
    public GameObject bedBtn;
    public GameObject plusBtn;

    public GameObject sofaTitleBtn;
    public GameObject sofaSofaBtn;
    public GameObject sofa1Btn;
    public GameObject sofa2Btn;
    public GameObject sofa3Btn;
    public GameObject sofaPlusBtn;

    public GameObject priceBtn;
    public GameObject seatingBtn;
    public GameObject metalBtn;
    public GameObject alloysBtn;
    public GameObject colorsBtn;
    public GameObject woodenBtn;

    public bool step1Done;
    public bool step2Done;
    public bool step3Done;
    public bool step4Done;
    public bool step5Done;
    public bool step6Done;
    public bool step7Done;
    public bool step8Done;
    public bool step9Done;
    public bool step10Done;
    public bool step11Done;

    public GameObject portal;

    public Text guideText;
    public GameObject confirmDialog;
    public GameObject instructionDialog;

    public Material[] portalMats;

    public GameObject pooledObjs;
    public GameObject spawnedObjs;
    #endregion

    #region privateVariables
    private RectTransform mainMenuRect;

    private RectTransform markerARPageRect;
    private RectTransform campaignPageRect;

    private RectTransform portalPageRect;
    private RectTransform plusPortalPageRect;

    private RectTransform markerlessMRPageRect;
    private RectTransform plusMarkerlessPageRect;

    private RectTransform healthcarePageRect;
    private RectTransform plusHealthcarePageRect;

    private RectTransform furniturePageRect;
    private RectTransform plusFurniturePageRect;

    private RectTransform sofaPageRect;
    private RectTransform plusSofaPageRect;

    private RectTransform chairPageRect;
    private RectTransform plusChairPageRect;

    private RectTransform tablePageRect;
    private RectTransform plusTablePageRect;

    private RectTransform bedPageRect;
    private RectTransform plusBedPageRect;

    private RectTransform selectionMenuRect;
    private RectTransform colorsPageRect;


    private bool openMainMenu = false;

    private bool openMarkerARPage = false;
    private bool openCampaignPage = false;

    private bool openPortalPage = false;
    private bool openPlusPortalPage = false;

    private bool openMarkerlessMRPage = false;
    private bool openPlusMarkerlessMRPage = false;

    private bool openHealthcarePage = false;
    private bool openFurniturePage = false;

    private bool openPlusHealthcarePage = false;
    private bool openPlusFurniturePage = false;

    private bool openSofaPage = false;
    private bool openPlusSofaPage = false;

    private bool openChairPage = false;
    private bool openPlusChairPage = false;

    private bool openTablePage = false;
    private bool openPlusTablePage = false;

    private bool openBedPage = false;
    private bool openPlusBedPage = false;

    private bool openSelectionMenu = false;
    private bool openColorsPage = false;


    private float lerpSpeed = 5000.0f;
    private GameObject currentPage;
    private GameObject backPage;
    //private Vector2 targetVector;
    private bool menuIconsSet = false;

    private Image furniturePlusImage;
    private Image healthcarePlusImage;
    private Image sofaPlusImage;
    private Image chairPlusImage;
    private Image tablePlusImage;
    private Image bedPlusImage;
    private Image portalPlusImage;
    private Image markerlessPlusImage;

    private List<Image> menuIcons;

    private string optionClick;

    private bool objPlaceInProg;
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

        healthcarePageRect = healthcarePage.GetComponent<RectTransform>();
        plusHealthcarePageRect = plusHealthcarePage.GetComponent<RectTransform>();

        furniturePageRect = furniturePage.GetComponent<RectTransform>();
        plusFurniturePageRect = plusFurniturePage.GetComponent<RectTransform>();

        sofaPageRect = sofaPage.GetComponent<RectTransform>();
        plusSofaPageRect = plusSofaPage.GetComponent<RectTransform>();

        chairPageRect = chairPage.GetComponent<RectTransform>();
        plusChairPageRect = plusChairPage.GetComponent<RectTransform>();

        tablePageRect = tablePage.GetComponent<RectTransform>();
        plusTablePageRect = plusTablePage.GetComponent<RectTransform>();

        bedPageRect = bedPage.GetComponent<RectTransform>();
        plusBedPageRect = plusBedPage.GetComponent<RectTransform>();

        portalPageRect = portalPage.GetComponent<RectTransform>();
        plusPortalPageRect = plusPortalPage.GetComponent<RectTransform>();

        markerlessMRPageRect = markerlessMRPage.GetComponent<RectTransform>();
        plusMarkerlessPageRect = plusMarkerlessPage.GetComponent<RectTransform>();

        selectionMenuRect = selectionMenu.GetComponent<RectTransform>();
        colorsPageRect = colorsPage.GetComponent<RectTransform>();

        furniturePlusImage = furniturePage.transform.GetChild(furniturePage.transform.childCount - 1).GetComponent<Image>();
        healthcarePlusImage = healthcarePage.transform.GetChild(healthcarePage.transform.childCount - 1).GetComponent<Image>();
        sofaPlusImage = sofaPage.transform.GetChild(sofaPage.transform.childCount - 1).GetComponent<Image>();

        chairPlusImage = chairPage.transform.GetChild(chairPage.transform.childCount - 1).GetComponent<Image>();
        tablePlusImage = tablePage.transform.GetChild(tablePage.transform.childCount - 1).GetComponent<Image>();
        bedPlusImage = bedPage.transform.GetChild(bedPage.transform.childCount - 1).GetComponent<Image>();


        portalPlusImage = portalPage.transform.GetChild(portalPage.transform.childCount - 1).GetComponent<Image>();
        markerlessPlusImage = markerlessMRPage.transform.GetChild(markerlessMRPage.transform.childCount - 1).GetComponent<Image>();


        //get the option that is to be loaded in the next scene
        optionClick = PlayerPrefs.GetString("Option");

        //optionClick = "no";
        Debug.Log("RARO clicked - " + optionClick);

        //if (optionClick != null)
        //{
        //    SpawnObject2(optionClick);
        //}

        if (optionClick.Contains("chair"))
        {
            SpawnObject2(optionClick);
        }


        else if (optionClick.Contains("car1"))
        {
            UIController.Instance.ToggleMainMenu();
            UIController.Instance.ToggleMarkerlessMRPage();
            //UIController.Instance.ToggleFurniturePage();

            SpawnObject2(optionClick);
        }


        else if (optionClick == "furniture")
        {
            UIController.Instance.ToggleMainMenu();
            UIController.Instance.ToggleMarkerlessMRPage();
            UIController.Instance.ToggleFurniturePage();
        }
        else if(optionClick == "healthcare")
        {
            UIController.Instance.ToggleMainMenu();
            UIController.Instance.ToggleMarkerlessMRPage();
            UIController.Instance.ToggleHealthcarePage();
            //openMarkerlessMRPage = false;
        }
        else if (optionClick == "portal1")
        {

            if (spawnedObjects.childCount != 0)
            {
                confirmDialog.SetActive(true);
            }
            else
            {
                UIController.Instance.ToggleMainMenu();
                UIController.Instance.PortalPage();
                ShowPortal(portalMats[0]);
            }


        }
        else if (optionClick == "portal2")
        {
            if (spawnedObjects.childCount != 0)
            {
                confirmDialog.SetActive(true);
            }
            else
            {
                UIController.Instance.ToggleMainMenu();
                UIController.Instance.PortalPage();
                ShowPortal(portalMats[1]);
            }
        }
        else if (optionClick == "portal3")
        {
            if (spawnedObjects.childCount != 0)
            {
                confirmDialog.SetActive(true);
            }
            else
            {
                UIController.Instance.ToggleMainMenu();
                UIController.Instance.PortalPage();
                ShowPortal(portalMats[2]);
            }
        }
        else if (optionClick == "portal4")
        {
            if (spawnedObjects.childCount != 0)
            {
                confirmDialog.SetActive(true);
            }
            else
            {
                UIController.Instance.ToggleMainMenu();
                UIController.Instance.PortalPage();
                ShowPortal(portalMats[3]);
            }
        }
        else if (optionClick == "portal5")
        {
            if (spawnedObjects.childCount != 0)
            {
                confirmDialog.SetActive(true);
            }
            else
            {
                UIController.Instance.ToggleMainMenu();
                UIController.Instance.PortalPage();
                ShowPortal(portalMats[4]);
            }
        }
        else if (optionClick == "portal6")
        {
            if (spawnedObjects.childCount != 0)
            {
                confirmDialog.SetActive(true);
            }
            else
            {
                Instance.ToggleMainMenu();
                Instance.PortalPage();
                ShowPortal(portalMats[5]);
            }
        }


    }

    /// <summary>
    /// Update this instance.
    /// </summary>
    private void Update()
    {
        TogglePages();
        //SetIcons();
        ToggleScrollbar();
        ToggleDeleteButton();
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
        MarkerlessMRPage();


        HealthcarePage();
        PortalPage();
        FurniturePage();
        SofaPage();

        ChairPage();
        TablePage();
        BedPage();

        //SelectionMenu();
        //ColorsPage();
    }


    /// <summary>
    /// Page1 this instance.
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
    /// Healthcares the page.
    /// </summary>
    private void HealthcarePage()
    {
        if (!panelScrollRect.enabled)
            MoveObj(openHealthcarePage, healthcarePage, healthcarePageRect, Direction.BT, panelScrollRect, new Vector2(0, (panelRect.rect.height - healthcarePageRect.rect.height) / 2f));
        else if (!openHealthcarePage)
        {
            if (Vector2.Distance(healthcarePageRect.anchoredPosition, new Vector2(0, -healthcarePageRect.rect.height)) > 1)
                MoveObj(openHealthcarePage, healthcarePage, healthcarePageRect, Direction.BT);
        }

        MoveObj(openPlusHealthcarePage, plusHealthcarePage, plusHealthcarePageRect, Direction.RL);
    }


    /// <summary>
    /// Portals the page.
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
    /// Furnitures the page.
    /// </summary>
    private void FurniturePage()
    {
        if (!panelScrollRect.enabled)
            MoveObj(openFurniturePage, furniturePage, furniturePageRect, Direction.TB, panelScrollRect, new Vector2(0, (panelRect.rect.height - furniturePageRect.rect.height) / 2f));
        else if (!openFurniturePage)
        {
            if (Vector2.Distance(furniturePageRect.anchoredPosition, new Vector2(0, furniturePageRect.rect.height)) > 1)
                MoveObj(false, furniturePage, furniturePageRect, Direction.TB);
        }

        MoveObj(openPlusFurniturePage, plusFurniturePage, plusFurniturePageRect, Direction.RL);
    }

    /// <summary>
    /// Sofas the page.
    /// </summary>
    private void SofaPage()
    {
        if (!panelScrollRect.enabled)
            MoveObj(openSofaPage, sofaPage, sofaPageRect, Direction.TB, panelScrollRect, new Vector2(0, -100f));
        else if (!openSofaPage)
        {
            if (Vector2.Distance(sofaPageRect.anchoredPosition, new Vector2(0, sofaPageRect.rect.height)) > 1)
                MoveObj(openSofaPage, sofaPage, sofaPageRect, Direction.TB);
        }

        MoveObj(openPlusSofaPage, plusSofaPage, plusSofaPageRect, Direction.RL);
    }

    /// <summary>
    /// Chairs the page.
    /// </summary>
    private void ChairPage()
    {
        if (!panelScrollRect.enabled)
            MoveObj(openChairPage, chairPage, chairPageRect, Direction.TB, panelScrollRect, new Vector2(0, -100f));
        else if (!openChairPage)
        {
            if (Vector2.Distance(chairPageRect.anchoredPosition, new Vector2(0, chairPageRect.rect.height)) > 1)
                MoveObj(openChairPage, chairPage, chairPageRect, Direction.TB);
        }

        MoveObj(openPlusChairPage, plusChairPage, plusChairPageRect, Direction.RL);
    }


    /// <summary>
    /// Tables the page.
    /// </summary>
    private void TablePage()
    {
        if (!panelScrollRect.enabled)
            MoveObj(openTablePage, tablePage, tablePageRect, Direction.TB, panelScrollRect, new Vector2(0, -100f));
        else if (!openTablePage)
        {
            if (Vector2.Distance(tablePageRect.anchoredPosition, new Vector2(0, tablePageRect.rect.height)) > 1)
                MoveObj(openTablePage, tablePage, tablePageRect, Direction.TB);
        }

        MoveObj(openPlusTablePage, plusTablePage, plusTablePageRect, Direction.RL);
    }

    /// <summary>
    /// Beds the page.
    /// </summary>
    private void BedPage()
    {
        if (!panelScrollRect.enabled)
            MoveObj(openBedPage, bedPage, bedPageRect, Direction.TB, panelScrollRect, new Vector2(0, -100f));
        else if (!openBedPage)
        {
            if (Vector2.Distance(bedPageRect.anchoredPosition, new Vector2(0, bedPageRect.rect.height)) > 1)
                MoveObj(openBedPage, bedPage, bedPageRect, Direction.TB);
        }

        MoveObj(openPlusBedPage, plusBedPage, plusBedPageRect, Direction.RL);
    }


    /// <summary>
    /// Selections the menu.
    /// </summary>
    private void SelectionMenu()
    {
        if (!openSelectionMenu)
        {
            if (Vector2.Distance(selectionMenuRect.anchoredPosition, new Vector2(0, -selectionMenuRect.rect.height)) > 1)
                MoveObj(openSelectionMenu, selectionMenu, selectionMenuRect, Direction.BT);
        }
        //Debug.Log(AppManager.Instance.selectedObj);
        if (AppManager.Instance.selectedObj == null)
            return;
        if (currentPage != selectionMenu)
            openSelectionMenu = true;
        if (currentPage == furniturePage)
        {
            openFurniturePage = false;
            openPlusFurniturePage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }

        else if (currentPage == sofaPage)
        {
            openSofaPage = false;
            openPlusSofaPage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }

        else if (currentPage == chairPage)
        {
            openChairPage = false;
            openPlusChairPage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }

        else if (currentPage == tablePage)
        {
            openTablePage = false;
            openPlusTablePage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }

        else if (currentPage == bedPage)
        {
            openBedPage = false;
            openPlusBedPage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }

        else if (currentPage == portalPage)
        {
            openPortalPage = false;
            openPlusPortalPage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }


        else if (currentPage == healthcarePage)
        {
            openHealthcarePage = false;
            openPlusHealthcarePage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }
        else if (currentPage == mainMenu)
        {
            openMainMenu = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }
        //Debug.Log(openSelectionMenu);
        if (!panelScrollRect.enabled)
        {
            MoveObj(openSelectionMenu, selectionMenu, selectionMenuRect, Direction.BT, panelScrollRect, new Vector2(0, -65f));
            currentPage = selectionMenu;
            //backPage = null;
        }
    }

    /// <summary>
    /// Colorses the page.
    /// </summary>
    private void ColorsPage()
    {
        if (!myScrollRect2.enabled)
        {
            MoveObj(openColorsPage, colorsPage, colorsPageRect, Direction.RL, myScrollRect2, new Vector2(0, colorsPageRect.anchoredPosition.y));
        }
        else if (!openColorsPage)
        {
            if (Vector2.Distance(colorsPageRect.anchoredPosition, new Vector2(colorsPageRect.rect.width, colorsPageRect.anchoredPosition.y)) > 1)
            {
                MoveObj(openColorsPage, colorsPage, colorsPageRect, Direction.RL, myScrollRect2, new Vector2(colorsPageRect.rect.width, colorsPageRect.anchoredPosition.y));
            }
        }
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
    /// Toggles the delete button.
    /// </summary>
    private void ToggleDeleteButton()
    {
    
        if ( AppManager.Instance.objToSpawn != null)
        {
            if(AppManager.Instance.objToSpawn.activeInHierarchy)
                deleteBtn.SetActive(true);
        }
        else if(AppManager.Instance.selectedObj != null)
        {
            deleteBtn.SetActive(true);
        }
        else if (AppManager.Instance.portalMode)
        {
            deleteBtn.SetActive(true);
        }
        else
            deleteBtn.SetActive(false);
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

        //check if it's the first run
        if (AppManager.Instance.doOnce == 0)
        {
            //step1Done = true;
            if (step8Done)
            {
                step11Done = true;
            }
        }


        if(AppManager.Instance.selectedObj != null)
        {

            AppManager.Instance.doubleTapPlacement = false;

            if(AppManager.Instance.PlatformIsAndroid())
            {
                //AppManager.Instance.session.SessionConfig.EnablePlaneFinding = false;
                AppManager.Instance.session.SessionConfig.PlaneFindingMode = DetectedPlaneFindingMode.Disabled;
                AppManager.Instance.session.OnEnable();
            }
            else if (AppManager.Instance.PlatformIsIPhone())
            {
                AppManager.Instance.unityARcamera.planeDetectionOFF();
            }


            InteractableObject obj = AppManager.Instance.selectedObj.GetComponent<InteractableObject>();
            obj.isSelected = false;
            //AppManager.Instance.myText.text = outline.enabled.ToString();
            AppManager.Instance.selectedObj = null;
            //AppManager.Instance.selectionMarkerScaleSet = false;

            menuBtnImage.sprite = sprites[3];

            objPlaceInProg = false;

            UIController.Instance.tutorialBg.SetActive(false);

            UIController.Instance.tutorialStep11.SetActive(false);
            Debug.Log("Tutorial Finished");


            //check if it's the first run
            if (AppManager.Instance.doOnce == 0)
            {
                UIController.Instance.step7Done = true;
            }
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


        if (currentPage == furniturePage)
        {
            openFurniturePage = false;
            openPlusFurniturePage = false;

            openMarkerlessMRPage = false;//++RARO
            openPlusMarkerlessMRPage = false;//++RARO

            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }


        if (currentPage == portalPage)
        {
            openPortalPage = false;
            openPlusPortalPage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;

            SwapSprite(openPlusPortalPage, portalPlusImage, sprites[3], sprites[2]);
        }


        else if (currentPage == sofaPage)
        {
            openSofaPage = false;
            openPlusSofaPage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;

            SwapSprite(openPlusSofaPage, sofaPlusImage, sprites[3], sprites[2]);
        }

        else if (currentPage == chairPage)
        {
            openChairPage = false;
            openPlusChairPage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;

            SwapSprite(openPlusChairPage, chairPlusImage, sprites[3], sprites[2]);
        }

        else if (currentPage == tablePage)
        {
            openTablePage = false;
            openPlusTablePage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;

            SwapSprite(openPlusTablePage, tablePlusImage, sprites[3], sprites[2]);
        }

        else if (currentPage == bedPage)
        {
            openBedPage = false;
            openPlusBedPage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;

            SwapSprite(openPlusBedPage, bedPlusImage, sprites[3], sprites[2]);
        }

        else if (currentPage == healthcarePage)
        {
            openHealthcarePage = false;
            openPlusHealthcarePage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;

            SwapSprite(openPlusHealthcarePage, healthcarePlusImage, sprites[3], sprites[2]);
        }

        if (currentPage == markerlessMRPage)
        {
            openMarkerlessMRPage = false;
            openPlusMarkerlessMRPage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;

            SwapSprite(openPlusMarkerlessMRPage, markerlessPlusImage, sprites[3], sprites[2]);
        }


        else if (currentPage == selectionMenu)
        {
            openSelectionMenu = false;
            openColorsPage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
            currentPage = mainMenu;
        }

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
        

        SwapSprite(openMainMenu, menuBtnImage, sprites[1], sprites[0]);
    }

    /// <summary>
    /// Moves to vuforia.
    /// </summary>
    public void MoveToVuforia()
    {

        //show the confirm dialog if any objects are placed in the scene
        if (spawnedObjects.childCount != 0)
        {
            confirmDialog.SetActive(true);
        }
        else
        {
            guideText.gameObject.SetActive(true);
            guideText.text = "Loading...";
            SceneManager.LoadScene(3);
        }

    }


    /// <summary>
    /// Confirms the data loss.
    /// </summary>
    /// <param name="confirm">If set to <c>true</c> confirm.</param>
    public void ConfirmDataLoss(bool confirm)
    {
        if (confirm)
        {
            for (int i = 0; i < spawnedObjs.transform.childCount; i++)
            {
                spawnedObjs.transform.GetChild(i).gameObject.SetActive(false);
            }

            guideText.gameObject.SetActive(true);
            guideText.text = "Loading...";

            SceneManager.LoadScene(3);
        }
        else
            confirmDialog.SetActive(false);

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
    /// Toggles the markerless MR page.
    /// </summary>
    public void ToggleMarkerlessMRPage()
    {
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
    /// Toggles the healthcare page.
    /// </summary>
    public void ToggleHealthcarePage()
    {

        Debug.Log("RARO toggle healthcare");

        //openMainMenu = !openMainMenu;
        openMarkerlessMRPage = !openMarkerlessMRPage; //++RARO
        if (openPlusMarkerlessMRPage)
        {
            openPlusMarkerlessMRPage = false;
        }

        //openPlusMarkerlessMRPage = !openPlusMarkerlessMRPage; //++RARO
        //TogglePlusMarkerlessMRPage();
        openHealthcarePage = !openHealthcarePage;

        if (!openMainMenu)
        {
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }

        currentPage = healthcarePage;
        backPage = markerlessMRPage;
    }

    /// <summary>
    /// Toggles the plus healthcare.
    /// </summary>
    public void TogglePlusHealthcare()
    {
        openPlusHealthcarePage = !openPlusHealthcarePage;

        SwapSprite(openPlusHealthcarePage, healthcarePlusImage, sprites[3], sprites[2]);
    }

    /// <summary>
    /// Toggles the furniture page.
    /// </summary>
    public void ToggleFurniturePage()
    {

        Debug.Log("RARO toggle furniture");

        //check if it's the first run
        //if (AppManager.Instance.doOnce == 0)
        //{
        //    step2Done = true;
        //}


        //openMainMenu = !openMainMenu;

        openMarkerlessMRPage = !openMarkerlessMRPage; //++RARO
        openFurniturePage = !openFurniturePage;

        //Debug.Log("openFurniturePage-> " + openFurniturePage);
        if (!openMainMenu)
        {
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }
        currentPage = furniturePage;
        backPage = markerlessMRPage;
    }

    /// <summary>
    /// Toggles the plus furniture page.
    /// </summary>
    public void TogglePlusFurniturePage()
    {
        openPlusFurniturePage = !openPlusFurniturePage;

        SwapSprite(openPlusFurniturePage, furniturePlusImage, sprites[3], sprites[2]);
    }

    /// <summary>
    /// Toggles the portal page.
    /// </summary>
    public void TogglePortalPage()
    {
        openMainMenu = !openMainMenu;
        openPortalPage = !openPortalPage;
        //Debug.Log("openFurniturePage-> " + openFurniturePage);
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
    /// Toggles the sofa page.
    /// </summary>
    public void ToggleSofaPage()
    {

        //Tutorial step 4
        //check if it's the first run
        if (AppManager.Instance.doOnce == 0)
        {
            step4Done = true;
        }

        openFurniturePage = !openFurniturePage;
        openSofaPage = !openSofaPage;

        if (!openFurniturePage)
        {
            openPlusFurniturePage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }

        SwapSprite(openPlusFurniturePage, furniturePlusImage, sprites[3], sprites[2]);

        currentPage = sofaPage;
        backPage = furniturePage;
    }

    /// <summary>
    /// Toggles the plus sofa.
    /// </summary>
    public void TogglePlusSofa()
    {
        openPlusSofaPage = !openPlusSofaPage;

        SwapSprite(openPlusSofaPage, sofaPlusImage, sprites[3], sprites[2]);
    }

    //------------------------
    /// <summary>
    /// Toggles the chair page.
    /// </summary>
    public void ToggleChairPage()
    {

        openFurniturePage = !openFurniturePage;
        openChairPage = !openChairPage;

        if (!openFurniturePage)
        {
            openPlusFurniturePage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }

        SwapSprite(openPlusFurniturePage, furniturePlusImage, sprites[3], sprites[2]);

        currentPage = chairPage;
        backPage = furniturePage;
    }

    /// <summary>
    /// Toggles the plus chair.
    /// </summary>
    public void TogglePlusChair()
    {
        openPlusChairPage = !openPlusChairPage;

        SwapSprite(openPlusChairPage, chairPlusImage, sprites[3], sprites[2]);
    }


    /// <summary>
    /// Toggles the table page.
    /// </summary>
    public void ToggleTablePage()
    {

        openFurniturePage = !openFurniturePage;
        openTablePage = !openTablePage;

        if (!openFurniturePage)
        {
            openPlusFurniturePage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }

        SwapSprite(openPlusFurniturePage, furniturePlusImage, sprites[3], sprites[2]);

        currentPage = tablePage;
        backPage = furniturePage;
    }

    /// <summary>
    /// Toggles the plus table.
    /// </summary>
    public void TogglePlusTable()
    {
        openPlusTablePage = !openPlusTablePage;

        SwapSprite(openPlusTablePage, tablePlusImage, sprites[3], sprites[2]);
    }

    /// <summary>
    /// Toggles the bed page.
    /// </summary>
    public void ToggleBedPage()
    {

        openFurniturePage = !openFurniturePage;
        openBedPage = !openBedPage;

        if (!openFurniturePage)
        {
            openPlusFurniturePage = false;
            panelScrollRect.enabled = false;
            panelScrollRect.content = null;
        }

        SwapSprite(openPlusFurniturePage, furniturePlusImage, sprites[3], sprites[2]);

        currentPage = bedPage;
        backPage = furniturePage;
    }

    /// <summary>
    /// Toggles the plus bed.
    /// </summary>
    public void TogglePlusBed()
    {
        openPlusBedPage = !openPlusBedPage;

        SwapSprite(openPlusBedPage, bedPlusImage, sprites[3], sprites[2]);
    }
    //------------------------






    /// <summary>
    /// Toggles the colors page.
    /// </summary>
    public void ToggleColorsPage()
    {
        openColorsPage = !openColorsPage;
    }

    /// <summary>
    /// Applies the color.
    /// </summary>
    /// <param name="color">Color.</param>
    public void ApplyColor(string color)
    {
        string[] s = color.Split(',');
        float r = 0, g = 0, b = 0;
        float.TryParse(s[0], out r);
        float.TryParse(s[1], out g);
        float.TryParse(s[2], out b);
        if (AppManager.Instance.selectedObj != null)
        {
            Renderer rend = AppManager.Instance.selectedObj.GetComponent<Renderer>();
            rend.material.SetColor("_Color", new Color(r, g, b));
            Debug.Log(rend.material.color);
        }
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

        if (currentPage == markerlessMRPage)
        {
            openMainMenu = !openMainMenu;
            openMarkerlessMRPage = !openMarkerlessMRPage;

            if (!openMarkerlessMRPage)
                openPlusMarkerlessMRPage = false;

            SwapSprite(openPlusMarkerlessMRPage, markerlessPlusImage, sprites[3], sprites[2]);
            currentPage = backPage;
        }


        if (currentPage == healthcarePage)
        {
            //openMainMenu = !openMainMenu;
            openMarkerlessMRPage = !openMarkerlessMRPage;

            openHealthcarePage = !openHealthcarePage;

            if (!openHealthcarePage)
                openPlusHealthcarePage = false;

            SwapSprite(openPlusHealthcarePage, healthcarePlusImage, sprites[3], sprites[2]);
            currentPage = backPage;
        }

        if (currentPage == portalPage)
        {
            openMainMenu = !openMainMenu;
            openPortalPage = !openPortalPage;

            if (!openPortalPage)
                openPlusPortalPage = false;

            SwapSprite(openPlusHealthcarePage, healthcarePlusImage, sprites[3], sprites[2]);
            currentPage = backPage;
        }


        else if (currentPage == furniturePage)
        {

            Debug.Log("RARO back from furniture");

            //openMarkerlessMRPage = true;
            //openFurniturePage = false;

            openMarkerlessMRPage = !openMarkerlessMRPage;
            //openMainMenu = !openMainMenu;

            //close the sofa page if open
            if (openPlusSofaPage)
            {
                TogglePlusSofa();
            }


            openFurniturePage = !openFurniturePage;

            if (!openFurniturePage)
                openPlusFurniturePage = false;

            //Debug.Log("openPage1-> " + openMainMenu);
            //Debug.Log("openFurniturePage-> " + openFurniturePage);
            if (!openFurniturePage)
                panelScrollRect.enabled = false;

            //if (!openFurniturePage)
            //    openPlusFurniturePage = false;

            SwapSprite(openPlusFurniturePage, furniturePlusImage, sprites[3], sprites[2]);
            currentPage = backPage;
        }

        else if (currentPage == sofaPage)
        {
            openFurniturePage = !openFurniturePage;
            openSofaPage = !openSofaPage;

            if (!openSofaPage)
                openPlusSofaPage = false;

            SwapSprite(openPlusSofaPage, sofaPlusImage, sprites[3], sprites[2]);
            currentPage = backPage;
        }

        else if (currentPage == chairPage)
        {
            openFurniturePage = !openFurniturePage;
            openChairPage = !openChairPage;

            if (!openChairPage)
                openPlusChairPage = false;

            SwapSprite(openPlusChairPage, chairPlusImage, sprites[3], sprites[2]);
            currentPage = backPage;
        }

        else if (currentPage == tablePage)
        {
            openFurniturePage = !openFurniturePage;
            openTablePage = !openTablePage;

            if (!openTablePage)
                openPlusTablePage = false;

            SwapSprite(openPlusTablePage, tablePlusImage, sprites[3], sprites[2]);
            currentPage = backPage;
        }

        else if (currentPage == bedPage)
        {
            openFurniturePage = !openFurniturePage;
            openBedPage = !openBedPage;

            if (!openBedPage)
                openPlusBedPage = false;

            SwapSprite(openPlusBedPage, bedPlusImage, sprites[3], sprites[2]);
            currentPage = backPage;
        }



    }

    /// <summary>
    /// Spawns the object.
    /// </summary>
    /// <param name="objInfo">Object info.</param>
    public void SpawnObject(string objInfo)
    {
        string[] s = objInfo.Split(',');
        string fileName = s[0];
        string campaignModel = s[1];
        string brandName = s[2];
        string path = System.IO.Path.Combine(Application.persistentDataPath, AppManager.Instance.ObjSubFolder, fileName + ".unity3d");

        if (AppManager.Instance.objToSpawn != null)
        {
            AppManager.Instance.objToSpawn.SetActive(false);
            AppManager.Instance.objToSpawn.transform.SetParent(ObjectPooler.Instance.parent.transform);
            AppManager.Instance.objToSpawn = null;
        }

        AppManager.Instance.boundaryPolygonPoints.Clear();
        if (AppManager.Instance.objToSpawnFile == string.Empty)
            AppManager.Instance.objToSpawnFile = fileName;
        if (!System.IO.File.Exists(path))
        {
            downloadingObj.SetActive(true);
            GameObject currentObj = EventSystem.current.currentSelectedGameObject;
            AppManager.Instance.canvasGraphicRaycaster.enabled = false;
            WebRequest.Instance.DownloadObjectInfo(fileName, campaignModel, brandName, AppManager.Instance.ObjSubFolder, currentObj);
            Debug.Log("file does not exist downloading from web");
        }
        else
        {
            if (ObjectPooler.Instance.poolDictionary.ContainsKey(fileName))
            {
                if (AppManager.Instance.objToSpawn == null)
                {
                    AppManager.Instance.objToSpawn = ObjectPooler.Instance.GetFromPool(fileName, spawnedObjects);
                    //Start the tracking
                    AppManager.Instance.trackingState = true;
                }
            }
        }
    }

    /// <summary>
    /// Spawns the object2.
    /// </summary>
    /// <param name="objInfo">Object info.</param>
    public void SpawnObject2(string objInfo)
    {

        Debug.Log("RARO spawn object 2");

        if (objPlaceInProg)
            return;

        //starts the object placement chain
        objPlaceInProg = true;

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            checkNetTextObj.SetActive(true);
            AppManager.Instance.objInfo = objInfo;
        }
        if (AppManager.Instance.objToSpawn != null)
        {
            AppManager.Instance.objToSpawn.SetActive(false);
            AppManager.Instance.objToSpawn.transform.SetParent(ObjectPooler.Instance.parent.transform);
            AppManager.Instance.objToSpawn = null;
        }
        string[] s = objInfo.Split(',');
        string objName = s[0];
        string url = s[1];
        string path = System.IO.Path.Combine(Application.persistentDataPath, AppManager.Instance.ObjSubFolder, objName + ".unity3d");
        AppManager.Instance.filePath = path;
        AppManager.Instance.boundaryPolygonPoints.Clear();
        if (!System.IO.File.Exists(path))
        {
            Debug.Log("RARO spawn object 2 model to download");
            if (Application.internetReachability == NetworkReachability.NotReachable)
                return;
            downloadingObj.SetActive(true);
            cancelDownloadObj.SetActive(true);
            if (AppManager.Instance.objToSpawnFile == string.Empty)
                AppManager.Instance.objToSpawnFile = objName;
            AppManager.Instance.canvasGraphicRaycaster.enabled = false;
            WebRequest.Instance.DownloadAssetBundle(url, objName, AppManager.Instance.ObjSubFolder);

            //Tutorial step 5 , first sofa
            //check if it's the first run
            if (AppManager.Instance.doOnce == 0)
            {
                step5Done = true;
            }

        }
        else
        {
            if (ObjectPooler.Instance.poolDictionary.ContainsKey(objName))
            {
                Debug.Log("RARO spawn object 2 model already downloaded");
                //AppManager.Instance.disablePlane = false;
                if (AppManager.Instance.objToSpawn == null)
                {
                    scanPlane.SetActive(true);
                    checkNetTextObj.SetActive(false);
                    Debug.Log("RARO - try to get from pool");
                    AppManager.Instance.objToSpawn = ObjectPooler.Instance.GetFromPool(objName, spawnedObjects);
                    Debug.Log("RARO - " + AppManager.Instance.objToSpawn);
                    //AppManager.Instance.trackingState = true;
                }
            }
        }
    }


    /// <summary>
    /// Shows the portal.
    /// </summary>
    public void ShowPortal(Material portalMat)
    {
        Debug.Log("RARO SHOW PORTAL");

        if (portal.activeInHierarchy)
        {
            guideText.gameObject.SetActive(true);
            guideText.text = "Remove other portal";
            StartCoroutine(FlashMessage());
        }
        else
        {
            //apply the portal material
            portal.transform.GetChild(4).GetComponent<Renderer>().material = portalMat;
            //enable the portal
            AppManager.Instance.PortalEnable();
        }
    }


    /// <summary>
    /// Removes the object.
    /// </summary>
    public void RemoveObject()
    {

        if (objPlaceInProg)
        {
            objPlaceInProg = false;
        }

        AppManager.Instance.doubleTapPlacement = false;

        //disable the plane detection
        if (AppManager.Instance.PlatformIsIPhone())
        {
            AppManager.Instance.unityARcamera.planeDetectionOFF();
        }
        else if (AppManager.Instance.PlatformIsAndroid())
        {
            AppManager.Instance.disablePlane = true;
            AppManager.Instance.session.SessionConfig.PlaneFindingMode = DetectedPlaneFindingMode.Disabled;
            AppManager.Instance.session.OnEnable();
        }


        //disable portal if active
        if (AppManager.Instance.portalMode)
        {
            AppManager.Instance.portalMode = false;
            AppManager.Instance.portalIsPlaced = false;
            portal.SetActive(false);

            //disable the plane detection
            if (AppManager.Instance.PlatformIsIPhone())
            {
                AppManager.Instance.unityARcamera.planeDetectionOFF();
            }
            else if (AppManager.Instance.PlatformIsAndroid())
            {
                AppManager.Instance.disablePlane = true;
                AppManager.Instance.session.SessionConfig.PlaneFindingMode = DetectedPlaneFindingMode.Disabled;
                AppManager.Instance.session.OnEnable();
            }
        }


        AppManager.Instance.disablePlane = true;
        if(AppManager.Instance.objToSpawn != null)
        {
            AppManager.Instance.objToSpawn.SetActive(false);
            AppManager.Instance.objToSpawn.transform.SetParent(ObjectPooler.Instance.parent.transform);
            AppManager.Instance.objToSpawn = null;
        }
        else if(AppManager.Instance.selectedObj != null)
        {
            AppManager.Instance.selectedObj.SetActive(false);
            AppManager.Instance.selectedObj.transform.SetParent(ObjectPooler.Instance.parent.transform);
            AppManager.Instance.selectedObj = null;
        }

        //open up the main menu
        //TogglePage1();
        ToggleMainMenu();

    }

    /// <summary>
    /// Cancels the download.
    /// </summary>
    public void CancelDownload()
    {

        if (AppManager.Instance.objToSpawn != null)
        {
            AppManager.Instance.objToSpawn.SetActive(false);
            AppManager.Instance.objToSpawn.transform.SetParent(ObjectPooler.Instance.parent.transform);
            AppManager.Instance.objToSpawn = null;
        }

        if (WebRequest.Instance.uwr != null)
        {
            WebRequest.Instance.uwr.Dispose();
            WebRequest.Instance.uwr = null;
            System.GC.Collect();
            Debug.Log("RARO uwr URL has been disposed");
        }

        if(System.IO.File.Exists(AppManager.Instance.filePath))
            System.IO.File.Delete(AppManager.Instance.filePath);
        downloadingObj.SetActive(false);
        AppManager.Instance.objToSpawnFile = string.Empty;
        AppManager.Instance.canvasGraphicRaycaster.enabled = true;
        cancelDownloadObj.SetActive(false);
    }

    /// <summary>
    /// Sets the icons.
    /// </summary>
    public void SetIcons()
    {
        if (AppManager.Instance.TexList.Count <= 0 || menuIconsSet)
            return;
        Texture2D tex = null;
        for (int i = 0; i < menuIcons.Count; i++)
        {
            for (int j = 0; j < AppManager.Instance.TexList.Count; j++)
            {
                if (menuIcons[i].name == AppManager.Instance.TexList[j].name)
                {
                    tex = (Texture2D)AppManager.Instance.TexList[j];
                    Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
                    menuIcons[i].sprite = sprite;
                    break;
                }
            }
        }
        menuIconsSet = true;
    }


    /// <summary>
    /// Comings the soon.
    /// </summary>
    public void ComingSoon()
    {
        guideText.gameObject.SetActive(true);
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
        guideText.gameObject.SetActive(false);
    }



    #endregion
}
