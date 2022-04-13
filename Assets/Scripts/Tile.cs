using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [HideInInspector]
    public bool isAbleToBuildHere = false;
    [HideInInspector]
    public bool isRoad = false;
    [HideInInspector]
    public bool hasBonusResource = false;
    public virtual void OnEditBuild()
    {
        //buildTile
    }
}
