using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator map;
    public GameObject mapTileEmpty;
    public int horizontalSizeOfField;
    public int verticalSizeOfField;
    public List<GameObject> fieldList = new List<GameObject>();
    public List<GameObject> roadPathList = new List<GameObject>();
    [Header("TILE FOLDER")]
    public Transform tileFolder;

    private Vector3 _roadPointer;
    private Vector3 _passedCoordinate;
    private GameObject _tile;
    private int _lastDestroyedRoadId = -1;
    private bool _isRoadEnded;
    private void Awake()
    {
        map = this;
        GenerateMap();
    }
    private void Start()
    {
        GameEvents.events.OnGetSquareId += ReplaceTile;
        GameEvents.events.OnPassCoordinates += GetPassedCoordinate;
    }
    private void GenerateMap()
    {
        for (int i = 0; i < horizontalSizeOfField; i++)
        {
            for (int k = 0; k < verticalSizeOfField; k++)
            {
                switch (Random.Range(0, 100))
                {
                    case int number when (number < 70):
                        fieldList.Add(MakeTile(mapTileEmpty, i, k));
                        break;
                    case int number when (number >= 70):
                        fieldList.Add(MakeTile(GameManager.game.obstaclesList[Random.Range(0, GameManager.game.obstaclesList.Count)], i, k));
                        break;
                }
            }
        }
    }
    private GameObject MakeTile(GameObject _tile, int horizontal, int vertical)
    {
        var go = Instantiate(_tile, new Vector3(horizontal, 0, vertical), Quaternion.identity);
        go.transform.SetParent(tileFolder);
        go.GetComponent<TileClass>().ID = fieldList.Count;
        return go;
    }
    private void ReplaceTile(int id)
    {
        if (!roadPathList.Contains(fieldList[id])) // if the pressed tile is not a Road
        {
            switch (GameManager.game.currentTool)
            {
                case "road":
                    if (!_isRoadEnded)
                    {
                        if (CheckPointerRoadDistance(id))
                        {
                            GameEvents.events.RoadTileSet();
                            var replacedTile = RemoveFieldTileAtId(id);
                            _tile = CreateAndInsertTile(GameManager.game.straightRoad, (int)_passedCoordinate.x, (int)_passedCoordinate.z, id, "road");
                            _tile.name = RandomCodeName();
                            if (replacedTile.GetComponent<TileClass>().Type != "road")
                            {
                                roadPathList.Add(_tile);
                                ClearPathFromEmpties();
                                _roadPointer = fieldList[id].transform.position;
                            }
                            else
                            {
                                roadPathList[_lastDestroyedRoadId] = _tile;
                            }
                            Destroy(replacedTile);
                            if (roadPathList.Count >= 3)
                            {
                                var posCur = roadPathList[roadPathList.Count - 1].transform.position;
                                var posMid = roadPathList[roadPathList.Count - 2].transform.position;
                                var posPrevPrev = roadPathList[roadPathList.Count - 3].transform.position;
                                if (!(posCur.z == posMid.z && posMid.z == posPrevPrev.z || posCur.x == posMid.x && posCur.x == posPrevPrev.x))
                                {
                                    if ((posCur.x - posPrevPrev.x == 1 && posCur.z - posPrevPrev.z == 1) ||
                                        (posCur.x - posPrevPrev.x == 1 && posCur.z - posPrevPrev.z == -1) ||
                                        (posCur.x - posPrevPrev.x == -1 && posCur.z - posPrevPrev.z == -1) ||
                                        (posCur.x - posPrevPrev.x == -1 && posCur.z - posPrevPrev.z == 1))
                                    {
                                        Debug.LogWarning("Идем на поворот");
                                        Destroy(RemoveFieldTileAtId(roadPathList[roadPathList.Count - 2].GetComponent<TileClass>().ID));
                                        _tile = CreateAndInsertTile(GameManager.game.cornerRoad, (int)posMid.x, (int)posMid.z, roadPathList[roadPathList.Count - 2].GetComponent<TileClass>().ID, "road");
                                        if (posCur.x < posPrevPrev.x && posCur.z > posPrevPrev.z)
                                        {
                                            if (posCur.z == posMid.z)
                                            {
                                                _tile.transform.Rotate(0, -90, 0);
                                                print(-90);
                                            }
                                            else
                                            {
                                                _tile.transform.Rotate(0, 90, 0);
                                                print(90);
                                            }
                                        }
                                        else if (posCur.x > posPrevPrev.x && posCur.z < posPrevPrev.z)
                                        {
                                            if (posMid.z == posCur.z)
                                            {
                                                _tile.transform.Rotate(0, 90, 0);
                                                print(90);
                                            }
                                            else
                                            {
                                                _tile.transform.Rotate(0, -90, 0);
                                                print(-90);
                                            }
                                        }
                                        else if ((posCur.x > posPrevPrev.x && posCur.z > posPrevPrev.z && posCur.z == posMid.z) ||
                                            (posCur.x < posPrevPrev.x && posCur.z < posPrevPrev.z && posCur.x == posMid.x))
                                        {
                                            _tile.transform.rotation = new Quaternion(0, -180, 0, 0);
                                            print(-180);
                                        }
                                    }
                                    else
                                    {
                                        print("Поворот не требуется");
                                    }
                                    roadPathList[roadPathList.Count - 2] = _tile;
                                }
                                else
                                {
                                    if (posCur.z == posMid.z)
                                    {
                                        roadPathList[roadPathList.Count - 2].transform.Rotate(0, 90, 0);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Debug.LogError("Unreacheble point");
                        }
                    }
                    break;
                case "random":
                    var g = RemoveFieldTileAtId(id);
                    CreateAndInsertTile(GameManager.game.obstaclesList[Random.Range(0, GameManager.game.obstaclesList.Count)], (int)_passedCoordinate.x, (int)_passedCoordinate.z, id, "obstacle");
                    Destroy(g);
                    break;
                case "end":
                    if (CheckPointerRoadDistance(id))
                    {
                        var start = RemoveFieldTileAtId(roadPathList[0].gameObject.GetComponent<TileClass>().ID);
                        var passedVector = start.transform.position;
                        var startTileNew = CreateAndInsertTile(GameManager.game.startRoad, (int)passedVector.x, (int)passedVector.z, start.GetComponent<TileClass>().ID, "road");
                        roadPathList[0] = startTileNew;
                        Destroy(start);
                        
                        var c = RemoveFieldTileAtId(id);
                        var roadEnd = CreateAndInsertTile(GameManager.game.endRoad, (int)_passedCoordinate.x, (int)_passedCoordinate.z, id, "road");
                        var endPos = roadEnd.transform.position;
                        var prevEnd = roadPathList[roadPathList.Count - 1].transform.position;
                        roadPathList.Add(roadEnd);
                        Destroy(c);
                        //check for corners before end
                        if (endPos.x == prevEnd.x && endPos.z > prevEnd.z)
                        {
                            roadEnd.transform.Rotate(0, 180, 0);
                        }
                        else if (endPos.z == prevEnd.z && endPos.x < prevEnd.x)
                        {
                            roadEnd.transform.Rotate(0, 90, 0);
                        }
                        else if (endPos.z == prevEnd.z && endPos.x > prevEnd.x)
                        {
                            roadEnd.transform.Rotate(0, -90, 0);
                        }
                        //check for endTile rotation
                        if (endPos.z == prevEnd.z && endPos.x < prevEnd.x)
                        {
                            roadPathList[roadPathList.Count - 2].transform.Rotate(0, 90, 0);
                        }
                        else if (endPos.z == prevEnd.z && endPos.x > prevEnd.x)
                        {
                            roadPathList[roadPathList.Count - 2].transform.Rotate(0, -90, 0);
                        }
                        //check for Rotation of StartTile
                        var startHouse = roadPathList[0].transform.position;
                        var nextToStart = roadPathList[1].transform.position;
                        if (nextToStart.x > startHouse.x && nextToStart.z == startHouse.z)
                        {
                            roadPathList[0].transform.Rotate(0, 90, 0);
                        }
                        else if (nextToStart.x < startHouse.x && nextToStart.z == startHouse.z)
                        {
                            roadPathList[0].transform.Rotate(0, -90, 0);
                        }
                        else if (nextToStart.x == startHouse.x && nextToStart.z < startHouse.z)
                        {
                            roadPathList[0].transform.Rotate(0, -180, 0);
                        }

                        _isRoadEnded = true;
                        GameEvents.events.RoadEnded();
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private GameObject CreateAndInsertTile(GameObject tileObject, int x, int z, int id, string tileType)
    {
        var tile = MakeTile(tileObject, x, z);
        tile.GetComponent<TileClass>().SetIdAndType(id, tileType);
        fieldList.Insert(id, tile);
        return tile;
    }

    private void ClearPathFromEmpties()
    {
        for (int i = 0; i < roadPathList.Count; i++)
        {
            if (!roadPathList[i])
            {
                var index = roadPathList.IndexOf(roadPathList[i]);
                roadPathList.RemoveAt(index);
            }
        }
    }
    private void GetPassedCoordinate(Vector3 pos)
    {
        _passedCoordinate = pos;
    }

    private GameObject RemoveFieldTileAtId(int id)
    {
        var g = fieldList[id].gameObject;
        if (g.GetComponent<TileClass>().Type == "road")
        {
            _lastDestroyedRoadId = roadPathList.IndexOf(g);
        }
        fieldList.RemoveAt(id);
        return g;
    }


    private bool CheckPointerRoadDistance(int id)
    {
        if (roadPathList.Count != 0)
        {
            var tileX = System.Math.Abs(fieldList[id].transform.position.x);
            var tileZ = System.Math.Abs(fieldList[id].transform.position.z);
            var pointerX = System.Math.Abs(_roadPointer.x);
            var pointerZ = System.Math.Abs(_roadPointer.z);
            if ((pointerX == tileX && System.Math.Abs(pointerZ - tileZ) <= 1) || (pointerZ == tileZ && System.Math.Abs(pointerX - tileX) <= 1))
            {
                return true;
            }
            else return false;
        }
        else return true;
    }

    private string RandomCodeName()
    {
        return Random.Range(10, 100) + "C" + Random.Range(1, 10);
    }

}