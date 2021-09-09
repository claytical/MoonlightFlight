using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    private string selectedScene;
    private AsyncOperation AO;


    public void SetSceneToLoad(string s)
    {
        selectedScene = s;
    }

    public void SelectScene()
    {
        StartCoroutine("loadScene");
    }

    IEnumerator loadScene()
    {
        AO = SceneManager.LoadSceneAsync(selectedScene, LoadSceneMode.Single);
        AO.allowSceneActivation = false;
        while (AO.progress < 0.9f)
        {
            yield return null;
        }
        AO.allowSceneActivation = true;
    }


}
