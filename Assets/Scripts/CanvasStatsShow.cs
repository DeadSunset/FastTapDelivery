using UnityEngine;

public class CanvasStatsShow : MonoBehaviour
{
    private Canvas canvas;
    private void Start()
    {
        canvas = gameObject.GetComponent<Canvas>();
        GameEvents.events.OnFightStart += SetVisibility;
        GameEvents.events.OnFightEnd += FightEnd;
        GameEvents.events.OnMapRestart += TurnVisibility;
    }

    private void FightEnd(bool t)
    {
        TurnVisibility();
    }
    private void SetVisibility()
    {
        canvas.enabled = true;
    }

    private void TurnVisibility()
    {
        canvas.enabled = false;
    }
}
