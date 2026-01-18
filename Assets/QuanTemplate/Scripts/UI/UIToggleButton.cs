using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

public class UIToggleButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public UnityEvent<bool> onClick;
    public bool state = false;

    public Color colorOn = Color.gray;
    public Color colorOff = Color.white;

    Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        onClick?.Invoke(state);
        image.color = state ? colorOn : colorOff;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        state = !state;
        onClick?.Invoke(state);
        image.color = state ? colorOn : colorOff;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }
}
