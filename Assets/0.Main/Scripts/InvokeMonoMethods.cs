using UnityEngine;
using UnityEngine.Events;

public class InvokeMonoMethods : MonoBehaviour
{
    public UnityEvent OnAwake;
    public UnityEvent OnStart;

    private void Awake()
    {
        OnAwake?.Invoke();
    }

    private void Start()
    {
        OnStart?.Invoke();
    }
}
