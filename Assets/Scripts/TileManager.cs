using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager map;
    public List<GameObject> tilesTypeList = new List<GameObject>();
    public List<Transform> tileList = new List<Transform>();
    

    private void Awake()
    {
        if (map == null)
        {
            map = this;
        }
        else
        {
            Destroy(map);
        }
    }
}
