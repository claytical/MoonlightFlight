using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMeter : MonoBehaviour
{

    public GameObject unit;
    // Start is called before the first frame update

    public void Populate(int amount)
    {
        DestroyAllChildren(transform);
        for (int i = 0; i < amount; i++)
        {
            Instantiate(unit, transform);
        }
    }

    public void Add(CanvasMeter container)
    {
        Debug.Log("ADDING UNIT");
        Instantiate(unit, transform);
        int index = transform.childCount - 1;
        container.Deactivate(index);
    }
    public int Count()
    {
        return transform.childCount;
    }
    public void Deactivate(int index)
    {
        GetComponentsInChildren<Image>()[index].enabled = false;

    }
    public void DestroyAllChildren(Transform parentTransform)
    {
        foreach (Transform child in parentTransform)
        {
            Destroy(child.gameObject);
        }
    }
}
