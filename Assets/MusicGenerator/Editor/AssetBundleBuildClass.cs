namespace ProcGenMusic
{
	using UnityEngine;
	using UnityEditor;
	using System.Diagnostics;
	using System;
	public static class AssetBundleBuildClass
	{
		/// Will build prefabs, create asset bundles of any audio files in "MusicGenerator/Assets/Resources/Music/"
		/// for win/Linux/Mac and cleanup created prefabs.
		/// To note: this will build _all_ of your asset bundles. 
		/// If you're adding custom sounds, best case is to create a temporary scene then export.
		[MenuItem("Assets/Build And Clean All Music Assets")]
		public static void BuildAndCleanAll()
		{
			PrefabBuilder.CreatePrefabsFromClips();
			BuildLinuxAssetBundles();
			BuildWindowsAssetBundles();
			BuildMacAssetBundles();
			CleanupPrefabs();
		}

		/// Will create asset bundles for linux
		[MenuItem("Assets/Build Linux Music Assets")]
		public static void BuildLinuxAssetBundles()
		{
			string assetPath = Application.streamingAssetsPath + "/MusicGenerator/Linux";
			bool pathExists = System.IO.Directory.Exists(assetPath);
			if (!pathExists)
				throw new Exception(assetPath + " doesn't exist. Please Move the StreamingAsset folder from the MusicGenerator asset to your main Assests path (Application.datapath)");

			BuildAssetBundles(Application.streamingAssetsPath + "/MusicGenerator/Linux/", BuildTarget.StandaloneLinux64);
		}

		/// Will create asset bundles for windows
		[MenuItem("Assets/Build Windows Music Assets")]
		public static void BuildWindowsAssetBundles()
		{
			string assetPath = Application.streamingAssetsPath + "/MusicGenerator/Windows";
			bool pathExists = System.IO.Directory.Exists(assetPath);
			if (!pathExists)
				throw new Exception(assetPath + " doesn't exist. Please Move the StreamingAsset folder from the MusicGenerator asset to your main Assests path (Application.datapath)");

			BuildAssetBundles(Application.streamingAssetsPath + "/MusicGenerator/Windows", BuildTarget.StandaloneWindows64);
		}

		/// Will create asset bundles for Mac
		[MenuItem("Assets/Build Mac Music Assets")]
		public static void BuildMacAssetBundles()
		{
			string assetPath = Application.streamingAssetsPath + "/MusicGenerator/Mac";
			bool pathExists = System.IO.Directory.Exists(assetPath);
			if (!pathExists)
				throw new Exception(assetPath + " doesn't exist. Please Move the StreamingAsset folder from the MusicGenerator asset to your main Assests path (Application.datapath)");
			BuildAssetBundles(Application.streamingAssetsPath + "/MusicGenerator/Mac", BuildTarget.StandaloneOSXIntel64);
		}

		public static void BuildAssetBundles(string pathIN, BuildTarget target)
		{
			BuildPipeline.BuildAssetBundles(pathIN, BuildAssetBundleOptions.None, target);
		}

		/// Will Clean up all prefabs in the MusicGenerator/Prefabs folder.
		[MenuItem("Assets/Clean up Music Prefabs")]
		public static void CleanupPrefabs()
		{
			/// clean up our prefabs we made.
			string tempDirectory = PrefabBuilder.mPrefabTempDirectory;
			if (!System.IO.Directory.Exists(tempDirectory))
				return;
			foreach (string file in System.IO.Directory.GetFiles(tempDirectory))
			{
				System.IO.File.Delete(file);
			}
			System.IO.Directory.Delete(tempDirectory);
			System.IO.File.Delete(PrefabBuilder.mPrefabTempDirectoryParent + "TempMusicPrefabs.meta");
		}
	}
}