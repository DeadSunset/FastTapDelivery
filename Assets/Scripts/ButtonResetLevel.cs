using UnityEngine;

public class ButtonResetLevel : MonoBehaviour
{
    public void Reset()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
