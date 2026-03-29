using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class CanvasGroupPlayableAsset : PlayableAsset
{
    public float alpha = 1f;
    public ClipCaps clipCaps => ClipCaps.Blending | ClipCaps.Extrapolation;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<CanvasGroupBehavior>.Create(graph);
        var behavior = playable.GetBehaviour();
        behavior.alpha = alpha;
        return playable;
    }
}
