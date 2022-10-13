using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingManagerUI : MonoBehaviour
{
    [SerializeField] GameObject floatingCanvas;
    [SerializeField] Image uiFloatingExclaimPrefab;
    Image uiFloatingExclaim;
    [SerializeField] Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        uiFloatingExclaim = Instantiate(uiFloatingExclaimPrefab, floatingCanvas.transform).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        uiFloatingExclaim.transform.position = Camera.main.WorldToScreenPoint(transform.position + offset);
        Unit selectingUnit = UnitActionSystem.Instance.GetSelectingUnit();
        float dist = 1 / Vector3.Distance(transform.position, selectingUnit.GetWorldPosition()) * 2;
        dist = Mathf.Clamp(dist, 1f, 3f);
        uiFloatingExclaim.transform.localScale = new Vector3(dist, dist, 0);
    }
}
