using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Room1Manager : MonoBehaviour
{
    [SerializeField] private DialogueData mission2Dialogue;
    [SerializeField] UnityEvent onMission2Start;
    [SerializeField] private float delayBeforeMission2 = 1f;

    [SerializeField] UnityEvent onInitMission3;
    [SerializeField] UnityEvent onStartMission3;

    [SerializeField] string room2SceneName = "Room2";

    private bool mission2Started = false;

    public void StartMission2()
    {
        if (mission2Started) return; // Prevent multiple starts
        this.DelayFunction(delayBeforeMission2, () =>
        {
            onMission2Start.Invoke();
            PlayerInteract.Instance.Dialogue(mission2Dialogue);
        });
        mission2Started = true;
    }

    public void InitMission3()
    {
        onInitMission3.Invoke();
    }

    public void StartMission3()
    {
        onStartMission3.Invoke();
    }

    public void LoadRoom2()
    {
        SceneManager.LoadScene(room2SceneName);
    }
}
