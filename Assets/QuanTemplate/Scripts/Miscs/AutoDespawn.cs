using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDespawn : MonoBehaviour
{
    public float delay = 5f;

    private void OnEnable()
    {
        Despawn();
    }

    public void Despawn()
    {
        StopAllCoroutines();
        this.DelayFunction(delay, () => SimplePool.Despawn(gameObject));
    }
}
