using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UILook : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public UnityEvent<Vector2> onLook;
    public void OnDrag(PointerEventData eventData)
    {
        var delta = eventData.delta;
        delta.y = -delta.y;
        onLook?.Invoke(delta * GameManager.cameraSensitivity);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        onLook?.Invoke(Vector2.zero);
    }
}
