namespace ProcGenMusic
{
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// Handles event calls from the music generator.
public class UIManager : HelperSingleton<UIManager> {
	public MeasureEditor mMeasureEditor = null;
	public MusicGeneratorUIPanel mGeneratorUIPanel = null;									//< ui panel for general settings
	public Tooltips mTooltips = null;														//< tooltips class
	public AdvancedSettingsPanel mAdvancedSettingsPanel = null;								//< advanced settings panel
	public GlobalEffectsPanel mGlobalEffectsPanel = null;									//< global effects panel
	public InstrumentListPanelUI mInstrumentListPanelUI = null;								//< instrument list panel
	public InstrumentPanelUI mInstrumentPanelUI = null;										//< instrument panel
	public StaffPlayerUI mStaffPlayer = null;
	private MusicGenerator mMusicGenerator = null;
	
	/// Awake is called when the script instance is being loaded.
	public override void Awake()
	{
		base.Awake();
		mMusicGenerator = GameObject.Find("MusicGenerator").GetComponent<MusicGenerator>();
		mMusicGenerator.Started += OnStarted;
		mMusicGenerator.HasVisiblePlayer += OnHasVisiblePlayer;
	}

	public void OnStarted(object source, EventArgs e)
	{
		mMusicGenerator = MusicGenerator.Instance;
		mMusicGenerator.StateSet += OnStateSet;
		mMusicGenerator.VolumeFaded += OnVolumeFaded;
		mMusicGenerator.ProgressionGenerated += OnProgressionGenerated;
		mMusicGenerator.InstrumentsCleared += OnInstrumentsCleared;
		mMusicGenerator.KeyChanged += OnKeyChanged;
		mMusicGenerator.NormalMeasureExited += OnNormalMeasureExited;
		mMusicGenerator.RepeatNotePlayed += OnRepeatNotePlayed;
		mMusicGenerator.RepeatedMeasureExited += OnRepeatedMeasureExited;
		mMusicGenerator.BarlineColorSet += OnSetBarlineColor;
		mMusicGenerator.UIPlayerIsEditing += OnUIPlayerIsEditing;
		mMusicGenerator.UIStaffNotePlayed += OnUIStaffNotePlayed;
		mMusicGenerator.ClipLoaded += OnClipLoaded;
		mMusicGenerator.EditorClipPlayed += OnEditorClipPlayed;
		mMusicGenerator.PlayerReset += OnPlayerReset;
		mMusicGenerator.UIStaffNoteStrummed += OnUIStaffNoteStrummed;
		Init();
	}
	
	public void Init()
	{
		mTooltips = Tooltips.Instance;
		mTooltips.Init();
		
		mStaffPlayer = StaffPlayerUI.Instance;
		
		mInstrumentListPanelUI = InstrumentListPanelUI.Instance;
		mInstrumentListPanelUI.Init(mMusicGenerator);

		mMeasureEditor = MeasureEditor.Instance;
		mMeasureEditor.Init(mMusicGenerator);
		
		mInstrumentPanelUI = InstrumentPanelUI.Instance;
		mInstrumentPanelUI.Init();

		mAdvancedSettingsPanel = AdvancedSettingsPanel.Instance;
		mAdvancedSettingsPanel.Init();

		mGlobalEffectsPanel = GlobalEffectsPanel.Instance;
		mGlobalEffectsPanel.Init(mMusicGenerator);

		mGeneratorUIPanel = MusicGeneratorUIPanel.Instance;
		mGeneratorUIPanel.Init(mMusicGenerator);
		mStaffPlayer.Init(mMusicGenerator);
	}

	public bool OnHasVisiblePlayer(object source, EventArgs e)
	{
		return true;
	}

	public void OnRepeatedMeasureExited(object source, MusicGeneratorStateArgs e)
	{
			mStaffPlayer.ClearNotes ();

			if (e.mState >= eGeneratorState.editing)
			{
				if (mMeasureEditor.mCurrentMeasure.value < mMeasureEditor.mNumberOfMeasures.value)
					mMeasureEditor.mCurrentMeasure.value += 1;
				else
					mMeasureEditor.mCurrentMeasure.value = 0;
				mMeasureEditor.UIToggleAllInstruments(true);
			}
	}

	public void OnVolumeFaded(object source, FloatArgs e)
	{
		mGeneratorUIPanel.FadeVolume(e.mFloat);
	}

	public void OnStateSet(object source, MusicGeneratorStateArgs e)
	{
		SetState(e.mState);
	}	

