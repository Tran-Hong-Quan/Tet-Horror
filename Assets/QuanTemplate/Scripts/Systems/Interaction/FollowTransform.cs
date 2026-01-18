using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public Transform target;
    public bool follow = true;
    void LateUpdate()
    {
        if (!follow) return;
        if(target == null) return;
        transform.position = target.position;
    }
}
