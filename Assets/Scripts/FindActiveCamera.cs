using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindActiveCamera : MonoBehaviour
{
    [SerializeField]
    private Camera _fightCamera;

    private Camera[] _cams;
    private void Start()
    {
        _fightCamera = PlayerStats.playerStats.playerController.fightCamera.GetComponent<Camera>();
        gameObject.GetComponent<Canvas>().worldCamera = _fightCamera;
    }
}
