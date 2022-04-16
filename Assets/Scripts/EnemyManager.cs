using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager enemyManager;
    public int movesToCreateEnemy = 5;
    public List<EnemyClass> allActiveEnemies = new List<EnemyClass>();
    public List<EnemyClass> standardEnemiesPrefabs = new List<EnemyClass>();

    private List<GameObject> _roadList;
    private List<GameObject> _fieldList;
    private int _enemyCreateCoolDown;

    private void Awake()
    {
        enemyManager = this;
    }

    private void Start()
    {
        _roadList = MapGenerator.map.roadPathList;
        _fieldList = MapGenerator.map.fieldList;
        _enemyCreateCoolDown = 0;

        GameEvents.events.OnPlayerMoved += CreateEnemies;
    }

    private void CreateEnemies()
    {
        _enemyCreateCoolDown++;
        if (_enemyCreateCoolDown >= movesToCreateEnemy)
        {
            _enemyCreateCoolDown = 0;
            var enemy = Instantiate(standardEnemiesPrefabs[Random.Range(0, standardEnemiesPrefabs.Count)], _roadList[Random.Range(0, _roadList.Count)].transform.position, Quaternion.identity);
            allActiveEnemies.Add(enemy);
        }
    }
}
