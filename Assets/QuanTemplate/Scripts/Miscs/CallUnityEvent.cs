using UnityEngine;
using UnityEngine.Events;

public class CallUnityEvent : MonoBehaviour
{
    public UnityEvent onCall;

    public void Call()
    {
        onCall?.Invoke();
    }
}
