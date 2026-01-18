using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterHoldObjectData", menuName = "ScriptableObjects/CharacterHoldObjectData")]
public class CharacterHoldObjectTransformDatas : ScriptableObject
{
    [SerializeField] List<DictionaryData> datas = new List<DictionaryData>();
    public Dictionary<string, TransformData> Datas = new Dictionary<string, TransformData>();

    [System.Serializable]
    public struct DictionaryData
    {
        public string key;
        public TransformData value;
    }

    private void OnEnable()
    {
        foreach (var data in datas)
        {
            if (Datas.ContainsKey(data.key)) continue;
            Datas[data.key] = data.value;
        }
    }

    public TransformData GetHeldObjectTransformData(string objectNameKey)
    {
        return Datas.TryGetValue(objectNameKey, out TransformData value) ? value : TransformData.Default;
    }
}
