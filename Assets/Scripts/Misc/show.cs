using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class show : MonoBehaviour
{
    public GameObject skip1;
    public GameObject skip2;
    public GameObject skip3;
    public GameObject skip4;
    public GameObject skip5;
    public GameObject skip6;
    public GameObject delete;
    public void skip()
    {
        skip1.SetActive(false);
        skip2.SetActive(false);
        skip3.SetActive(false);
        skip4.SetActive(false);
        skip5.SetActive(false);
        skip6.SetActive(false);
        delete.SetActive(false);
       
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
}
