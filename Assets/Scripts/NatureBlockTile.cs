using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureBlockTile : Tile
{
    private void OnEnable()
    {
        isAbleToBuildHere = false;
        isRoad = false;
        hasBonusResource = false;
    }
}