	public void SetState(eGeneratorState stateIN)
	{
		switch(stateIN)
		{
			case eGeneratorState.initializing:
				break;
			case eGeneratorState.ready:
				break;
			case eGeneratorState.stopped:
			{
				mMeasureEditor.mCurrentMeasure.value = 0;
				mStaffPlayer.SetBarlineColor(-1, false);
				break;
			}
			case eGeneratorState.playing:
				break;
			case eGeneratorState.repeating:
				break;
			case eGeneratorState.paused:
			{
				if (stateIN == eGeneratorState.editing)
					mMeasureEditor.mLoadedClips [mMeasureEditor.mCurrentClip].mIsPlaying = false ;
				break;
			}
			case eGeneratorState.editing:
			{
				mStaffPlayer.ClearNotes (true, !mMeasureEditor.mShowEditorHints.isOn);
				break;
			}
			case eGeneratorState.editorPaused:
				break;
			case eGeneratorState.editorStopped:
			{
				mMeasureEditor.mCurrentMeasure.value = 0;
				mStaffPlayer.SetBarlineColor(-1, false);
				break;
			}
			case eGeneratorState.editorPlaying:
			{
				break;
			}
			default:
				break;
		}
	}

	public void OnProgressionGenerated(object source, EventArgs e)
	{
		if(mMusicGenerator.mState == eGeneratorState.editing)
			mMeasureEditor.UIToggleHelperNotes ();
	}

	public void OnInstrumentsCleared(object source, EventArgs e)
	{
		mInstrumentListPanelUI.ClearInstruments();
	}

	public void OnKeyChanged(object source, IntegerArgs e)
	{
		mGeneratorUIPanel.SetKey(e.mInteger);
	}

	public void OnNormalMeasureExited(object source, EventArgs e)
	{
		mStaffPlayer.ClearNotes ();
	}

	public void OnSetBarlineColor(object source, MusicGenerator.BarlineArgs e)
	{
		mStaffPlayer.SetBarlineColor (e.mSteps, e.mIsRepeating);
	}

	public bool OnUIPlayerIsEditing(object source, EventArgs args)
	{
		
		if(mMusicGenerator.mState >= eGeneratorState.editorPlaying)
			return true;
		return false;
	}
	
	public void OnRepeatNotePlayed(object source, RepeatNoteArgs e)
	{
		int unplayed = -1;
		List<Instrument> instruments = e.instrumentSet.mInstruments;
		Instrument instrument = instruments[e.indexA];
		int note = instrument.mClipNotes[mMeasureEditor.mCurrentMeasure.value][e.repeatingCount][e.indexB];
		if(instrument.mClipNotes [mMeasureEditor.mCurrentMeasure.value][e.repeatingCount] [e.indexB] != unplayed)
		{
			mMusicGenerator.PlayAudioClip (e.instrumentSet, mMusicGenerator.GetClips() [(int)instrument.mInstrumentTypeIndex][e.instrumentSubIndex] [note],
			instrument.mVolume, e.indexA);
			mStaffPlayer.DisplayNote(note,
				(int)instrument.mStaffPlayerColor, false, e.instrumentSet);
		}
	}

	public void OnUIStaffNotePlayed(object source, UIStaffNoteArgs e)
	{
		mStaffPlayer.DisplayNote (e.instIndex, e.color, false, MusicGenerator.Instance.mInstrumentSet);
	}

	public void OnUIStaffNoteStrummed(object source, UIStaffNoteArgs e)
	{
		mStaffPlayer.DisplayNote (e.instIndex, e.color, false, MusicGenerator.Instance.mInstrumentSet, true);
	}

	public void OnClipLoaded(object source, ClipLoadedArgs e)
	{
		MeasureEditor.Instance.mKey.value = e.mKey;
		MeasureEditor.Instance.mScale.value = e.mScale;
		MeasureEditor.Instance.mMode.value = e.mMode;
		MeasureEditor.Instance.mTempo.value = e.mTempo;
	}

	public void OnEditorClipPlayed(object source, EventArgs e)
	{
		MeasureEditor editor = MeasureEditor.Instance;
		editor.mLoadedClips[editor.mCurrentClip].mInstrumentSet.PlayEditorClip();
	}

	public void OnPlayerReset(object source, EventArgs e)
	{
		if(mMusicGenerator.mState >= eGeneratorState.editing)
		{
			MeasureEditor.Instance.mCurrentInstSet.Reset();
		}
	}
}
}