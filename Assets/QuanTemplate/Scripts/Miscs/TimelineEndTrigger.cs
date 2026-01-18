using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class TimelineEndTrigger : MonoBehaviour
{
    public PlayableDirector timeline;
    public UnityEvent<PlayableDirector> onTrigger;
    void Awake()
    {
        timeline = GetComponent<PlayableDirector>();
        timeline.stopped += OnTimelineEnd;
    }

    private void OnTimelineEnd(PlayableDirector director)
    {
        onTrigger?.Invoke(director);
    }
}
