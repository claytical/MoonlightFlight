namespace ProcGenMusic
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using System.IO;
	using System.Diagnostics;
	using System;
	
	/// This class handles loading and saving of config files.
	public class MusicFileConfig : MonoBehaviour
	{
		/// saves a global configuration: instruments, instrument and global settings.
		public static void SaveConfiguration(string fileNameIN, string saveType, string serializedString)
		{
			string directory = Application.persistentDataPath + "/InstrumentSaves/" + fileNameIN;
			if (!Directory.Exists(directory))
				Directory.CreateDirectory(directory);


			File.WriteAllText(directory + "/" + saveType + ".txt", serializedString);
			UnityEngine.Debug.Log(directory + "/" + saveType + ".txt" + "writtenToFile");
		}

		/// Loads a global configuration: instruments, global settings, etc.
		public void LoadConfig(string folderIN)
		{
			MusicGenerator.Instance.ClearInstruments(MusicGenerator.Instance.mInstrumentSet);
			string generatorString = "";

			string resourceDirectory = Application.streamingAssetsPath + "/MusicGenerator/InstrumentSaves/" + folderIN;
			string persistentDirectory = Application.persistentDataPath + "/InstrumentSaves/" + folderIN;

			if (File.Exists(resourceDirectory + "/generator.txt"))
				generatorString = File.ReadAllText(resourceDirectory + "/generator.txt");
			else if (File.Exists(persistentDirectory + "/generator.txt"))
				generatorString = File.ReadAllText(persistentDirectory + "/generator.txt");

			if (generatorString == "")
				throw new Exception(folderIN + "/generator.txt does not exist");

			GeneratorSave generatorSave = JsonUtility.FromJson<GeneratorSave>(generatorString);

			if (generatorSave == null)
				throw new Exception(folderIN + " generator.txt is corrupt");

			MusicGenerator.Instance.LoadGeneratorSave(generatorSave);

			///Load the instruments:
			InstrumentSet set = MusicGenerator.Instance.mInstrumentSet;
			for (int i = 0; i < MusicGenerator.mMaxInstruments; i++)
			{
				string instrumentString = "";
				if (File.Exists(resourceDirectory + "/instruments" + i.ToString() + ".txt"))
					instrumentString = File.ReadAllText(resourceDirectory + "/instruments" + i.ToString() + ".txt");
				else
				{
					if (File.Exists(persistentDirectory + "/instruments" + i.ToString() + ".txt"))
						instrumentString = File.ReadAllText(persistentDirectory + "/instruments" + i.ToString() + ".txt");
				}

				InstrumentSave instrumentSave = JsonUtility.FromJson<InstrumentSave>(instrumentString);

				if (instrumentSave != null)
				{
					MusicGenerator.Instance.AddInstrument(set);
					set.mInstruments[set.mInstruments.Count - 1].LoadInstrument(instrumentSave);
					uint index = MusicGenerator.Instance.LoadBaseClips(set.mInstruments[set.mInstruments.Count - 1].mInstrumentType);
					set.mInstruments[set.mInstruments.Count - 1].SetInstrumentTypeIndex(index);
				}
			}
		}

		/// async Loads a global configuration: instruments, global settings, etc.
		public IEnumerator AsyncLoadConfig(string folderIN, System.Action<bool> callback)
		{
			MusicGenerator.Instance.ClearInstruments(MusicGenerator.Instance.mInstrumentSet);
			string generatorString = "";

			string resourceDirectory = Application.streamingAssetsPath + "/MusicGenerator/InstrumentSaves/" + folderIN;
			string persistentDirectory = Application.persistentDataPath + "/InstrumentSaves/" + folderIN;

			if (File.Exists(resourceDirectory + "/generator.txt"))
				generatorString = File.ReadAllText(resourceDirectory + "/generator.txt");
			else if (File.Exists(persistentDirectory + "/generator.txt"))
				generatorString = File.ReadAllText(persistentDirectory + "/generator.txt");

			if (generatorString == "")
			{
				UnityEngine.Debug.Log(folderIN + "/generator.txt does not exist");
				yield break;
			}

			GeneratorSave generatorSave = JsonUtility.FromJson<GeneratorSave>(generatorString);
			yield return null;
			if (generatorSave == null)
			{
				UnityEngine.Debug.Log(folderIN + " generator.txt is corrupt");
				yield break;
			}

			MusicGenerator.Instance.LoadGeneratorSave(generatorSave);
			yield return null;
			
			///Load the instruments:
			InstrumentSet set = MusicGenerator.Instance.mInstrumentSet;
			for (int i = 0; i < MusicGenerator.mMaxInstruments; i++)
			{
				string instrumentString = "";
				if (File.Exists(resourceDirectory + "/instruments" + i.ToString() + ".txt"))
					instrumentString = File.ReadAllText(resourceDirectory + "/instruments" + i.ToString() + ".txt");
				else
				{
					if (File.Exists(persistentDirectory + "/instruments" + i.ToString() + ".txt"))
					{
						instrumentString = File.ReadAllText(persistentDirectory + "/instruments" + i.ToString() + ".txt");
						yield return null;
					}
				}

				InstrumentSave instrumentSave = JsonUtility.FromJson<InstrumentSave>(instrumentString);
				yield return null;

				if (instrumentSave != null)
				{
					MusicGenerator.Instance.AddInstrument(set);
					set.mInstruments[set.mInstruments.Count - 1].LoadInstrument(instrumentSave);
					uint index = 999;
					yield return null;
					StartCoroutine(MusicGenerator.Instance.AsyncLoadBaseClips(set.mInstruments[set.mInstruments.Count - 1].mInstrumentType, ((x) => { index = x; })));
					yield return new WaitUntil(() => index != 999);
					set.mInstruments[set.mInstruments.Count - 1].SetInstrumentTypeIndex(index);
				}
				else
					break;
				yield return null;
			}
			callback(true);
			yield return null;
		}

		/// saves the tooltips.
		public static void SaveTooltips(string fileNameIN, TooltipSave save)
		{
			string fileName = Application.streamingAssetsPath + "/MusicGenerator/tooltips.txt";
			string serializedString = JsonUtility.ToJson(save);
			File.WriteAllText(fileName, serializedString);
			UnityEngine.Debug.Log("tooltips saved");
		}

		/// loads the tooltips:
		public static TooltipSave LoadTooltips()
		{
			string fileName = Application.streamingAssetsPath + "/MusicGenerator/tooltips.txt";
			string tooltipsString = File.ReadAllText(fileName);

			TooltipSave saveOUT = JsonUtility.FromJson<TooltipSave>(tooltipsString);

			if (saveOUT == null)
				throw new ArgumentNullException("tooltip file was not sucessfully loaded");

			return saveOUT;
		}

		/// saves a clip configuration: 
		public static void SaveClipConfiguration(string fileNameIN, string serializedString)
		{
			string directory = Application.streamingAssetsPath + "/MusicGenerator/InstrumentClips/";
			if (!Directory.Exists(directory))
				Directory.CreateDirectory(directory);

			File.WriteAllText(directory + fileNameIN + ".txt", serializedString);
			UnityEngine.Debug.Log(fileNameIN + " saved");
		}

		/// Loads a clip configuration.
		public static ClipSave LoadClipConfigurations(string fileNameIN)
		{
			string clipSaveString = "";

			string streamingAssetsDir = Application.streamingAssetsPath + "/MusicGenerator/InstrumentClips/";
			string persistentDirectory = Application.persistentDataPath + "/MusicGenerator/InstrumentClips/";


			if (File.Exists(streamingAssetsDir + fileNameIN))
				clipSaveString = File.ReadAllText(streamingAssetsDir + fileNameIN);
			else
				clipSaveString = File.ReadAllText(persistentDirectory + fileNameIN);

			ClipSave clipSave = JsonUtility.FromJson<ClipSave>(clipSaveString);

			if (clipSave == null)
				throw new ArgumentNullException("clipSave");

			return clipSave;
		}
	}
}