using UnityEngine;
using UnityEngine.UI;

public class ButtonRoadEnd : MonoBehaviour
{
    public Text number;
    public int numberForRoadEndAvaliable = 15;
    public GameObject buttonRoad;
    private int count =0;

    private bool isButton;
    private void OnEnable()
    {
        number.text = numberForRoadEndAvaliable.ToString();
        gameObject.GetComponent<Button>().interactable = false;
        number.gameObject.SetActive(true);
        gameObject.GetComponent<Button>().interactable = false;
    }
    private void Start()
    {
        isButton = false;
        GameEvents.events.OnRoadTileSet += SetButton;
        GameEvents.events.OnRoadEnded += DeactivateButtonsForRoad;
    }
    private void SetButton()
    {
        count++;
        number.text = (numberForRoadEndAvaliable - count).ToString();
        if (count >= numberForRoadEndAvaliable && !isButton)
        {
            isButton = true;
            GameEvents.events.PointIsReady();
            number.gameObject.SetActive(false);
            gameObject.GetComponent<Button>().interactable = true;
        }
    }

    private void DeactivateButtonsForRoad()
    {
        buttonRoad.SetActive(false);
        gameObject.SetActive(false);
    }
}
