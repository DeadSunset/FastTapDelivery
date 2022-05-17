using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    private float _cellStepOverTime;
    [SerializeField]
    private float _rotationTime;



    public bool onMenuOpened;
    [Header("TOOL IN WORK")]
    public string currentTool;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        onMenuOpened = false;
        player = null;
        game = this;
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("indexCount"))
        {
            PlayerPrefs.SetInt("indexCount", 0);
        }
        GameEvents.events.OnSetCurrentTool += SetToolOnEvent;
        GameEvents.events.OnFightStart += ChangeToolForFight;
    }

    private void SetToolOnEvent(string tool)
    {
        currentTool = tool;
    }

    public void SetPlayerObject(PlayerController player)
    {
        this.player = player;
        AssignSpeedHero();
    }

    public void AssignSpeedHero()
    {
        player.AssignSpeed(_cellStepOverTime, _rotationTime);
    }

    public void ChangeToolForFight()
    {
        currentTool = "empty";
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(1);
    }
}
