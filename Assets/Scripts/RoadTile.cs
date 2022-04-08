using System.Collections.Generic;
using UnityEngine;

public class RoadTile : Tile
{
    public List<GameObject> roadTypes = new List<GameObject>();
    public void OnMouseDrag()
    {
        
    }
    private void OnEnable()
    {
        isRoad = true;
    }
    override public void OnEditBuild()
    {
        //check for neighbour tiles and change road with rotation -- for edit phase (developer)
        isAbleToBuildHere = false;
        hasBonusResource = false;
    }
}
