using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class World : MonoBehaviour {
	private GameState gameState;
//	public Image background;
//	public Pager pager;
	public bool allLevelsUnlocked;

	// Use this for initialization
	void Start () {
		gameState = (GameState)FindObjectOfType (typeof(GameState));	
		if (gameState != null) {
			GetComponent<Text> ().text = gameState.SelectedWorld.ToUpper();
	//		background.sprite = Resources.Load<Sprite>(gameState.SelectedWorld + "/background");
			//find levels
			LevelSelector[] levels = FindObjectsOfType(typeof(LevelSelector)) as LevelSelector[];
			for (int i = 0; i < levels.Length; i++) {
				levels [i].scene = gameState.SelectedWorld + "_" + levels [i].scene;
				if (PlayerPrefs.HasKey (levels [i].scene) || allLevelsUnlocked) {
					levels [i].locked = false;
				} else {
					levels [i].locked = true;
				}
				levels [i].SetLocks ();
			}
		//	pager.ChoosePage (0);
		} else {
			Debug.Log ("Couldn't find game state");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
