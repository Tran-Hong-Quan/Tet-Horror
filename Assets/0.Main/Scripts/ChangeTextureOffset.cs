using UnityEngine;

public class ChangeTextureOffset : MonoBehaviour
{
    public Material material;
    public Vector2 offset = Vector2.zero;

    public void SetTextureOffset()
    {
        if (material == null) return;
        
        material.mainTextureOffset = offset;
    }

    public void SetTextureOffset(Vector2 newOffset)
    {
        if (material == null) return;
        
        material.mainTextureOffset = newOffset;
    }
}
