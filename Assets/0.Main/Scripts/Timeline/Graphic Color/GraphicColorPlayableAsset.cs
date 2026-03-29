using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class GraphicColorPlayableAsset : PlayableAsset
{
    public Color color = Color.white;
    public ClipCaps clipCaps => ClipCaps.Blending | ClipCaps.Extrapolation;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<GraphicColorBehaviour>.Create(graph);
        var behavior =  playable.GetBehaviour();
        behavior.color = color;
        return playable;
    }
}
