namespace ProcGenMusic
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.EventSystems;

	public enum eStaffPlayerColors { Red, Green, Blue, Yellow, Pink, Orange };

	public class HoverObj : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		public Vector2 index = new Vector2(0, 0);
		public bool isOver = false;
		public void OnPointerEnter(PointerEventData eventData)
		{
			isOver = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			isOver = false;
		}
	}

	public class StaffPlayerUI : HelperSingleton<StaffPlayerUI>
	{
		[SerializeField]
		private List<Transform> mNotes = new List<Transform>();
		[SerializeField]
		private List<RectTransform> mBarLines = new List<RectTransform>();
		[SerializeField]
		private RectTransform mBarlineFirst = null;
		[SerializeField]
		private RectTransform mBarlineLast = null;
		private List<HoverObj> mHoverObjects = new List<HoverObj>();
		[SerializeField]
		private List<Image> mBarLinesImages = new List<Image>();
		[SerializeField]
		private Color mBarSelected = Color.white;
		[SerializeField]
		private Color mBarRepeating = Color.white;

		private float mTotalBarlineDistance = 0.0f;
		[SerializeField]
		private Animator mLoadingSeqAnimator = null;

		private List<StaffPlayerNote> mPlayedNotes = new List<StaffPlayerNote>();
		private List<StaffPlayerNote> mShadowNotes = new List<StaffPlayerNote>();
		private List<StaffPlayerNote> mEditorNotes = new List<StaffPlayerNote>();
		private List<EditorNote> mPlacedEditorNotes = new List<EditorNote>();

		[SerializeField]
		public List<Color> mColors = new List<Color>();

		[SerializeField]
		private GameObject mBarlineObject = null;
		private MusicGenerator mMusicGenerator = null;
		private int mMaxPlayedNotes = 0;

		[SerializeField]
		private GameObject mBaseNoteImage = null;
		[SerializeField]
		private GameObject mBaseShadowImage = null;
		[SerializeField]
		private GameObject mBaseEditorNote = null;
		[SerializeField]
		private GameObject mBasePlayedEditorNote = null;

		private int mCurrentNote = 0;
		private int mCurrentPlacedEditorNote = 0;
		private int mCurrentEditorNote = 0;

		public InputField mExportFileName = null;

		public Dropdown mPreset = null;
		private MusicGeneratorUIPanel mGeneratorUIPanel = null;
		private MeasureEditor mMeasureEditor = null;
		private Image mTimerBar = null;
		private InstrumentPanelUI mInstrumentPanel = null;
		private Dropdown mTimeSignatureDropdown = null;

		[SerializeField]
		private Text mCurrentProgStep = null;

		public override void Awake()
		{
			base.Awake();
			Component[] components2 = this.GetComponentsInChildren(typeof(Transform), true);
			foreach (Component cp in components2)
			{
				if (cp.name == "TimerBar")
					mTimerBar = cp.gameObject.GetComponentInChildren<Image>();
				if (cp.name == "Presets")
					mPreset = cp.gameObject.GetComponentInChildren<Dropdown>();
				if (cp.name == "Export")
				{
					mExportFileName = cp.gameObject.GetComponentInChildren<InputField>();
				}
				if (cp.name == "LoadingSequence")
					mLoadingSeqAnimator = cp.gameObject.GetComponent<Animator>();
				if (cp.name == "TimeSignature")
					mTimeSignatureDropdown = cp.gameObject.GetComponentInChildren<Dropdown>();
			}

			for (int i = 0; i < mBarLines.Count; i++)
				mBarLinesImages.Add(mBarLines[i].GetComponentInChildren<Image>());

			for (int i = 0; i < mNotes.Count; i++)
			{
				mNotes[i].gameObject.AddComponent<HoverObj>();
				mHoverObjects.Add(mNotes[i].GetComponent<HoverObj>());
			}
			mTotalBarlineDistance = mBarlineLast.localPosition.x - mBarlineFirst.localPosition.x;
		}

		/// Changes our time signature.
		public void ChangeTimeSignature(int timeSignature = -1)
		{
			if(timeSignature != -1)//if we're actually trying to force the ui to change. The UI will pass in -1.
			{
				mTimeSignatureDropdown.value = timeSignature;
				return;
			}
			eTimeSignature signature = (eTimeSignature)mTimeSignatureDropdown.value;
				
			InstrumentSet set = (mMusicGenerator.mState >= eGeneratorState.editorInitializing) ? mMeasureEditor.mCurrentInstSet : mMusicGenerator.mInstrumentSet;
			set.mTimeSignature.SetTimeSignature(signature);

			float xPos = mBarLines[0].localPosition.x;
			float nextPos = mTotalBarlineDistance / (set.mTimeSignature.mStepsPerMeasure - 1);
			for (int i = 0; i < mBarLinesImages.Count; i++)
			{
				if (i < set.mTimeSignature.mStepsPerMeasure)
				{
					mBarLinesImages[i].enabled = true;
					Vector3 pos = mBarLines[i].localPosition;
					mBarLines[i].localPosition = new Vector3(xPos, pos.y, pos.z);
					xPos += nextPos;
				}
				else
				{
					mBarLinesImages[i].enabled = false;
				}
			}
		}

		public void Init(MusicGenerator managerIN)
		{
			mMusicGenerator = managerIN;
			UIManager uimanager = UIManager.Instance;
			mGeneratorUIPanel = uimanager.mGeneratorUIPanel;
			mMeasureEditor = uimanager.mMeasureEditor;
			mInstrumentPanel = uimanager.mInstrumentPanelUI;
			mMaxPlayedNotes = MusicGenerator.mMaxInstruments * 4 * 16;//number of instruments, times size of chord * number of steps per measure 

			//TODO, set these up to just draw/not draw. Kinda silly to reposition them
			for (int i = 0; i < mMaxPlayedNotes; i++)//sorry for magic number. In theory, the max number of notes that might play, given maxInstruments.
			{
				mPlayedNotes.Add((Instantiate(mBaseNoteImage, this.transform) as GameObject).GetComponent<StaffPlayerNote>());
				mPlayedNotes[i].transform.position = new Vector3(-10000, -10000, 0);
				mPlayedNotes[i].gameObject.SetActive(false);

				mShadowNotes.Add((Instantiate(mBaseShadowImage, this.transform) as GameObject).GetComponent<StaffPlayerNote>());
				mShadowNotes[i].transform.position = new Vector3(-10000, -10000, 0);
				mShadowNotes[i].gameObject.SetActive(false);

				mEditorNotes.Add((Instantiate(mBaseEditorNote, this.transform) as GameObject).GetComponent<StaffPlayerNote>());
				mEditorNotes[i].transform.position = new Vector3(-10000, -10000, 0);
				mEditorNotes[i].gameObject.SetActive(false);

				mPlacedEditorNotes.Add((Instantiate(mBasePlayedEditorNote, this.transform) as GameObject).GetComponent<EditorNote>());
				mPlacedEditorNotes[i].transform.position = new Vector3(-10000, -10000, 0);
				mPlacedEditorNotes[i].Init(this);
				mPlacedEditorNotes[i].gameObject.SetActive(false);
			}
			mTimeSignatureDropdown.value = (int)mMusicGenerator.mInstrumentSet.mTimeSignature.mSignature;
			//ChangeTimeSignature(eTimeSignature.ThreeFour);
		}

		void Update()
		{
			if (mMusicGenerator == null)
				throw new System.ArgumentNullException("music generator does not exist. Please ensure a game object with this class exists in your scene hierarchy.");

			if (mMusicGenerator.mState == eGeneratorState.editorInitializing)
				return;

			mCurrentProgStep.text = "";
			InstrumentSet set = (mMusicGenerator.mState >= eGeneratorState.editorInitializing) ? mMeasureEditor.mCurrentInstSet : mMusicGenerator.mInstrumentSet;

			List<int> prog = mMusicGenerator.mChordProgression;
			int stepsTaken = set.mProgressionStepsTaken >= 0 ? set.mProgressionStepsTaken : 0;
			int currentStep = prog[stepsTaken];
			mCurrentProgStep.text = "{" + prog[0] + "-" + prog[1] + "-" + prog[2] + "-" + prog[3] + "}" + " :" +
			currentStep;


			float sixteenthStepTimer = set.mSixteenthStepTimer;
			int sixteenStepsTaken = set.mSixteenthStepsTaken - 1;
			float sixteenthMeasure = set.mBeatLength;

			float dist = mBarLines[1].position.x - mBarLines[0].position.x;
			float perc = (sixteenthStepTimer / sixteenthMeasure);

			if (sixteenStepsTaken < 0)
			{
				mTimerBar.gameObject.transform.position = new Vector3(mBarLines[0].position.x,
					mTimerBar.transform.position.y, mTimerBar.transform.position.z);
			}
			else
			{
				mTimerBar.gameObject.transform.position = new Vector3(mBarLines[sixteenStepsTaken].position.x +
					dist * (1 - perc), mTimerBar.transform.position.y, mTimerBar.transform.position.z);
			}
		}

		/// Displays a note on the staff player:
		public void DisplayNote(int noteIN, int colorIN, bool useShadow, InstrumentSet setIN, bool strummed = false)
		{
			int sixteenStepsTaken = setIN.mSixteenthStepsTaken;

			if (!useShadow)
			{
				mPlayedNotes[mCurrentNote].gameObject.SetActive(true);
				if (!strummed)
					mPlayedNotes[mCurrentNote].transform.position = new Vector3(mBarLines[sixteenStepsTaken].position.x, mNotes[noteIN].transform.position.y, 0);
				else
					mPlayedNotes[mCurrentNote].transform.position = new Vector3(mTimerBar.transform.position.x, mNotes[noteIN].transform.position.y, 0);
				mPlayedNotes[mCurrentNote].mBaseImage.color = mColors[colorIN];
			}
			else
			{
				mShadowNotes[mCurrentNote].gameObject.SetActive(true);
				if (!strummed)
					mShadowNotes[mCurrentNote].transform.position = new Vector2(mBarLines[sixteenStepsTaken].position.x, mNotes[noteIN].transform.position.y);
				else
					mShadowNotes[mCurrentNote].transform.position = new Vector2(mTimerBar.transform.position.x, mNotes[noteIN].transform.position.y);

				Component[] components2 = mShadowNotes[mCurrentNote].GetComponentsInChildren<Image>();
				foreach (Component cp in components2)
				{
					if (cp.name != "shadow")
						cp.GetComponent<Image>().color = mColors[colorIN];
				}
			}
			mCurrentNote += 1;
		}

		public void PlayLoadingSequence(bool isLoading)
		{
			mLoadingSeqAnimator.SetBool("isLoading", isLoading);
		}

		/// removes all notes from the staff player:
		public void ClearNotes(bool clearSetNotes = false, bool clearHighlightedNotes = false)
		{
			for (int i = 0; i < mPlayedNotes.Count; i++)
			{
				mPlayedNotes[i].transform.position = new Vector2(-10000, -10000);
				mPlayedNotes[i].gameObject.SetActive(false);
			}
			for (int i = 0; i < mShadowNotes.Count; i++)
			{
				mShadowNotes[i].transform.position = new Vector2(-10000, -10000);
				mShadowNotes[i].gameObject.SetActive(false);
			}

			if (clearSetNotes)
			{

				for (int i = 0; i < mPlacedEditorNotes.Count; i++)
				{
					mPlacedEditorNotes[i].transform.position = new Vector2(-10000, -10000);
					mPlacedEditorNotes[i].gameObject.SetActive(false);
				}
			}
			if (clearHighlightedNotes)
			{
				for (int i = 0; i < mEditorNotes.Count; i++)
				{
					mEditorNotes[i].transform.position = new Vector2(-10000, -10000);
					mEditorNotes[i].GetComponentInChildren<Text>().text = "";
					mEditorNotes[i].gameObject.SetActive(false);
				}
			}


			mCurrentNote = 0;
			mCurrentPlacedEditorNote = 0;
		}

		/// called on export button press. Exports all config files for this music generator configuration.
		public void ExportFile()
		{
			if (mMusicGenerator.mState < eGeneratorState.editing)
			{
				if (mExportFileName.textComponent.text == "")
					return;
				Debug.Log("exporting configuration " + mExportFileName.textComponent.text);
				mGeneratorUIPanel.SaveConfiguration(mExportFileName.textComponent.text);
				if (!mGeneratorUIPanel.mPresetFileNames.Contains(mExportFileName.textComponent.text))
					mGeneratorUIPanel.mPresetFileNames.Add(mExportFileName.textComponent.text);
			}
			else
				mMeasureEditor.SaveClip(mExportFileName.textComponent.text);
		}

		/// Adds this preset to our options. This is called on a delay waiting for the exported file to finish writing.
		public void AddPresetOption(string fileNameIN)
		{
			bool fileExists = false;
			for (int i = 0; i < mPreset.options.Count; i++)
			{
				if (mPreset.options[i].text == fileNameIN)
					fileExists = true;
			}
			if (!fileExists)
			{
				Dropdown.OptionData newOption = new Dropdown.OptionData();
				newOption.text = (fileNameIN == "AAADefault") ? "Default" : fileNameIN;
				mPreset.options.Add(newOption);
			}
		}

		/// Sets the color for the bar line counter.
		public void SetBarlineColor(int lineIN, bool isRepeating)
		{
			if (lineIN == 0 || lineIN == -1)
			{
				for (int i = 0; i < mBarLinesImages.Count; i++)
					mBarLinesImages[i].color = Color.white;
			}

			if (lineIN != -1)
			{
				mBarLinesImages[lineIN].color = isRepeating ? mBarRepeating : mBarSelected;
			}
		}

		/// Shows highlighted helper notes for the measure editor.
		public void ShowHighlightedNotes(Instrument instrumentIN)
		{
			if (instrumentIN.mSuccessionType == eSuccessionType.lead)
				ShowLeadNotes(instrumentIN);
			else
				ShowRhythmNotes(instrumentIN);
		}

		/// Shows highlights for lead notes when in the measure editor.
		public void ShowLeadNotes(Instrument instrumentIN)
		{
			mCurrentEditorNote = 0;

			for (int i = 0; i < mMeasureEditor.mCurrentInstSet.mTimeSignature.mStepsPerMeasure; i++)
			{
				if (i % 4 != 0)
					ShowLeadNote(instrumentIN, i);
				else
					ShowRhythmNote(instrumentIN, i);
			}
		}

		/// Shows a single lead note in the editor.
		public void ShowLeadNote(Instrument instrumentIN, int step)
		{
			int totalScaleNotes = Instrument.mMajorScale.Count * 3;
			int totalNotes = MusicGenerator.mMaxInstrumentNotes;
			int scaleLength = Instrument.mMusicScales[mMeasureEditor.mScale.value].Count;
			List<int> scale = Instrument.mMusicScales[mMeasureEditor.mScale.value];
			TimeSignature signature = mMeasureEditor.mCurrentInstSet.mTimeSignature;

			if (step % (signature.mTimestepNumInverse[(int)instrumentIN.mTimeStep]) == 0)
			{
				int invProgRate = mMusicGenerator.mInstrumentSet.GetInverseProgressionRate(mMeasureEditor.mProgressionRate.value);
				int chordStep = mMusicGenerator.mChordProgression[step / invProgRate];

				for (int j = 0; j < totalScaleNotes; j++)
				{
					mEditorNotes[mCurrentEditorNote].gameObject.SetActive(true);

					int index = mMeasureEditor.mKey.value;
					for (int x = 0; x < j; x++)
					{
						int subindex = (x + mMeasureEditor.mMode.value) % scaleLength;
						index += scale[subindex];
					}
					index = index % totalNotes;
					Vector2 position = new Vector2(mBarLines[step].position.x, mNotes[index].transform.position.y);
					mEditorNotes[mCurrentEditorNote].transform.position = position;

					mEditorNotes[mCurrentEditorNote].mBaseImage.color = new Color(
							mColors[(int)instrumentIN.mStaffPlayerColor].r,
							mColors[(int)instrumentIN.mStaffPlayerColor].g,
							mColors[(int)instrumentIN.mStaffPlayerColor].b, 0.4f
						);

					mCurrentEditorNote += 1;
				}
			}
		}

		//shows the possible notes for a rhythm instrument:
		private void ShowRhythmNotes(Instrument instrumentIN)
		{
			TimeSignature signature = mMeasureEditor.mCurrentInstSet.mTimeSignature;
			mCurrentEditorNote = 0;
			for (int i = 0; i < signature.mStepsPerMeasure; i++)
			{
				if (i % (signature.mTimestepNumInverse[(int)instrumentIN.mTimeStep]) == 0)
				{
					ShowRhythmNote(instrumentIN, i);
				}
			}
		}

		/// Shows a single rhythm note for the measure editor.
		public void ShowRhythmNote(Instrument instrumentIN, int index)
		{
			for (int j = 0; j < Instrument.mMajorScale.Count * 3; j++)
			{
				if (j % 7 == 0 || j % 7 == 2 || j % 7 == 4 || j % 7 == 6)
				{
					SetSingleEditorNote(index, j, instrumentIN, mEditorNotes, mCurrentEditorNote);
					Color tempColor;
					if (j % 7 == 6)
					{
						tempColor = new Color(
							Color.black.r,
							Color.black.g,
							Color.black.b, 0.4f
						);
					}
					else
					{
						tempColor = new Color(
							mColors[(int)instrumentIN.mStaffPlayerColor].r,
							mColors[(int)instrumentIN.mStaffPlayerColor].g,
							mColors[(int)instrumentIN.mStaffPlayerColor].b, 0.4f
						);
					}
					mEditorNotes[mCurrentEditorNote].gameObject.SetActive(true);
					mEditorNotes[mCurrentEditorNote].mBaseImage.color = tempColor;
					if (j % 7 == 6)
						mEditorNotes[mCurrentEditorNote].gameObject.GetComponentInChildren<Text>().text = "7";
					else if (j % 7 == 0)
						mEditorNotes[mCurrentEditorNote].gameObject.GetComponentInChildren<Text>().text = "R";
					mCurrentEditorNote += 1;
				}
			}
		}

		/// sets the selected notes for this instrument this measure.
		public void SetMeasure(Instrument instrumentIN)
		{
			ClearNotes(true, true);
			mCurrentPlacedEditorNote = 0;
			List<List<int>> clips = instrumentIN.mClipNotes[mMeasureEditor.mCurrentMeasure.value];
			for (int i = 0; i < clips.Count; i++)
			{
				for (int j = 0; j < clips[i].Count; j++)
				{
					if (clips[i][j] != -1)
					{
						Vector2 pos = new Vector2(0, 0);
						pos.x = mBarLines[i].position.x;
						pos.y = mNotes[clips[i][j]].transform.position.y;
						mPlacedEditorNotes[mCurrentPlacedEditorNote].gameObject.SetActive(true);
						mPlacedEditorNotes[mCurrentPlacedEditorNote].transform.position = pos;
						mPlacedEditorNotes[mCurrentPlacedEditorNote].mBaseImage.color =
							mColors[(int)instrumentIN.mStaffPlayerColor];
						mCurrentPlacedEditorNote += 1;
					}
				}
			}
		}

		/// Sets a single Editor note in the staff player.
		public void SetSingleEditorNote(int timestep, int note, Instrument instrumentIN, List<StaffPlayerNote> editorNotesIN, int currentNote)
		{
			int index = mMeasureEditor.mKey.value;
			int scale = mMeasureEditor.mScale.value;

			int progressionRate = mMeasureEditor.mCurrentInstSet.GetProgressionRate(mMeasureEditor.mProgressionRate.value);
			int chordStep = (int)(timestep / progressionRate) % 4;

			int stepToTake = mMusicGenerator.mChordProgression[chordStep];
			for (int x = 0; x < note + stepToTake; x++)
			{
				int subindex = (x + (int)mMeasureEditor.mMode.value) % Instrument.mMusicScales[scale].Count;
				subindex = subindex % Instrument.mMusicScales[scale].Count;
				index += Instrument.mMusicScales[scale][subindex];
			}
			index += (Instrument.mOctave * (int)(note / Instrument.mMusicScales[scale].Count));
			index = index % MusicGenerator.mMaxInstrumentNotes;
			editorNotesIN[currentNote].gameObject.SetActive(true);
			editorNotesIN[currentNote].transform.position = new Vector2(mBarLines[timestep].position.x, mNotes[index].transform.position.y);
		}

		/// Shows the set editor notes:
		public void ShowSetEditorNotes(List<List<int>> notesIN, int instrumentIndex)
		{
			List<Instrument> instruments = new List<Instrument>();
			if (mMusicGenerator.mState >= eGeneratorState.editorInitializing)
				instruments = MeasureEditor.Instance.mLoadedClips[MeasureEditor.Instance.mCurrentClip].mInstrumentSet.mInstruments;
			else
				instruments = mMusicGenerator.mInstrumentSet.mInstruments;

			for (int i = 0; i < mMeasureEditor.mCurrentInstSet.mTimeSignature.mStepsPerMeasure; i++)
			{
				for (int j = 0; j < notesIN[i].Count; j++)
				{
					if (notesIN[i][j] != -1)
					{
						Vector2 pos = new Vector2(0, 0);
						pos.x = mBarLines[i].position.x;
						pos.y = mNotes[notesIN[i][j]].transform.position.y;
						mPlacedEditorNotes[mCurrentPlacedEditorNote].gameObject.SetActive(true);
						mPlacedEditorNotes[mCurrentPlacedEditorNote].index = new Vector2(i, notesIN[i][j]);
						mPlacedEditorNotes[mCurrentPlacedEditorNote].transform.position = pos;
						mPlacedEditorNotes[mCurrentPlacedEditorNote].mBaseImage.color =
							mColors[(int)instruments[instrumentIndex].mStaffPlayerColor];
						mCurrentPlacedEditorNote += 1;
					}
				}
			}
		}

		public void RemoveNote(EditorNote note)
		{
			Instrument instrument = mInstrumentPanel.mInstrument;
			if (instrument.RemoveClipNote((int)note.index.x, (int)note.index.y, mMeasureEditor.mCurrentMeasure.value))
			{
				note.transform.position = new Vector2(-10000, -10000);
				note.gameObject.SetActive(false);
				mMeasureEditor.UIToggleAllInstruments(true);
			}
		}
		/// if left/right mouse click are pressed, adds or removes an editor note, respectively.
		public void SetEditorNotes(Instrument instrumentIN)
		{
			if (Input.GetKeyDown("mouse 0"))
			{
				int note = 0;
				int timestep = 0;

				Vector2 pos = new Vector2(0, 0);
				bool isOver = false;
				for (int i = 0; i < mHoverObjects.Count; i++)
				{
					if (mHoverObjects[i].isOver)
					{
						pos.y = mNotes[i].transform.position.y;
						note = i;
						isOver = true;
					}
				}

				if (!isOver)
					return;

				float nearest = 100000;
				for (int j = 0; j < mMeasureEditor.mCurrentInstSet.mTimeSignature.mStepsPerMeasure; j++)
				{
					Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, mBarLines[j].position);
					float dist = Vector2.Distance(Input.mousePosition, screenPos);
					if (dist < nearest)
					{
						nearest = dist;
						pos.x = mBarLines[j].position.x;
						timestep = j;
					}
				}

				if (instrumentIN.AddClipNote(timestep, note, mMeasureEditor.mCurrentMeasure.value))
				{
					mPlacedEditorNotes[mCurrentPlacedEditorNote].transform.position = pos;
					mPlacedEditorNotes[mCurrentPlacedEditorNote].mBaseImage.color = mColors[(int)instrumentIN.mStaffPlayerColor];
					mPlacedEditorNotes[mCurrentPlacedEditorNote].index = new Vector2(timestep, note);
					mCurrentPlacedEditorNote += 1;
					mMeasureEditor.UIToggleAllInstruments();
				}
			}
		}
	}
}