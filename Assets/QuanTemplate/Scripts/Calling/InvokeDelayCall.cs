using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeDelayCall : MonoBehaviour
{
    public UnityEvent onDone;
    public void StartInvokeDelay(float duration)
    {
        this.DelayFunction(duration, () => onDone?.Invoke());
    }
}
