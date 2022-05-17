using UnityEngine;
using UnityEngine.UI;

public class win : MonoBehaviour
{
    private string _playerName;
    public void Func()
    {
        _playerName = gameObject.GetComponent<InputField>().text;
        var index = PlayerPrefs.GetInt("indexCount") + 1;
        PlayerPrefs.SetInt("indexCount", index);
        PlayerPrefs.SetInt("exist"+index, 1);
        PlayerPrefs.SetString("name" + index, _playerName);
        PlayerPrefs.SetInt("record" + index, CounterAnimalFed.fed._counter);
    }
}
