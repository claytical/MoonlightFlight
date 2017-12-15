namespace ProcGenMusic
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using System.IO;

	/// Music generator user interface panel.
	public class MusicGeneratorUIPanel : HelperSingleton<MusicGeneratorUIPanel>
	{
		private Dropdown mMode = null;
		private Dropdown mScale = null;
		public Dropdown mKey = null;
		private Slider mTempo = null;
		private Text mTempoText = null;
		private Slider mVol = null;
		private Text mVolText = null;
		private Dropdown mRepeatThemeOptions = null;
		private Dropdown mRepeatLength = null;
		private MusicGenerator mMusicGenerator = null;
		public Dropdown mProgressionRateDropdown = null;
		private Slider mGroupOdds1 = null;
		private Text mGroupOdds1Text = null;
		private Slider mGroupOdds2 = null;
		private Text mGroupOdds2Text = null;
		private Slider mGroupOdds3 = null;
		private Text mGroupOdds3Text = null;
		private Slider mGroupOdds4 = null;
		private Text mGroupOdds4Text = null;
		private Slider mNewThemeOdds = null;
		private Text mNewThemeOutput = null;
		private Slider mRepeatThemeOdds = null;
		private Text mRepeatThemeOutput = null;
		private Slider mKeyChangeOdds = null;
		private Text mKeyChangeOddsOutput = null;
		private Slider mProgressionChangeOdds = null;
		private Text mProgressionChangeOutput = null;

		private InstrumentListPanelUI mInstrumentListPanelUI = null;
		private InstrumentPanelUI mInstrumentPanelUI = null;
		private StaffPlayerUI mStaffPlayerUI = null;
		[SerializeField]
		public List<string> mPresetFileNames = null;

		public void SetKey(int keyIN) { mKey.value = keyIN; }

		private Animator mAnimator = null;
		private AdvancedSettingsPanel mAdvSettingsPanel = null;
		private GlobalEffectsPanel mGlobalEffectsPanel = null;
		private MeasureEditor mMeasureEditor = null;
		private Tooltips mTooltips = null;
		[SerializeField]
		private string mCurrentLoadedConfig = "";

		private string mFileCurrentlyWriting = "";

		public void Init(MusicGenerator managerIN)
		{
			mMusicGenerator = managerIN;
			mTooltips = UIManager.Instance.mTooltips;
			mAdvSettingsPanel = UIManager.Instance.mAdvancedSettingsPanel;
			mGlobalEffectsPanel = UIManager.Instance.mGlobalEffectsPanel;
			mInstrumentPanelUI = UIManager.Instance.mInstrumentPanelUI;
			mInstrumentListPanelUI = UIManager.Instance.mInstrumentListPanelUI;
			mStaffPlayerUI = UIManager.Instance.mStaffPlayer;
			mAnimator = GetComponentInParent<Animator>();
			mMeasureEditor = UIManager.Instance.mMeasureEditor;

			mInstrumentListPanelUI.Init(mMusicGenerator);

			AddPresets();

			Component[] components = this.GetComponentsInChildren(typeof(Transform), true);
			foreach (Component cp in components)
			{
				if (cp.name == "Mode")
					mTooltips.AddUIElement(ref mMode, cp, "Mode");
				if (cp.name == "Scale")
					mTooltips.AddUIElement(ref mScale, cp, "Scale");
				if (cp.name == "Tempo")
					mTooltips.AddUIElement(ref mTempo, cp, "Tempo");
				if (cp.name == "TempoOutput")
					mTempoText = cp.GetComponentInChildren<Text>();
				if (cp.name == "Key")
					mTooltips.AddUIElement(ref mKey, cp, "Key");
				if (cp.name == "MasterVol")
					mTooltips.AddUIElement(ref mVol, cp, "MasterVol");
				if (cp.name == "ProgressionRate")
					mTooltips.AddUIElement(ref mProgressionRateDropdown, cp, "ProgressionRate");
				if (cp.name == "RepeatLength")
					mTooltips.AddUIElement(ref mRepeatLength, cp, "RepeatLength");
				if (cp.name == "NewThemeOdds")
					mTooltips.AddUIElement(ref mNewThemeOdds, cp, "NewThemeOdds");
				if (cp.name == "RepeatThemeOdds")
					mTooltips.AddUIElement(ref mRepeatThemeOdds, cp, "ThemeRepeat");
				if (cp.name == "KeyChange")
					mTooltips.AddUIElement(ref mKeyChangeOdds, cp, "KeyChangeOdds");
				if (cp.name == "ProgressionChange")
					mTooltips.AddUIElement(ref mProgressionChangeOdds, cp, "ProgressionChangeOdds");

				if (cp.name == "VolumeOutput")
					mVolText = cp.GetComponentInChildren<Text>();
				if (cp.name == "RepeatAndThemeOptions")
					mTooltips.AddUIElement(ref mRepeatThemeOptions, cp, "ThemeRepeat");
				if (cp.name == "GroupOdds1")
					mTooltips.AddUIElement(ref mGroupOdds1, cp, "GroupOdds");
				if (cp.name == "GroupOdds2")
					mTooltips.AddUIElement(ref mGroupOdds2, cp, "GroupOdds");
				if (cp.name == "GroupOdds3")
					mTooltips.AddUIElement(ref mGroupOdds3, cp, "GroupOdds");
				if (cp.name == "GroupOdds4")
					mTooltips.AddUIElement(ref mGroupOdds4, cp, "GroupOdds");

				if (cp.name == "Group1OddsOutput")
					mGroupOdds1Text = cp.GetComponentInChildren<Text>();
				if (cp.name == "Group2OddsOutput")
					mGroupOdds2Text = cp.GetComponentInChildren<Text>();
				if (cp.name == "Group3OddsOutput")
					mGroupOdds3Text = cp.GetComponentInChildren<Text>();
				if (cp.name == "Group4OddsOutput")
					mGroupOdds4Text = cp.GetComponentInChildren<Text>();
				if (cp.name == "NewThemeOutput")
					mNewThemeOutput = cp.GetComponentInChildren<Text>();
				if (cp.name == "RepeatThemeOutput")
					mRepeatThemeOutput = cp.GetComponentInChildren<Text>();
				if (cp.name == "KeyChangeOutput")
					mKeyChangeOddsOutput = cp.GetComponentInChildren<Text>();
				if (cp.name == "ProgressionChangeOutput")
					mProgressionChangeOutput = cp.GetComponentInChildren<Text>();
			}
			mCurrentLoadedConfig = "AAADefault";

			if (mMusicGenerator.AreUsingAsyncLoad())
				StartCoroutine(AsyncLoadNewConfiguration(mCurrentLoadedConfig));
			else
				NonAsyncLoadNewConfiguration(mCurrentLoadedConfig);
		}

		/// Loads a new configuration.
		private void NonAsyncLoadNewConfiguration(string configName)
		{
			mMusicGenerator.ClearInstruments(mMusicGenerator.mInstrumentSet);
			mMusicGenerator.ResetPlayer();
			mMusicGenerator.SetState(eGeneratorState.initializing);
			mMusicGenerator.mMusicFileConfig.LoadConfig(configName);
			for (int i = 0; i < mMusicGenerator.mInstrumentSet.mInstruments.Count; i++)
				LoadNewInstrument(mMusicGenerator.mInstrumentSet.mInstruments[i]);

			SetGeneratorUIValues();
			mInstrumentListPanelUI.mInstrumentIcons[0].ToggleSelected();
			mMusicGenerator.SetState(eGeneratorState.ready);
			mMusicGenerator.ResetPlayer();
		}

		/// Async loads a new configuration.
		private IEnumerator AsyncLoadNewConfiguration(string configName)
		{
			mStaffPlayerUI.PlayLoadingSequence(true);
			mMusicGenerator.ClearInstruments(mMusicGenerator.mInstrumentSet);
			mMusicGenerator.ResetPlayer();
			yield return null;
			mMusicGenerator.SetState(eGeneratorState.initializing);
			bool finished = false;
			StartCoroutine(mMusicGenerator.mMusicFileConfig.AsyncLoadConfig(configName, ((x) => { finished = x; })));
			yield return new WaitUntil(() => finished);
			for (int i = 0; i < mMusicGenerator.mInstrumentSet.mInstruments.Count; i++)
			{
				LoadNewInstrument(mMusicGenerator.mInstrumentSet.mInstruments[i]);
				yield return null;
			}
			SetGeneratorUIValues();
			yield return null;
			mInstrumentListPanelUI.mInstrumentIcons[0].ToggleSelected();
			mMusicGenerator.SetState(eGeneratorState.ready);
			mStaffPlayerUI.PlayLoadingSequence(false);
			yield return null;
		}

		/// Adds files from persistentDataPath and streaming assets folder.
		private void AddPresets()
		{
			mPresetFileNames = new List<string>();
			if (System.IO.Directory.Exists(Application.streamingAssetsPath + "/MusicGenerator/InstrumentSaves"))
			{
				foreach (string folder in System.IO.Directory.GetDirectories(Application.streamingAssetsPath + "/MusicGenerator/InstrumentSaves"))
				{
					string name = new DirectoryInfo(folder).Name;
					if (!mPresetFileNames.Contains(name))
						mPresetFileNames.Add(name);
				}
			}
			if (System.IO.Directory.Exists(Application.persistentDataPath + "/InstrumentSaves"))
			{
				foreach (string folder in System.IO.Directory.GetDirectories(Application.persistentDataPath + "/InstrumentSaves"))
				{
					string name = new DirectoryInfo(folder).Name;
					if (!mPresetFileNames.Contains(name))
						mPresetFileNames.Add(name);
				}
			}

			for (int i = 0; i < mPresetFileNames.Count; i++)
				mStaffPlayerUI.AddPresetOption(mPresetFileNames[i]);
		}

		/// used to adjust the volume slider (currently, by fade in UIManager)
		public void FadeVolume(float volIN)
		{
			mVol.value = volIN;
		}

		/// Updates the Generator UI Panel:
		/// Sets the MusicGenerator() values, based on UI values.
		void Update()
		{
			if (mMusicGenerator.mState < eGeneratorState.ready)
				return;

			/// Check to see if we're in the process of saving a new config and adds preset when it's finihsed.
			if (mFileCurrentlyWriting != "")
			{
				bool completed = MusicHelpers.CheckConfigWriteComplete(mFileCurrentlyWriting);
				if (completed)
				{
					Debug.Log(mFileCurrentlyWriting + " save complete");
					mStaffPlayerUI.AddPresetOption(mFileCurrentlyWriting);
					mFileCurrentlyWriting = "";
				}
			}

			/// update our generator values from UI sliders, dropdowns, etc:
			mMusicGenerator.SetVolume(mVol.value);
			mMusicGenerator.SetThemeRepeatOption((eThemeRepeatOptions)mRepeatThemeOptions.value);
			mMusicGenerator.SetProgressionChangeOdds(mProgressionChangeOdds.value);
			mProgressionChangeOutput.text = ((int)mMusicGenerator.mProgressionChangeOdds).ToString();

			if (mMusicGenerator.mState < eGeneratorState.editing)
			{
				mMusicGenerator.mInstrumentSet.SetTempo(mTempo.value);
				mTempoText.text = ((int)mTempo.value).ToString();

				mMusicGenerator.mScale = (eScale)mScale.value;
				mMusicGenerator.mMode = (eMode)mMode.value;
				mMusicGenerator.mKey = (eKey)mKey.value;
			}
			else
			{
				mMusicGenerator.mInstrumentSet.SetTempo(mMeasureEditor.mTempo.value);
				mTempoText.text = ((int)mTempo.value).ToString();
				mMusicGenerator.mMode = (eMode)mMeasureEditor.mMode.value;
				mMusicGenerator.mScale = (eScale)mMeasureEditor.mScale.value;
				mMusicGenerator.mKey = (eKey)mMeasureEditor.mKey.value;
			}

			mMusicGenerator.SetKeyChangeOdds(mKeyChangeOdds.value);
			mKeyChangeOddsOutput.text = ((int)mKeyChangeOdds.value).ToString();

			mMusicGenerator.mGroupOdds[0] = mGroupOdds1.value;
			mMusicGenerator.mGroupOdds[1] = mGroupOdds2.value;
			mMusicGenerator.mGroupOdds[2] = mGroupOdds3.value;
			mMusicGenerator.mGroupOdds[3] = mGroupOdds4.value;

			mGroupOdds1Text.text = ((int)mGroupOdds1.value).ToString();
			mGroupOdds2Text.text = ((int)mGroupOdds2.value).ToString();
			mGroupOdds3Text.text = ((int)mGroupOdds3.value).ToString();
			mGroupOdds4Text.text = ((int)mGroupOdds4.value).ToString();

			mVolText.text = ((int)mVol.value).ToString();
			mMusicGenerator.mInstrumentSet.SetRepeatMeasuresNum((mRepeatLength.value + 1));

			mMusicGenerator.SetThemeOddsOfSetting(mNewThemeOdds.value);
			mNewThemeOutput.text = ((int)mMusicGenerator.mSetThemeOdds).ToString();

			mMusicGenerator.SetPlayThemeOdds(mRepeatThemeOdds.value);

			mRepeatThemeOutput.text = ((int)mMusicGenerator.mPlayThemeOdds).ToString();
			int progressionRate = mMusicGenerator.mInstrumentSet.mTimeSignature.mTimestepNumInverse[mProgressionRateDropdown.value];
			mMusicGenerator.mInstrumentSet.SetProgressionRate(progressionRate);
		}

		/// Quits the application
		public void QuitGenerator()
		{
			Application.Quit();
		}
		
		/// Loads the configuration from UI presets dropdown.
		public void LoadConfigFromUI()
		{
			if (mMusicGenerator.AreUsingAsyncLoad())
				StartCoroutine(AsyncLoadNewConfiguration(mPresetFileNames[mStaffPlayerUI.mPreset.value]));
			else
				NonAsyncLoadNewConfiguration(mPresetFileNames[mStaffPlayerUI.mPreset.value]);
		}

		/// Loads a new instrument from UI "AddNewInstrument" button.
		private void LoadNewInstrument(Instrument instrumentIN)
		{
			bool isPercussion = instrumentIN.mInstrumentType.Contains("P_") ? true : false;
			mInstrumentListPanelUI.AddInstrument();
			int iconIndex = mInstrumentListPanelUI.mInstrumentIcons.Count - 1;
			InstrumentListUIObject icon = mInstrumentListPanelUI.mInstrumentIcons[iconIndex];
			icon.mInstrument = instrumentIN;
			icon.ToggleSelected();
			icon.SetDropdown(isPercussion);
			int instIndex = (int)instrumentIN.mInstrumentIndex;

			mInstrumentListPanelUI.mInstrumentIcons[instIndex].mGroupText.text = ("Group: " + (instrumentIN.mGroup + 1).ToString());

			mInstrumentListPanelUI.mInstrumentIcons[instIndex].mPanelBack.color =
				mStaffPlayerUI.mColors[(int)instrumentIN.mStaffPlayerColor];

			mInstrumentPanelUI.SetInstrument(mMusicGenerator.mInstrumentSet.mInstruments[instIndex]);
		}

		/// When initially setting, we grab our values from the generator:
		private void SetGeneratorUIValues()
		{
			mScale.value = (int)mMusicGenerator.mScale;
			mKey.value = (int)mMusicGenerator.mKey;
			mTempo.value = mMusicGenerator.mInstrumentSet.mTempo;

			mStaffPlayerUI.ChangeTimeSignature((int)mMusicGenerator.mInstrumentSet.mTimeSignature.mSignature);
			float volOUT = 0.0f;
			mMusicGenerator.mMixer.GetFloat("MasterVol", out volOUT);
			mVol.value = volOUT;

			mRepeatThemeOptions.value = (int)mMusicGenerator.mThemeRepeatOptions;

			mRepeatLength.value = (int)mMusicGenerator.mInstrumentSet.mRepeatMeasuresNum - 1;

			switch ((int)mMusicGenerator.mInstrumentSet.mProgressionRate)
			{
				case 1:
					mProgressionRateDropdown.value = 0;
					break;
				case 2:
					mProgressionRateDropdown.value = 1;
					break;
				case 4:
					mProgressionRateDropdown.value = 2;
					break;
				case 8:
					mProgressionRateDropdown.value = 3;
					break;
				case 16:
					mProgressionRateDropdown.value = 4;
					break;
				default:
					break;
			}

			mGroupOdds1.value = mMusicGenerator.mGroupOdds[0];
			mGroupOdds2.value = mMusicGenerator.mGroupOdds[1];
			mGroupOdds3.value = mMusicGenerator.mGroupOdds[2];
			mGroupOdds4.value = mMusicGenerator.mGroupOdds[3];

			mNewThemeOdds.value = mMusicGenerator.mSetThemeOdds;

			mRepeatThemeOdds.value = mMusicGenerator.mPlayThemeOdds;
			mKeyChangeOdds.value = mMusicGenerator.mKeyChangeOdds;
			mMode.value = (int)mMusicGenerator.mMode;

			mAdvSettingsPanel.mTonicInfluence.value = mMusicGenerator.mChordProgressions.mTonicInfluence;
			mAdvSettingsPanel.mSubdominantInfluence.value = mMusicGenerator.mChordProgressions.mSubdominantInfluence;
			mAdvSettingsPanel.mDominantInfluence.value = mMusicGenerator.mChordProgressions.mDominantInfluence;
			mAdvSettingsPanel.mTritoneSubInfluence.value = mMusicGenerator.mChordProgressions.mTritoneSubInfluence;
			mAdvSettingsPanel.mAscendDescendKey.value = mMusicGenerator.mKeyChangeAscendDescend;
			mProgressionChangeOdds.value = mMusicGenerator.mProgressionChangeOdds;
			mAdvSettingsPanel.mGroupRate.value = (int)mMusicGenerator.mGroupRate;
			mAdvSettingsPanel.mDynamicStyle.value = (int)mMusicGenerator.mDynamicStyle;
			mAdvSettingsPanel.mVolumeFadeRate.value = (int)mMusicGenerator.mVolFadeRate;

			//because we don't actually keep track of these, grab from the mixer. 
			float audi = 0.0f;
			mMusicGenerator.mMixer.GetFloat("MasterDistortion", out audi);
			mGlobalEffectsPanel.mDistortion.mSlider.value = audi;

			mMusicGenerator.mMixer.GetFloat("MasterCenterFrequency", out audi);
			mGlobalEffectsPanel.mCenterFrequency.mSlider.value = audi;

			mMusicGenerator.mMixer.GetFloat("MasterOctaveRange", out audi);
			mGlobalEffectsPanel.mOctaveRange.mSlider.value = audi;

			mMusicGenerator.mMixer.GetFloat("MasterFrequencyGain", out audi);
			mGlobalEffectsPanel.mFrequencyGain.mSlider.value = audi;

			mMusicGenerator.mMixer.GetFloat("MasterLowpassCutoffFreq", out audi);
			mGlobalEffectsPanel.mLowpassCutoffFreq.mSlider.value = audi;

			mMusicGenerator.mMixer.GetFloat("MasterLowpassResonance", out audi);
			mGlobalEffectsPanel.mLowpassResonance.mSlider.value = audi;

			mMusicGenerator.mMixer.GetFloat("MasterHighpassCutoffFreq", out audi);
			mGlobalEffectsPanel.mHighpassCutoffFreq.mSlider.value = audi;

			mMusicGenerator.mMixer.GetFloat("MasterFrequencyGain", out audi);
			mGlobalEffectsPanel.mFrequencyGain.mSlider.value = audi;

			mMusicGenerator.mMixer.GetFloat("MasterLowpassCutoffFreq", out audi);
			mGlobalEffectsPanel.mLowpassCutoffFreq.mSlider.value = audi;

			mMusicGenerator.mMixer.GetFloat("MasterLowpassResonance", out audi);
			mGlobalEffectsPanel.mLowpassResonance.mSlider.value = audi;

			mMusicGenerator.mMixer.GetFloat("MasterHighpassCutoffFreq", out audi);
			mGlobalEffectsPanel.mHighpassCutoffFreq.mSlider.value = audi;

			mMusicGenerator.mMixer.GetFloat("MasterHighpassResonance", out audi);
			mGlobalEffectsPanel.mHighpassResonance.mSlider.value = audi;

			mMusicGenerator.mMixer.GetFloat("MasterEchoDelay", out audi);
			mGlobalEffectsPanel.mEchoDelay.mSlider.value = audi;

			mMusicGenerator.mMixer.GetFloat("MasterEchoDecay", out audi);
			mGlobalEffectsPanel.mEchoDecay.mSlider.value = audi;

			mMusicGenerator.mMixer.GetFloat("MasterEchoDry", out audi);
			mGlobalEffectsPanel.mEchoDry.mSlider.value = audi;

			mMusicGenerator.mMixer.GetFloat("MasterEchoWet", out audi);
			mGlobalEffectsPanel.mEchoWet.mSlider.value = audi;

			mMusicGenerator.mMixer.GetFloat("MasterNumEchoChannels", out audi);
			mGlobalEffectsPanel.mNumEchoChannels.mSlider.value = audi;

			mMusicGenerator.mMixer.GetFloat("MasterReverb", out audi);
			mGlobalEffectsPanel.mReverb.mSlider.value = audi;

			mMusicGenerator.mMixer.GetFloat("MasterRoomSize", out audi);
			mGlobalEffectsPanel.mRoomSize.mSlider.value = audi;

			mMusicGenerator.mMixer.GetFloat("MasterReverbDecay", out audi);
			mGlobalEffectsPanel.mReverbDecay.mSlider.value = audi;

			List<bool> excludes = mMusicGenerator.mChordProgressions.mExcludedProgSteps;
			for (int i = 0; i < excludes.Count; i++)
				mAdvSettingsPanel.mExcludedSteps[i].isOn = excludes[i];

			mAdvSettingsPanel.CheckAvoidSteps();
		}

		/// Saves the configuration to file.
		public void SaveConfiguration(string fileName)
		{
			mFileCurrentlyWriting = fileName;

			/// we want to clear this directory if it already exists.
			string directory = Application.persistentDataPath + "/InstrumentSaves/" + fileName;
			if (!Directory.Exists(directory))
				Directory.CreateDirectory(directory);
			else
			{
				Directory.Delete(directory, true);
				Directory.CreateDirectory(directory);
			}

			string generatorsave = JsonUtility.ToJson(mMusicGenerator.GetGeneratorSave());
			MusicFileConfig.SaveConfiguration(fileName, "generator", generatorsave);

			for (int i = 0; i < mMusicGenerator.mInstrumentSet.mInstruments.Count; i++)
			{
				InstrumentSave instrumentSave = mMusicGenerator.mInstrumentSet.mInstruments[i].SaveInstrument();
				string instrumentSaveString = JsonUtility.ToJson(instrumentSave);
				MusicFileConfig.SaveConfiguration(fileName, "instruments" + i.ToString(), instrumentSaveString);
			}
		}

		/// Toggles the panel animation:
		public void GeneratorPanelToggle()
		{
			if (mAnimator.GetInteger("mState") == 0)
				mAnimator.SetInteger("mState", 1);
			else
				mAnimator.SetInteger("mState", 0);
		}
	}
}