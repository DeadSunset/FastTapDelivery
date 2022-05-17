using System.Collections.Generic;
using UnityEngine;

public class HeartSetImages : MonoBehaviour
{
    [SerializeField]
    private GameObject _heart;
    [SerializeField]
    private GameObject _heartHolder;
    [SerializeField]
    public List<GameObject> heartsList = new List<GameObject>();

    private void Start()
    {
        GameEvents.events.OnRoadEnded += SetHearts;
        SetHearts();
    }
    public void SetHearts()
    {
        foreach (var item in heartsList)
        {
            Destroy(item);
        }
        heartsList.Clear();

        for (int i = 0; i < PlayerStats.playerStats.maxHealthPoints; i++)
        {
            var h = Instantiate(_heart, _heartHolder.transform);
            h.transform.SetParent(_heartHolder.transform);
            heartsList.Add(h);
        }
    }
}
