using UnityEngine;
using UnityEngine.UI;

public class CounterAnimalFed : MonoBehaviour
{
    public static CounterAnimalFed fed;
    private Text text;

    public int _counter;

    [SerializeField]
    private int _numToWin;

    private void Awake()
    {
        fed = this;
    }
    private void Start()
    {
       _numToWin = Random.Range(8, 15);
        text = gameObject.GetComponent<Text>();
        _counter = 0;
        GameEvents.events.OnFightEnd += UpdateText;
        text.text = "   0 из " + _numToWin.ToString();
    }

    private void UpdateText(bool win)
    {
        if (win)
        {
            _counter++;
            text.text = "   " + _counter.ToString() + " из " + _numToWin.ToString();
            if (_counter >= _numToWin)
            {
                GameEvents.events.GameEnd(true);
            }
        }
    }
}
