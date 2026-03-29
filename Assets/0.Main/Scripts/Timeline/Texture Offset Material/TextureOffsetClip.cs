using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class TextureOffsetClip : PlayableAsset, ITimelineClipAsset
{
    public Vector2 offset = Vector2.zero;
    public Material material;

    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TextureOffsetPlayable>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour.offset = offset;
        behaviour.material = material;
        return playable;
    }
}