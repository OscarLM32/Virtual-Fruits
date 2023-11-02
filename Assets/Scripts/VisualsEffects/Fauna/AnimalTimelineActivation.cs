using UnityEngine;
using UnityEngine.Playables;

public class AnimalTimelineActivation : MonoBehaviour
{
    private PlayableDirector timeline;

    private void Start()
    {
        timeline = GetComponent<PlayableDirector>();
        timeline.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        timeline.enabled = true;
    }
}
