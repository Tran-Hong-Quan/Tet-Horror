using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class TextureOffsetPlayable : PlayableBehaviour
{
    public Vector2 offset = Vector2.zero; 
    public Material material;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (material == null) return;
        material.SetTextureOffset("_MainTex", offset);
    }
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (material == null) return;
        material.SetTextureOffset("_MainTex", Vector2.zero); 
    }
}