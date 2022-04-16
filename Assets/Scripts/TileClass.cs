using UnityEngine;

public class TileClass : MonoBehaviour
{
    public int ID;
    public string Type;
    private Color _tempColor;
    private Material[] _mats;
    private bool _isClicked;
    private void Awake()
    {
        _mats = gameObject.GetComponent<Renderer>().materials;
        _tempColor = _mats[_mats.Length - 1].color;
    }
    private void OnMouseEnter()
    {
        gameObject.GetComponent<Renderer>().materials[_mats.Length - 1].color = Color.white;
        if (!GameManager.game.onMenuOpened && Input.GetMouseButton(0) && GameManager.game.currentTool != "end")
        {
            GameEvents.events.PassCoordinates(gameObject.transform.position);
            GameEvents.events.GetSquareId(ID);
        }
    }
    private void OnMouseExit()
    {
        gameObject.GetComponent<Renderer>().materials[_mats.Length - 1].color = _tempColor;
    }

    private void OnMouseDown()
    {
        if (!GameManager.game.onMenuOpened)
        {
            GameEvents.events.PassCoordinates(gameObject.transform.position);
            GameEvents.events.GetSquareId(ID);
        }
    }

    public void SetIdAndType(int ID, string Type)
    {
        this.ID = ID;
        this.Type = Type;
    }

}
