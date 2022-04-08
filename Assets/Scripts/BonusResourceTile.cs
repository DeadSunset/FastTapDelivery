public class BonusResourceTile : Tile
{
    private void OnEnable()
    {
        hasBonusResource = true;
    }

    override public void OnEditBuild()
    {
        isRoad = false;
        isAbleToBuildHere = true; // game phase
    }
}
