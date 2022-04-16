using UnityEngine;
using UnityEngine.UI;

public class ButtonPanelMove : MonoBehaviour
{
    public Button button;
    private GameObject _panel;
    private Rect _rect;
    private float _y;
    private bool _isClosed;
    private Vector2 _anchor;
    private void Awake()
    {
        _isClosed = false;
        _panel = gameObject;
        _rect = gameObject.GetComponent<RectTransform>().rect;
        _anchor = GetComponent<RectTransform>().anchoredPosition;
        _y = _rect.y;
    }
    public void OnClick()
    {
        if (!_isClosed)
        {
            _panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(_anchor.x, -System.Math.Abs(_anchor.y));
            button.transform.rotation = Quaternion.Euler(0, 0, 180);
            _isClosed = true;
        }
        else
        {
            _panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(_anchor.x, System.Math.Abs(_anchor.y));
            button.transform.rotation = Quaternion.Euler(0, 0, 0);
            _isClosed = false;
        }
    }
}
