using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class without_tutorial : MonoBehaviour
{
   public GameObject tutorial;
   
   
   	public GameObject skip1;
    public GameObject skip2;
    public GameObject skip3;
    public GameObject skip4;
    public GameObject skip5;
    public GameObject skip6;

    public void showUIwithoutTutorial()
    {
    	if (tutorial.activeInHierarchy == false)
    	{
    	skip1.SetActive(false);
    	skip2.SetActive(false);
    	skip3.SetActive(false);
    	skip4.SetActive(false);
    	skip5.SetActive(false);
    	skip6.SetActive(false);
    	}
    }
}
