using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PageSet
{
	public GameObject[] items;
	public Image navImage;
}
public class Pager : MonoBehaviour {
	public PageSet[] pages;
	public Sprite activePage;
	public Sprite inactivePage;
	// Use this for initialization
	void Start () {
	}

	public void ChoosePage(int page) {
		Debug.Log ("Choosing Page");
		for (int i = 0; i < pages.Length; i++) {
			pages [i].navImage.sprite = inactivePage;
			for (int j = 0; j < pages [i].items.Length; j++) {				
				pages [i].items [j].SetActive (false);
			}
		}
		pages [page].navImage.sprite = activePage;

		for (int i = 0; i < pages [page].items.Length; i++) {
			pages [page].items [i].SetActive (true);
		}

	}

}
