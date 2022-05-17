using UnityEngine;

public class GameProcess : MonoBehaviour
{
    public GameObject hero;
    private GameObject _char;
    void Start()
    {
        GameEvents.events.OnRoadEnded += StartGame;
    }

    private void StartGame()
    {
        var cellStart = MapGenerator.map.roadPathList[1].transform.position;
        cellStart = new Vector3(cellStart.x, 0, cellStart.z);
        _char = Instantiate(hero, MapGenerator.map.roadPathList[1].transform.position, Quaternion.identity);
        _char.transform.position = new Vector3(cellStart.x, hero.transform.position.y + 0.31f, cellStart.z);
        _char = _char.GetComponentInChildren<PlayerController>().gameObject;
        _char.GetComponent<Animator>().Play("Run");
    }

}
