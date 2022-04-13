using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float cellStepOverTime;
    public float rotationTime;

    private List<GameObject> roadList;
    private int _currentCell = 0;
    void Start()
    {
        GameManager.game.SetPlayerObject(this);
        roadList = MapGenerator.map.roadPathList;
        CheckRotaton();
        StartCoroutine("MoveToCell");
    }

    private void CheckRotaton()
    {
        var cell = _currentCell;
        if (_currentCell + 1 >= MapGenerator.map.roadPathList.Count) cell = 0;
        if (roadList[cell + 1].transform.position.x > roadList[_currentCell].transform.position.x) gameObject.transform.DORotateQuaternion(Quaternion.Euler(0, 90, 0), rotationTime);
        else if (roadList[cell + 1].transform.position.x < roadList[_currentCell].transform.position.x) gameObject.transform.DORotateQuaternion(Quaternion.Euler(0, -90, 0), rotationTime);
        else if (roadList[cell + 1].transform.position.z > roadList[_currentCell].transform.position.z) gameObject.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), rotationTime);
        else gameObject.transform.DORotateQuaternion(Quaternion.Euler(0, 180, 0), rotationTime);
    }

    public IEnumerator MoveToCell()
    {
        var cellPos = MapGenerator.map.roadPathList[_currentCell].transform.position;
        cellPos = new Vector3(cellPos.x, 0, cellPos.z);
        gameObject.transform.DOMove(cellPos, cellStepOverTime);
        yield return new WaitForSeconds(cellStepOverTime);
        if (_currentCell <= MapGenerator.map.roadPathList.Count)
        {
            StartCoroutine("MoveToCell");
            CheckRotaton();
            _currentCell++;
            if (_currentCell >= MapGenerator.map.roadPathList.Count) _currentCell = 0;

            GameEvents.events.PlayerMoved();
        }
        else
        {
            gameObject.GetComponent<Animator>().Play("Idle");
        }

    }
}
