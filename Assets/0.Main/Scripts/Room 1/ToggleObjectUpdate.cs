using UnityEngine;

public class ToggleObjectUpdate : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float interval = 1f;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            target.SetActive(!target.activeSelf);
            timer = 0f;
        }
    }
}