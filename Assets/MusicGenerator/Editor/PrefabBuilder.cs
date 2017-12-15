namespace ProcGenMusic
{
﻿using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;

public static class PrefabBuilder
{
	public static string mPrefabTempDirectory = "Assets/TempMusicPrefabs/";
	public static string mPrefabTempDirectoryParent = "Assets/";

	/// creates a prefab from a game object
	public static void CreatePrefab(GameObject go)
	{
		if(!Directory.Exists(mPrefabTempDirectory))
			Directory.CreateDirectory(mPrefabTempDirectory);
		string path = mPrefabTempDirectory + go.name + ".prefab";
		if(PrefabUtility.CreatePrefab(path, go) == null)
			throw new ArgumentNullException(go.name);
		else
		{
			AssetImporter.GetAtPath(path).assetBundleName = go.name;
		}
	}
	
	/// Creates prefab game objects from clips, so we can create assetBundles from them 
	/// This is a bit of a workaround, as I don't think I'm able to just create assetBundle of a single audioClip.
	/// So, we create a gameObject with an audio source, set its audioClip, and then save the prefab to create the 
	/// asset.
	/// this is super ugly, sorry :P
	[MenuItem("Assets/Build PrefabsFromPaths")]
	public static void CreatePrefabsFromClips()
	{
		string[] pathName = Directory.GetDirectories( MusicHelpers.GetMusicGeneratorPath() + "/Assets/Resources/Music/");
		for(int i = 0; i < pathName.Length; i++)
			pathName[i] = Path.GetFileName(pathName[i]);
		
		List<List<List<AudioClip>>> mAllClips = new List<List<List<AudioClip>>>();
		int numNotes = 37;
		string generatorPath = MusicHelpers.GetMusicGeneratorPath();
		for (int j = 0; j < pathName.Length; j++)
		{
			mAllClips.Add (new List<List<AudioClip>> ());
			mAllClips[mAllClips.Count -1].Add (new List<AudioClip> ());
			GameObject go = new GameObject();
			go.name = pathName[j];
			InstrumentPrefabList instrumentList = go.AddComponent<InstrumentPrefabList>();
			
			for (int i = 1; i < numNotes; i++)
			{
				string clipPath = generatorPath + "/Assets/Resources/Music/" +pathName [j] + "/" + i.ToString () ;
				
				string assetPath = clipPath;
				string toRemove = Application.dataPath + "/";
				assetPath = "Assets/"+assetPath.Remove(assetPath.IndexOf(toRemove), toRemove.Length);
				
				AudioClip clip = Resources.Load ("Music/" +pathName [j] +"/"+ (i).ToString ())as AudioClip;
				if (clip)
				{
					mAllClips [j] [mAllClips [j].Count-1].Add (clip);
					AudioSource source = instrumentList.gameObject.AddComponent<AudioSource>();
					source.clip = clip;
					instrumentList.mAudioSources[i-1] = (source);
					if(File.Exists(clipPath+ ".mp3"))
						AssetImporter.GetAtPath(assetPath + ".mp3").assetBundleName = go.name;
				}
			}

			PrefabBuilder.CreatePrefab(go);
			UnityEngine.Object.DestroyImmediate(go);
		}
	}
}
}