namespace ProcGenMusic
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using System;

	public enum eTimeSignature { FourFour, ThreeFour, FiveFour };
	[Serializable]
	public class TimeSignature
	{
		public int mStepsPerMeasure { get; private set; }                                                //< number of steps per measure
		public List<int> mTimestepNum { get; private set; }// = new List<int> { 16, 8, 4, 2, 1 };       //< notes per timestep
		public List<int> mTimestepNumInverse { get; private set; }// = new List<int> { 1, 2, 4, 8, 16 };//< inverted notes per timestep
		public int mSixteenth = 16;    //< sixteenth note;
		public int mEighth = 8;        //< eighth note;
		public int mQuarter = 4;       //< quarter note;
		public int mHalf = 2;          //< half note;
		public int mWhole = 0;         //< whole note;
		public eTimeSignature mSignature = eTimeSignature.FourFour; ///our currently set time signature.

		public void Init()
		{
			mStepsPerMeasure = 16;
			mTimestepNum = new List<int> { 16, 8, 4, 2, 1 };
			mTimestepNumInverse = new List<int> { 1, 2, 4, 8, 16 };
		}

		/// Sets our time signature and adjusts values.
		public void SetTimeSignature(eTimeSignature signature)
		{
			mSignature = signature;
			switch (mSignature)
			{
				case eTimeSignature.FourFour:
					{
						mStepsPerMeasure = 16;
						mTimestepNum = new List<int> { 16, 8, 4, 2, 1 };
						mTimestepNumInverse = new List<int> { 1, 2, 4, 8, 16 };
						mSixteenth = 16;
						mEighth = 8;
						mQuarter = 4;
						mHalf = 2;
						mWhole = 0;
						break;
					}
				case eTimeSignature.ThreeFour:
					{
						mStepsPerMeasure = 12;
						mTimestepNum = new List<int> { 12, 6, 3, 3, 1 };
						mTimestepNumInverse = new List<int> { 1, 3, 3, 6, 12 };
						mSixteenth = 12;
						mEighth = 6;
						mQuarter = 3;
						mHalf = 3;
						mWhole = 0;
						break;
					}
				case eTimeSignature.FiveFour:
					{
						mStepsPerMeasure = 20;
						mTimestepNum = new List<int> { 20, 10, 5, 5, 1 };
						mTimestepNumInverse = new List<int> { 1, 5, 5, 10, 20 };
						mSixteenth = 20;
						mEighth = 10;
						mQuarter = 5;
						mHalf = 5;
						mWhole = 0;
						break;
					}
			}
			MusicGenerator.Instance.ResetPlayer();
		}
	}

	/// The set of instruments for a configuration. Handles the timing, playing, repeating and other settings for its instruments.
	/// For normal uses of the generator, you should not need to call any of the public functions in here, as they're handled by the
	/// MusicGenerator or the SingleClip logic.
	public class InstrumentSet : MonoBehaviour
	{
		static readonly private float mMinTempo = 1.0f;     //< anything less than this isn't a functional tempo. Edit at your own risk.
		static readonly private float mMaxTempo = 350.0f;   //< anything greater than this is likely to cause problems. Edit at your own risk :)
		static readonly private int mMaxFullstepsTaken = 4; //< max number of steps per progression. Currently only support 4

		/// Timers and various time step settings:
		private int mSixteenthRepeatCount = 0;              //< number of sixteenth notes we've played
		private int mEighthRepeatCount = 0;                 //< number of eighth notes we've played
		private int mQuarterRepeatCount = 0;                //< number of quarter notes we've played
		private int mHalfRepeatCount = 0;                   //< number of half notes we've played
		private int mFullRepeatCount = 0;                   //< number of whole notes we've played

		public int mSixteenthStepsTaken { get; private set; }   //< number of 1/16 steps taken for current measure
		public float mSixteenthStepTimer { get; private set; }  //< timer for single steps
		public float mBeatLength { get; private set; }          //< length of measures. Used for timing.set on start

		public TimeSignature mTimeSignature = new TimeSignature();             //< our time signature.

		[SerializeField]
		public int mRepeatCount { get; private set; }           //< how many times we've repeated the measure.
		public void ResetRepeatCount() { mRepeatCount = 0; }    //< resets the repeat count;

		public float mMeasureStartTimer { get; private set; }   //< delay to balance out when we start a new measure

		public int mProgressionStepsTaken { get; private set; } //< how many steps in the chord progression have been taken
		public void ResetProgressionSteps() { mProgressionStepsTaken = -1; }//< resets the progression steps taken.

		public int mProgressionRate { get; private set; }       //< how quickly we step through our chord progression

		public float mTempo { get; private set; }               //< current tempo
		public void SetTempo(float valueIN) { mTempo = valueIN >= mMinTempo && valueIN <= mMaxTempo ? valueIN : mTempo; }

		public int mRepeatMeasuresNum { get; private set; }        //< number measure we'll repeat, if we're repeating measures :P
		public void SetRepeatMeasuresNum(int value) { mRepeatMeasuresNum = value >= 1 && value <= 4 ? value : 1; }  //< setter for repeat Measures Num.

		public List<Instrument> mInstruments { get; private set; } //< list of our current instruments
		private MusicGenerator mMusicGenerator = null;             //< reference to MusicGenerator
		private int mUnplayed = -1;                                //< unplayed note
		public int mCurrentGroupLevel = 0;                         //< if using linear dynamic style, this is our current level of groups that are playing.

		void Awake()
		{
			mSixteenthStepsTaken = 0;
			mSixteenthStepTimer = 0;
			mBeatLength = 0;
			mMeasureStartTimer = 0;
			mRepeatCount = 0;
			mRepeatCount = 1;
			mProgressionStepsTaken = -1;
			mProgressionRate = 8;
			mInstruments = new List<Instrument>();
			mRepeatMeasuresNum = 1;
			mTempo = 100.0f;
			UpdateTempo();
			mTimeSignature.Init();
			mTimeSignature.SetTimeSignature(eTimeSignature.FourFour);
		}

		//Initializes music set.
		public void Init()
		{
			mMusicGenerator = MusicGenerator.Instance;
		}

		/// Resets the instrument set values:
		public void Reset()
		{
			if (mMusicGenerator == null)
				return;

			mRepeatCount = 0;
			mProgressionStepsTaken = -1;
			ExitNormalMeasure();
			ExitRepeatingMeasure(true);

			if (mMusicGenerator.mState == eGeneratorState.repeating)
				mMusicGenerator.SetState(eGeneratorState.playing);

			for (int i = 0; i < mInstruments.Count; i++)
			{
				mInstruments[i].mThemeNotes.Clear();
				mInstruments[i].mPlayedMelodicNotes.Clear();
				mInstruments[i].ClearPatternNotes();
			}
		}

		/// Plays the measure. a measure = 16 sixteeth steps
		public void PlayMeasure(Action CheckKeyChange = null, Action SetThemeRepeat = null, Action GenerateNewProgression = null)
		{
			if (mMusicGenerator == null)
				return;

			UpdateTempo();
			mSixteenthStepTimer -= Time.deltaTime;
			if (mSixteenthStepTimer <= 0 && mSixteenthStepsTaken < mTimeSignature.mStepsPerMeasure)
			{
				mMusicGenerator.OnBarlineColorSet(new MusicGenerator.BarlineArgs(mSixteenthStepsTaken, false));

				if (mSixteenthStepsTaken % mProgressionRate == mTimeSignature.mWhole)
				{
					mProgressionStepsTaken += 1;
					mProgressionStepsTaken = mProgressionStepsTaken % mMusicGenerator.mChordProgression.Count;
					if (CheckKeyChange != null)
						CheckKeyChange();
				}
				if (mSixteenthStepsTaken % mTimeSignature.mHalf == 0)
					TakeStep(eTimestep.eighth, mProgressionStepsTaken);
				if (mSixteenthStepsTaken % mTimeSignature.mQuarter == 0)
					TakeStep(eTimestep.quarter, mProgressionStepsTaken);
				if (mSixteenthStepsTaken % mTimeSignature.mEighth == 0)
					TakeStep(eTimestep.half, mProgressionStepsTaken);
				if (mSixteenthStepsTaken % mTimeSignature.mSixteenth == 0)
				{
					TakeStep(eTimestep.whole, mProgressionStepsTaken);
					mMeasureStartTimer = 0.0f;
				}

				TakeStep(eTimestep.sixteenth, mProgressionStepsTaken);

				mSixteenthStepTimer = mBeatLength;
				mSixteenthStepsTaken += 1;
			}
			else if (mSixteenthStepsTaken == mTimeSignature.mStepsPerMeasure)
			{
				mMeasureStartTimer += Time.deltaTime;

				if (mMeasureStartTimer > mBeatLength)//We don't actually want to reset until the next beat.
				{
					GenerateNewProgression();
					ExitNormalMeasure(SetThemeRepeat);
				}
			}
		}

		/// Exits a non-repeating measure, resetting values to be able to play the next:
		public void ExitNormalMeasure(Action SetThemeRepeat = null)
		{
			if (mMusicGenerator == null)
				throw new ArgumentNullException("music generator does not exist. Please ensure a game object with this class exists");

			mRepeatCount += 1;

			if (SetThemeRepeat != null)
				SetThemeRepeat();

			for (int i = 0; i < mInstruments.Count; i++)
			{
				mInstruments[i].ClearPatternNotes();
				mInstruments[i].ResetPatternStepsTaken();
				mInstruments[i].mPlayedMelodicNotes.Clear();
			}
			mSixteenthStepsTaken = 0;

			//select groups:
			SelectGroups();

			if (mProgressionStepsTaken >= mMaxFullstepsTaken - 1)
				mProgressionStepsTaken = -1;

			if (mMusicGenerator.mThemeRepeatOptions == eThemeRepeatOptions.eNone)
			{
				for (int i = 0; i < mInstruments.Count; i++)
					mInstruments[i].mRepeatingNotes.Clear();
			}

			mMeasureStartTimer = 0.0f;
			mSixteenthStepTimer = 0.0f;

			ResetMultipliers();
		}

		/// Repeats the measure.
		/// steps through, and plays the appropriate notes at the right timestep intervals:
		public void RepeatMeasure()
		{
			if (mMusicGenerator == null)
				return;

			UpdateTempo();

			mSixteenthStepTimer -= Time.deltaTime;
			if (mSixteenthStepTimer <= 0 && mSixteenthStepsTaken < mTimeSignature.mStepsPerMeasure)
			{
				mMusicGenerator.OnBarlineColorSet(new MusicGenerator.BarlineArgs(mSixteenthStepsTaken, true));

				if (mSixteenthStepsTaken % mTimeSignature.mHalf == 0)
				{
					TakeRepeatStep(eTimestep.eighth, mEighthRepeatCount);
					mEighthRepeatCount += 1;
				}
				if (mSixteenthStepsTaken % mTimeSignature.mQuarter == 0)
				{
					TakeRepeatStep(eTimestep.quarter, mQuarterRepeatCount);
					mQuarterRepeatCount += 1;
				}
				if (mSixteenthStepsTaken % mTimeSignature.mEighth == 0)
				{
					TakeRepeatStep(eTimestep.half, mHalfRepeatCount);
					mHalfRepeatCount += 1;
				}
				if (mSixteenthStepsTaken % mTimeSignature.mSixteenth == 0)
				{
					TakeRepeatStep(eTimestep.whole, mFullRepeatCount);
					mFullRepeatCount += 1;
					mMeasureStartTimer = 0.0f;
				}

				TakeRepeatStep((int)eTimestep.sixteenth, mSixteenthRepeatCount);
				mSixteenthRepeatCount += 1;
				mSixteenthStepTimer = mBeatLength;
				mSixteenthStepsTaken += 1;
			}
			else if (mSixteenthStepsTaken == mTimeSignature.mStepsPerMeasure)
			{
				mMeasureStartTimer += Time.deltaTime;
				if (mMeasureStartTimer > mBeatLength)
				{
					ExitRepeatingMeasure();

				}
			}
		}

		/// Plays a premade clip. There should be little need to 
		/// call this manually, the SingleClip class handles the playing of this.
		public void PlayEditorClip(bool isRepeating = true)
		{
			if (mMusicGenerator == null)
				return;
			UpdateTempo();
			mSixteenthStepTimer -= Time.deltaTime;
			if (mSixteenthStepTimer <= 0)
			{
				if (mSixteenthStepsTaken % mProgressionRate == 0)
					mProgressionStepsTaken += 1;
				if (mProgressionStepsTaken > mMaxFullstepsTaken - 1)
					mProgressionStepsTaken = -1;

				PlayClipNotes((int)eTimestep.sixteenth, mSixteenthRepeatCount);
				mSixteenthRepeatCount += 1;
				mSixteenthStepTimer = mBeatLength;

				mSixteenthStepsTaken += 1;

				if (mSixteenthStepsTaken == mTimeSignature.mStepsPerMeasure)
				{
					bool hardReset = false;
					ExitRepeatingMeasure(hardReset, isRepeating);
				}
			}
		}

		/// Exits the repeating measure. Resets values for next measure
		public void ExitRepeatingMeasure(bool hardReset = false, bool isRepeating = true)
		{
			mMusicGenerator.OnRepeatedMeasureExited(new MusicGeneratorStateArgs(mMusicGenerator.mState));

			mRepeatCount += 1;

			mMeasureStartTimer = 0.0f;
			mSixteenthStepTimer = 0.0f;
			ResetMultipliers();
			mSixteenthStepsTaken = 0;

			if (!isRepeating)
				return;

			//if we've repeated all the measures set to repeat in their entirety, reset the step counts.
			if (mRepeatCount >= mRepeatMeasuresNum * 2 || mMusicGenerator.OnUIPlayerIsEditing() || hardReset)
			{
				mRepeatCount = 0;
				mHalfRepeatCount = 0;
				mQuarterRepeatCount = 0;
				mSixteenthRepeatCount = 0;
				mEighthRepeatCount = 0;
				mFullRepeatCount = 0;
				for (int i = 0; i < mInstruments.Count; i++)
					mInstruments[i].mRepeatingNotes.Clear();

				if (mMusicGenerator.mState > eGeneratorState.stopped && mMusicGenerator.mState < eGeneratorState.editorInitializing)
					mMusicGenerator.SetState(eGeneratorState.playing);
			}
		}

		public int GetInverseProgressionRate(int valueIN)
		{
			valueIN = valueIN >= 0 && valueIN < mTimeSignature.mTimestepNumInverse.Count ? valueIN : 0;
			return mTimeSignature.mTimestepNumInverse[valueIN];
		}

		public int GetProgressionRate(int valueIN)
		{
			valueIN = valueIN >= 0 && valueIN < mTimeSignature.mTimestepNum.Count ? valueIN : 0;
			return mTimeSignature.mTimestepNum[valueIN];
		}

		public void SetProgressionRate(int valueIN)
		{
			valueIN = valueIN > 0 && valueIN <= mTimeSignature.mStepsPerMeasure ? valueIN : mTimeSignature.mStepsPerMeasure;
			mProgressionRate = valueIN;
		}

		////////////////////////////////////////////
		////////////////////////////////////////////
		/// Private utility functions
		////////////////////////////////////////////

		/// Updates the tempo.
		private void UpdateTempo()
		{
			// keep tempo within range
			mTempo = mTempo >= mMinTempo ? mTempo : mMinTempo;
			mTempo = mTempo <= mMaxTempo ? mTempo : mMaxTempo;

			int minute = 60;
			mBeatLength = minute / mTempo; //beats per minute
		}

		/// Plays a note within the parameters set for this instrument.
		private void TakeStep(eTimestep timeStepIN, int stepsTaken)
		{
			for (int instIndex = 0; instIndex < mInstruments.Count; instIndex++)
			{
				if (mInstruments[instIndex].mGroup >= mMusicGenerator.mGroupOdds.Count || mProgressionRate < 0)
					return;

				Instrument instrument = mInstruments[instIndex];
				bool groupIsPlaying = mMusicGenerator.mGroupIsPlaying[(int)instrument.mGroup];

				if (instrument.mTimeStep == timeStepIN && groupIsPlaying && !instrument.mIsMuted)
					PlayNotes(instrument, stepsTaken, instIndex);
			}
		}

		/// Plays the clip notes the instrument returns to it.
		private void PlayNotes(Instrument instrument, int stepsTaken, int instIndex)
		{
			/// we want to fill this whether we play it or not:
			int progressionStep = mMusicGenerator.mChordProgression[stepsTaken];
			List<int> clip = instrument.GetProgressionNotes(progressionStep);
			if (instrument.mStrumLength == 0.0f)
			{
				for (int j = 0; j < clip.Count; j++)
				{
					if (clip[j] != mUnplayed)//we ignore -1
					{
						int numSubInstruments = mMusicGenerator.GetClips()[(int)instrument.mInstrumentTypeIndex].Count;
						int instrumentSubIndex = UnityEngine.Random.Range(0, numSubInstruments);
						try
						{
							mMusicGenerator.PlayAudioClip(this, mMusicGenerator.GetClips()[(int)instrument.mInstrumentTypeIndex][instrumentSubIndex][clip[j]], instrument.mVolume, instIndex);
							mMusicGenerator.OnUIStaffNotePlayed(new UIStaffNoteArgs(clip[j], (int)instrument.mStaffPlayerColor));
						}
						catch (ArgumentOutOfRangeException e)
						{
							throw new ArgumentOutOfRangeException(e.Message);
						}
					}
				}
			}
			else
				StartCoroutine(StrumClip(clip, instIndex));
		}

		//staggers the playClip() call:
		private IEnumerator StrumClip(List<int> clipIN, int i)
		{
			clipIN.Sort();
			float variation = UnityEngine.Random.Range(0, mInstruments[i].mStrumVariation);
			for (int j = 0; j < clipIN.Count; j++)
			{
				if (clipIN[j] != mUnplayed)
				{
					int instrumentSubIndex = UnityEngine.Random.Range(0, mMusicGenerator.GetClips()[(int)mInstruments[i].mInstrumentTypeIndex].Count);
					mMusicGenerator.PlayAudioClip(this, mMusicGenerator.GetClips()[(int)mInstruments[i].mInstrumentTypeIndex][instrumentSubIndex][clipIN[j]], mInstruments[i].mVolume, i);
					mMusicGenerator.OnUIStaffNoteStrummed(new UIStaffNoteArgs(clipIN[j], (int)mInstruments[i].mStaffPlayerColor));
					yield return new WaitForSeconds(mInstruments[i].mStrumLength + variation);
				}
			}
		}

		/// Selects which instrument groups will play next measure.
		private void SelectGroups()
		{
			uint rate = mMusicGenerator.mGroupRate;
			if (rate == (uint)eGroupRate.eEndOfMeasure ||
				(rate == (uint)eGroupRate.eEndOfProgression && mProgressionStepsTaken >= mMaxFullstepsTaken - 1))
			{
				/// Either randomly choose which groups play or:
				if (mMusicGenerator.mDynamicStyle == eDynamicStyle.Random)
				{
					for (int i = 0; i < mMusicGenerator.mGroupIsPlaying.Count; i++)
						mMusicGenerator.mGroupIsPlaying[i] = (UnityEngine.Random.Range(0, 100.0f) < mMusicGenerator.mGroupOdds[i]);
				}
				else //we ascend / descend through our levels.
				{
					int ascend = 1;
					int descend = -1;
					int numGroup = mMusicGenerator.mGroupOdds.Count;

					int change = UnityEngine.Random.Range(0, 100) < 50 ? ascend : descend;
					int PotentialLevel = change + mCurrentGroupLevel;

					if (PotentialLevel < 0 || PotentialLevel >= mMusicGenerator.mGroupOdds.Count)
						PotentialLevel = mCurrentGroupLevel;

					//roll to see if we can change.
					if (change > 0 && UnityEngine.Random.Range(0, 100.0f) > mMusicGenerator.mGroupOdds[PotentialLevel])
						PotentialLevel = mCurrentGroupLevel;

					mCurrentGroupLevel = PotentialLevel;
					for (int i = 0; i < numGroup; i++)
						mMusicGenerator.mGroupIsPlaying[i] = i <= mCurrentGroupLevel;
				}
			}
		}

		/// Sets all multipliers back to their base.
		private void ResetMultipliers()
		{
			for (int i = 0; i < mInstruments.Count; i++)
				mInstruments[i].SetOddsOfPlayingMultiplier(Instrument.mOddsOfPlayingMultiplierBase);
		}

		/// repeats note from a measure:
		private void TakeRepeatStep(eTimestep timeStepIN, int repeatStep)
		{
			bool usingTheme = mMusicGenerator.mThemeRepeatOptions == eThemeRepeatOptions.eUseTheme;
			bool repeatingMeasure = mMusicGenerator.mThemeRepeatOptions == eThemeRepeatOptions.eRepeat;
			for (int instIndex = 0; instIndex < mInstruments.Count; instIndex++)
			{
				Instrument instrument = mInstruments[instIndex];
				int instType = (int)instrument.mInstrumentTypeIndex;

				if ((instrument.mTimeStep == timeStepIN || mMusicGenerator.OnUIPlayerIsEditing()) && !instrument.mIsMuted)
				{
					if (instType >= mMusicGenerator.GetClips().Count)
						throw new ArgumentOutOfRangeException("Single clip instrument has not been loaded into the generator");

					int instrumentSubIndex = UnityEngine.Random.Range(0, mMusicGenerator.GetClips()[instType].Count);
					if (mMusicGenerator.OnUIPlayerIsEditing())
					{
						for (int chordNote = 0; chordNote < instrument.mChordSize; chordNote++)
							mMusicGenerator.OnRepeatNotePlayed(new RepeatNoteArgs(instIndex, chordNote, repeatStep, instrumentSubIndex, this));
					}
					else if (usingTheme)
						PlayThemeNotes(instrument, repeatStep, instType, instrumentSubIndex, instIndex);
					else if (repeatingMeasure)
						PlayRepeatNotes(instrument, repeatStep, instIndex, instrumentSubIndex);
				}
			}
		}

		/// Plays the repeating notes for this timestep.
		private void PlayRepeatNotes(Instrument instrument, int repeatStep, int instIndex, int instSubIndex)
		{
			for (int chordNote = 0; chordNote < instrument.mChordSize; chordNote++)
			{
				if (instrument.mRepeatingNotes.Count > repeatStep && instrument.mRepeatingNotes[repeatStep][chordNote] != mUnplayed)
				{
					if (instrument.mStrumLength == 0.0f)
					{
						AudioClip clip = mMusicGenerator.GetClips()[(int)instrument.mInstrumentTypeIndex][instSubIndex][instrument.mRepeatingNotes[repeatStep][chordNote]];
						mMusicGenerator.PlayAudioClip(this, clip, instrument.mVolume, instIndex);
						mMusicGenerator.OnUIStaffNotePlayed(new UIStaffNoteArgs(instrument.mRepeatingNotes[repeatStep][chordNote], (int)instrument.mStaffPlayerColor));
					}
					else
					{
						List<int> clip = instrument.mThemeNotes[repeatStep];
						StartCoroutine(StrumClip(clip, instIndex));
						break;
					}
				}
			}
		}

		/// Plays the theme notes for this repeat step.
		private void PlayThemeNotes(Instrument instrument, int repeatCount, int instType, int instSubIndex, int instIndex)
		{
			for (int chordNote = 0; chordNote < instrument.mChordSize; chordNote++)
			{
				List<List<int>> notes = instrument.mThemeNotes;
				if (notes.Count > repeatCount &&
					notes[repeatCount].Count > chordNote &&
					notes[repeatCount][chordNote] != mUnplayed)
				{
					if (instrument.mStrumLength == 0.0f)
					{
						int note = notes[repeatCount][chordNote];
						AudioClip clip = mMusicGenerator.GetClips()[instType][instSubIndex][note];
						mMusicGenerator.PlayAudioClip(this, clip, instrument.mVolume, instIndex);
						mMusicGenerator.OnUIStaffNotePlayed(new UIStaffNoteArgs(note, (int)instrument.mStaffPlayerColor));
					}
					else
					{
						StartCoroutine(StrumClip(notes[mRepeatCount], instIndex));
						break;
					}
				}
			}
		}

		/// Plays the set clip notes.
		private void PlayClipNotes(eTimestep timeStepIN, int repeatingCount)
		{
			for (int i = 0; i < mInstruments.Count; i++)
			{
				if (!mInstruments[i].mIsMuted)
				{
					for (int j = 0; j < mInstruments[i].mChordSize; j++)
					{
						int note = mInstruments[i].mClipNotes[mRepeatCount][repeatingCount][j];
						int instrumentSubIndex = UnityEngine.Random.Range(0, mMusicGenerator.GetClips()[(int)mInstruments[i].mInstrumentTypeIndex].Count);
						if (note != mUnplayed)
						{
							/// set percussion to 0
							if (mInstruments[i].mInstrumentType.Contains("P_"))
								note = 0;

							mMusicGenerator.PlayAudioClip(this, mMusicGenerator.GetClips()[(int)mInstruments[i].mInstrumentTypeIndex][instrumentSubIndex][note],
							mInstruments[i].mVolume, i);
						}
					}
				}
			}
		}
	}
}