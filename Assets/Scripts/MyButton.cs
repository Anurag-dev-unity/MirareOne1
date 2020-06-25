
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool buttonPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        print(transform.gameObject.name);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }
}