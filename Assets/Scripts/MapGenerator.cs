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
    private Vector3 _startTilePosition;
    private Vector3 _endTilePosition;
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
                    case int number when (number < 20):
                        fieldList.Add(MakeTile(GameManager.game.farmTileList[0], i, k, "farm"));
                        break;
                    case int number when (number >= 20 && number <= 50):
                        fieldList.Add(MakeTile(GameManager.game.obstaclesList[Random.Range(0, GameManager.game.obstaclesList.Count)], i, k, "obstacle"));
                        break;
                    default:
                        fieldList.Add(MakeTile(mapTileEmpty, i, k, "empty"));
                        break;
                }
            }
        }
    }
    private GameObject MakeTile(GameObject _tile, int horizontal, int vertical, string tileType)
    {
        var go = Instantiate(_tile, new Vector3(horizontal, 0, vertical), Quaternion.identity);
        go.transform.SetParent(tileFolder);
        go.GetComponent<TileClass>().ID = fieldList.Count;
        go.GetComponent<TileClass>().Type = tileType;
        return go;
    }
    private void ReplaceTile(int id)
    {
        switch (GameManager.game.currentTool)
        {
            case "road":
                if (!_isRoadEnded && !roadPathList.Contains(fieldList[id]))
                {
                    if (CheckPointerRoadDistance(id))
                    {
                        GameEvents.events.RoadTileSet();
                        var replacedTile = RemoveFieldTileAtId(id);
                        _tile = CreateAndInsertTile(GameManager.game.straightRoad, (int)_passedCoordinate.x, (int)_passedCoordinate.z, id, "road");
                        _tile.name = RandomCodeName();

                        roadPathList.Add(_tile);
                        ClearPathFromEmpties();
                        _roadPointer = fieldList[id].transform.position;
                        Destroy(replacedTile);
                        if (roadPathList.Count == 1)
                        {
                            _startTilePosition = roadPathList[0].transform.position;
                        }
                        if (roadPathList.Count >= 3)
                        {
                            var posCur = roadPathList[roadPathList.Count - 1].transform.position;
                            var posMid = roadPathList[roadPathList.Count - 2].transform.position;
                            var posPrevPrev = roadPathList[roadPathList.Count - 3].transform.position;
                            CheckForRotationAndCorners(posCur, posMid, posPrevPrev, roadPathList.Count - 2);
                        }
                    }
                    else
                    {
                        Debug.LogError("Unreacheble point");
                    }
                }
                break;
            case "random":
                if (!roadPathList.Contains(fieldList[id]))
                {
                    var g = RemoveFieldTileAtId(id);
                    CreateAndInsertTile(GameManager.game.obstaclesList[Random.Range(0, GameManager.game.obstaclesList.Count)], (int)_passedCoordinate.x, (int)_passedCoordinate.z, id, "obstacle");
                    Destroy(g);
                }
                break;
            case "end":
                if (CheckPointerRoadDistance(id) && (!roadPathList.Contains(fieldList[id]) || roadPathList[0].GetComponent<TileClass>().ID == id))
                {
                    if (fieldList[id].transform.position != roadPathList[0].transform.position) // check if end tile is not on start tile
                    {
                        var startTile = RemoveFieldTileAtId(roadPathList[0].gameObject.GetComponent<TileClass>().ID);
                        var startPosition = startTile.transform.position;
                        var startTileNew = CreateAndInsertTile(GameManager.game.startRoad, (int)startPosition.x, (int)startPosition.z, startTile.GetComponent<TileClass>().ID, "road");
                        roadPathList[0] = startTileNew;
                        Destroy(startTile);

                        var c = RemoveFieldTileAtId(id);
                        var roadEnd = CreateAndInsertTile(GameManager.game.endRoad, (int)_passedCoordinate.x, (int)_passedCoordinate.z, id, "road");
                        var endPos = roadEnd.transform.position;
                        var prevMid = roadPathList[roadPathList.Count - 1].transform.position;
                        var prevPrevPos = roadPathList[roadPathList.Count - 2].transform.position;
                        roadPathList.Add(roadEnd);
                        Destroy(c);


                        //check rotation of end Tile
                        if (endPos.x == prevMid.x && endPos.z > prevMid.z)
                        {
                            roadEnd.transform.Rotate(0, 180, 0); print("поворот");
                        }
                        else if (endPos.z == prevMid.z && endPos.x < prevMid.x)
                        {
                            roadEnd.transform.Rotate(0, 90, 0); print("поворот");
                        }
                        else if (endPos.z == prevMid.z && endPos.x > prevMid.x)
                        {
                            roadEnd.transform.Rotate(0, -90, 0); print("поворот");
                        }

                        CheckForRotationAndCorners(endPos, prevMid, prevPrevPos, roadPathList.Count - 2);

                        var startHouse = roadPathList[0].transform.position;
                        var nextToStart = roadPathList[1].transform.position;
                        if (nextToStart.x > startHouse.x && nextToStart.z == startHouse.z)
                        {
                            roadPathList[0].transform.Rotate(0, 90, 0); print("поворот");
                        }
                        else if (nextToStart.x < startHouse.x && nextToStart.z == startHouse.z)
                        {
                            roadPathList[0].transform.Rotate(0, -90, 0); print("поворот");
                        }
                        else if (nextToStart.x == startHouse.x && nextToStart.z < startHouse.z)
                        {
                            roadPathList[0].transform.Rotate(0, -180, 0);
                            print("поворот");
                        }
                    }


                    else // if end == start
                    {
                        var startPos = roadPathList[0].transform;
                        var endPos = roadPathList[roadPathList.Count - 1].transform.position;
                        CheckForRotationAndCorners(startPos.position, roadPathList[roadPathList.Count - 1].transform.position, roadPathList[roadPathList.Count - 2].transform.position, roadPathList.Count - 1);
                        CheckForRotationAndCorners(roadPathList[1].transform.position, roadPathList[0].transform.position, endPos, 0);
                    }

                    print(roadPathList[roadPathList.Count - 2].gameObject.name);
                    _isRoadEnded = true;
                    bool res = false;
                    foreach (var item in roadPathList)
                    {
                        if (item == null)
                        {
                            res = true;
                        }
                    }
                    if (res) GameEvents.events.RoadEnded();
                    else ClearPathFromEmpties();
                    GameEvents.events.RoadEnded();
                }
                break;
            default:
                break;
        }
    }

    private GameObject CreateAndInsertTile(GameObject tileObject, int x, int z, int id, string tileType)
    {
        var tile = MakeTile(tileObject, x, z, tileType);
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
        //   if (g.GetComponent<TileClass>().Type == "road")
        //   {
        //       _lastDestroyedRoadId = roadPathList.IndexOf(g);
        //   }
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

    private bool CheckForRotationAndCorners(Vector3 posCur, Vector3 posMid, Vector3 posPrevPrev, int roadPathID)
    {
        print(posCur);
        print(posMid);
        print(posPrevPrev);
        if (posPrevPrev == roadPathList[roadPathID].transform.position) ;
        bool result = false;
        if (!(posCur.z == posMid.z && posMid.z == posPrevPrev.z || posCur.x == posMid.x && posCur.x == posPrevPrev.x))
        {
            if ((posCur.x - posPrevPrev.x == 1 && posCur.z - posPrevPrev.z == 1) ||
                (posCur.x - posPrevPrev.x == 1 && posCur.z - posPrevPrev.z == -1) ||
                (posCur.x - posPrevPrev.x == -1 && posCur.z - posPrevPrev.z == -1) ||
                (posCur.x - posPrevPrev.x == -1 && posCur.z - posPrevPrev.z == 1))
            {
                Destroy(RemoveFieldTileAtId(roadPathList[roadPathID].GetComponent<TileClass>().ID));
                _tile = CreateAndInsertTile(GameManager.game.cornerRoad, (int)posMid.x, (int)posMid.z, roadPathList[roadPathID].GetComponent<TileClass>().ID, "road");
                _tile.transform.rotation = new Quaternion(0, 0, 0, 0);
                _tile.name = RandomCodeName();
                if (posCur.x < posPrevPrev.x && posCur.z > posPrevPrev.z)
                {
                    if (posCur.z == posMid.z)
                    {
                        _tile.transform.Rotate(0, -90, 0);
                        result = true;
                    }
                    else
                    {
                        _tile.transform.Rotate(0, 90, 0);
                        result = true;
                    }
                }
                else if (posCur.x > posPrevPrev.x && posCur.z < posPrevPrev.z)
                {
                    if (posMid.z == posCur.z)
                    {
                        _tile.transform.Rotate(0, 90, 0);
                        result = true;
                    }
                    else
                    {
                        _tile.transform.Rotate(0, -90, 0);
                        result = true;
                    }
                }
                else if ((posCur.x > posPrevPrev.x && posCur.z > posPrevPrev.z && posCur.z == posMid.z) ||
                    (posCur.x < posPrevPrev.x && posCur.z < posPrevPrev.z && posCur.x == posMid.x))
                {
                    _tile.transform.rotation = new Quaternion(0, -180, 0, 0);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            roadPathList[roadPathList.Count - 2] = _tile;
        }
        else
        {
            // only for line road (if corner is rotated - it bug)
            if (posCur.z == posPrevPrev.z)
            {
                roadPathList[roadPathList.Count - 2].transform.Rotate(0, 90, 0);
                result = true;
            }
            else
            {
                result = false;
            }
        }
        return result;
    }
}