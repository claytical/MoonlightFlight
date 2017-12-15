
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcGenMusic;

public class BumpinMusic : MonoBehaviour {
	MusicGenerator mMusicGenerator = null;
	// Use this for initialization
	void Start () {
		mMusicGenerator = MusicGenerator.Instance;
		mMusicGenerator.SetState(eGeneratorState.playing);
			
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
