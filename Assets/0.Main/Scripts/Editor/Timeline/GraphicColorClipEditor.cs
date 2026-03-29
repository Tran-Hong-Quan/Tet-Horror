using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;
using UnityEditor.Timeline;

[CustomTimelineEditor(typeof(GraphicColorPlayableAsset))]   
public class GraphicColorClipEditor : ClipEditor
{
    public override void DrawBackground(TimelineClip clip, ClipBackgroundRegion region)
    {
        //var assets = clip.asset as GraphicColorPlayableAsset;
        //if (assets != null)
        //{
        //    EditorGUI.DrawRect(region.position, assets.color);
        //}
        base.DrawBackground(clip, region);
    }

    public override ClipDrawOptions GetClipOptions(TimelineClip clip)
    {
        var assets = clip.asset as GraphicColorPlayableAsset;
        if (assets != null)
        {
            var options = base.GetClipOptions(clip);
            options.highlightColor = assets.color;
        }
        return base.GetClipOptions(clip);
    }
}
