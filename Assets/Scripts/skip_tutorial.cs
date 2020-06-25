using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skip_tutorial : MonoBehaviour
{
    public GameObject skip1;
    public GameObject skip2;
    public GameObject skip3;
    public GameObject skip4;
    public GameObject skip5;
    public GameObject skip6;
    public GameObject skip7;
    public MyButton bt1;
    public MyButton bt2;
    public MyButton bt3;
    public MyButton bt4;
    public MyButton bt5;
    public MyButton bt6;
    public GameObject delete;

    public void skip()
    {
        skip1.SetActive(false);
        skip2.SetActive(false);
        skip3.SetActive(false);
        skip4.SetActive(false);
        skip5.SetActive(false);
        skip6.SetActive(false);
        skip7.SetActive(false);
        bt1.enabled = true;
        bt2.enabled = true;
        bt3.enabled = true;
        bt4.enabled = true;
        bt5.enabled = true;
        bt6.enabled = true;
        delete.SetActive(false);
    }
}
