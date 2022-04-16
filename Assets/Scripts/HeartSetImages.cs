using System.Collections.Generic;
using UnityEngine;

public class HeartSetImages : MonoBehaviour
{
    [SerializeField]
    private GameObject _heart;
    private List<GameObject> _heartsList = new List<GameObject>();

    private void Start()
    {
        GameEvents.events.OnRoadEnded += SetHearts;
        SetHearts();
    }
    public void SetHearts()
    {
        foreach (var item in _heartsList)
        {
            Destroy(item);
        }
        _heartsList.Clear();

        for (int i = 0; i < PlayerStats.playerStats.healthPoints; i++)
        {
            var h = Instantiate(_heart, transform);
            h.transform.SetParent(this.transform);
            _heartsList.Add(h);
        }
    }
}
