using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager game;
    [HideInInspector]
    public PlayerController player;
    public List<GameObject> obstaclesList = new List<GameObject>();
    public List<GameObject> roadList = new List<GameObject>();
    public List<GameObject> farmTileList = new List<GameObject>();
    public GameObject straightRoad;
    public GameObject cornerRoad;
    public GameObject endRoad;
    public GameObject startRoad;
    [SerializeField]
    private int targetFPS;

    public bool onMenuOpened;
    [Header("TOOL IN WORK")]
    public string currentTool;

    private void Awake()
    {
        Application.targetFrameRate = targetFPS;
        onMenuOpened = false;
        player = null;
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

    public void SetPlayerObject(PlayerController player)
    {
        this.player = player;
    }
}
