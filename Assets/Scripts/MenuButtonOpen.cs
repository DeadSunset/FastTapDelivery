using UnityEngine;

public class MenuButtonOpen : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.game.onMenuOpened = true;
    }
}
