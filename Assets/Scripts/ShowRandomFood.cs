using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRandomFood : MonoBehaviour
{
    public List<GameObject> food = new List<GameObject>();

    public void TakeRandom()
    {
        food[Random.Range(0, food.Count)].gameObject.SetActive(true);
    }

    public void TurnOff()
    {
        foreach (var item in food)
        {
            item.SetActive(false);
        }
    }
}
