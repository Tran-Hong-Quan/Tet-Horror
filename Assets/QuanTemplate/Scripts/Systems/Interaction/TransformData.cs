using UnityEngine;

[System.Serializable]
public struct TransformData
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;

    public TransformData(Vector3 position, Vector3 rotation, Vector3 scale)
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }
    public static TransformData Default => new TransformData(Vector3.zero, Vector2.zero, Vector3.one);

    public readonly void AppyLocRot(Transform transform)
    {
        transform.SetPositionAndRotation(position, Quaternion.Euler(rotation));
    }
    public readonly void AppyLocalLocRot(Transform transform)
    {
        transform.SetLocalPositionAndRotation(position, Quaternion.Euler(rotation));
    }
}
