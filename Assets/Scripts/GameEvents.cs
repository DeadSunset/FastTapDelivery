using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{

    public static GameEvents events;
    public event Action<int> OnGetSquareId;
    public event Action OnToolIconResize;
    public event Action OnRoadEnded;
    public event Action OnRoadTileSet;
    public event Action<string> OnSetCurrentTool;
    public event Action<Vector3> OnPassCoordinates;
    public void GetSquareId(int id) => OnGetSquareId?.Invoke(id);
    public void ToolIconResize() => OnToolIconResize?.Invoke();
    public void RoadEnded() => OnRoadEnded?.Invoke();
    public void RoadTileSet() => OnRoadTileSet?.Invoke();
    public void SetCurrentTool(string tool) => OnSetCurrentTool?.Invoke(tool);
    public void PassCoordinates(Vector3 pos) => OnPassCoordinates?.Invoke(pos);

    private void Awake()
    {
        events = this;
    }
}
