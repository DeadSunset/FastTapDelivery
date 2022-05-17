using UnityEngine;
using UnityEngine.UI;

public class GameResultShow : MonoBehaviour
{
    [SerializeField]
    private Sprite _win;
    [SerializeField]
    private Sprite _lose;

    [SerializeField]
    private Image _image;
    private void Start()
    {
        GameEvents.events.OnGameEnd += Assign;
    }

    private void Assign(bool win)
    {
        gameObject.GetComponent<Canvas>().enabled = true;
        if (win)
        {
            _image.sprite = _win;
        }
        else
        {
            _image.sprite = _lose;
        }

        PlayerStats.playerStats.playerController.StopAllCoroutines();
        Destroy(PlayerStats.playerStats.playerController.gameObject.GetComponent<PlayerController>());
    }
}
