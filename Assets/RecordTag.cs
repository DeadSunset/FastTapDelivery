using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordTag : MonoBehaviour
{
    [SerializeField]
    private Text name;

    [SerializeField]
    private Text record;

    public void WriteRecord(string name, int record)
    {
        this.name.text = name;
        this.record.text = record.ToString();
    }
}
