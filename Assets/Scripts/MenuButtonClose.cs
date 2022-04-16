using UnityEngine;

public class MenuButtonClose : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.game.onMenuOpened = false;
    }
}
