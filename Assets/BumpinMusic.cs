
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcGenMusic;

public class BumpinMusic : MonoBehaviour {
	MusicGenerator mMusicGenerator = null;
	private bool playingMusic;
	// Use this for initialization
	void Awake() {
		MusicGenerator[] musicGenerators = FindObjectsOfType (typeof(MusicGenerator)) as MusicGenerator[];	
		if (musicGenerators.Length > 1) {
			Debug.Log ("Getting rid of excess game states");
			Destroy (gameObject);
		} else {
			DontDestroyOnLoad (gameObject);

		}
	}

	void Start () {
		playingMusic = false;

	}
	
	// Update is called once per frame
	void Update () {
		if (MusicGenerator.Instance != null && !playingMusic) {
			playingMusic = true;
			mMusicGenerator = MusicGenerator.Instance;
			mMusicGenerator.SetVolume(2);
			mMusicGenerator.SetState(eGeneratorState.playing);
		}
	}
}
