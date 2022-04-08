using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager game;
    public List<GameObject> obstaclesList = new List<GameObject>();
    public List<GameObject> roadList = new List<GameObject>();
    public GameObject straightRoad;
    public GameObject cornerRoad;
    public GameObject endRoad;
    public GameObject startRoad;
    [Header("TOOL IN WORK")]
    public string currentTool;

    private void Awake()
    {
        game = this;
    }

    private void Start()
    {
        GameEvents.events.OnSetCurrentTool += SetToolOnEvent;
    }

    private void SetToolOnEvent(string tool)
    {
        currentTool = tool;
    }
}
