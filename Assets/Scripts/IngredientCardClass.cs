using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientCardClass : MonoBehaviour
{

    [SerializeField]
    protected Sprite foodSprite;

    public float hungerFullfilment;
    public bool isGoingToDish;

    [SerializeField]
    private Text _textName;
    [SerializeField]
    private Text _textFullfilment;
    [SerializeField]
    private string _foodName;
    private DishClass _dish;
    private Outline[] _lines;

    private void Start()
    {
        _lines = gameObject.GetComponents<Outline>();
        _dish = gameObject.GetComponentInParent<DishClass>();
        _textName.text = _foodName;
        _textFullfilment.text = hungerFullfilment.ToString();
        gameObject.GetComponent<Image>().sprite = foodSprite;
    }

    public void OnClick()
    {
        if (isGoingToDish)
        {
            DisableOutlines();
            isGoingToDish = false;
        }
        else
        {
            foreach (var outline in _lines)
            {
                outline.enabled = true;
            }
            isGoingToDish = true;
        }
        GameEvents.events.IngredientTake();

    }

    public void DisableOutlines()
    {
        foreach (var outline in _lines)
        {
            outline.enabled = false;
        }
    }


}
