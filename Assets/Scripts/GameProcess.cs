using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProcess : MonoBehaviour
{
    public GameObject hero;
    public float cellStepOverTime;
    public float rotationTime;
    private int _currentCell = 1;
    private GameObject _char;
    private List<GameObject> roadList;
    void Start()
    {
        roadList = MapGenerator.map.roadPathList;
        GameEvents.events.OnRoadEnded += StartGame;
    }

    private void StartGame()
    {
        var cellStart = MapGenerator.map.roadPathList[1].transform.position;
        cellStart = new Vector3(cellStart.x, 0, cellStart.z);
        _char = Instantiate(hero, MapGenerator.map.roadPathList[1].transform.position, Quaternion.identity);
        _char.transform.position = new Vector3(cellStart.x, hero.transform.position.y + 0.15f, cellStart.z);
        _char.GetComponent<Animator>().Play("Run");
        CheckRotaton();
        StartCoroutine("MoveToCell");
    }

    private void CheckRotaton()
    {
        print(_currentCell);
        if (_currentCell < MapGenerator.map.roadPathList.Count - 1)
        {
            if (roadList[_currentCell + 1].transform.position.x > roadList[_currentCell].transform.position.x) _char.transform.DORotateQuaternion(Quaternion.Euler(0, 90, 0), rotationTime);
            else if (roadList[_currentCell + 1].transform.position.x < roadList[_currentCell].transform.position.x) _char.transform.DORotateQuaternion(Quaternion.Euler(0, -90, 0), rotationTime);
            else if (roadList[_currentCell + 1].transform.position.z > roadList[_currentCell].transform.position.z) _char.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), rotationTime);
            else _char.transform.DORotateQuaternion(Quaternion.Euler(0, 180, 0), rotationTime);
        }
    }


    public IEnumerator MoveToCell()
    {

        print("MoveToCell");
        print(_char.transform.position);
        var cellPos = MapGenerator.map.roadPathList[_currentCell].transform.position;
        cellPos = new Vector3(cellPos.x, 0, cellPos.z);
        _char.transform.DOMove(cellPos, cellStepOverTime);
        yield return new WaitForSeconds(cellStepOverTime);
        if (_currentCell < MapGenerator.map.roadPathList.Count)
        {
            CheckRotaton();
            StartCoroutine("MoveToCell");
            _currentCell++;
        }
        else
        {
            _char.GetComponent<Animator>().Play("Idle");
        }

    }
}
