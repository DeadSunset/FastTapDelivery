using UnityEngine;

public class PlayerStatsView : MonoBehaviour
{
    [SerializeField]
    private GameObject _panel;

    private void Start()
    {
        GameEvents.events.OnRoadEnded += SetVisibility;
        GameEvents.events.OnMapRestart += TurnVisibility;
    }

    private void SetVisibility()
    {
        _panel.SetActive(!_panel.activeSelf);
    }

    private void TurnVisibility()
    {
        _panel.SetActive(false);
    }
}
