using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {
	public GameObject retryMenu;
	public Text levelReachedLabel;
	// Use this for initialization
	void Start () {
	
	}

	public void showRetryMenu() {
		retryMenu.SetActive(true);
		gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
