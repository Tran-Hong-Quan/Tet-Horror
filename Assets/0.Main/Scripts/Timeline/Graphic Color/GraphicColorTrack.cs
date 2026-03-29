using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

[TrackClipType(typeof(GraphicColorPlayableAsset))]
[TrackBindingType(typeof(Graphic))] 
public class GraphicColorTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        var mixer = ScriptPlayable<GraphicColorMixerBehaviour>.Create(graph, inputCount);
        return mixer;
    }
}
