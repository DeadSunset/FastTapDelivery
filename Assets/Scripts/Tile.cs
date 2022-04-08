using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [HideInInspector]
    public bool isAbleToBuildHere = false; //for game phase
    [HideInInspector]
    public bool isRoad = false;
    [HideInInspector]
    public bool hasBonusResource = false;
    private void OnEnable()
    {
        //TileManager.map.tileList.Add(gameObject.transform);
    }

    public virtual void OnEditBuild()
    {
        //buildTile
    }
}
