using UnityEngine;
using UnityEngine.UI;

public class InputAssign : MonoBehaviour
{
    public Text number;
    public void AssignVertical()
    {
        MapGenerator.map.verticalSizeOfField = System.Convert.ToInt32(number.text.ToString());
    }

    public void AssignHorizontal()
    {
        MapGenerator.map.horizontalSizeOfField = System.Convert.ToInt32(number.text.ToString());
    }
}
