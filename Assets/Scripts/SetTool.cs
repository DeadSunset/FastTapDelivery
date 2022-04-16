using UnityEngine;

public class SetTool : MonoBehaviour
{
    public string tool;
    public Vector2 upscaleCellSize;
    private float cellSize;
    private Rect thisCell;
    private void Start()
    {
        GameEvents.events.OnToolIconResize += Resize;
        thisCell = gameObject.GetComponent<RectTransform>().rect;
        cellSize = thisCell.width;
    }
    public void SetCurrentTool()
    {
        if (!GameManager.game.onMenuOpened)
        {
            GameEvents.events.SetCurrentTool(tool);
            gameObject.GetComponent<RectTransform>().sizeDelta = upscaleCellSize;
            GameEvents.events.ToolIconResize();
        }
    }

    private void Resize()
    {
        if (tool != GameManager.game.currentTool)
        {
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(cellSize, cellSize);
        }
    }
}
