using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager cameraManager;

    public Canvas canvasFight;
    public Canvas canvasMain;
    public Canvas canvasStats;

    [SerializeField]
    private Camera _mainCamera;

    private void Awake()
    {
        cameraManager = this;
    }

    private void Start()
    {
        GameEvents.events.OnFightEnd += EnableMainCamera;
    }
    public void ChangeCameraViewToNewCamera(GameObject camera)
    {
        _mainCamera.enabled = false;
        camera.GetComponent<Camera>().enabled = true;
    }

    public void EnableMainCamera(bool win)
    {
        var cams = Camera.allCameras;
        foreach (var cam in cams)
        {
            cam.enabled = false;
        }
        _mainCamera.enabled = true;
    }
}
