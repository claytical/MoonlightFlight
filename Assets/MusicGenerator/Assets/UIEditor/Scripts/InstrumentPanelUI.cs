namespace ProcGenMusic
{
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// Just a simple UI panel for the instruments:
public class InstrumentPanelUI : HelperSingleton<InstrumentPanelUI>
{
	public Instrument mInstrument = null;

	//The following variables are basically all references to
	//the UI for their instrument's variables:
	public Toggle mUseSevenths = null;

	private Slider mPatternLengthSlider = null;
	private Text mPatternLengthOutput = null;

	private Slider mPatternReleaseSlider = null;
	private Text mPatternReleaseOutput = null;

	private Slider mOddsOfPlayingSlider = null;
	private Text mOddsOfPlayingValueText = null;

	private Slider mStrumLength = null;
	private Text mStrumLengthOutput = null;

	private Slider mStrumVariation = null;
	private Text mStrumVariationOutput = null;

	private Slider mLeadVariation = null;
	private Text mLeadVariationOutput = null;

	private Slider mLeadMaxSteps = null;
	private Text mLeadMaxStepsOutput = null;

	private Slider mMultiplierSlider = null;
	private Text mMultiplierText = null;

	private Slider mVolumeSlider = null;
	private Text mVolumeText = null;

	public Toggle mMuteToggle = null;

	private Slider mReverbSlider = null;
	private Text mReverbOutput = null;

	private Slider mRoomSizeSlider = null;
	private Text mRoomSizeOutput = null;

	private Slider mChorusSlider = null;
	private Text mChorusOutput = null;

	private Slider mFlangerSlider = null;
	private Text mFlangerOutput = null;

	private Slider mDistortionSlider = null;
	private Text mDistortionOutput = null;

	private Slider mEchoSlider = null;
	private Text mEchoOutput = null;

	private Slider mEchoDelaySlider = null;
	private Text mEchoDelayOutput = null;

	private Slider mEchoDecaySlider = null;
	private Text mEchoDecayOutput = null;

	private Slider mAudioGroupVolume = null;
	private Text mAudioGroupVolumeOutput = null;

	private Dropdown mTimestep = null;

	private Dropdown mSuccession = null;

	private MusicGenerator mMusicGenerator = null;
	private GameObject mMasterObject = null;

	private Slider mOddsOfPlayingChordNoteSlider = null;
	private Text mOddsOfPlayingChordNoteText = null;

	private List<int> mOctavesToUse = new List<int>();
	private Toggle mOctave1 = null;
	private Toggle mOctave2 = null;
	private Toggle mOctave3 = null;

	private Dropdown mGroup = null;
	private Dropdown mColor = null;

	private Slider mStereoPan = null;
	private InstrumentListPanelUI mInstrumentListUI = null;
	private StaffPlayerUI mStaffPlayerUI = null;

	private Toggle mFreeMelody = null;
	private Dropdown mUsePattern = null;
	private Tooltips mTooltips = null;
	private MeasureEditor mMeasureEditor = null;

	public void Init()
	{
		mMusicGenerator = MusicGenerator.Instance;
		mTooltips = UIManager.Instance.mTooltips;
		mInstrumentListUI = UIManager.Instance.mInstrumentListPanelUI;
		mStaffPlayerUI = UIManager.Instance.mStaffPlayer;
		mMeasureEditor = UIManager.Instance.mMeasureEditor;
		Component[] components = this.GetComponentsInChildren(typeof(Transform), true);
		foreach (Component cp in components)
		{
			if (cp.name == "LeadVariation")
				mTooltips.AddUIElement<Slider>(ref mLeadVariation, cp, "LeadVariation");
			if (cp.name == "LeadMaxSteps")
				mTooltips.AddUIElement<Slider>(ref mLeadMaxSteps, cp, "LeadMaxSteps");
			if (cp.name == "MasterObject")
				mMasterObject = cp.gameObject;

			if (cp.name == "PatternRelease")
				mTooltips.AddUIElement<Slider>(ref mPatternReleaseSlider, cp, "PatternRelease");
			if (cp.name == "PatternLength")
				mTooltips.AddUIElement<Slider>(ref mPatternLengthSlider, cp, "PatternLength");
			if (cp.name == "StrumLength")
				mTooltips.AddUIElement<Slider>(ref mStrumLength, cp, "StrumLength");
			if (cp.name == "StrumVariation")
				mTooltips.AddUIElement<Slider>(ref mStrumVariation, cp, "StrumVariation");
			if (cp.name == "UseSevenths")
				mTooltips.AddUIElement<Toggle>(ref mUseSevenths, cp, "UseSevenths");
			if (cp.name == "OddsOfPlaying")
				mTooltips.AddUIElement<Slider>(ref mOddsOfPlayingSlider, cp, "OddsOfPlaying");
			if (cp.name == "MultiplierOdds")
				mTooltips.AddUIElement<Slider>(ref mMultiplierSlider, cp, "MultiplierOdds");
			if (cp.name == "VolumeSlider")
				mTooltips.AddUIElement<Slider>(ref mVolumeSlider, cp, "Volume");
			if (cp.name == "Mute")
				mTooltips.AddUIElement<Toggle>(ref mMuteToggle, cp, "Mute");
			if (cp.name == "Echo")
				mTooltips.AddUIElement<Slider>(ref mEchoSlider, cp, "Echo");
			if (cp.name == "EchoDecay")
				mTooltips.AddUIElement<Slider>(ref mEchoDecaySlider, cp, "EchoDecay");
			if (cp.name == "EchoDelay")
				mTooltips.AddUIElement<Slider>(ref mEchoDelaySlider, cp, "EchoDelay");
			if (cp.name == "Reverb")
				mTooltips.AddUIElement<Slider>(ref mReverbSlider, cp, "Reverb");
			if (cp.name == "RoomSize")
				mTooltips.AddUIElement<Slider>(ref mRoomSizeSlider, cp, "RoomSize");
			if (cp.name == "Timestep")
				mTooltips.AddUIElement<Dropdown>(ref mTimestep, cp, "Timestep");
			if (cp.name == "Flanger")
				mTooltips.AddUIElement<Slider>(ref mFlangerSlider, cp, "Flanger");
			if (cp.name == "Distortion")
				mTooltips.AddUIElement<Slider>(ref mDistortionSlider, cp, "Distortion");
			if (cp.name == "Chorus")
				mTooltips.AddUIElement<Slider>(ref mChorusSlider, cp, "Chorus");
			if (cp.name == "Succession")
				mTooltips.AddUIElement<Dropdown>(ref mSuccession, cp, "Succession");
			if (cp.name == "OddsOfPlayingChordNote")
				mTooltips.AddUIElement<Slider>(ref mOddsOfPlayingChordNoteSlider, cp, "ChordNote");
			if (cp.name == "Octave1")
				mTooltips.AddUIElement<Toggle>(ref mOctave1, cp, "OctavesToUse");
			if (cp.name == "Octave2")
				mTooltips.AddUIElement<Toggle>(ref mOctave2, cp, "OctavesToUse");
			if (cp.name == "Octave3")
				mTooltips.AddUIElement<Toggle>(ref mOctave3, cp, "OctavesToUse");
			if (cp.name == "Group")
				mTooltips.AddUIElement<Dropdown>(ref mGroup, cp, "Group");
			if (cp.name == "Color")
				mTooltips.AddUIElement<Dropdown>(ref mColor, cp, "Color");
			if (cp.name == "StereoPan")
				mTooltips.AddUIElement<Slider>(ref mStereoPan, cp, "StereoPan");
			if (cp.name == "UsePattern")
				mTooltips.AddUIElement<Dropdown>(ref mUsePattern, cp, "Pattern");
			if (cp.name == "FreeMelody")
				mTooltips.AddUIElement<Toggle>(ref mFreeMelody, cp, "Lead");
			if (cp.name == "AudioGroupVolume")
				mTooltips.AddUIElement<Slider>(ref mAudioGroupVolume, cp, "AudioGroupVolume");

			//output:
			if (cp.name == "PatternLengthOutput")
				mPatternLengthOutput = cp.gameObject.GetComponentInChildren<Text>();
			if (cp.name == "PatternReleaseOutput")
				mPatternReleaseOutput = cp.gameObject.GetComponentInChildren<Text>();
			if (cp.name == "StrumLengthOutput")
				mStrumLengthOutput = cp.gameObject.GetComponentInChildren<Text>();
			if (cp.name == "StrumVariationOutput")
				mStrumVariationOutput = cp.gameObject.GetComponentInChildren<Text>();
			if (cp.name == "LeadVariationOutput")
				mLeadVariationOutput = cp.gameObject.GetComponentInChildren<Text>();
			if (cp.name == "LeadMaxStepsOutput")
				mLeadMaxStepsOutput = cp.gameObject.GetComponentInChildren<Text>();
			if (cp.name == "OddsOfPlayingOutput")
				mOddsOfPlayingValueText = cp.gameObject.GetComponentInChildren<Text>();
			if (cp.name == "MultiplierOutput")
				mMultiplierText = cp.gameObject.GetComponentInChildren<Text>();
			if (cp.name == "VolumeOutput")
				mVolumeText = cp.gameObject.GetComponentInChildren<Text>();
			if (cp.name == "RoomSizeOutput")
				mRoomSizeOutput = cp.gameObject.GetComponentInChildren<Text>();
			if (cp.name == "ReverbOutput")
				mReverbOutput = cp.gameObject.GetComponentInChildren<Text>();
			if (cp.name == "EchoOutput")
				mEchoOutput = cp.gameObject.GetComponentInChildren<Text>();
			if (cp.name == "EchoDelayOutput")
				mEchoDelayOutput = cp.gameObject.GetComponentInChildren<Text>();
			if (cp.name == "EchoDecayOutput")
				mEchoDecayOutput = cp.gameObject.GetComponentInChildren<Text>();
			if (cp.name == "FlangerOutput")
				mFlangerOutput = cp.gameObject.GetComponentInChildren<Text>();
			if (cp.name == "DistortionOutput")
				mDistortionOutput = cp.gameObject.GetComponentInChildren<Text>();
			if (cp.name == "ChorusOutput")
				mChorusOutput = cp.gameObject.GetComponentInChildren<Text>();
			if (cp.name == "OddsOfPlayingChordNoteOutput")
				mOddsOfPlayingChordNoteText = cp.gameObject.GetComponentInChildren<Text>();
			if (cp.name == "AudioGroupVolumeOutput")
				mAudioGroupVolumeOutput = cp.gameObject.GetComponentInChildren<Text>();
		}

		mMasterObject.SetActive(false);
		mColor.options.Clear();
		for (int i = 0; i < mStaffPlayerUI.mColors.Count; i++)
		{
			Texture2D texture = new Texture2D(1, 1);
			texture.SetPixel(0, 0, mStaffPlayerUI.mColors[i]);
			texture.Apply();
			Dropdown.OptionData data = new Dropdown.OptionData(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0)));
			mColor.options.Add(data);
		}
	}

	/// changes the color of the instrument in the UI
	public void ChangeColor()
	{
		mColor.gameObject.GetComponent<Image>().color = mStaffPlayerUI.mColors[mColor.value];
	}

	/// sets a new instrument to be displayed:
	public void SetInstrument(Instrument instrumenIN)
	{
		if (mInstrumentListUI.mInstrumentIcons.Count <= 0)
			return;

		if (instrumenIN == null)
		{
			InstrumentSet set = (mMusicGenerator.mState >= eGeneratorState.editorInitializing) ? MeasureEditor.Instance.mCurrentInstSet : mMusicGenerator.mInstrumentSet;
			List<Instrument> instruments = set.mInstruments;
			instrumenIN = instruments[instruments.Count - 1];
		}

		mMasterObject.SetActive(true);

		mInstrument = instrumenIN;
		mSuccession.value = (int)mInstrument.mSuccessionType;
		mStrumLength.value = mInstrument.mStrumLength / mMusicGenerator.mInstrumentSet.mBeatLength; ;
		mStrumVariation.value = mInstrument.mStrumVariation;
		mUseSevenths.isOn = mInstrument.mChordSize == 4;
		mLeadMaxSteps.value = mInstrument.mLeadMaxSteps;
		mLeadVariation.value = mInstrument.AscendDescendInfluence;
		mStereoPan.value = mInstrument.mStereoPan;
		mOddsOfPlayingSlider.value = mInstrument.mOddsOfPlaying;
		mMultiplierSlider.value = mInstrument.mOddsOfPlayingMultiplierMax;
		mOddsOfPlayingValueText.text = mInstrument.mOddsOfPlaying.ToString();
		mMultiplierText.text = "x" + mInstrument.mOddsOfPlayingMultiplierMax.ToString();
		mVolumeSlider.value = mInstrument.mVolume;
		mVolumeText.text = mInstrument.mVolume.ToString();
		mMuteToggle.isOn = mInstrument.mIsMuted;
		mTimestep.value = (int)mInstrument.mTimeStep;
		mOddsOfPlayingChordNoteSlider.value = mInstrument.mOddsOfUsingChordNotes;
		mOddsOfPlayingChordNoteText.text = mInstrument.mOddsOfUsingChordNotes.ToString();
		mColor.value = (int)mInstrument.mStaffPlayerColor;
		mInstrumentListUI.mInstrumentIcons[(int)mInstrument.mInstrumentIndex].mPanelBack.color = mStaffPlayerUI.mColors[mColor.value];
		mUsePattern.value = (int)mInstrument.mUsePattern;
		mPatternLengthSlider.value = mInstrument.mPatternlength;
		mPatternReleaseSlider.value = mInstrument.mPatternRelease;
		mRoomSizeSlider.value = mInstrument.mRoomSize;
		mReverbSlider.value = mInstrument.mReverb;
		mEchoSlider.value = mInstrument.mEcho;
		mEchoDelaySlider.value = mInstrument.mEchoDelay;
		mEchoDecaySlider.value = mInstrument.mEchoDecay;
		mFlangerSlider.value = mInstrument.mFlanger;
		mDistortionSlider.value = mInstrument.mDistortion;
		mChorusSlider.value = mInstrument.mChorus;
		mGroup.value = (int)mInstrument.mGroup;
		mInstrumentListUI.mInstrumentIcons[(int)mInstrument.mInstrumentIndex].mGroupText.text = ("Group: " + (mGroup.value + 1).ToString());
		mAudioGroupVolume.value = mInstrument.mAudioSourceVolume;

		SetOctavesFrominstrument();

		ToggleChorusMelody();
	}

	/// mutes instrument;
	public void SetMute(bool isMuted)
	{
		mMuteToggle.isOn = isMuted;
	}

	/// toggles whether this is a chorus or melodic instrument
	public void ToggleChorusMelody()
	{
		bool isMelody = false;
		if (mSuccession.value != (int)eSuccessionType.rhythm)
			isMelody = true;
		mInstrument.SetSuccessionType((eSuccessionType)mSuccession.value);
		mOddsOfPlayingSlider.transform.parent.gameObject.SetActive(isMelody);
		mOddsOfPlayingValueText.transform.parent.gameObject.SetActive(isMelody);
		mMultiplierSlider.transform.parent.gameObject.SetActive(isMelody);
		mMultiplierSlider.transform.parent.gameObject.SetActive(isMelody);
		mLeadMaxSteps.transform.parent.gameObject.SetActive(isMelody);
		mLeadVariation.transform.parent.gameObject.SetActive(isMelody);
		mFreeMelody.gameObject.SetActive(isMelody);
		mStrumLength.transform.parent.gameObject.SetActive(!isMelody);
		mStrumVariation.transform.parent.gameObject.SetActive(!isMelody);
		mOddsOfPlayingSlider.value = mInstrument.mOddsOfPlaying;

		if (mMusicGenerator.mState >= eGeneratorState.editorInitializing)
			UIManager.Instance.mMeasureEditor.ToggleHelperNotes();
	}

	void Update()
	{
		if (mInstrumentListUI.mInstrumentIcons.Count <= 0)
			mInstrument = null;
		
		if(mMusicGenerator.mState == eGeneratorState.editorInitializing)
			return;
			
		List<Instrument> instruments = (mMusicGenerator.mState >= eGeneratorState.editorInitializing) ?
			MeasureEditor.Instance.mCurrentInstSet.mInstruments : mMusicGenerator.mInstrumentSet.mInstruments;

		if (mInstrument != null && mMasterObject.activeSelf && mInstrumentListUI.mInstrumentIcons.Count > 0 &&
			mInstrument.mInstrumentIndex < instruments.Count)
		{
			mInstrument.SetAudioSourceVolume(mAudioGroupVolume.value);

			mInstrument.SetStrumLength(mStrumLength.value * mMusicGenerator.mInstrumentSet.mBeatLength);
			mStrumLengthOutput.text = mInstrument.mStrumLength.ToString();
			mInstrument.SetStrumVariation(mStrumVariation.value * mMusicGenerator.mInstrumentSet.mBeatLength);
			mStrumVariationOutput.text = mInstrument.mStrumVariation.ToString();

			uint chordSize = (uint)(mUseSevenths.isOn ? 4 : 3);
			mInstrument.SetChordSize(chordSize);
			mInstrument.SetOddsOfPlaying((uint)mOddsOfPlayingSlider.value);
			mInstrument.SetOddsOfPlayingMultiplierMax(mMultiplierSlider.value);
			mInstrument.SetVolume(mVolumeSlider.value);

			if (mInstrument.mIsMuted != mMuteToggle.isOn)
			{
				mInstrument.mIsMuted = mMuteToggle.isOn;
				mInstrumentListUI.mInstrumentIcons[(int)mInstrument.mInstrumentIndex].mMuteToggle.isOn = mMuteToggle.isOn;
			}

			mInstrument.SetLeadMaxSteps((uint)mLeadMaxSteps.value);
			mInstrument.SetLeadAscendDescend(mLeadVariation.value);
			mInstrument.SetTimestep((eTimestep)mTimestep.value);
			mInstrument.SetOddsOfUsingChordNotes(mOddsOfPlayingChordNoteSlider.value);

			mInstrument.SetRoomSize(mRoomSizeSlider.value);
			mInstrument.SetReverb(mReverbSlider.value);
			mInstrument.SetEcho(mEchoSlider.value);
			mInstrument.SetEchoDelay(mEchoDelaySlider.value);
			mInstrument.SetEchoDecay(mEchoDecaySlider.value);
			mInstrument.SetFlanger(mFlangerSlider.value);
			mInstrument.SetDistortion(mDistortionSlider.value);
			mInstrument.SetChorus(mChorusSlider.value);

			mMusicGenerator.mMixer.SetFloat("RoomSize" + (mInstrument.mInstrumentIndex).ToString(), mRoomSizeSlider.value);
			mMusicGenerator.mMixer.SetFloat("Reverb" + (mInstrument.mInstrumentIndex).ToString(), mReverbSlider.value);
			mMusicGenerator.mMixer.SetFloat("Echo" + (mInstrument.mInstrumentIndex).ToString(), mEchoSlider.value);
			mMusicGenerator.mMixer.SetFloat("EchoDelay" + (mInstrument.mInstrumentIndex).ToString(), mEchoDelaySlider.value);
			mMusicGenerator.mMixer.SetFloat("EchoDecay" + (mInstrument.mInstrumentIndex).ToString(), mEchoDecaySlider.value);
			mMusicGenerator.mMixer.SetFloat("Flange" + (mInstrument.mInstrumentIndex).ToString(), mFlangerSlider.value);
			mMusicGenerator.mMixer.SetFloat("Distortion" + (mInstrument.mInstrumentIndex).ToString(), mDistortionSlider.value);
			mMusicGenerator.mMixer.SetFloat("Chorus" + (mInstrument.mInstrumentIndex).ToString(), mChorusSlider.value);

			mInstrument.SetGroup((uint)mGroup.value);
			mInstrumentListUI.mInstrumentIcons[(int)mInstrument.mInstrumentIndex].mGroupText.text = ("Group: " + mGroup.value.ToString());

			mInstrument.SetStaffPlayerColor((eStaffPlayerColors) mColor.value);
			mInstrumentListUI.mInstrumentIcons[(int)mInstrument.mInstrumentIndex].mPanelBack.color = mStaffPlayerUI.mColors[mColor.value];
			mInstrument.SetStereoPan(mStereoPan.value);

			mInstrument.SetSuccessionType((eSuccessionType)mSuccession.value);
			mUsePattern.value = (int)mInstrument.mSuccessionType == 2 ? 1 : mUsePattern.value;
			mInstrument.SetUsingPattern((uint)mUsePattern.value);

			mInstrument.SetPatternLength((uint)mPatternLengthSlider.value);
			mInstrument.SetPatternRelease((uint)mPatternReleaseSlider.value);
			mPatternReleaseOutput.text = mPatternReleaseSlider.value.ToString();
			mPatternLengthOutput.text = mPatternLengthSlider.value.ToString();

			mPatternLengthSlider.transform.parent.gameObject.SetActive(mUsePattern.value == 1);
			mPatternReleaseSlider.transform.parent.gameObject.SetActive(mUsePattern.value == 1);

			GetOctaves();

			mLeadVariationOutput.text = mInstrument.AscendDescendInfluence.ToString();
			mLeadMaxStepsOutput.text = mInstrument.mLeadMaxSteps.ToString();
			mOddsOfPlayingValueText.text = mInstrument.mOddsOfPlaying.ToString();
			mMultiplierText.text = "x" + mInstrument.mOddsOfPlayingMultiplierMax.ToString();
			mVolumeText.text = mInstrument.mVolume.ToString();
			mRoomSizeOutput.text = mRoomSizeSlider.value.ToString();
			mReverbOutput.text = mReverbSlider.value.ToString();
			mEchoOutput.text = mEchoSlider.value.ToString();
			mEchoDelayOutput.text = mEchoDelaySlider.value.ToString();
			mEchoDecayOutput.text = mEchoDecaySlider.value.ToString();
			mFlangerOutput.text = mFlangerSlider.value.ToString();
			mDistortionOutput.text = mDistortionSlider.value.ToString();
			mChorusOutput.text = mChorusSlider.value.ToString();
			mOddsOfPlayingChordNoteText.text = mOddsOfPlayingChordNoteSlider.value.ToString();
			mAudioGroupVolumeOutput.text = mAudioGroupVolume.value.ToString();
		}
		else if (mInstrumentListUI.mInstrumentIcons.Count > 0)
		{
			mInstrumentListUI.mInstrumentIcons[mInstrumentListUI.mInstrumentIcons.Count - 1].ToggleSelected();
		}
	}

	///Sets octaves from instrument.
	private void SetOctavesFrominstrument()
	{
		mOctave1.isOn = mInstrument.mOctavesToUse.Contains(0);
		mOctave2.isOn = mInstrument.mOctavesToUse.Contains(1); ;
		mOctave3.isOn = mInstrument.mOctavesToUse.Contains(2); ;
	}

	///returns selected octaves:
	private void GetOctaves()
	{
		mOctavesToUse.Clear();
		if (mOctave1.isOn)
			mOctavesToUse.Add(0);
		if (mOctave2.isOn)
			mOctavesToUse.Add(1);
		if (mOctave3.isOn)
			mOctavesToUse.Add(2);

		/// Safety check.
		if (mOctavesToUse.Count == 0)
		{
			mOctavesToUse.Add(0);
			mOctave1.isOn = true;
		}

		mInstrument.mOctavesToUse.Clear();
		for (int i = 0; i < mOctavesToUse.Count; i++)
			mInstrument.mOctavesToUse.Add(mOctavesToUse[i]);
	}

	///toggles the lead setting on/off
	public void ToggleLead()
	{
		if (mFreeMelody.isOn)
		{
			mUsePattern.value = (int)mInstrument.mUsePattern;
		}
		mMeasureEditor.UIToggleHelperNotes();
	}
}
}