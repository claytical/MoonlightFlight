////////////////////////////////////////////
/// Instrument Class. Handles selecting notes to play and other instrument settings.
/// There are quite a few magic numbers here. It's semi-unavoidable, as often times we need
/// to adjust the fifth note, or the sixth note, etc. Music theory gets weird. Generally, if you
/// see a random looking integer, I'm just accessing a particular note in the scale for exceptions..
////////////////////////////////////////////
namespace ProcGenMusic
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using System;

	[System.Serializable]
	public class Instrument
	{
		static readonly public int mOctave = 12;                                        //< how many notes in an octave.
		static readonly public List<int> mSeventhChord = new List<int> { 0, 2, 4, 6 };  //< scale steps in a seventh chord
		static readonly private int mTriadCount = 3;                                    //< number of notes in a triad :P
		static readonly private int mDescendingInfluence = -1;                          //< for lead influence
		static readonly private int mAscendingInfluence = 1;                            //< for lead influence

		//TODO: enable different time steps. Currently just 4/4:
		static readonly private int mStepsPerMeasure = 20;		//< number of steps per measure
		static readonly private int mUnplayed = -1;				//< value for an 'unused' note 

		//steps between notes. Used in scales:
		static readonly private int mHalfStep = 1;
		static readonly private int mFullStep = 2;
		static readonly private int mFullPlusHalf = 3;
		//static readonly private int mDoubleStep = 4; //for exotic scales. Currently not implemented

		static readonly private int mTritoneStep = 5; //amount to adjust tritone chords/ notes.

		// our scales: TO NOTE: Melodic minor is both ascending and descending, which isn't super accurate for classical theory, generally. But, it was causing issue so
		// now just uses the scale in both ascend/descending melodies. It's on the wishlist, but is problematic for a few reasons.
		static readonly public List<int> mMelodicMinor = new List<int> { mFullStep, mHalfStep, mFullStep, mFullStep, mFullStep, mFullStep, mHalfStep };
		static readonly public List<int> mNaturalMinor = new List<int> { mFullStep, mHalfStep, mFullStep, mFullStep, mHalfStep, mFullStep, mFullStep };
		static readonly public List<int> mMajorScale = new List<int> { mFullStep, mFullStep, mHalfStep, mFullStep, mFullStep, mFullStep, mHalfStep };
		static readonly private List<int> mHarmonicMinor = new List<int> { mFullStep, mHalfStep, mFullStep, mFullStep, mHalfStep, mFullPlusHalf, mHalfStep };
		static readonly private List<int> mHarmonicMajor = new List<int> { mFullStep, mFullStep, mHalfStep, mFullStep, mHalfStep, mFullPlusHalf, mHalfStep };
		static readonly public List<List<int>> mMusicScales = new List<List<int>> { mMajorScale, mNaturalMinor, mMelodicMinor, mHarmonicMinor, mHarmonicMajor, mMelodicMinor };
		static readonly private int mScaleLength = 7;                          //< length of our scales. Currently all 7 notes.

		//Editable variables. See 'Setters' section below.
		public uint mChordSize { get; private set; }                           //< whether we use 7th chords or not. This should be 3 or 4. Other values will probably break things :P
		public uint mStaffPlayerColor { get; private set; }                    //< the color of the m staff player notes used by this instrument:
		public eSuccessionType mSuccessionType { get; private set; }           //< melody, rhythm or lead
		public uint mInstrumentTypeIndex { get; private set; }                 //< Type of instrument (violin, piano, etc)
		public string mInstrumentType { get; private set; }                    //< name of this instrument type
		public List<int> mOctavesToUse { get; private set; }                   //< which octaves will be used, keep within 0 through 2
		public float mStereoPan { get; private set; }                          //< how clips will pan in the audioSource -1 through 1
		public uint mOddsOfPlaying { get; private set; }                       //< odds this note will play, each timestep. 0-100
		private uint mPreviousOdds = 0;                                        //< saves odds when moved to chorus, for recall 0-100
		public float mVolume { get; private set; }                             //< instrument volume 0 -1;
		public float mAudioSourceVolume { get; private set; }                  //< volume of this instrument's audio source.
		public float mOctaveOdds { get; private set; }                         //< odds an octave note will be used: 0 through 100.
		static public float mOddsOfPlayingMultiplierBase { get; private set; } //< just a 1 :P
		public float mOddsOfPlayingMultiplierMax { get; private set; }         //< the higher end of the multiplier. Odds of playing are multiplied against this on a successful roll
		public float mOddsOfPlayingMultiplier { get; private set; }            //< the currently used multiplier
		public uint mGroup { get; private set; }                               //< which instrument group this instrument belongs to
		public bool mIsSolo { get; set; }                                      //< whether the instrument is solo:
		public bool mIsMuted { get; set; }                                     //< whether instrument is muted
		public eTimestep mTimeStep { get; private set; }                       //< currently used timestep
		public float mOddsOfUsingChord { get; private set; }                   //< ugh. This is unused, but a pain to remove :P sorry. don't use it though.
		public float mOddsOfUsingChordNotes { get; private set; }              //< odds each note of the chord will play
		public uint mInstrumentIndex { get; private set; }                     //< index of MusicGenerator.mInstruments()
		public float mStrumVariation { get; private set; }                     //< variation between different strums.
		public float mRedundancyAvoidance { get; private set; }                //< if we have a redundant melodic note, odds we pick another
		public float mStrumLength { get; private set; }                        //< delay between notes for strum effect.

		//effects values:
		public float mRoomSize { get; private set; }
		public float mReverb { get; private set; }
		public float mEcho { get; private set; }
		public float mEchoDelay { get; private set; }
		public float mEchoDecay { get; private set; }
		public float mFlanger { get; private set; }
		public float mDistortion { get; private set; }
		public float mChorus { get; private set; }

		//pattern variables:
		private List<List<int>> mPatternNoteOffset = new List<List<int>>();   //< list of our chord offsets to use in a pattern
		private List<List<int>> mPatternOctaveOffset = new List<List<int>>(); //< list of our octave offsets to use in a pattern
		public uint mUsePattern { get; private set; }                         //< whether we'll use a pattern for this instrument
		public uint mPatternlength { get; private set; }                      //< length of our pattern:
		public uint mPatternstepsTaken { get; private set; }                  //< which steps we're using.
		public uint mPatternRelease { get; private set; }                     //< at which point we stop using the pattern
		public float mRedundancyOdds { get; private set; }                    //< odds we ever play the same note twice. No UI controller for this. Set manually.

		public List<int> mPlayedMelodicNotes { get; private set; }            //< list of melodic notes we've already played (for determining ascend/descend)
		public uint mLeadMaxSteps { get; private set; }                       //< how many scale steps a melody can take at once
		public float AscendDescendInfluence { get; private set; }             //< likelihood of melody continuing to ascend/descend

		//Variables used by the music generator
		public List<List<int>> mRepeatingNotes { get; private set; }          //< list of notes played last measure
		public List<List<int>> mThemeNotes { get; private set; }              //< notes of saved theme measure
		public List<List<List<int>>> mClipNotes = new List<List<List<int>>>();//<measure; timestep ; note.  Info for playing a clip.

		//internal to this class:
		private MusicGenerator mMusicGenerator = null;
		private bool mbAreRepeatingPattern = false;               //< whether we're repeating the pattern this iteration
		private bool mbAreSettingPattern = false;                 //< whether we're setting the pattern this iteration
		private uint mCurrentPatternStep = 0;                     //< which index of the pattern we're playing this iteration
		private List<int> mCurrentPatternNotes = new List<int>(); //< list of current pattern notes for this step:
		private List<int> mCurrentPatternOctave = new List<int>();//< list of current pattern octave offsets for this step:
		private int mCurrentProgressionStep = 0;                  //< what our current step of the chord progression is.

		private int mLeadInfluence = mAscendingInfluence;         //< how much we tend to ascend/descend 
		private List<int> mProgressionNotes = new List<int>();    //< our progression notes to be played
		public string mName = "";                                 //< unused, but I imagine will be  useful for someone :P

		/// Instrument initialization
		public void Init(int index)
		{
			///how I wish unity's version of C# allowed initializing these with get; set; :P
			mChordSize = 3;
			mStaffPlayerColor = (int)eStaffPlayerColors.Red;
			mSuccessionType = eSuccessionType.melody;
			mInstrumentTypeIndex = 0;
			mInstrumentType = "ViolinShort";
			mOctavesToUse = new List<int> { 0, 1, 2 };
			mStereoPan = 0;
			mOddsOfPlaying = 50;
			mPreviousOdds = 100;
			mVolume = 0.5f;
			mAudioSourceVolume = 0.0f;
			mOctaveOdds = 20;
			mOddsOfPlayingMultiplierBase = 1.0f;
			mOddsOfPlayingMultiplierMax = 1.5f;
			mOddsOfPlayingMultiplier = 1.0f;
			mGroup = 0;
			mIsSolo = false;
			mIsMuted = false;
			mTimeStep = eTimestep.quarter;
			mOddsOfUsingChord = 50.0f;
			mOddsOfUsingChordNotes = 50.0f;
			mInstrumentIndex = 0;
			mStrumLength = 0.00f;
			mStrumVariation = 0.0f;
			mRedundancyAvoidance = 100.0f;
			mRoomSize = -10000.0f;
			mReverb = -2000.0f;
			mEcho = 0.0f;
			mEchoDelay = 0.0f;
			mEchoDecay = 0.0f;
			mFlanger = 0.0f;
			mDistortion = 0.0f;
			mChorus = 0.0f;
			mUsePattern = 0;
			mPatternlength = 4;
			mPatternstepsTaken = 0;
			mPatternRelease = 4;
			mRedundancyOdds = 50.0f;
			AscendDescendInfluence = 75.0f;
			mLeadMaxSteps = 3;

			mPlayedMelodicNotes = new List<int>();
			mRepeatingNotes = new List<List<int>>();
			mThemeNotes = new List<List<int>>();

			mInstrumentIndex = (uint)index;
			mMusicGenerator = MusicGenerator.Instance;
			LoadClipNotes();
			LoadPatternNotes();
		}

		/// Returns a list of ints coresponding to the notes to play
		public List<int> GetProgressionNotes(int progressionStep)
		{
			SetupNextNotes(progressionStep);

			bool isPercussion = mMusicGenerator.GetClips()[(int)mInstrumentTypeIndex][0].Count == 1;
			if (isPercussion)
				SelectPercussionNotes();
			else
				SelectNotes();

			if (mProgressionNotes.Count != mSeventhChord.Count)
			{
				Debug.Log("We haven't fully filled our note array. Something has gone wrong.");
				mProgressionNotes.Clear();
				FillEmptyPattern();
			}

			CheckForOutOfRangeNotes();
			mRepeatingNotes.Add(new List<int>(mProgressionNotes));

			SetMultiplier();

			return mProgressionNotes;
		}

		////////////////////////////////
		/// Internal note generation logic:
		////////////////////////////////

		/// Sets the pattern variables. Mostly for readability in other functions :\
		private void SetupNextNotes(int progressionStep)
		{
			mProgressionNotes.Clear();

			// chord progressions are set in their sensible way: I-IV-V for example starting on 1. 
			// it's easier to leave like that as it's readable and adjust here, rather than 0 based:
			mCurrentProgressionStep = progressionStep - ((progressionStep < 0) ? -1 : 1);

			int invProgRate = mMusicGenerator.mInstrumentSet.GetInverseProgressionRate((int)mTimeStep);
			int progRate = mMusicGenerator.mInstrumentSet.GetProgressionRate((int)mTimeStep);
			mPatternstepsTaken = (uint)(mMusicGenerator.mInstrumentSet.mSixteenthStepsTaken / invProgRate);
			mCurrentPatternStep = mPatternstepsTaken % mPatternlength;

			mbAreRepeatingPattern = (mPatternstepsTaken >= mPatternlength && mPatternstepsTaken < progRate - mPatternRelease);

			mbAreSettingPattern = (mPatternstepsTaken < mPatternlength);

			if (mCurrentPatternStep < mPatternNoteOffset.Count - 1)
			{
				mCurrentPatternNotes = mPatternNoteOffset[(int)mCurrentPatternStep];
				mCurrentPatternOctave = mPatternOctaveOffset[(int)mCurrentPatternStep];
			}
			else//if for some reason this instrument was added in the middle of playing a measure and is supposed to use a pattern that doesn't exist...
			{
				mCurrentPatternNotes = new List<int>();
				mCurrentPatternOctave = new List<int>();
			}
		}

		/// Fills our array, based on rhythm/leading and other variables:
		private void SelectNotes()
		{
			//if we're repeating a pattern, they're handled the same, 
			//otherwise lead and melodic notes have different logic.
			if (mSuccessionType == eSuccessionType.lead)
				AddLeadNote();
			else if (mbAreRepeatingPattern && mUsePattern == 1)
				AddRepeatNotes();
			else
			{
				if (mSuccessionType == 0)
					AddMelodicNotes();
				else
					AddRhythmNotes();
			}
		}

		///	Selects notes for percussion instrument:
		private void SelectPercussionNotes()
		{
			if (mSuccessionType == eSuccessionType.rhythm || UnityEngine.Random.Range(0, 100) <= mOddsOfPlaying)
			{
				mProgressionNotes.Add(0);
				/// roll odds for additional notes, and check for 7th.
				/// It's fairly arbitrary since they're all the same note, but allows for
				/// varying the number of beats the percussion will play.
				mProgressionNotes.Add(mStrumLength > 0 && UnityEngine.Random.Range(0, 100) < mOddsOfUsingChordNotes ? 0 : mUnplayed);
				mProgressionNotes.Add(mStrumLength > 0 && UnityEngine.Random.Range(0, 100) < mOddsOfUsingChordNotes ? 0 : mUnplayed);
				mProgressionNotes.Add(mStrumLength > 0 &&
										UnityEngine.Random.Range(0, 100) < mOddsOfUsingChordNotes &&
										mChordSize == mSeventhChord.Count ?
										0 : mUnplayed
									);
			}
			else
				AddEmptyNotes();
		}

		/// Checks for out of range notes in our list and forces it back within range.
		private void CheckForOutOfRangeNotes()
		{
			for (int i = 0; i < mProgressionNotes.Count; i++)
			{
				int note = mProgressionNotes[i];
				int clipArraySize = MusicGenerator.mMaxInstrumentNotes;

				if (note == mUnplayed || (note < clipArraySize && note >= mUnplayed))
					continue;

				if (note < 0)
					note = MusicHelpers.SafeLoop(note, mOctave);
				else if (note >= clipArraySize)
				{
					note = MusicHelpers.SafeLoop(note, mOctave);
					note += (mUsePattern == 1 && mbAreRepeatingPattern) ? mCurrentPatternOctave[i] * mOctave : 2 * mOctave;
				}

				if (note < 0 || note > clipArraySize)
				{
					Debug.Log("something's gone wrong note is out of range.");
					note = 0;
				}

				mProgressionNotes[i] = note;
			}
		}

		/// Repeats the rhythm notes.
		private void AddRepeatNotes()
		{
			for (int i = 0; i < mCurrentPatternNotes.Count; i++)
			{
				int note = mCurrentPatternNotes[i];
				mProgressionNotes.Add((note != mUnplayed) ? GetChordNote(note, i) : mUnplayed);
			}
		}

		/// Adds a rhythm note.
		private void AddRhythmNotes()
		{
			//because we generally want to play at least one note for rhythm
			//we start backward and just play the root chord note if other chord notes don't play.
			bool successfulNote = false;
			for (int i = (int)mChordSize - 1; i >= 0; i--)
			{
				if (UnityEngine.Random.Range(0, 100) <= mOddsOfUsingChordNotes || (i == 0 && !successfulNote))
				{
					mProgressionNotes.Add(GetChordNote(mSeventhChord[i], i));
					successfulNote = true;
				}
				else
					mProgressionNotes.Add(EmptyPatternedNote(i));
			}

			//add an empty 7th if we're not using them so we still have 4 notes.
			int fifthChord = mSeventhChord.Count - 1;
			if (mChordSize == fifthChord)
				mProgressionNotes.Add(EmptyPatternedNote(fifthChord));
		}

		/// Adds a lead note.
		private void AddLeadNote()
		{
			if (UnityEngine.Random.Range(0, 100) > mOddsOfUsingChordNotes)
			{
				if (UnityEngine.Random.Range(0, 100) < mOddsOfPlaying * mOddsOfPlayingMultiplier)
				{
					int nextNote = GetRawLeadNoteIndex();
					if (nextNote < 0)// just a safety check to keep from going under low C.
						nextNote *= -1;

					/// here we find the shortest rhythm step and make sure we're not playing something dischordant if it may be playing as well.
					int shortestTimestep = (int)mMusicGenerator.GetShortestRhythmTimestep();
					shortestTimestep = mMusicGenerator.mInstrumentSet.GetInverseProgressionRate(shortestTimestep);
					if (shortestTimestep == 1 || mMusicGenerator.mInstrumentSet.mSixteenthStepsTaken % shortestTimestep == 0)
					{
						if (IsAvoidNote(nextNote))
							nextNote = FixAvoidNote(nextNote);
					}

					mPlayedMelodicNotes.Add(nextNote);

					int note = AdjustRawLeadIndex(nextNote);

					AddSingleNote(note);
				}
				else
					AddEmptyNotes();
			}
			else
				AddMelodicNotes();
		}

		/// Fixes an avoid note to (hopefully) not be dischordant:
		private int FixAvoidNote(int nextNote)
		{
			int adjustedNote = nextNote + mLeadInfluence;
			int maxOctaves = 3;
			if ((adjustedNote + (int)mMusicGenerator.mKey + mCurrentPatternStep > (mScaleLength * maxOctaves) - 3) ||
				adjustedNote < 0)
			{
				mLeadInfluence *= -1;
				adjustedNote = nextNote + mLeadInfluence;
			}
			bool isAvoidNote = true;
			int maxAttempts = mScaleLength;
			for (int i = 1; i < maxAttempts && isAvoidNote; i++)
			{
				adjustedNote = nextNote + (i * mLeadInfluence);
				if (!IsAvoidNote(adjustedNote))
				{
					nextNote = adjustedNote;
					isAvoidNote = false;
				}
			}
			return adjustedNote;
		}

		/// steps the note through the scale, adjusted for mode, key, progression step to find th
		/// actual note index instead of our raw steps.
		private int AdjustRawLeadIndex(int noteIN)
		{
			int note = 0;
			int progressionstep = (mCurrentProgressionStep < 0) ? mCurrentProgressionStep * -1 : mCurrentProgressionStep;

			for (int j = 0; j < noteIN + progressionstep; j++)
			{
				int index = j + (int)mMusicGenerator.mMode;
				index = index % mScaleLength;
				note += mMusicScales[(int)mMusicGenerator.mScale][index];
			}
			note += (int)mMusicGenerator.mKey;
			return note;
		}

		///Returns true if this lead note is a half step above a chord note:
		private bool IsAvoidNote(int noteIN)
		{
			int note = MusicHelpers.SafeLoop(noteIN - 1, mScaleLength);
			int progressionstep = mCurrentProgressionStep < 0 ? mCurrentProgressionStep * -1 : mCurrentProgressionStep;
			int tritone = mCurrentProgressionStep < 0 ? -mTritoneStep : 0;
			int adjustedNote = MusicHelpers.SafeLoop(note + tritone, mScaleLength);
			int scaleNote = MusicHelpers.SafeLoop(adjustedNote + (int)mMusicGenerator.mMode + progressionstep, mScaleLength);
			List<int> scale = mMusicScales[(int)mMusicGenerator.mScale];
			bool isHalfStep = scale[scaleNote] == mHalfStep;

			bool isAboveChordNode = (adjustedNote == mSeventhChord[0] ||
									adjustedNote == mSeventhChord[1] ||
									adjustedNote == mSeventhChord[2] ||
									(adjustedNote == mSeventhChord[3] && mChordSize == mSeventhChord.Count));

			bool isSeventh = (mChordSize != mSeventhChord.Count && note == 6);
			if ((isHalfStep && isAboveChordNode) || isSeventh)
				return true;
			return false;
		}

		/// adds a melodic note. Basically randomly chooses a chord note.
		/// Use lead if you'd like scale notes.  The only real difference between
		/// this and a rhythm instrument with no chord note odds, is it will not always play.
		private void AddMelodicNotes()
		{
			if (UnityEngine.Random.Range(0, 100) < mOddsOfPlaying * mOddsOfPlayingMultiplier)
			{
				if (UnityEngine.Random.Range(0, 100) > mOddsOfUsingChordNotes)
				{
					int note = UnityEngine.Random.Range(0, (int)mChordSize);
					AddSingleNote(GetChordNote(mSeventhChord[note], 0), true);
				}
				else
					AddRhythmNotes();
			}
			else
				AddEmptyNotes();
		}

		/// Returns the octave the last note was played in.
		private int GetLeadOctave()
		{
			int lastNote = mPlayedMelodicNotes[mPlayedMelodicNotes.Count - 1];
			return lastNote / mScaleLength;
		}

		/// adds chord note in the octave nearest our curent melody.
		private void AddLeadChordNote()
		{
			int newOctave = 0;
			/// If this is the first note in the measure, just randomly pick somewhere to start:
			if (mPlayedMelodicNotes.Count == 0)
			{
				newOctave = mOctavesToUse[UnityEngine.Random.Range(0, mOctavesToUse.Count - 1)];

				int note = UnityEngine.Random.Range(0, (int)mChordSize);

				int finalNote = mSeventhChord[note];
				int playedNote = mSeventhChord[note] + (newOctave * mScaleLength);

				mPlayedMelodicNotes.Add(playedNote);
				AddSingleNote(GetChordNote(finalNote, 0, newOctave * mOctave), true);

				if (UnityEngine.Random.Range(0, 100) > AscendDescendInfluence)
					mLeadInfluence *= -1;
			}
			else /// otherwise, we try to find a chord note nearest to where we last played:
			{
				newOctave = GetLeadOctave();
				int lastNote = mPlayedMelodicNotes[mPlayedMelodicNotes.Count - 1];
				int chordNote = 0;
				switch (lastNote % mScaleLength)
				{
					case 0:
						if (UnityEngine.Random.Range(0, 100) < 50)
							chordNote = (UnityEngine.Random.Range(0, 100) < 25) ? mSeventhChord[0] : mSeventhChord[2];
						else
							chordNote = (UnityEngine.Random.Range(0, 100) < 25) ? mSeventhChord[0] : mSeventhChord[1];
						break;
					case 1:
						chordNote = (UnityEngine.Random.Range(0, 100) < 50) ? mSeventhChord[0] : mSeventhChord[1];
						break;
					case 2:
					case 3:
						chordNote = (UnityEngine.Random.Range(0, 100) < 50) ? mSeventhChord[1] : mSeventhChord[2];
						break;
					case 4:
					case 5:
					case 6:
						if (mChordSize == mSeventhChord.Count)
							chordNote = (UnityEngine.Random.Range(0, 100) < 50) ? mSeventhChord[2] : mSeventhChord[3];
						else
							chordNote = (UnityEngine.Random.Range(0, 100) < 50) ? mSeventhChord[2] : mSeventhChord[0];
						break;
					default:
						break;
				}
				mPlayedMelodicNotes.Add(chordNote + newOctave * mScaleLength);
				AddSingleNote(GetChordNote(chordNote, 0, newOctave * mOctave), true);

				if (UnityEngine.Random.Range(0, 100) > AscendDescendInfluence)
					mLeadInfluence *= -1;
			}
		}

		/// Gets the next melodic note.
		private int GetRawLeadNoteIndex()
		{
			List<int> scale = mMusicScales[(int)mMusicGenerator.mScale];
			int noteOUT = UnityEngine.Random.Range(0, (scale.Count - 1)) + (GetOctave());

			if (mPlayedMelodicNotes.Count == 1)
			{
				noteOUT = mPlayedMelodicNotes[mPlayedMelodicNotes.Count - 1] +
					(UnityEngine.Random.Range(1, (int)mLeadMaxSteps) * mLeadInfluence);
			}
			if (mPlayedMelodicNotes.Count > 1)
			{
				int ultimateNote = mPlayedMelodicNotes[mPlayedMelodicNotes.Count - 1];
				noteOUT = UnityEngine.Random.Range(ultimateNote + mLeadInfluence, ultimateNote + ((int)mLeadMaxSteps * mLeadInfluence));
			}

			/// here, we try to stay within range, and adjust the ascend/ descent influence accordingly:
			int maxOctaves = 3;
			if (noteOUT + (int)mMusicGenerator.mKey + mCurrentPatternStep > (mScaleLength * maxOctaves) - 2)
			{
				int progressionStep = mCurrentProgressionStep < 0 ? mCurrentProgressionStep * -1 : mCurrentProgressionStep;

				noteOUT = (mScaleLength * maxOctaves) - 3 - (int)mMusicGenerator.mKey - progressionStep;
				mLeadInfluence = mDescendingInfluence;
			}
			else if (noteOUT < 0)
			{
				noteOUT = UnityEngine.Random.Range(1, (int)mLeadMaxSteps);
				mLeadInfluence = mAscendingInfluence;
			}
			else if (UnityEngine.Random.Range(0, 100) > AscendDescendInfluence)
				mLeadInfluence *= -1;

			return noteOUT;
		}

		/// returns the appropriate note for the interval along the chord progression
		/// given our key and scale
		private int GetChordNote(int chordNote = 0, int chordIndex = 0, int octaveOffsetIN = -1)
		{
			int note = (int)mMusicGenerator.mKey;

			//tri-tone check.
			int progressionStep = (mCurrentProgressionStep < 0) ? mCurrentProgressionStep * -1 : mCurrentProgressionStep;

			//add octave offset:
			int newOctave = (octaveOffsetIN == -1) ? GetOctave(chordIndex) : octaveOffsetIN;
			note += (mbAreRepeatingPattern && mUsePattern == 1 && mSuccessionType != eSuccessionType.lead) ? mCurrentPatternOctave[chordIndex] * mOctave : newOctave;

			//for melodies we don't want to keep playing the same note repeatedly
			if (CheckRedundancy(chordNote))
			{
				int extraStep = 2;
				chordNote = (chordNote != mSeventhChord[(int)mChordSize - 1]) ? chordNote + extraStep : 0;
			}

			mCurrentPatternNotes[chordIndex] = chordNote;

			note += GetChordOffset(progressionStep, (int)mMusicGenerator.mMode, chordNote);
			if (mCurrentProgressionStep < 0)
				note += mTritoneStep;

			return note;
		}

		/// Returns a random note in the chord:
		/// we basically step through the staff of 36 notes until we find our actual note:
		/// so, step through key/mode/chord root note plus chord offset
		private int GetChordOffset(int rootOffset, int mode, int chordNote = 0)
		{
			int noteOUT = 0;
			List<int> scale = mMusicScales[(int)mMusicGenerator.mScale];
			for (int i = 0; i < rootOffset + chordNote; i++)
			{
				int index = (i + mode) % scale.Count;
				noteOUT += scale[index];
			}
			return noteOUT;
		}

		/// Sets the theme notes from the repeating list.
		public void SetThemeNotes()
		{
			mThemeNotes.Clear();
			mThemeNotes = new List<List<int>>(mRepeatingNotes);
		}

		///////////////////////////////////
		/// Private utility functions:
		///////////////////////////////////

		/// sets our multiplier for the next played note:
		private void SetMultiplier()
		{
			mOddsOfPlayingMultiplier = mOddsOfPlayingMultiplierBase;
			for (int i = 0; i < mProgressionNotes.Count; i++)
			{
				if (mProgressionNotes[i] != mUnplayed)
					mOddsOfPlayingMultiplier = mOddsOfPlayingMultiplierMax;
			}
		}

		/// Returns whether this note is too redundant:
		private bool CheckRedundancy(int noteIN)
		{
			if (mSuccessionType == eSuccessionType.rhythm || mMusicGenerator.mInstrumentSet.mSixteenthStepsTaken == 0)
				return false;

			if (noteIN == mPatternNoteOffset[mMusicGenerator.mInstrumentSet.mSixteenthStepsTaken - 1][0])
				return (UnityEngine.Random.Range(0.0f, 100.0f) < mRedundancyAvoidance) ? true : false;

			return false;
		}

		/// Just initializes our clip notes to unplayed.
		private void LoadClipNotes()
		{
			mClipNotes.Clear();
			int numMeasures = 4;
			for (int x = 0; x < numMeasures; x++)
			{
				mClipNotes.Add(new List<List<int>>());
				for (int i = 0; i < mStepsPerMeasure; i++)
				{
					mClipNotes[x].Add(new List<int>());
					for (int j = 0; j < MusicGenerator.mMaxInstrumentNotes; j++)
					{
						mClipNotes[x][i].Add(mUnplayed);
					}
				}
			}
		}

		/// fills our arrays with base values:
		private void LoadPatternNotes()
		{
			mPatternNoteOffset.Clear();
			mPatternOctaveOffset.Clear();
			for (int i = 0; i < mStepsPerMeasure; i++)
			{
				mPatternNoteOffset.Add(new List<int>());
				mPatternOctaveOffset.Add(new List<int>());
				for (int j = 0; j < mSeventhChord.Count; j++)
				{
					mPatternNoteOffset[i].Add(mUnplayed);
					mPatternOctaveOffset[i].Add(0);
				}
			}
		}

		/// Clears the pattern notes.
		public void ClearPatternNotes()
		{
			for (int i = 0; i < mStepsPerMeasure; i++)
			{
				for (int j = 0; j < mSeventhChord.Count; j++)
				{
					mPatternNoteOffset[i][j] = mUnplayed;
					mPatternOctaveOffset[i][j] = 0;
				}
			}
		}

		/// fills our progression notes with empty values and sets pattern notes.
		private void FillEmptyPattern()
		{
			for (int i = 0; i < mSeventhChord.Count; i++)
			{
				mProgressionNotes.Add(EmptyPatternedNote(i));
			}
		}

		/// Adds a single note to the progression and 3 unplayed or patterned notes:
		private void AddSingleNote(int noteIN, bool addPattern = false)
		{
			mProgressionNotes.Add(noteIN);
			mProgressionNotes.Add(addPattern ? EmptyPatternedNote(1) : mUnplayed);
			mProgressionNotes.Add(addPattern ? EmptyPatternedNote(2) : mUnplayed);
			mProgressionNotes.Add(addPattern ? EmptyPatternedNote(3) : mUnplayed);
		}

		/// fills the current octaves and notes with non-played values.
		private void AddEmptyNotes()
		{
			for (int i = 0; i < mSeventhChord.Count; i++)
			{
				mProgressionNotes.Add(mUnplayed);
				mCurrentPatternNotes[i] = mUnplayed;
				mCurrentPatternOctave[i] = 0;
			}
		}

		/// Sets the empty patterned notes. Plays no regular notes:
		private int EmptyPatternedNote(int index)
		{
			mCurrentPatternNotes[index] = mUnplayed;
			mPatternOctaveOffset[(int)mCurrentPatternStep][index] = 0;
			return mUnplayed;
		}

		/// Returns a random octave within available octaves for this instrument:
		private int GetOctave(int indexIN = 0)
		{
			int octave = UnityEngine.Random.Range(0, mOctavesToUse.Count);

			//add it to our octave pattern, if needs be.
			if (mUsePattern == 1 && mbAreSettingPattern)
				mCurrentPatternOctave[indexIN] = mOctavesToUse[octave];

			return mOctavesToUse[octave] * mOctave;
		}

		// -----------------------------------------------------------
		// clip management: This is only used the the UI version of the player to set clip notes:
		// should probably be moved out of here, or stored elsewhere.
		// -----------------------------------------------------------

		/// Adds the clip note.
		public bool AddClipNote(int timestep, int note, int measure = 0)
		{
			for (int i = 0; i < mClipNotes[measure][timestep].Count; i++)
			{
				if (mClipNotes[measure][timestep][i] == mUnplayed)
				{
					mClipNotes[measure][timestep][i] = note;
					return true;
				}
			}
			return false;
		}

		/// Removes the clip note.
		public bool RemoveClipNote(int timestep, int note, int measure = 0)
		{
			for (int i = 0; i < mClipNotes[measure][timestep].Count; i++)
			{
				if (mClipNotes[measure][timestep][i] == note)
				{
					mClipNotes[measure][timestep][i] = mUnplayed;
					return true;
				}
			}
			return false;
		}

		/// Clears the clip notes.
		public void ClearClipNotes()
		{
			for (int i = 0; i < mClipNotes.Count; i++)
			{
				for (int j = 0; j < mClipNotes[i].Count; j++)
				{
					for (int x = 0; x < mClipNotes[i][j].Count; x++)
						mClipNotes[i][j][x] = mUnplayed;
				}
			}
		}

		/////////////////////////////////////////
		/// Save / Load functions.
		/////////////////////////////////////////

		/// loads and sets values from save file.
		public void LoadInstrument(InstrumentSave save)
		{
			mChordSize = save.mChordSize;
			mStaffPlayerColor = save.mStaffPlayerColor;
			mSuccessionType = save.mSuccessionType;
			mInstrumentType = save.mInstrumentType;
			mOctavesToUse = save.mOctavesToUse;
			mStereoPan = save.mStereoPan;
			mOddsOfPlaying = save.mOddsOfPlaying;
			mPreviousOdds = save.mPreviousOdds;
			mVolume = save.mVolume;
			mOctaveOdds = save.mOctaveOdds;
			mOddsOfPlayingMultiplierMax = save.mOddsOfPlayingMultiplierMax;
			mOddsOfPlayingMultiplier = save.mOddsOfPlayingMultiplier;
			mGroup = save.mGroup;
			mIsSolo = save.mIsSolo;
			mIsMuted = save.mIsMuted;
			mTimeStep = save.mTimeStep;
			mOddsOfUsingChord = save.mOddsOfUsingChord;
			mOddsOfUsingChordNotes = save.mOddsOfUsingChordNotes;
			mStrumLength = save.mStrumLength;
			mStrumVariation = save.mStrumVariation;
			mRedundancyAvoidance = save.mRedundancyAvoidance;
			mRoomSize = save.mRoomSize;
			mReverb = save.mReverb;
			mEcho = save.mEcho;
			mEchoDelay = save.mEchoDelay;
			mEchoDecay = save.mEchoDecay;
			mFlanger = save.mFlanger;
			mDistortion = save.mDistortion;
			mChorus = save.mChorus;
			mUsePattern = save.mUsePattern;
			mPatternlength = save.mPatternlength;
			mPatternRelease = save.mPatternRelease;
			mRedundancyOdds = save.mRedundancyOdds;
			mLeadMaxSteps = save.mLeadMaxSteps;
			AscendDescendInfluence = save.AscendDescendInfluence;
			mAudioSourceVolume = save.mAudioSourceVolume;
			mMusicGenerator.mMixer.SetFloat("Volume" + mInstrumentIndex.ToString(), save.mAudioSourceVolume);
			mMusicGenerator.mMixer.SetFloat("Reverb" + mInstrumentIndex.ToString(), mReverb);
			mMusicGenerator.mMixer.SetFloat("RoomSize" + mInstrumentIndex.ToString(), mRoomSize);
			mMusicGenerator.mMixer.SetFloat("Chorus" + mInstrumentIndex.ToString(), mChorus);
			mMusicGenerator.mMixer.SetFloat("Flange" + mInstrumentIndex.ToString(), mFlanger);
			mMusicGenerator.mMixer.SetFloat("Distortion" + mInstrumentIndex.ToString(), mDistortion);
			mMusicGenerator.mMixer.SetFloat("Echo" + mInstrumentIndex.ToString(), mEcho);
			mMusicGenerator.mMixer.SetFloat("EchoDelay" + mInstrumentIndex.ToString(), mEchoDelay);
			mMusicGenerator.mMixer.SetFloat("EchoDecay" + mInstrumentIndex.ToString(), mEchoDecay);
		}

		/// info for serialized save class
		public InstrumentSave SaveInstrument()
		{
			InstrumentSave save = new InstrumentSave();

			save.mChordSize = mChordSize;
			save.mStaffPlayerColor = mStaffPlayerColor;
			save.mSuccessionType = mSuccessionType;
			save.mInstrumentType = mInstrumentType;
			save.mOctavesToUse = mOctavesToUse;
			save.mStereoPan = mStereoPan;
			save.mOddsOfPlaying = mOddsOfPlaying;
			save.mPreviousOdds = mPreviousOdds;
			save.mVolume = mVolume;
			save.mOctaveOdds = mOctaveOdds;
			save.mOddsOfPlayingMultiplierMax = mOddsOfPlayingMultiplierMax;
			save.mOddsOfPlayingMultiplier = mOddsOfPlayingMultiplier;
			save.mGroup = mGroup;
			save.mIsSolo = mIsSolo;
			save.mIsMuted = mIsMuted;
			save.mTimeStep = mTimeStep;
			save.mOddsOfUsingChord = mOddsOfUsingChord;
			save.mOddsOfUsingChordNotes = mOddsOfUsingChordNotes;
			save.mStrumLength = mStrumLength;
			save.mStrumVariation = mStrumVariation;
			save.mRedundancyAvoidance = mRedundancyAvoidance;
			save.mRoomSize = mRoomSize;
			save.mReverb = mReverb;
			save.mEcho = mEcho;
			save.mEchoDelay = mEchoDelay;
			save.mEchoDecay = mEchoDecay;
			save.mFlanger = mFlanger;
			save.mDistortion = mDistortion;
			save.mChorus = mChorus;
			save.mUsePattern = mUsePattern;
			save.mPatternlength = mPatternlength;
			save.mPatternRelease = mPatternRelease;
			save.mRedundancyOdds = mRedundancyOdds;
			save.mLeadMaxSteps = mLeadMaxSteps;
			save.AscendDescendInfluence = AscendDescendInfluence;
			save.mAudioSourceVolume = mAudioSourceVolume;
			return save;
		}


		/////////////////////////////
		/// Setters: 
		/// Lots of magic numbers here. They're mostly based on unity's min/max values for their respectives variables and/or common sense.
		////////////////////////////
		/// Sets the size of chords. Must be within 1-4 notes.
		public void SetChordSize(uint value) { mChordSize = value <= mSeventhChord.Count && value >= mTriadCount ? value : 0; }

		/// Sets the staff player color.
		public void SetStaffPlayerColor(eStaffPlayerColors value) { mStaffPlayerColor = (uint)value; }

		/// Sets the succession types (Lead, Melody, Rhythm).
		public void SetSuccessionType(eSuccessionType value) { mSuccessionType = value; }

		/// Sets the stero pan. The distribution of this instrument to each speaker.
		public void SetStereoPan(float value) { mStereoPan = value >= -1 && value <= 1 ? value : 0; }

		/// Set the odds of this note playing during its timestep.
		public void SetOddsOfPlaying(uint value) { mOddsOfPlaying = value <= 100 ? value : 100; }

		/// Sets the volume of this instrument.
		public void SetVolume(float value) { mVolume = value >= 0 && value <= 1 ? value : 0; }

		/// Sets the volume of this instruments audio source (separate from the instrument and global volumes).
		public void SetAudioSourceVolume(float value) { mAudioSourceVolume = value >= -80 && value <= 20 ? value : 0; }

		/// Set the odds of playing multiplier. On a successful note played, the _next_ note's odds are multiplied against this value.
		public void SetOddsOfPlayingMultiplierMax(float value) { mOddsOfPlayingMultiplierMax = value >= 1.0f ? value : 1.0f; }

		/// Set the current odds of playing multiplier.
		public void SetOddsOfPlayingMultiplier(float value) { mOddsOfPlayingMultiplier = value >= 1 ? value : 1; }

		/// Set the group that this instrument belongs to.
		public void SetGroup(uint value) { mGroup = value >= 0 && value < 4 ? value : 0; }

		/// Set the timestep of this instrument.
		public void SetTimestep(eTimestep value) { mTimeStep = value; }

		/// Set the odds of this instrument playing a chord.
		public void SetOddsOfUsingChord(float value) { mOddsOfUsingChord = value >= 0 && value <= 100 ? value : 50; }

		/// Set the odds of this instrument playing additional chord notes.
		public void SetOddsOfUsingChordNotes(float value) { mOddsOfUsingChordNotes = value >= 0 && value <= 100 ? value : 50; }

		/// Set the length of delay between strummed notes.
		public void SetStrumLength(float value) { mStrumLength = value >= 0 && value <= 1 ? value : 0; }

		/// Set the length of variation in the strum length.
		public void SetStrumVariation(float value) { mStrumVariation = value >= 0 && value <= 1 ? value : 0; }

		/// Set the rerverb's room size.
		public void SetRoomSize(float value) { mRoomSize = value >= -10000 && value <= 0 ? value : -10000; }

		/// Sets Reverb.
		public void SetReverb(float value) { mReverb = value >= -10000 && value <= 2000 ? value : -2000; }

		/// Sets the echo wet mix.
		public void SetEcho(float value) { mEcho = value >= 0 && value <= 1 ? value : 0.0f; }

		/// Sets the echo delay.
		public void SetEchoDelay(float value) { mEchoDelay = value >= 100 && value <= 1000 ? value : 0; }

		/// Sets the echo decay.
		public void SetEchoDecay(float value) { mEchoDecay = value >= 0 && value <= 0.9f ? value : 0; }

		/// Sets the flanger wet mix.
		public void SetFlanger(float value) { mFlanger = value >= 0 && value <= 100 ? value : 0; }

		/// Sets the distortion value.
		public void SetDistortion(float value) { mDistortion = value >= 0 && value <= 1 ? value : 0; }

		/// Sets the chorus wet mix.
		public void SetChorus(float value) { mChorus = value >= 0 && value <= 1 ? value : 0; }

		/// Sets whether this instrument follows a pattern.
		public void SetUsingPattern(uint value) { mUsePattern = value == 0 || value == 1 ? value : 0; }

		/// Sets how many notes are in this pattern.
		public void SetPatternLength(uint value) { mPatternlength = value <= mStepsPerMeasure / 2 ? value : 0; }

		/// Sets when in the measure the pattern will release (i.e. a value of 2 will not follow the pattern for the last two notes.)
		public void SetPatternRelease(uint value) { mPatternRelease = value <= mStepsPerMeasure / 2 ? value : 0; }

		/// Resets the number of pattern steps taken (this is handled internally by the instrument set and shouldn't need to be edited.)
		public void ResetPatternStepsTaken() { mPatternstepsTaken = 0; }

		/// Sets the odds the lead instrument will continue to ascend or descend. A lower value is more random, a higher value is less.
		public void SetLeadAscendDescend(float value) { AscendDescendInfluence = value >= 0 && value <= 100 ? value : 75.0f; }

		/// Sets the max number of steps a lead instrument will try to take (this can be adjusted by the instrument logic for various reasons.
		/// and is more of a guideline for it than a rule).
		public void SetLeadMaxSteps(uint value) { mLeadMaxSteps = value >= 1 && value <= MusicGenerator.mMaxInstrumentNotes ? value : 1; }

		/// for these, please just use the main generator class to add and remove instruments rather than changing them like this:
		/// If you must change on the fly for some reason, see InstrumentUIObject.ChangeInstrument() for the needed steps:
		public void SetInstrumentIndex(uint value) { mInstrumentIndex = value < mMusicGenerator.mInstrumentSet.mInstruments.Count ? value : 0; }
		public void SetInstrumentTypeIndex(uint value) { mInstrumentTypeIndex = value < mMusicGenerator.GetClips().Count ? value : 0; }
		public void SetInstrumentType(string value)
		{
			bool validType = false;
			List<string> instrumentTypes = mMusicGenerator.GetLoadedInstrumentNames();
			if (instrumentTypes.Count <= 0)
				return;

			for (int i = 0; i < instrumentTypes.Count; i++)
			{
				if (value == instrumentTypes[i])
					validType = true;
			}
			mInstrumentType = validType ? value : instrumentTypes[0];
		}

	}
}