using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject fightCamera;
    public float cellStepOverTime;
    public float rotationTime;
    public List<EnemyClass> currentEnemies = new List<EnemyClass>();

    private List<GameObject> roadList;
    private int _currentCell;

    private AudioSource _source;
    [SerializeField]
    private AudioClip _walkSound;

    void Start()
    {
        _source = gameObject.AddComponent<AudioSource>();
        _currentCell = 0;
        GameManager.game.SetPlayerObject(this);
        roadList = MapGenerator.map.roadPathList;
        CheckRotaton();
        StartCoroutine("MoveToCell");
        GameEvents.events.SetCurrentTool("fight");
        if (PlayerStats.playerStats.playerController == null)
        {
            PlayerStats.playerStats.playerController = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
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
        _source.clip = _walkSound;
        _source.Play();
        var cellPos = MapGenerator.map.roadPathList[_currentCell].transform.position;
        cellPos = new Vector3(cellPos.x, 0.16f, cellPos.z);
        gameObject.transform.DOMove(cellPos, cellStepOverTime);
        yield return new WaitForSeconds(cellStepOverTime);
        if (MapGenerator.map.roadPathList[0].transform.position != MapGenerator.map.roadPathList[MapGenerator.map.roadPathList.Count - 1].transform.position)
        {
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
        else
        {
            if (_currentCell <= MapGenerator.map.roadPathList.Count)
            {
                StartCoroutine("MoveToCell");
                CheckRotaton();
                _currentCell++;
                GameEvents.events.PlayerMoved();
            }
        }

    }

    public void StartMoving()
    {
        StartCoroutine(MoveToCell());
    }
    public int EnemiesCount()
    {
        return currentEnemies.Count;
    }

    public void AssignSpeed(float stepTime, float rotatTime)
    {
        cellStepOverTime = stepTime;
        rotationTime = rotatTime;
    }
}
