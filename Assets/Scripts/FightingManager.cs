using System.Collections;
using UnityEngine;

public class FightingManager : MonoBehaviour
{
    private PlayerStats _playerStats;

    private void Start()
    {
        _playerStats = PlayerStats.playerStats;
        GameEvents.events.OnFightStart += StartFight;
    }

    public void StartFight()
    {
        StartCoroutine(FightUpdate());
    }
    private IEnumerator FightUpdate()
    {
        while (PlayerStats.playerStats.currentHealthPoints > 0)
        {
            if (_playerStats.playerController.EnemiesCount() <= 0)
            {
                GameEvents.events.FightEnd(true);
                yield return new WaitForSeconds(1f);
            }
        }
        if (PlayerStats.playerStats.currentHealthPoints <= 0)
        {
            GameEvents.events.FightEnd(false);
            yield return new WaitForEndOfFrame();
        }

    }
}
