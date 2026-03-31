using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefsHelper
{
    private const string AllKeysKey = "AllKeys";

    // Thêm key mới
    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        AddToAllKeys(key);
        PlayerPrefs.Save();
    }

    private static void AddToAllKeys(string key)
    {
        List<string> keys = GetAllKeys();
        if (!keys.Contains(key)) keys.Add(key);
        PlayerPrefs.SetString(AllKeysKey, JsonUtility.ToJson(new Serialization<string>(keys)));
    }

    private static List<string> GetAllKeys()
    {
        if (!PlayerPrefs.HasKey(AllKeysKey))
            return new List<string>();

        return JsonUtility.FromJson<Serialization<string>>(PlayerPrefs.GetString(AllKeysKey)).ToList();
    }

    // Xóa key theo prefix
    public static void DeleteKeysWithPrefix(string prefix)
    {
        List<string> keys = GetAllKeys();
        List<string> keysToRemove = new List<string>();

        foreach (var key in keys)
        {
            if (key.StartsWith(prefix))
            {
                PlayerPrefs.DeleteKey(key);
                keysToRemove.Add(key);
            }
        }

        // Cập nhật lại danh sách
        foreach (var k in keysToRemove)
            keys.Remove(k);

        PlayerPrefs.SetString(AllKeysKey, JsonUtility.ToJson(new Serialization<string>(keys)));
        PlayerPrefs.Save();
    }
    // Helper class serialize List<string>
    [System.Serializable]
    public class Serialization<T>
    {
        public List<T> target;
        public Serialization(List<T> target) { this.target = target; }
        public List<T> ToList() { return target; }
    }
}


