using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class GraphicColorMixerBehaviour : PlayableBehaviour
{
    private Graphic targetGraphic;
    private Color originalColor;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        var graphic = playerData as Graphic;
        if (!graphic) return;

        if (targetGraphic == null)
        {
            targetGraphic = graphic;
            originalColor = graphic.color; 
        }

        Color blendedColor = originalColor;
        int inputCount = playable.GetInputCount();

        // Blend tất cả clip đang active
        for (int i = 0; i < inputCount; i++)
        {
            var input = (ScriptPlayable<GraphicColorBehaviour>)playable.GetInput(i);
            float weight = playable.GetInputWeight(i);
            var inputBehaviour = input.GetBehaviour();

            blendedColor = Color.Lerp(blendedColor, inputBehaviour.color, weight);
        }

        targetGraphic.color = blendedColor;
    }

    public override void OnGraphStop(Playable playable)
    {
        if (targetGraphic != null)
            targetGraphic.color = originalColor;
    }
}