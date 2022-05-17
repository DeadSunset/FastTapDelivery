using UnityEngine;

public class HidePanelsOnPlay : MonoBehaviour
{
    [SerializeField]
    private GameObject _panelLeft;
    [SerializeField]
    private GameObject _panelRight;

    private void Start()
    {
        GameEvents.events.OnRoadEnded += HideOnObjectOnPlay;
        GameEvents.events.OnGameEnd += ShowOnObjectOnIdle;
    }

    private void HideOnObjectOnPlay()
    {
        _panelLeft.SetActive(false);
        _panelRight.SetActive(false);
    }

    private void ShowOnObjectOnIdle(bool i)
    {
        _panelLeft.SetActive(true);
        _panelRight.SetActive(true);
    }
}
