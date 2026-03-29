//using UnityEngine;
//using UnityEngine.Playables;

//public class TextureOffsetMixer : PlayableBehaviour
//{
//    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
//    {
//        var matDict = new System.Collections.Generic.Dictionary<Material, Vector2>();

//        int inputCount = playable.GetInputCount();
//        for (int i = 0; i < inputCount; i++)
//        {
//            var inputPlayable = (ScriptPlayable<TextureOffsetPlayable>)playable.GetInput(i);
//            var inputBehaviour = inputPlayable.GetBehaviour();
//            if (inputBehaviour.material == null) continue;
//            if(!matDict.ContainsKey(inputBehaviour.material))
//            {
//                matDict[inputBehaviour.material] = inputBehaviour.offset;
//            }    
//        }

//        foreach (var kv in matDict)
//        {
//            kv.Key.SetTextureOffset("_MainTex", kv.Value);
//        }
//    }
//}
