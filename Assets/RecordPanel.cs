using UnityEngine;

public class RecordPanel : MonoBehaviour
{
    public int[] index = new int[10];
    [SerializeField]
    private GameObject recordTag;
    [SerializeField]
    private GameObject emptyTag;

    private void Start()
    {
        var res = PlayerPrefs.GetInt("indexCount");
        if (res == 0) Instantiate(emptyTag, transform);
        print(res);
        ShowSaves();
    }

    private void ShowSaves()
    {
        for (int i = 0; i < index.Length; i++)
        {
            var res = PlayerPrefs.GetInt("exist" + i);
            if (res == 1)
            {
                var tag = Instantiate(recordTag, transform);
                var name = PlayerPrefs.GetString("name" + i);
                var record = PlayerPrefs.GetInt("record" + i);
                tag.GetComponent<RecordTag>().WriteRecord(name, record);
            }
        }
    }
}
