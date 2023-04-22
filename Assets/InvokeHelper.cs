using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvokeHelper : MonoBehaviour
{
    public void InvokeClick()
    {
        GetComponent<Button>().onClick.Invoke();
        Debug.Log("Invoked click on " + transform.name);
    }
}
