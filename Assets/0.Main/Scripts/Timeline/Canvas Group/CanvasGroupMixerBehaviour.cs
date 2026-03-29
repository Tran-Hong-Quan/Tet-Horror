using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CanvasGroupMixerBehaviour : PlayableBehaviour
{
    private CanvasGroup canvasGroup;
    private float originalAlpha = 1;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (canvasGroup == null)
        {
            canvasGroup = playerData as CanvasGroup;
            originalAlpha = canvasGroup.alpha;
        }
        if (!canvasGroup) return;

        float blendedAlpha = originalAlpha;
        int inputCount = playable.GetInputCount();

        // Blend tất cả clip đang active
        for (int i = 0; i < inputCount; i++)
        {
            var input = (ScriptPlayable<CanvasGroupBehavior>)playable.GetInput(i);
            float weight = playable.GetInputWeight(i);
            var inputBehaviour = input.GetBehaviour();

            blendedAlpha = Mathf.Lerp(blendedAlpha, inputBehaviour.alpha, weight);
        }

        canvasGroup.alpha = blendedAlpha;
    }

    public override void OnGraphStop(Playable playable)
    {
        if (canvasGroup != null)
            canvasGroup.alpha = originalAlpha;
    }
}
