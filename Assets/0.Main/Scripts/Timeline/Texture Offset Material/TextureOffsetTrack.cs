using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackClipType(typeof(TextureOffsetClip))]
public class TextureOffsetTrack : TrackAsset
{
    //public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    //{
    //    var mixer = ScriptPlayable<TextureOffsetMixer>.Create(graph, inputCount);
    //    return mixer;
    //}

}
