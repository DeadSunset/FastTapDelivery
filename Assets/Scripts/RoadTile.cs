using System.Collections.Generic;
using UnityEngine;

public class RoadTile : Tile
{
    private void OnEnable()
    {
        isRoad = true;
    }
    override public void OnEditBuild()
    {
        isAbleToBuildHere = false;
        hasBonusResource = false;
    }
}
