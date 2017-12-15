namespace ProcGenMusic
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Audio;
	using System;
	using System.IO;

	/// Music Generator:
	/// See the included setup documentation file.
	/// Handles the state logic and other top level functions for the entire player. Loading assets, etc.
	public class MusicGenerator : HelperSingleton<MusicGenerator>
	{
		public string mPlatform { get; private set; }                                           //< which platform we're on, this is detected in Awake().
		public eGeneratorState mState { get; private set; }                                     //< Music Generator state
		private float mStateTimer = 0.0f;                                                       //< how long we've been in any given music generator state

		public eVolumeState mVolumeState { get; private set; }                                  //< whether we're fading in or out
		private float mMasterVolume = 1.0f;                                                     //< master volume for the music generator

		public float mVolFadeRate { get; private set; }                                         //< speed of volume fade * deltaT
		public void SetVolFadeRate(float value) { mVolFadeRate = Math.Abs(value); }             //< setter for fade rate. This must be positive value.

		static readonly public float mMaxVolume = 15;                                           //< based on decibels. Needs to match vol slider on ui (if using ui). Edit at your own risk :)
		static readonly public float mMinVolume = -100.0f;                                      //< based on decibels. Needs to match vol slider on ui (if using ui). Edit at your own risk :)
		static readonly public int mMaxInstrumentNotes = 36;                                    //< max number of notes per instrument
		static readonly public int mOctave = 12;                                                //< number of notes in an octave
		static private readonly float mNumStartingAudioSources = 10;                            //< number of audio sources we'll start with.

		[SerializeField]
		private string mDefaultConfig = "AAADefault";                                           //< default config loaded on start.

		public readonly int mMaxFullstepsTaken = 4;                                             //< max number of steps per chord progression. Currently only support 4
		public eMode mMode = eMode.Ionian;                                                      //< our current mode

		public eThemeRepeatOptions mThemeRepeatOptions { get; private set; }                    //< whether we're repeating or using a theme
		public void SetThemeRepeatOption(eThemeRepeatOptions option) { mThemeRepeatOptions = option; }//< Sets the them repeat options.

		private int mKeySteps = 0;                                                              //< for key changes. How many steps up/down we will take

		public float mKeyChangeAscendDescend { get; private set; }                              //< odds of ascending or descending on key change
		public void SetKeyChangeAscendDescend(float value) { mKeyChangeAscendDescend = Math.Abs(value); }   //< setter for key change ascend/descend value. Must be positive value

		public float mSetThemeOdds { get; private set; }                                        //< odds a new theme will be selected
		public void SetThemeOddsOfSetting(float value) { mSetThemeOdds = Math.Abs(value); }     //< setter for theme odds. Must be positive value.

		public float mPlayThemeOdds { get; private set; }                                       //< odds a theme will play
		public void SetPlayThemeOdds(float value) { mPlayThemeOdds = Math.Abs(value); }         //< setter for playing theme odds. Must be positive value.

		/// is frankly how many will fit at the moment...Only made 10 mixer groups. 
		/// In theory there's no hard limit if you want to add mixer groups and expose their variables.
		static readonly public int mMaxInstruments = 10;

		public List<int> mChordProgression { get; private set; }                                //< our currently played chord progression
		public void SetCurrentChordProgression(List<int> progressionIN)                         //< setter for chord progression. Must be length of 4.
		{
			mChordProgression.Clear();
			for (int i = 0; i < 4; i++)
			{
				int chord = 1;
				if (progressionIN.Count > i)
					chord = progressionIN[i];
				mChordProgression.Add(chord);
			}
		}

		public eScale mScale = 0;                                                                   //< our current scale
		public float mProgressionChangeOdds { get; private set; }                                   //< odds we'll pick a new chord progression 
		public void SetProgressionChangeOdds(float value) { mProgressionChangeOdds = Math.Abs(value); } //< setter as value must be positive value.

		public eKey mKey = 0;                                                                       //< current key
		private bool mKeyChangeNextMeasure = false;                                                 //< whether we'll change key next measure
		public float mKeyChangeOdds { get; private set; }                                           //< odds we change keys:
		public void SetKeyChangeOdds(float value) { mKeyChangeOdds = Math.Abs(value); }             //< setter as value must be positive.

		public List<float> mGroupOdds { get; private set; }                                         //< odds a group will play a measure
		public void SetGroupOdds(int index, float value) { mGroupOdds[index] = Math.Abs(value); }   //< value must be positive.

		public List<bool> mGroupIsPlaying { get; private set; }                                     //< whether this group is currently playing
		public uint mGroupRate { get; private set; }                                                //< whether we roll for the odds of the group playing at the end of measure
		public void SetGroupRate(uint value) { mGroupRate = value == 0 || value == 1 ? value : 0; } //< setter for group rate.

		public eDynamicStyle mDynamicStyle = eDynamicStyle.Linear;                                  //< The way in which we change which groups are playing.

		/// Effects settings: These defaults are more or less unity's base values:
		public Pair<string, float> mDistortion = new Pair<string, float>("MasterDistortion", 0.0f);
		public Pair<string, float> mCenterFreq = new Pair<string, float>("MasterCenterFrequency", 226.0f);
		public Pair<string, float> mOctaveRange = new Pair<string, float>("MasterOctaveRange", 3.78f);
		public Pair<string, float> mFreqGain = new Pair<string, float>("MasterFrequencyGain", 1.63f);
		public Pair<string, float> mLowpassCutoffFreq = new Pair<string, float>("MasterLowpassCutoffFreq", 22000.0f);
		public Pair<string, float> mLowpassResonance = new Pair<string, float>("MasterLowpassResonance", 1.0f);
		public Pair<string, float> mHighpassCutoffFreq = new Pair<string, float>("MasterHighpassCutoffFreq", 10.0f);
		public Pair<string, float> mHighpassResonance = new Pair<string, float>("MasterHighpassResonance", 1.0f);
		public Pair<string, float> mEchoDelay = new Pair<string, float>("MasterEchoDelay", 13.0f);
		public Pair<string, float> mEchoDecay = new Pair<string, float>("MasterEchoDecay", 0.23f);
		public Pair<string, float> mEchoDry = new Pair<string, float>("MasterEchoDry", 100.0f);
		public Pair<string, float> mEchoWet = new Pair<string, float>("MasterEchoWet", 0.0f);
		public Pair<string, float> mNumEchoChannels = new Pair<string, float>("MasterNumEchoChannels", 0.0f);
		public Pair<string, float> mReverb = new Pair<string, float>("MasterReverb", -10000.0f);
		public Pair<string, float> mRoomSize = new Pair<string, float>("MasterRoomSize", -10000.0f);
		public Pair<string, float> mReverbDecay = new Pair<string, float>("MasterReverbDecay", 0.1f);

		public ChordProgressions mChordProgressions { get; private set; }                       //< chord progression logic
		private List<AudioSource> mAudioSources = new List<AudioSource>();                      //< list of audio sources :P
		public AudioMixer mMixer { get; private set; }                                          //< our audio mixer

		public MusicFileConfig mMusicFileConfig { get; private set; }
		private List<string> mLoadedInstrumentNames = new List<string>();                       //< loaded instrument paths for the current configuration
		public List<string> GetLoadedInstrumentNames() { return mLoadedInstrumentNames; }       //< returns instrument paths

		[SerializeField]
		private List<string> mBaseInstrumentPaths = new List<string>();                         //< Set in the editor. Possible instrument paths
		public int GetNumBaseInstruments() { return mBaseInstrumentPaths.Count; }               //< how many instruments we loaded
		public List<string> GetBasePathNames() { return mBaseInstrumentPaths; }                 //< getter for path names

		private List<List<List<AudioClip>>> mAllClips = new List<List<List<AudioClip>>>();      //< multidimensional list of clips. top index is instrument
		public List<List<List<AudioClip>>> GetClips() { return mAllClips; }                     //< getter for all clips.
		[SerializeField]
		public InstrumentSet mInstrumentSet { get; private set; }                               //< ref to our instrument set
		public void SetInstrumentSet(InstrumentSet setIN) { if (setIN != null) mInstrumentSet = setIN; }

		/// keeps the asset bundles from being unloaded which causes an FMOD error if you try to edit the
		/// audio mixer in the editor while it's playing. Leave false unless you're using the audio mixer live in unity'd editor.
		[SerializeField]
		private bool mEnableLiveMixerEditing = false;

		/// Whether we'll use async loading. Cannot set while initializing already.
		[SerializeField]
		private bool mUseAsyncLoading = false;
		public bool AreUsingAsyncLoad() { return mUseAsyncLoading; }
		public void SetAsyncLoadingUse(bool use) { mUseAsyncLoading = mState != eGeneratorState.initializing ? use : mUseAsyncLoading; }

		////////////////////////////////////////////////////////////
		////////////////////////////////////////////////////////////
		/// public functions:
		////////////////////////////////////////////////////////////

		/// Loads a new configuration (song) to play.
		public void LoadConfiguration(string configIN, eGeneratorState continueState = eGeneratorState.ready)
		{
			if (mState == eGeneratorState.initializing)
				return;

			if (mUseAsyncLoading)
				StartCoroutine(AsyncLoadConfiguration(configIN, continueState));
			else
				NonAsyncLoadConfiguration(configIN, continueState);
		}

		/// Fades out the music before async loading a new configuration and fading back in.
		public IEnumerator FadeLoadConfiguration(string configIN)
		{
			mStateTimer = 0.0f;
			VolumeFadeOut();
			float maxWaitTime = 10.0f;
			yield return new WaitUntil(() => mVolumeState == eVolumeState.fadedOutIdle || mStateTimer > maxWaitTime);

			Stop();
			bool finished = false;
			StartCoroutine(mMusicFileConfig.AsyncLoadConfig(configIN, ((x) => { finished = x; })));
			yield return new WaitUntil(() => finished);
			mVolumeState = eVolumeState.idle;
			SetState(eGeneratorState.playing);
			yield return null;
		}

		/// returns mInstrumentSet.instruments
		public List<Instrument> GetInstruments()
		{
			return mInstrumentSet.mInstruments;
		}

		/// pauses the main music generator:
		public void Pause()
		{
			if (mState != eGeneratorState.initializing)
				SetState((mState < eGeneratorState.editing) ? eGeneratorState.paused : eGeneratorState.editorPaused);
		}

		/// plays the main music generator:
		public void Play()
		{
			if (mState != eGeneratorState.initializing)
				SetState((mState < eGeneratorState.editing) ? eGeneratorState.playing : eGeneratorState.editorPlaying);
		}

		/// stops the main music generator:
		public void Stop()
		{
			if (mState != eGeneratorState.initializing)
				SetState((mState < eGeneratorState.editing) ? eGeneratorState.stopped : eGeneratorState.editorStopped);
		}

		/// Set the music generator state:
		public void SetState(eGeneratorState stateIN)
		{
			mStateTimer = 0.0f;
			mState = stateIN;

			OnStateSet(new MusicGeneratorStateArgs(mState));

			switch (mState)
			{
				case eGeneratorState.stopped:
				case eGeneratorState.editorStopped:
					{
						ResetPlayer();
						break;
					}
				case eGeneratorState.initializing:
				case eGeneratorState.ready:
				case eGeneratorState.playing:
				case eGeneratorState.repeating:
				case eGeneratorState.paused:
				case eGeneratorState.editing:
				case eGeneratorState.editorPaused:
				case eGeneratorState.editorPlaying:
				default:
					break;
			}
		}

		/// fades volume out
		public void VolumeFadeOut()
		{
			mVolumeState = eVolumeState.fadingOut;
		}

		/// fades volume in
		public void VolumeFadeIn()
		{
			mVolumeState = eVolumeState.fadingIn;
		}

		/// Sets the audio source volume for mAudioSource[indexIN]. This is different from the instrument volume which controls the clips
		/// played by an instrument, in that it controls the attenuation of the audioSource itself. If everything
		/// feels too quiet, this may be a good place to increase volume.
		public void SetAudioSourceVolume(int indexIN, float volumeIN, InstrumentSet set = null)
		{
			if (set == null)
				set = mInstrumentSet;
			mMixer.SetFloat("Volume" + indexIN.ToString(), volumeIN);
			set.mInstruments[indexIN].SetAudioSourceVolume(volumeIN);
		}

		/// plays an audio clip:
		/// Look for an available clip that's not playing anything, creates a new one if necessary
		/// resets its properties  (volume, pan, etc) to match our new clip.
		public void PlayAudioClip(InstrumentSet set, AudioClip clipIN, float volumeIN, int indexIN)
		{
			bool foundUnplayed = false;
			for (int i = 0; i < mAudioSources.Count; i++)
			{
				if (!mAudioSources[i].isPlaying)
				{
					mAudioSources[i].panStereo = set.mInstruments[indexIN].mStereoPan;
					mAudioSources[i].loop = false;
					mAudioSources[i].outputAudioMixerGroup = mMixer.FindMatchingGroups(indexIN.ToString())[0];
					mAudioSources[i].volume = volumeIN;
					mAudioSources[i].clip = clipIN;
					mAudioSources[i].Play();
					foundUnplayed = true;
					return;
				}
			}

			if (!foundUnplayed) //make a new audio souce.
			{
				mAudioSources.Add(Camera.main.gameObject.AddComponent<AudioSource>());
				AudioSource audioSource = mAudioSources[mAudioSources.Count - 1];
				audioSource.panStereo = set.mInstruments[indexIN].mStereoPan;
				audioSource.outputAudioMixerGroup = mMixer.FindMatchingGroups(indexIN.ToString())[0];
				audioSource.volume = volumeIN;
				audioSource.loop = false;
				audioSource.clip = clipIN;
				//audioSource.spatialBlend = 0;
				audioSource.Play();
			}
		}

		/// resets all player settings.
		/// reset player is called on things like loading new configurations, loading new levels, etc.
		/// sets all timing values and other settings back to the start
		public void ResetPlayer()
		{
			if (mState <= eGeneratorState.initializing || mChordProgressions == null)
				return;

			mInstrumentSet.Reset();

			for (int i = 0; i < 4; i++)
				mGroupIsPlaying[i] = (i == 0) ? true : false;
			mInstrumentSet.mCurrentGroupLevel = 0;

			if (mState < eGeneratorState.editing)
				mChordProgression = mChordProgressions.GenerateProgression(mMode, mScale, mKeySteps);

			OnNormalMeasureExited();
			OnPlayerReset();
			mInstrumentSet.ResetRepeatCount();
			mInstrumentSet.ResetProgressionSteps();
		}

		/// Adds an instrument to our list. sets its instrument index
		public void AddInstrument(InstrumentSet setIN)
		{
			setIN.mInstruments.Add(new Instrument());
			setIN.mInstruments[setIN.mInstruments.Count - 1].Init(setIN.mInstruments.Count - 1);
		}

		/// Deletes all instruments:
		public void ClearInstruments(InstrumentSet setIN)
		{
			OnInstrumentsCleared();
			if (setIN.mInstruments.Count == 0)
				return;

			for (int i = (int)setIN.mInstruments.Count - 1; i >= 0; i--)
			{
				RemoveInstrument((int)setIN.mInstruments[i].mInstrumentIndex, setIN);
			}
			setIN.mInstruments.Clear();
		}

		/// Removes the instrument from our list. Fixes instrument indices.
		public void RemoveInstrument(int indexIN, InstrumentSet set)
		{
			int typeIndex = (int)set.mInstruments[indexIN].mInstrumentTypeIndex;
			set.mInstruments[indexIN] = null;
			set.mInstruments.RemoveAt(indexIN);

			for (uint i = 0; i < set.mInstruments.Count; i++)
				set.mInstruments[(int)i].SetInstrumentIndex(i);

			RemoveBaseClip(typeIndex, set);
		}

		/// Removes a base clip if there are no instruments using it.
		public void RemoveBaseClip(int typeIndex, InstrumentSet set)
		{
			bool isUsed = false;
			for (int i = 0; i < set.mInstruments.Count; i++)
			{
				if (set.mInstruments[i].mInstrumentTypeIndex == typeIndex)
					isUsed = true;
			}
			if (!isUsed)
			{
				mAllClips.RemoveAt(typeIndex);
				mLoadedInstrumentNames.RemoveAt(typeIndex);
			}
			CleanUpInstrumentTypeIndices(set);
		}

		/// Re-fixes the instrument types if something's been removed. There's probably a better way to do this.
		public void CleanUpInstrumentTypeIndices(InstrumentSet set)
		{
			for (int i = 0; i < set.mInstruments.Count; i++)
			{
				set.mInstruments[i].SetInstrumentTypeIndex((uint)GetLoadedInstrumentNames().IndexOf(set.mInstruments[i].mInstrumentType));
			}
		}

		/// Sets the volume.
		public void SetVolume(float volIN)
		{
			if (mVolumeState == eVolumeState.idle)
			{
				mMasterVolume = volIN;
				mMixer.SetFloat("MasterVol", mMasterVolume);
			}
		}

		/// Sets a global effect on the mixer. Can also edit manually on the master channel of the 'GeneratorMixer' in your scene.
		/// PLEASE USE WITH CAUTION!!! :) There's no idiot proofing on these values' effect on your speakers, game, pet's ears, your ears, etc. You could wake an Old One for all I know :P
		/// You can use the executable UI included with the asset to reset any of these to their defaults (click the reset button next to the slider)
		/// the mixer's current effects names and unity's min/max ranges :
		/// See: The pair<string, float> variables above for settings these:
		/// "MasterDistortion"  0 : 1
		/// "MasterCenterFrequency" 20 : 22000
		/// "MasterOctaveRange" .2 : 5
		/// "MasterRoomSize" -10000 : 0
		///	"MasterReverbDecay" .1 : 5
		///	"MasterFrequencyGain" .05 : 3
		/// "MasterLowpassCutoffFreq" 10 : 22000
		/// "MasterLowpassResonance" 1 : 10
		/// "MasterHighpassCutoffFreq" 10 : 22000
		/// "MasterHighpassResonance" 1 : 10
		/// "MasterEchoDelay" 1 : 5000
		/// "MasterEchoDecay" .01 : 1
		/// "MasterEchoDry" 0 : 100
		/// "MasterEchoWet" 0 : 100
		/// "MasterNumEchoChannels" 0 : 16
		/// "MasterReverb" -10000 : 2000
		public void SetGlobalEffect(Pair<string, float> valueIN)
		{
			mMixer.SetFloat(valueIN.First, valueIN.Second);
		}

		/// Returns the shortest rhythm timestep
		public eTimestep GetShortestRhythmTimestep()
		{
			List<Instrument> instruments = mInstrumentSet.mInstruments;
			eTimestep shortestTime = eTimestep.whole;
			for (int i = 0; i < instruments.Count; i++)
			{
				if (instruments[i].mSuccessionType != eSuccessionType.lead)
					shortestTime = instruments[i].mTimeStep < shortestTime ? instruments[i].mTimeStep : shortestTime;
			}
			return shortestTime;
		}

		////////////////////////////////////////////////////////////
		/// private utility functions:  Edit at your own risk :)
		////////////////////////////////////////////////////////////
		public override void Awake()
		{
			mPlatform = "/Windows";
			if (Application.platform == RuntimePlatform.LinuxPlayer || Application.platform == RuntimePlatform.LinuxEditor)
				mPlatform = "/Linux";
			if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
				mPlatform = "/Mac";

			mChordProgressions = new ChordProgressions();
			mChordProgressions.Init();

			DontDestroyOnLoad(this.gameObject);

			mMusicFileConfig = gameObject.AddComponent<MusicFileConfig>();

			//initialization of privately set values:
			mProgressionChangeOdds = 25.0f;
			mKeyChangeOdds = 0.0f;
			mGroupOdds = new List<float>() { 100.0f, 100.0f, 100.0f, 100.0f };
			mChordProgression = new List<int>() { 1, 4, 4, 5 };
			mGroupIsPlaying = new List<bool>() { true, false, false, false };
			mState = eGeneratorState.loading;
			mVolumeState = eVolumeState.idle;
			mInstrumentSet = gameObject.AddComponent<InstrumentSet>();
			mInstrumentSet.Init();
			mVolFadeRate = 50.0f;
			mKeyChangeAscendDescend = 50.0f;
			mSetThemeOdds = 10.0f;
			mPlayThemeOdds = 90.0f;
			mGroupRate = 0;
			mThemeRepeatOptions = (int)eThemeRepeatOptions.eNone;

			///load our mixer:
			string mixerPath = Application.streamingAssetsPath + "/MusicGenerator" + mPlatform + "/GeneratorMixer";
			if (!System.IO.File.Exists(mixerPath))
			{
				Debug.Log("Audio Mixer asset bundle does not exist.");
				throw new System.ArgumentNullException("Audio Mixer base file does not exist.");
			}
			else
			{
				var loadedAssetBundle = AssetBundle.LoadFromFile(mixerPath);
				if (loadedAssetBundle != null)
				{
					mMixer = loadedAssetBundle.LoadAsset<AudioMixer>("GeneratorMixer");
					if (mMixer == null)
						throw new System.ArgumentNullException("GeneratorMixer base file was not sucessfully loaded");

					loadedAssetBundle.Unload(false);
				}
			}

			// These are generated as needed, but we'll certainly need some on start
			// this just keeps it from trying to add a dozen audio sources after things have already started playing.
			for (int i = 0; i < mNumStartingAudioSources; i++)
				mAudioSources.Add(Camera.main.gameObject.AddComponent<AudioSource>());
		}

		///Loads an instrument set configuration:
		public void NonAsyncLoadConfiguration(string configIN, eGeneratorState continueState = eGeneratorState.ready)
		{
			Stop();
			ResetPlayer();
			SetState(eGeneratorState.initializing);
			if (mMusicFileConfig != null)
				mMusicFileConfig.LoadConfig(configIN);
			else
				throw new ArgumentNullException("configuration class doesn't exist or was not loaded yes");
			mChordProgression = mChordProgressions.GenerateProgression(mMode, mScale, 0);
			SetState(continueState);
		}

		/// Async Loads an instrument set configuration:
		public IEnumerator AsyncLoadConfiguration(string configIN, eGeneratorState continueState = eGeneratorState.ready)
		{
			ResetPlayer();
			SetState(eGeneratorState.initializing);
			bool finished = false;
			StartCoroutine(mMusicFileConfig.AsyncLoadConfig(configIN, ((x) => { finished = x; })));
			yield return new WaitUntil(() => finished);
			yield return null;
			mChordProgression = mChordProgressions.GenerateProgression(mMode, mScale, 0);
			SetState(continueState);
			yield return null;
		}

		/// Asynchronously Loads the instrument clips from the asset bundles into our mAllClips array
		public IEnumerator AsyncLoadBaseClips(string instrumentType, System.Action<uint> callback)
		{
			if (mLoadedInstrumentNames.Contains(instrumentType))
			{
				callback((uint)mLoadedInstrumentNames.IndexOf(instrumentType));
				yield break;
			}

			mLoadedInstrumentNames.Add(instrumentType);

			string path = Application.streamingAssetsPath + "/MusicGenerator" + mPlatform + "/" + instrumentType + "_1";

			// Check for instrument sub-types.
			if (File.Exists(path))
			{
				mAllClips.Add(new List<List<AudioClip>>());
				yield return null;
				for (int i = 1; i < mMaxInstruments + 1; i++)
				{
					if (File.Exists(Application.streamingAssetsPath + "/MusicGenerator" + mPlatform + "/" + instrumentType + "_" + i.ToString()))
					{
						InstrumentPrefabList list = null;
						StartCoroutine(AsyncLoadInstrumentPrefabList(instrumentType + "_" + i.ToString(), ((x) => { list = x; })));
						yield return new WaitUntil(() => list != null);

						mAllClips[mAllClips.Count - 1].Add(new List<AudioClip>());
						yield return null;
						for (int x = 0; x < list.mAudioSources.Length; x++)
						{
							if (list.mAudioSources[x] != null)
							{
								mAllClips[mAllClips.Count - 1][i - 1].Add(list.mAudioSources[x].clip);
								yield return null;
							}
							else break;
						}
						yield return null;
					}
					else break;
				}
				callback((uint)mAllClips.Count - 1);
				yield return null;
			}
			else    //load normal instrument without sub folders.
			{
				mAllClips.Add(new List<List<AudioClip>>());
				int index = mAllClips.Count - 1;
				mAllClips[index].Add(new List<AudioClip>());
				InstrumentPrefabList list = null;
				StartCoroutine(AsyncLoadInstrumentPrefabList(instrumentType, ((x) => { list = x; })));
				yield return new WaitUntil(() => list != null);

				for (int i = 0; i < list.mAudioSources.Length; i++)
				{
					if (list.mAudioSources[i] != null)
					{
						mAllClips[index][mAllClips[index].Count - 1].Add(list.mAudioSources[i].clip);
						yield return null;
					}
					else break;
				}

				callback((uint)mAllClips.Count - 1);
				yield return null;
			}
		}

		/// async Loads our audio clip from its asset bundle.
		/// this is intentially really slow, so as to interfere as little as possible with the framerate.
		private IEnumerator AsyncLoadInstrumentPrefabList(string pathName, System.Action<InstrumentPrefabList> callback)
		{
			string path = Path.Combine(Application.streamingAssetsPath + "/MusicGenerator" + mPlatform, pathName);

			if (!File.Exists(path))
			{
				Debug.Log("file at " + path + " does not exist");
				throw new ArgumentNullException("file at " + path + " does not exist");
			}

			var bundleRequest = AssetBundle.LoadFromFileAsync(path);
			yield return new WaitUntil(() => bundleRequest.isDone);
			yield return null;
			var myLoadedAssetBundle = bundleRequest.assetBundle;
			if (myLoadedAssetBundle != null)
			{
				var assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync<GameObject>(pathName);
				yield return new WaitUntil(() => assetLoadRequest.isDone);
				yield return null;
				GameObject go = assetLoadRequest.asset as GameObject;

				if (!mEnableLiveMixerEditing)
					myLoadedAssetBundle.Unload(false);
				yield return null;
				callback(go.GetComponent<InstrumentPrefabList>());
			}
			else
			{
				throw new ArgumentNullException("asset bundle for " + pathName + " could not be loaded or does not exist.");
			}
			yield return null;
		}

		/// non-async. Loads the instrument clips from file into our mAllClips array
		/// Use if you need / want to preload clips when loading other assets, instead of on the fly.
		public uint LoadBaseClips(string instrumentType)
		{
			if (mLoadedInstrumentNames.Contains(instrumentType))
				return (uint)mLoadedInstrumentNames.IndexOf(instrumentType);

			mLoadedInstrumentNames.Add(instrumentType);

			string path = Application.streamingAssetsPath + "/MusicGenerator" + mPlatform + "/" + instrumentType + "_1";

			// Here we test for sub folder for the instrument and load any additional variations:
			if (File.Exists(path))
			{
				mAllClips.Add(new List<List<AudioClip>>());
				for (int i = 1; i < mMaxInstruments + 1; i++)
				{
					if (File.Exists(Application.streamingAssetsPath + "/MusicGenerator" + mPlatform + "/" + instrumentType + "_" + i.ToString()))
					{
						InstrumentPrefabList list = LoadInstrumentPrefabList(instrumentType + "_" + i.ToString());

						if (list == null)
							throw new ArgumentNullException("prefab list for " + instrumentType + " does not exist");

						mAllClips[mAllClips.Count - 1].Add(new List<AudioClip>());

						for (int x = 0; x < list.mAudioSources.Length; x++)//musician who named them started at 1, just haven't fixed. :P
						{
							if (list.mAudioSources[x] != null)
								mAllClips[mAllClips.Count - 1][i - 1].Add(list.mAudioSources[x].clip);
						}
					}
					else break;
				}
				return (uint)mAllClips.Count - 1;
			}
			else    // otherwise load normal instrument without sub folders.
			{
				mAllClips.Add(new List<List<AudioClip>>());
				int index = mAllClips.Count - 1;
				mAllClips[index].Add(new List<AudioClip>());
				InstrumentPrefabList list = LoadInstrumentPrefabList(instrumentType);
				if (list == null)
					throw new ArgumentNullException("prefab list for " + instrumentType + " does not exist");

				for (int i = 0; i < list.mAudioSources.Length; i++)
				{
					if (list.mAudioSources[i] != null)
						mAllClips[index][mAllClips[index].Count - 1].Add(list.mAudioSources[i].clip);
				}

				return (uint)mAllClips.Count - 1; ;
			}
		}

		/// non-async. Loads our audio clip from its asset bundle.
		private InstrumentPrefabList LoadInstrumentPrefabList(string pathName)
		{
			string path = Path.Combine(Application.streamingAssetsPath + "/MusicGenerator" + mPlatform, pathName);

			var myLoadedAssetBundle = AssetBundle.LoadFromFile(path);

			if (myLoadedAssetBundle != null)
			{
				GameObject go = myLoadedAssetBundle.LoadAsset<GameObject>(pathName);
				if (!mEnableLiveMixerEditing)
					myLoadedAssetBundle.Unload(false);
				return go.GetComponent<InstrumentPrefabList>();
			}
			else
			{
				throw new ArgumentNullException("asset bundle for " + pathName + " could not be loaded or does not exist.");
			}
		}

		/// Loads the initial configuration.
		void Start()
		{
			OnStarted();
			if (!OnHasVisiblePlayer())//without the UI, we load manually, as the UI panel is not going to do it, and we don't need ui initialized.
			{
				if (mUseAsyncLoading)
					StartCoroutine(AsyncLoadConfiguration(mDefaultConfig));
				else
					LoadConfiguration(mDefaultConfig);
			}
			else
				mChordProgression = mChordProgressions.GenerateProgression(mMode, mScale, 0);
		}

		/// Generates new chord progression:
		private void GenNewChordProgression()
		{
			mChordProgression = mChordProgressions.GenerateProgression(mMode, mScale, mKeySteps);
			OnProgressionGenerated();
		}

		/// State update:
		void Update() { UpdateState(Time.deltaTime); }
		private void UpdateState(float deltaT)
		{
			mStateTimer += deltaT;

			UpdateLiveSettings();

			/// just idiot-proofing. I don't want blame for anyone speakers :P
			/// Feel free to adjust min/max values to your needs.
			mMasterVolume = mMasterVolume <= mMaxVolume ? mMasterVolume : mMaxVolume;
			mMasterVolume = mMasterVolume >= mMinVolume ? mMasterVolume : mMinVolume;

			switch (mState)
			{
				case eGeneratorState.ready:
					break;
				case eGeneratorState.playing:
					{
						PlayMeasure();
						break;
					}
				case eGeneratorState.repeating:
					{
						mInstrumentSet.RepeatMeasure();
						break;
					}
				case eGeneratorState.editorPlaying:
					{
						PlayMeasureEditorClip();
						break;
					}
				/// nothing to do:
				case eGeneratorState.initializing:
				case eGeneratorState.paused:
				case eGeneratorState.stopped:
				/// these are handled only by the ui measure editor
				case eGeneratorState.editing:
				case eGeneratorState.editorPaused:
				case eGeneratorState.editorStopped:
				default:
					break;
			}

			/// handles the volume fade in / fade out:
			switch (mVolumeState)
			{
				case eVolumeState.fadingIn:
				case eVolumeState.fadingOut:
					FadeVolume(deltaT);
					break;
				case eVolumeState.fadedOutIdle:
				case eVolumeState.idle:
				default:
					break;
			}
			OnStateUpdated(new MusicGeneratorStateArgs(mState));
		}

		/// fades the volume:
		private void FadeVolume(float deltaT)
		{
			float currentVol;
			mMixer.GetFloat("MasterVol", out currentVol);

			switch (mVolumeState)
			{
				case eVolumeState.fadingIn:
					{
						if (currentVol < mMasterVolume - (mVolFadeRate * deltaT))
							currentVol += mVolFadeRate * deltaT;
						else
						{
							currentVol = mMasterVolume;
							mVolumeState = eVolumeState.idle;
						}
						break;
					}
				case eVolumeState.fadingOut:
					{
						if (currentVol > mMinVolume + (mVolFadeRate * deltaT))
							currentVol -= mVolFadeRate * deltaT;
						else
						{
							currentVol = mMinVolume;
							mVolumeState = eVolumeState.fadedOutIdle;
						}
						break;
					}
				default:
					break;
			}

			mMixer.SetFloat("MasterVol", currentVol);

			OnVolumeFaded(new FloatArgs(currentVol));
		}

		// handles necessary updates when music is not stopped or paused.
		private void UpdateLiveSettings()
		{
			UpdateEffects();
			ParseAudioSources();
		}

		/// Updates mixer effects.
		private void UpdateEffects()
		{
			SetVolume(mMasterVolume);
			mMixer.SetFloat(mDistortion.First, mDistortion.Second);
			mMixer.SetFloat(mCenterFreq.First, mCenterFreq.Second);
			mMixer.SetFloat(mOctaveRange.First, mOctaveRange.Second);
			mMixer.SetFloat(mFreqGain.First, mFreqGain.Second);
			mMixer.SetFloat(mLowpassCutoffFreq.First, mLowpassCutoffFreq.Second);
			mMixer.SetFloat(mLowpassResonance.First, mLowpassResonance.Second);
			mMixer.SetFloat(mHighpassCutoffFreq.First, mHighpassCutoffFreq.Second);
			mMixer.SetFloat(mHighpassResonance.First, mHighpassResonance.Second);
			mMixer.SetFloat(mEchoDelay.First, mEchoDelay.Second);
			mMixer.SetFloat(mEchoDecay.First, mEchoDecay.Second);
			mMixer.SetFloat(mEchoDry.First, mEchoDry.Second);
			mMixer.SetFloat(mEchoWet.First, mEchoWet.Second);
			mMixer.SetFloat(mNumEchoChannels.First, mNumEchoChannels.Second);
			mMixer.SetFloat(mReverb.First, mReverb.Second);
			mMixer.SetFloat(mRoomSize.First, mRoomSize.Second);
			mMixer.SetFloat(mReverbDecay.First, mReverbDecay.Second);
			for (int i = 0; i < mInstrumentSet.mInstruments.Count; i++)
			{
				mMixer.SetFloat("Volume" + i.ToString(), mInstrumentSet.mInstruments[i].mAudioSourceVolume);
			}
		}

		/// Stops the audio sources if they're finished
		/// (they're not available for use again until they've stopped).
		private void ParseAudioSources()
		{
			for (int i = 0; i < mAudioSources.Count; i++)
			{
				if (!mAudioSources[i].isPlaying)
					mAudioSources[i].Stop();
			}
		}

		/// Plays the measure.
		private void PlayMeasure()
		{
			if (mInstrumentSet.mInstruments.Count == 0)
				return;

			Action possibleKeyChange = () =>
			{
				if (mInstrumentSet.mProgressionStepsTaken == 1)
					KeyChangeSetup();

			};
			Action generateNewProgression = () =>
			{
				if (mInstrumentSet.mProgressionStepsTaken >= mMaxFullstepsTaken - 1)
				{
					SetKeyChange();

					if (UnityEngine.Random.Range(0, 100) < mProgressionChangeOdds || mKeyChangeNextMeasure)
						mChordProgression = mChordProgressions.GenerateProgression(mMode, mScale, mKeySteps);
				}
			};
			Action setThemeRepeat = () =>
			{
				if (mInstrumentSet.mRepeatCount >= mInstrumentSet.mRepeatMeasuresNum)
				{
					if (mThemeRepeatOptions > (int)eThemeRepeatOptions.eNone && mState == eGeneratorState.playing && mInstrumentSet.mRepeatCount >= mInstrumentSet.mRepeatMeasuresNum)
						SetState(eGeneratorState.repeating);

					// set our theme notes if we're going to use them.
					bool newInstrumentDetected = false;
					for (int i = 0; i < mInstrumentSet.mInstruments.Count; i++)
					{
						if (mInstrumentSet.mInstruments[i].mThemeNotes.Count == 0 &&
							 mInstrumentSet.mInstruments[i].mRepeatingNotes.Count != 0)
							newInstrumentDetected = true;
					}
					if (UnityEngine.Random.Range(0, 100.0f) < mSetThemeOdds || newInstrumentDetected || mInstrumentSet.mInstruments[0].mThemeNotes.Count == 0)
					{
						if (mInstrumentSet.mRepeatCount >= mInstrumentSet.mRepeatMeasuresNum)
						{
							for (int i = 0; i < mInstrumentSet.mInstruments.Count; i++)
								mInstrumentSet.mInstruments[i].SetThemeNotes();
						}
					}
				}
				OnNormalMeasureExited();
			};
			mInstrumentSet.PlayMeasure(possibleKeyChange, setThemeRepeat, generateNewProgression);
		}

		/// sets up whether we'll change keys next measure: 
		private void KeyChangeSetup()
		{
			if (mKeyChangeNextMeasure)
			{
				mKey += mKeySteps;
				mKey = (int)mKey >= mOctave ? (eKey)((mOctave - (int)mKey) * -1) : (eKey)(mOctave + mKey);

				mKeyChangeNextMeasure = false;
				OnKeyChanged(new IntegerArgs((int)mKey));
			}
		}

		/// changes the key for the current instrument set:
		/// alters the current chord progression to allow a smooth transition to 
		/// the new key
		private void SetKeyChange()
		{
			if (UnityEngine.Random.Range(0.0f, 100.0f) < mKeyChangeOdds)
			{
				mKeyChangeNextMeasure = true;
				mKeySteps = (UnityEngine.Random.Range(0, 100) < mKeyChangeAscendDescend) ? -7 : 7;
			}
			else
				mKeySteps = 0;
		}

		/// UI measure editor version only (for normal use, play clips through the singleClip state functions: )
		private void PlayMeasureEditorClip()
		{
			OnEditorClipPlayed();
		}

		////////////////////////////////////////////////////////////
		////////////////////////////////////////////////////////////
		/// Events: Most of these are for the UI and can be safely ignored, unless you need to subscribe for your app uses.
		/// feel free to subscribe to them or call them, if your use of the generator requires knowing any of these events.
		////////////////////////////////////////////////////////////
		/// On manager start event:
		public delegate void StartedEventHandler(object source, EventArgs args);
		public event StartedEventHandler Started;
		protected virtual void OnStarted()
		{
			if (Started != null)
				Started(this, EventArgs.Empty);
		}

		/// Event for Generator state set:
		public delegate void StateSetEventHandler(object source, MusicGeneratorStateArgs args);
		public event StateSetEventHandler StateSet;
		protected virtual void OnStateSet(MusicGeneratorStateArgs args)
		{
			if (StateSet != null)
				StateSet(this, args);
		}

		/// Event for state Update()
		public delegate void StateUpdatedEventHandler(object source, MusicGeneratorStateArgs args);
		public event StateUpdatedEventHandler StateUpdated;
		protected virtual void OnStateUpdated(MusicGeneratorStateArgs args)
		{
			if (StateUpdated != null)
				StateUpdated(this, args);
		}

		/// Event Handler for UI manager detection:
		public delegate bool HasVisiblePlayerEventHandler(object source, EventArgs args);
		public event HasVisiblePlayerEventHandler HasVisiblePlayer;
		protected virtual bool OnHasVisiblePlayer()
		{
			if (HasVisiblePlayer != null)
				return HasVisiblePlayer(this, EventArgs.Empty);
			return false;
		}

		/// Event Handler for fading volume
		public delegate void VolumeFadedEventHandler(object source, FloatArgs args);
		public event VolumeFadedEventHandler VolumeFaded;
		protected virtual void OnVolumeFaded(FloatArgs args)
		{
			if (VolumeFaded != null)
				VolumeFaded(this, args);
		}

		/// Event Handler for Generates a chord progression:
		public delegate void ProgressionGeneratedEventHandler(object source, EventArgs args);
		public event ProgressionGeneratedEventHandler ProgressionGenerated;
		protected virtual void OnProgressionGenerated()
		{
			if (ProgressionGenerated != null)
				ProgressionGenerated(this, EventArgs.Empty);
		}

		/// Event handler for clear instruments:
		public delegate void InstrumentsClearedEventHandler(object source, EventArgs args);
		public event InstrumentsClearedEventHandler InstrumentsCleared;
		public virtual void OnInstrumentsCleared()
		{
			if (InstrumentsCleared != null)
				InstrumentsCleared(this, EventArgs.Empty);
		}

		/// Event Handler for exiting normal measure
		public delegate void NormalMeasureExitedEventHandler(object source, EventArgs args);
		public event NormalMeasureExitedEventHandler NormalMeasureExited;
		public void OnNormalMeasureExited()
		{
			if (NormalMeasureExited != null)
				NormalMeasureExited(this, EventArgs.Empty);
		}

		/// Event Handler for impending key change:
		public delegate void KeyChangedEventHandler(object source, IntegerArgs args);
		public event KeyChangedEventHandler KeyChanged;
		public void OnKeyChanged(IntegerArgs args)
		{
			if (KeyChanged != null)
				KeyChanged(this, args);
		}

		// Event for exiting the repeating measure.
		public delegate void RepeatedMeasureExitedEventHandler(object source, MusicGeneratorStateArgs e);
		public event RepeatedMeasureExitedEventHandler RepeatedMeasureExited;
		public void OnRepeatedMeasureExited(MusicGeneratorStateArgs args)
		{
			if (RepeatedMeasureExited != null)
				RepeatedMeasureExited(this, args);
		}

		/// Measure editor events:
		public class BarlineArgs : EventArgs
		{
			public BarlineArgs(int steps, bool isRepeating) { mSteps = steps; mIsRepeating = isRepeating; }
			public int mSteps = 0;
			public bool mIsRepeating = false;
		}

		/// Set barline color event
		public delegate void BarlineColorSetEventHandler(object source, BarlineArgs args);
		public event BarlineColorSetEventHandler BarlineColorSet;
		public void OnBarlineColorSet(BarlineArgs argsIN)
		{
			if (BarlineColorSet != null)
				BarlineColorSet(this, argsIN);
		}

		/// Editor clip played event:
		public delegate void EditorClipPlayedEventHandler(object source, EventArgs args);
		public event EditorClipPlayedEventHandler EditorClipPlayed;
		public void OnEditorClipPlayed()
		{
			if (EditorClipPlayed != null)
				EditorClipPlayed(this, EventArgs.Empty);
		}

		/// Event Handler for a the measure editor opening
		public delegate bool UIPlayerIsEditingEventHandler(object source, EventArgs args);
		public event UIPlayerIsEditingEventHandler UIPlayerIsEditing;
		public bool OnUIPlayerIsEditing()
		{
			if (UIPlayerIsEditing != null)
				return UIPlayerIsEditing(this, EventArgs.Empty);

			return false;
		}

		/// Events for repeating notes:
		public delegate void RepeatNotePlayedEventhandler(object source, RepeatNoteArgs args);
		public event RepeatNotePlayedEventhandler RepeatNotePlayed;
		public void OnRepeatNotePlayed(RepeatNoteArgs args)
		{
			if (RepeatNotePlayed != null)
				RepeatNotePlayed(this, args);
		}

		/// Event for staff player:
		public delegate void UIStaffNotePlayedEventHandler(object source, UIStaffNoteArgs args);
		public event UIStaffNotePlayedEventHandler UIStaffNotePlayed;
		public void OnUIStaffNotePlayed(UIStaffNoteArgs args)
		{
			if (UIStaffNotePlayed != null)
				UIStaffNotePlayed(this, args);
		}

		/// Event for staff player strummed:
		public delegate void UIStaffNoteStrummedEventHandler(object source, UIStaffNoteArgs args);
		public event UIStaffNoteStrummedEventHandler UIStaffNoteStrummed;
		public void OnUIStaffNoteStrummed(UIStaffNoteArgs args)
		{
			if (UIStaffNoteStrummed != null)
				UIStaffNoteStrummed(this, args);
		}

		/// Event Single clip being loaded:
		public delegate void ClipLoadedEventHandler(object source, ClipLoadedArgs args);
		public event ClipLoadedEventHandler ClipLoaded;
		public void OnClipLoaded(ClipLoadedArgs args)
		{
			if (ClipLoaded != null)
				ClipLoaded(this, args);
		}

		/// Event for player being reset:
		public delegate void PlayerResetEventHandler(object source, EventArgs args);
		public event PlayerResetEventHandler PlayerReset;
		public void OnPlayerReset()
		{
			if (PlayerReset != null)
				PlayerReset(this, EventArgs.Empty);
		}

		/////////////////////////////////
		/// Save / Load /////////////////
		/////////////////////////////////

		/// creates and returns GeneratorSave save file.
		public GeneratorSave GetGeneratorSave()
		{
			GeneratorSave save = new GeneratorSave();

			save.mMasterVolume = mMasterVolume;
			save.mVolFadeRate = mVolFadeRate;
			save.mMode = mMode;
			save.mThemeRepeatOptions = mThemeRepeatOptions;
			save.mKeySteps = mKeySteps;
			save.mKeyChangeAscendDescend = mKeyChangeAscendDescend;
			save.mSetThemeOdds = mSetThemeOdds;
			save.mPlayThemeOdds = mPlayThemeOdds;
			save.mScale = mScale;
			save.mProgressionChangeOdds = mProgressionChangeOdds;
			save.mKey = mKey;
			save.mKeyChangeOdds = mKeyChangeOdds;
			save.mGroupOdds = mGroupOdds;
			save.mGroupRate = mGroupRate;

			save.mTempo = mInstrumentSet.mTempo;
			save.mProgressionRate = mInstrumentSet.mProgressionRate;
			save.mRepeatMeasuresNum = mInstrumentSet.mRepeatMeasuresNum;
			save.mTimeSignature = mInstrumentSet.mTimeSignature.mSignature;
			
			save.mExcludedProgSteps = mChordProgressions.mExcludedProgSteps;
			save.mTonicInfluence = mChordProgressions.mTonicInfluence;
			save.mSubdominantInfluence = mChordProgressions.mSubdominantInfluence;
			save.mDominantInfluence = mChordProgressions.mDominantInfluence;
			save.mTritoneSubInfluence = mChordProgressions.mTritoneSubInfluence;

			save.mDistortion = mDistortion.Second;
			save.mCenterFreq = mCenterFreq.Second;
			save.mOctaveRange = mOctaveRange.Second;
			save.mFreqGain = mFreqGain.Second;
			save.mLowpassCutoffFreq = mLowpassCutoffFreq.Second;
			save.mLowpassResonance = mLowpassResonance.Second;
			save.mHighpassCutoffFreq = mHighpassCutoffFreq.Second;
			save.mHighpassResonance = mHighpassResonance.Second;
			save.mEchoDelay = mEchoDelay.Second;
			save.mEchoDecay = mEchoDecay.Second;
			save.mEchoDry = mEchoDry.Second;
			save.mEchoWet = mEchoWet.Second;
			save.mNumEchoChannels = mNumEchoChannels.Second;
			save.mRever = mReverb.Second;
			save.mRoomSize = mRoomSize.Second;
			save.mReverbDecay = mReverbDecay.Second;
			save.mDynamicStyle = mDynamicStyle;
			Debug.Log("generator saved successfully");
			return save;
		}

		/// Sets variables from save file.
		public void LoadGeneratorSave(GeneratorSave save)
		{
			if (save == null)
				throw new ArgumentNullException("save file could not be loaded");

			mMasterVolume = save.mMasterVolume;
			mVolFadeRate = save.mVolFadeRate;
			mMode = save.mMode;
			mThemeRepeatOptions = (eThemeRepeatOptions)save.mThemeRepeatOptions;
			mKeySteps = save.mKeySteps;
			mKeyChangeAscendDescend = save.mKeyChangeAscendDescend;
			mSetThemeOdds = save.mSetThemeOdds;
			mPlayThemeOdds = save.mPlayThemeOdds;
			mScale = save.mScale;
			mProgressionChangeOdds = save.mProgressionChangeOdds;
			mKey = save.mKey;
			mKeyChangeOdds = save.mKeyChangeOdds;
			mGroupOdds = save.mGroupOdds;
			mGroupRate = save.mGroupRate;
			mDynamicStyle = save.mDynamicStyle;

			mInstrumentSet.SetTempo(save.mTempo);
			mInstrumentSet.SetRepeatMeasuresNum(save.mRepeatMeasuresNum);
			mInstrumentSet.SetProgressionRate(save.mProgressionRate);
			mInstrumentSet.mTimeSignature.SetTimeSignature(save.mTimeSignature);

			mChordProgressions.SetExcludedProgressionSteps(save.mExcludedProgSteps);
			mChordProgressions.SetTonicInflience(save.mTonicInfluence);
			mChordProgressions.SetSubdominantInfluence(save.mSubdominantInfluence);
			mChordProgressions.SetDominantInfluence(save.mDominantInfluence);
			mChordProgressions.SetTritoneSubInfluence(save.mTritoneSubInfluence);

			mDistortion.Second = save.mDistortion;
			mCenterFreq.Second = save.mCenterFreq;
			mOctaveRange.Second = save.mOctaveRange;
			mFreqGain.Second = save.mFreqGain;
			mLowpassCutoffFreq.Second = save.mLowpassCutoffFreq;
			mLowpassResonance.Second = save.mLowpassResonance;
			mHighpassCutoffFreq.Second = save.mHighpassCutoffFreq;
			mHighpassResonance.Second = save.mHighpassResonance;
			mEchoDelay.Second = save.mEchoDelay;
			mEchoDecay.Second = save.mEchoDecay;
			mEchoDry.Second = save.mEchoDry;
			mEchoWet.Second = save.mEchoWet;
			mNumEchoChannels.Second = save.mNumEchoChannels;
			mReverb.Second = save.mRever;
			mRoomSize.Second = save.mRoomSize;
			mReverbDecay.Second = save.mReverbDecay;

			SetVolume(mMasterVolume);
			SetGlobalEffect(mDistortion);
			SetGlobalEffect(mCenterFreq);
			SetGlobalEffect(mOctaveRange);
			SetGlobalEffect(mFreqGain);
			SetGlobalEffect(mLowpassCutoffFreq);
			SetGlobalEffect(mLowpassResonance);
			SetGlobalEffect(mHighpassCutoffFreq);
			SetGlobalEffect(mHighpassResonance);
			SetGlobalEffect(mEchoDelay);
			SetGlobalEffect(mEchoDecay);
			SetGlobalEffect(mEchoDry);
			SetGlobalEffect(mEchoWet);
			SetGlobalEffect(mNumEchoChannels);
			SetGlobalEffect(mReverb);
			SetGlobalEffect(mRoomSize);
			SetGlobalEffect(mReverbDecay);
		}
	}
}