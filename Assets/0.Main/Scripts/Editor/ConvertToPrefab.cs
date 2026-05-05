using UnityEditor;
using UnityEngine;

public class ConvertToPrefab : EditorWindow
{
    private GameObject targetObject;
    private GameObject targetPrefab;
    private string savePath = "Assets/0.Main/Prefabs/NewPrefab.prefab";

    [MenuItem("Tools/Convert To Prefab Window")]
    public static void Open()
    {
        GetWindow<ConvertToPrefab>("Prefab Converter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Convert Scene Object To Prefab", EditorStyles.boldLabel);

        GUILayout.Space(10);

        // Scene object
        targetObject = (GameObject)EditorGUILayout.ObjectField(
            "Scene Object",
            targetObject,
            typeof(GameObject),
            true
        );

        // Prefab reference (optional replace)
        targetPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Prefab (Optional)",
            targetPrefab,
            typeof(GameObject),
            false
        );

        GUILayout.Space(10);

        savePath = EditorGUILayout.TextField("Save Path", savePath);

        GUILayout.Space(10);

        GUI.enabled = targetObject != null;

        if (GUILayout.Button("Convert To Prefab"))
        {
            Convert();
        }

        GUI.enabled = true;
    }

    private void Convert()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("No target object selected");
            return;
        }

        // nếu có prefab target → replace logic (optional)
        if (targetPrefab != null)
        {
            ReplaceWithPrefab();
            return;
        }

        // create prefab from scene object
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(targetObject, savePath);

        Debug.Log("Prefab created at: " + savePath);
    }

    private void ReplaceWithPrefab()
    {
        if (targetPrefab == null || targetObject == null) return;

        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(targetPrefab);

        // giữ transform
        instance.transform.SetPositionAndRotation(
            targetObject.transform.position,
            targetObject.transform.rotation
        );

        instance.transform.localScale = targetObject.transform.localScale;

        // giữ children đặc biệt (metarig fix)
        foreach (Transform child in targetObject.transform)
        {
            if (child.name.ToLower().Contains("metarig"))
            {
                child.SetParent(instance.transform, true);
            }
        }

        Object.DestroyImmediate(targetObject);

        Debug.Log("Replaced with prefab while preserving metarig");
    }
}