using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager enemyManager;
    public int movesToCreateEnemy = 5;
    public List<EnemyClass> allActiveEnemies = new List<EnemyClass>();
    public List<EnemyClass> standardEnemiesPrefabs = new List<EnemyClass>();

    private List<GameObject> _roadList;
    [SerializeField]
    private List<bool> _roadListFree = new List<bool>();
    private int _enemyCreateCoolDown;

    private void Awake()
    {
        enemyManager = this;
    }
    private void Start()
    {
        GameEvents.events.OnRoadEnded += StartManaging;
        GameEvents.events.OnDisableAllEnemiesExcept += DisableForBattle;
        GameEvents.events.OnFightEnd += ActivateAllEnemies;
    }
    private void StartManaging()
    {
        _roadList = MapGenerator.map.roadPathList;

        for (int i = 0; i < _roadList.Count; i++)
        {
            _roadListFree.Add(true);
        }
        _roadListFree[0] = false;
        _roadListFree[_roadList.Count - 1] = false;
        _enemyCreateCoolDown = 0;

        GameEvents.events.OnPlayerMoved += CreateEnemies;
    }

    private void CreateEnemies()
    {
        _enemyCreateCoolDown++;
        if (_enemyCreateCoolDown >= movesToCreateEnemy)
        {
            _enemyCreateCoolDown = 0;
            while (true)
            {
                var index = Random.Range(0, _roadList.Count);
                var pos = _roadList[index].transform.position;
                if (_roadListFree[index] == true && PlayerStats.playerStats.playerController.gameObject.transform.position.x != pos.x && PlayerStats.playerStats.playerController.gameObject.transform.position.z != pos.z)
                {
                    var enemy = Instantiate(standardEnemiesPrefabs[Random.Range(0, standardEnemiesPrefabs.Count)], _roadList[Random.Range(0, _roadList.Count)].transform.position, Quaternion.identity);
                    allActiveEnemies.Add(enemy);
                    enemy.ID = index;
                    _roadListFree[index] = false;
                    break;
                }
            }
        }
    }

    public void EnemyDeadFreeRoad(EnemyClass enemy)
    {
        foreach (var item in allActiveEnemies)
        {
            if (item == enemy)
            {
                for (int i = 0; i < allActiveEnemies.Count; i++)
                {
                    if (allActiveEnemies[i].ID == enemy.ID)
                    {
                        _roadListFree[enemy.ID] = true;
                    }
                }

            }
        }
    }

    public void DisableForBattle(EnemyClass notDisableEnemy)
    {
        foreach (var enemy in allActiveEnemies)
        {
            if (enemy != notDisableEnemy)
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }

    public void ActivateAllEnemies(bool win)
    {
        if (win)
        {
            foreach (var enemy in allActiveEnemies)
            {
                enemy.gameObject.SetActive(true);
            }
        }
    }
}
