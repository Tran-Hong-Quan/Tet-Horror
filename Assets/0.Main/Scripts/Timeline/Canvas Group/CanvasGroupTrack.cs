using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[TrackClipType(typeof(CanvasGroupPlayableAsset))]
[TrackBindingType(typeof(CanvasGroup))]
public class CanvasGroupTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<CanvasGroupMixerBehaviour>.Create(graph, inputCount);
    }
}
