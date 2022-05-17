using UnityEngine;
using UnityEngine.UI;

public class EnemyStatsView : MonoBehaviour
{
    [SerializeField]
    private GameObject _panel;
    [SerializeField]
    private Slider _slider;

    private void Start()
    {
        GameEvents.events.OnFightStart += SetVisibility;
        GameEvents.events.OnMapRestart += TurnVisibility;
        GameEvents.events.OnFightEnd += EndFight;
        GameEvents.events.OnDealDamageToEnemy += CorrectSliderValue;
    }
    private void EndFight(bool t)
    {
        TurnVisibility();
    }
    private void SetVisibility()
    {
        _slider.maxValue = PlayerStats.playerStats.playerController.currentEnemies[0].health;
        _slider.value = _slider.maxValue;
        _panel.SetActive(true);
    }

    private void TurnVisibility()
    {
        _panel.SetActive(false);
    }

    public void CorrectSliderValue()
    {
        if (PlayerStats.playerStats.playerController.currentEnemies.Count > 0)
        {
            _slider.value = PlayerStats.playerStats.playerController.currentEnemies[0].health;
        }
    }
}
