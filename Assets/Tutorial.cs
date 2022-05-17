using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject _canvasStart;

    [SerializeField]
    private GameObject _canvasBuildEndPoint;
    [SerializeField]
    private GameObject _canvasStartGamePlay;


    private void Start()
    {
        _canvasStart.SetActive(true);
        GameEvents.events.OnPointIsReady += BuildPoint;
        GameEvents.events.OnRoadEnded += GamePlayStart;

    }

    private void GamePlayStart()
    {
        _canvasBuildEndPoint.SetActive(false);
        _canvasStartGamePlay.SetActive(true);
    }

    private void BuildPoint()
    {
        _canvasStart.SetActive(false);
        _canvasBuildEndPoint.SetActive(true);
    }
}
