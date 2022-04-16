using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator map;
    public GameObject mapTileEmpty;
    public int horizontalSizeOfField;
    public int verticalSizeOfField;
    public bool isRoadEnded;
    public List<GameObject> fieldList = new List<GameObject>();
    public List<GameObject> roadPathList = new List<GameObject>();
    [Header("TILE FOLDER")]
    public Transform tileFolder;

    private Vector3 _roadPointer;
    private Vector3 _passedCoordinate;
    private GameObject _tile;
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
    public void GenerateMap()
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
                fieldList[fieldList.Count - 1].name = (fieldList[fieldList.Count - 1].GetComponent<TileClass>().ID).ToString();
            }
        }
        var offsetX = horizontalSizeOfField % 2 == 0 ? horizontalSizeOfField / 2 - 0.5f : horizontalSizeOfField / 2;
        var offsetY = (verticalSizeOfField / 2) + (verticalSizeOfField % 2) + 1.5f;
        var offsetZ = verticalSizeOfField % 2 == 0 ? verticalSizeOfField / 2 + (verticalSizeOfField / 2.0f) * 0.45f : verticalSizeOfField / 2.0f + ((verticalSizeOfField / 2.0f) * 0.45f);
        Camera.main.transform.position = new Vector3(offsetX, offsetY, offsetZ);
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
                if (!isRoadEnded && !roadPathList.Contains(FindFieldObjectWithID(id)))
                {
                    if (CheckPointerRoadDistance(id))
                    {
                        GameEvents.events.RoadTileSet();
                        var replacedTile = RemoveFieldTileAtId(id);
                        if (roadPathList.Count == 0) _tile = CreateAndInsertTile(GameManager.game.startRoad, (int)_passedCoordinate.x, (int)_passedCoordinate.z, id, "road");
                        else _tile = CreateAndInsertTile(GameManager.game.straightRoad, (int)_passedCoordinate.x, (int)_passedCoordinate.z, id, "road");
                        _tile.name = RandomCodeName();

                        roadPathList.Add(_tile);
                        ClearPathFromEmpties();
                        _roadPointer = FindFieldObjectWithID(id).transform.position;
                        Destroy(replacedTile);
                        if (roadPathList.Count >= 3)
                        {
                            var posCur = roadPathList[roadPathList.Count - 1].transform.position;
                            var posMid = roadPathList[roadPathList.Count - 2].transform.position;
                            var posPrevPrev = roadPathList[roadPathList.Count - 3].transform.position;
                            CheckForRotationAndCorners(posCur, posMid, posPrevPrev, roadPathList.Count - 2);
                        }
                        else if (roadPathList.Count == 2)
                        {
                            var posCur = roadPathList[roadPathList.Count - 1].transform.position;
                            var posMid = roadPathList[roadPathList.Count - 2].transform.position;
                            CheckForRotationAndCorners(posCur, posMid, posMid, roadPathList.Count - 2);
                            roadPathList[roadPathList.Count - 2].transform.Rotate(0, 180, 0);
                        }
                    }
                    else
                    {
                        Debug.LogError("Unreacheble point");
                    }
                }
                break;
            case "random":
                if (!roadPathList.Contains(FindFieldObjectWithID(id)))
                {
                    var g = RemoveFieldTileAtId(id);
                    CreateAndInsertTile(GameManager.game.obstaclesList[Random.Range(0, GameManager.game.obstaclesList.Count)], (int)_passedCoordinate.x, (int)_passedCoordinate.z, id, "obstacle");
                    Destroy(g);
                }
                break;
            case "end":
                if (!isRoadEnded)
                {
                    if (CheckPointerRoadDistance(id) && (!roadPathList.Contains(FindFieldObjectWithID(id)) || roadPathList[0].GetComponent<TileClass>().ID == id || roadPathList[roadPathList.Count - 1].GetComponent<TileClass>().ID == id))
                    {
                        if (FindFieldObjectWithID(id).transform.position != roadPathList[0].transform.position) // check if end tile is not on start tile
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
                            }
                        }


                        else // if end == start
                        {
                            var startPos = roadPathList[0].transform;
                            var start = roadPathList[0].gameObject;
                            var endPos = roadPathList[roadPathList.Count - 1].transform.position;
                            bool corner = CheckForRotationAndCorners(roadPathList[1].transform.position, startPos.position, endPos, 0);
                            if (!corner)
                            {
                                var startRot = start.transform.rotation.eulerAngles;
                                var startId = start.GetComponent<TileClass>().ID;
                                print("start" + startId);
                                roadPathList[0] = CreateAndInsertTile(GameManager.game.straightRoad, (int)startPos.position.x, (int)startPos.position.z, startId, "road");
                                if (roadPathList[0].transform.rotation.eulerAngles != startRot) roadPathList[0].transform.rotation = Quaternion.Euler(startRot);
                                roadPathList[0].name = RandomCodeName();
                            }
                            corner = CheckForRotationAndCorners(startPos.position, endPos, roadPathList[roadPathList.Count - 2].transform.position, roadPathList.Count - 1);
                            if (!corner)
                            {
                                var end = roadPathList[roadPathList.Count - 1].gameObject;
                                var endRot = end.transform.rotation.eulerAngles;
                                var endId = end.GetComponent<TileClass>().ID;
                                print("end" + endId);
                                Destroy(end);
                                roadPathList[roadPathList.Count - 1] = CreateAndInsertTile(GameManager.game.straightRoad, (int)endPos.x, (int)endPos.z, endId, "road");
                                if (roadPathList[roadPathList.Count - 1].transform.rotation.eulerAngles != endRot) roadPathList[roadPathList.Count - 1].transform.rotation = Quaternion.Euler(endRot);
                                roadPathList[roadPathList.Count - 1].name = RandomCodeName();
                            }

                            Destroy(start);
                        }
                        ClearPathFromEmpties();
                        isRoadEnded = true;
                    }
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
        fieldList.Add(tile);
        return tile;
    }

    private GameObject FindFieldObjectWithID(int id)
    {
        return fieldList.Find(g => g != null && g.GetComponent<TileClass>().ID == id);
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
        var g = FindFieldObjectWithID(id);
        fieldList.Remove(g);
        return g;
    }


    private bool CheckPointerRoadDistance(int id)
    {
        if (roadPathList.Count != 0)
        {
            var tileX = System.Math.Abs(FindFieldObjectWithID(id).transform.position.x);
            var tileZ = System.Math.Abs(FindFieldObjectWithID(id).transform.position.z);
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
        if (posPrevPrev == roadPathList[roadPathID].transform.position) ;
        bool result = false;
        if (!(posCur.z == posMid.z && posMid.z == posPrevPrev.z || posCur.x == posMid.x && posCur.x == posPrevPrev.x))
        {
            if ((posCur.x - posPrevPrev.x == 1 && posCur.z - posPrevPrev.z == 1) ||
                (posCur.x - posPrevPrev.x == 1 && posCur.z - posPrevPrev.z == -1) ||
                (posCur.x - posPrevPrev.x == -1 && posCur.z - posPrevPrev.z == -1) ||
                (posCur.x - posPrevPrev.x == -1 && posCur.z - posPrevPrev.z == 1))
            {
                var roadId = roadPathList[roadPathID].GetComponent<TileClass>().ID;
                var road = RemoveFieldTileAtId(roadId);
                Destroy(road);
                _tile = CreateAndInsertTile(GameManager.game.cornerRoad, (int)posMid.x, (int)posMid.z, roadId, "road");
                _tile.transform.rotation = Quaternion.Euler(0, 0, 0);
                _tile.name = RandomCodeName();
                roadPathList[roadPathID] = _tile;
                if (posCur.x < posPrevPrev.x && posCur.z > posPrevPrev.z)
                {
                    if (posCur.z == posMid.z)
                    {
                        _tile.transform.Rotate(0, -90, 0);
                    }
                    else
                    {
                        _tile.transform.Rotate(0, 90, 0);
                    }
                }
                else if (posCur.x > posPrevPrev.x && posCur.z < posPrevPrev.z)
                {
                    if (posMid.z == posCur.z)
                    {
                        _tile.transform.Rotate(0, 90, 0);
                    }
                    else
                    {
                        _tile.transform.Rotate(0, -90, 0);
                    }
                }
                else if ((posCur.x > posPrevPrev.x && posCur.z > posPrevPrev.z && posCur.z == posMid.z) ||
                    (posCur.x < posPrevPrev.x && posCur.z < posPrevPrev.z && posCur.x == posMid.x))
                {
                    _tile.transform.rotation = new Quaternion(0, -180, 0, 0);
                }
            }
            result = true;
        }
        else
        {
            // only for line road (if corner is rotated - it bug)
            if (posCur.z == posPrevPrev.z)
            {
                if (roadPathList[roadPathID].transform.rotation.eulerAngles.y != 90)
                {
                    roadPathList[roadPathID].transform.rotation = Quaternion.Euler(0, 0, 0);
                    roadPathList[roadPathID].transform.rotation = Quaternion.Euler(0, 90, 0);
                }
            }
            result = false;
        }
        ClearPathFromEmpties();
        return result;
    }

}