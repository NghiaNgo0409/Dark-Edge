using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] List<GameObject> itemIconList;
    [SerializeField] List<GameObject> itemIconLockList;
    [SerializeField] List<GameObject> buttonMapSelectList;
    [SerializeField] List<GameObject> buttonMapSelectLockList;
    // Start is called before the first frame update
    void Start()
    {
        UpdateMapSelect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateMapSelect()
    {
        for (int i = 2; i < 5; i++)
        {
            if (GameManager.Instance.GetMap(i) != 0)
            {
                if (i == 4) return;
                int index = i - 2;
                itemIconList[index].SetActive(true);
                itemIconLockList[index].SetActive(false);
                buttonMapSelectList[index].SetActive(true);
                buttonMapSelectLockList[index].SetActive(false);
            }
        }
    }
}
