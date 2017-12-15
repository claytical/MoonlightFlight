namespace ProcGenMusic
{
	using System.Collections.Generic;
	using UnityEngine;

	/// Generates chord progressions
	public class ChordProgressions 
	{
		/// list of progression steps to avoid
		/// Please note that these are ignored when changing keys.
		public List<bool> mExcludedProgSteps { get; private set; }

		public float mTonicInfluence { get; private set; }          //< percent odds that the Tonic chords are used
		public float mSubdominantInfluence { get; private set; }    //< percent odds that the subdominant chords are used
		public float mDominantInfluence { get; private set; }       //< percent odds that the dominant chord are used
		public float mTritoneSubInfluence { get; private set; }     //< percent odds that a Tri-tone substitution takes place

		// which steps belong to which type of chord:
		static readonly private List<int> mTonicChords = new List<int> { 1, 3, 6 };
		static readonly private List<int> mSubdominantChords = new List<int> { 4, 2 };
		static readonly private List<int> mDominantChords = new List<int> { 5, 7 };

		private MusicGenerator mMusicGenerator = null;

		public void Init()
		{
			mMusicGenerator = MusicGenerator.Instance;
			mExcludedProgSteps = new List<bool>() { false, false, false, false, false, false, false };
			mTonicInfluence = 50.0f;
			mSubdominantInfluence = 50.0f;
			mDominantInfluence = 50.0f;
			mTritoneSubInfluence = 50.0f;
		}

		/// Generates a new progression. 
		public List<int> GenerateProgression(eMode modeIN, eScale scaleIN, int keyChange)
		{
			List<int> progressionOUT = new List<int>();
			//here we decide which chord step we'll use based on tonal influences and whether we'll change keys:
			// this is a bit mangled, but it works :P
			for (int i = 0; i < mMusicGenerator.mMaxFullstepsTaken; i++)
			{
				List<int> chords = new List<int>();

				switch (i)
				{
					case 0:
						chords = Random.Range(0, 100) < mTonicInfluence ? chords = mTonicChords : mSubdominantChords;
						break;
					case 1:
						chords = Random.Range(0, 100) < mSubdominantInfluence ? chords = mSubdominantChords : mTonicChords;
						break;
					case 2:
						chords = Random.Range(0, 100) < mSubdominantInfluence ? chords = mSubdominantChords : mDominantChords;
						break;
					case 3:
						if (Random.Range(0, 100) < mDominantInfluence)
							chords = mDominantChords;
						else if (Random.Range(0, 100) < mSubdominantInfluence)
							chords = mSubdominantChords;
						else
							chords = mTonicChords;
						break;
					default:
						break;
				}
				int tritone = (chords == mDominantChords && Random.Range(0, 100) < mTritoneSubInfluence) ? -1 : 1;
				progressionOUT.Add(tritone * GetProgressionSteps(chords, modeIN, scaleIN, keyChange));
			}
			return progressionOUT;
		}

		/// Gets the chord interval.
		private int GetProgressionSteps(List<int> chords, eMode modeIN, eScale isMajorScale, int keyChange)
		{
			List<int> temp = new List<int>();

			//create a new list of possible chord steps, excluding the steps we'd like to avoid:
			for (int i = 0; i < chords.Count; i++)
			{
				/// we're going to ignore excluded steps when changing keys, if it's not an avoid note.
				/// it's too likely that the note that's excluded is the only available note that's shared between
				/// the two keys for that chord type (like, if V is excluded, VII is never shared in major key ascending fifth step up)
				if ((keyChange != 0 && CheckKeyChangeAvoid(isMajorScale, keyChange, chords[i], modeIN)) ||
					mExcludedProgSteps[chords[i] - 1] != true)
				{
					temp.Add(chords[i]);
				}
			}

			if (temp.Count == 0)
				Debug.Log("progression steps == 0");

			return temp[Random.Range(0, temp.Count)];
		}

		// Musically, this could be more robust, but essentially checks to make sure a given chord will not sound
		// bad when changing keys. We change the key early in the generator, so, for example, we don't
		// want to play the 4th chord in the new key if we're descending, that chord is not shared
		// between the two keys. 
		// TODO: more intelligent key changes :P 
		private bool CheckKeyChangeAvoid(eScale scaleIN, int keyChange, int chord, eMode modeIN)
		{
			int mode = (int)modeIN;

			//if we're not changing keys, there's nothing to avoid:
			if (keyChange == 0) return true;

			bool isNotAvoidNote = true;
			if (scaleIN == eScale.Major || scaleIN == eScale.HarmonicMajor)
			{
				if ((keyChange > 0 && chord == 7 - mode) ||
					(keyChange < 0 && chord == 4 - mode))
					isNotAvoidNote = false;
			}
			else if (scaleIN != 0)
			{
				if ((keyChange > 0 && chord == 2 - mode) ||
					(keyChange < 0 && chord == 6 - mode))
					isNotAvoidNote = false;
			}
			return isNotAvoidNote;
		}

		////////////////////////////
		/// Setters:
		////////////////////////////
		public void SetTonicInflience(float value) { mTonicInfluence = value >= 0 && value <= 100 ? value : 50; }
		public void SetSubdominantInfluence(float value) { mSubdominantInfluence = value >= 0 && value <= 100 ? value : 50; }
		public void SetDominantInfluence(float value) { mDominantInfluence = value >= 0 && value <= 100 ? value : 50; }
		public void SetTritoneSubInfluence(float value) { mTritoneSubInfluence = value >= 0 && value <= 100 ? value : 50; }
		public void SetExcludedProgressionSteps(List<bool> stepsIN)
		{
			if (stepsIN.Count != 7)
			{
				Debug.Log("You must set all 7 progression steps");
				return;
			}
			mExcludedProgSteps = stepsIN;
		}
	}
}