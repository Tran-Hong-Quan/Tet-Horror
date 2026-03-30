using UnityEngine;

public class InspectItemCanvas : MonoBehaviour
{
    public static InspectItemCanvas Instance { get; private set; }

    private const string ResourcePath = "InspectItemCanvas";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public static InspectItemCanvas Get()
    {
        if (Instance != null)
            return Instance;

        GameObject prefab = Resources.Load<GameObject>(ResourcePath);

        if (prefab == null)
        {
            Debug.LogError($"Could not find prefab at Resources/{ResourcePath}");
            return null;
        }

        GameObject obj = Instantiate(prefab);

        Instance = obj.GetComponent<InspectItemCanvas>();

        if (Instance == null)
        {
            Debug.LogError("Prefab does not contain InspectItemCanvas component.");
            return null;
        }

        return Instance;
    }
}