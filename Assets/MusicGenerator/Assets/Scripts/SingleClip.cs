namespace ProcGenMusic
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using System.IO;

	/// A single Clip. Used for playing a specific set of instruments/notes. Create via the Measure Editor in the executable.
	public class SingleClip : MonoBehaviour
	{
		private float mTempo = 0.0f;        ///< our tempo for this clip.

		public bool mIsPlaying { get; set; }        ///< whether we're playing or not.
		public int mStaffPlayerColor { get; private set; }  ///< used only in the measure editor UI.
		public float mSixteenthStepTimer { get; private set; }
		public int mSixteenthStepCount { get; private set; }
		public int mMeasureStepCount { get; private set; }
		public float mSixteenthMeasure { get; private set; }
		public InstrumentSet mInstrumentSet { get; private set; }
		public eClipState mState { get; private set; }
		public bool mIsRepeating { get; set; }
		public int mNumMeasures { get; private set; }

		void Awake()
		{
			mIsPlaying = false;
			mStaffPlayerColor = 0;
			mSixteenthStepTimer = 0.0f;
			mSixteenthStepCount = 0;
			mMeasureStepCount = 0;
			mSixteenthMeasure = 0.0f;
			mInstrumentSet = null;
			mState = eClipState.Stop;
			mIsRepeating = false;
			mNumMeasures = 1;
		}

		/// async init.
		public IEnumerator AsyncInit(ClipSave save, System.Action<bool> callback)
		{
			mInstrumentSet = gameObject.AddComponent<InstrumentSet>();
			mInstrumentSet.Init();
			mTempo = save.mTempo;
			mNumMeasures = save.mNumberOfMeasures;
			mSixteenthMeasure = 60 / mTempo;
			mIsRepeating = save.mClipIsRepeating;

			mInstrumentSet.SetTempo(mTempo);
			mInstrumentSet.SetProgressionRate(save.mProgressionRate);
			mInstrumentSet.SetRepeatMeasuresNum(mNumMeasures);
			bool isFinished = false;
			StartCoroutine(AsyncLoadInstruments(save, ((x) => {isFinished = x;})));
			yield return new WaitUntil(()=> isFinished);
			callback(isFinished);
			yield return null;
		}

		/// non-async init.
		public void Init(ClipSave save)
		{
			mInstrumentSet = gameObject.AddComponent<InstrumentSet>();
			mInstrumentSet.Init();
			mTempo = save.mTempo;
			mNumMeasures = save.mNumberOfMeasures;
			mSixteenthMeasure = 60 / mTempo;
			mIsRepeating = save.mClipIsRepeating;

			mInstrumentSet.SetTempo(mTempo);
			mInstrumentSet.SetProgressionRate(save.mProgressionRate);
			mInstrumentSet.SetRepeatMeasuresNum(mNumMeasures);
			LoadInstruments(save);
		}

		public void ResetClip()
		{
			mInstrumentSet.ExitNormalMeasure();
			mInstrumentSet.ExitRepeatingMeasure(true);
		}

		/// Set the clip state:
		public void SetState(eClipState stateIN)
		{
			mState = stateIN;
			switch (mState)
			{
				case eClipState.Pause:
					break;
				case eClipState.Play:
					mInstrumentSet.ResetRepeatCount();
					break;
				case eClipState.Stop:
					ResetClip();
					break;
				default:
					break;
			}
		}

		/// Clip update:
		void Update()
		{
			switch (mState)
			{
			case eClipState.Play:
				if (mInstrumentSet.mRepeatCount <= mNumMeasures)
					mInstrumentSet.PlayEditorClip (mIsRepeating);
				else if (mIsRepeating) 
				{
					ResetClip();
				}
				else
				{
					SetState(eClipState.Stop);
					ResetClip();
				}
				break;
				default:
					break;
			}
		}

		///load the clip:
		public IEnumerator AsyncLoadInstruments(ClipSave save, System.Action<bool> callback)
		{
			for (int i = 0; i < save.mClipInstrumentSaves.Count; i++)
			{
				ClipInstrumentSave instrumentSave = save.mClipInstrumentSaves[i];
				mInstrumentSet.mInstruments.Add(new Instrument());
				mInstrumentSet.mInstruments[i].Init(i);

				Instrument instrument = mInstrumentSet.mInstruments[i];

				for (int x = 0; x < instrumentSave.mClipMeasures.Count; x++)
				{
					for (int y = 0; y < instrumentSave.mClipMeasures[x].timestep.Count; y++)
					{
						for (int z = 0; z < instrumentSave.mClipMeasures[x].timestep[y].notes.Count; z++)
							instrument.mClipNotes[x][y][z] = instrumentSave.mClipMeasures[x].timestep[y].notes[z];
					}
				}
				uint index = 999;
				StartCoroutine(MusicGenerator.Instance.AsyncLoadBaseClips(instrumentSave.mInstrumentType,((x) => {index = x;})));
				yield return new WaitUntil(()=> index != 999);
				instrument.SetInstrumentType(instrumentSave.mInstrumentType);
				instrument.SetVolume(instrumentSave.mVolume);
				instrument.SetInstrumentTypeIndex(index);
				instrument.SetStaffPlayerColor((eStaffPlayerColors)instrumentSave.mStaffPlayerColor);
				instrument.SetTimestep(instrumentSave.mTimestep);
				instrument.SetSuccessionType(instrumentSave.mSuccessionType);
				instrument.SetStereoPan(instrumentSave.mStereoPan);
			}
			callback(true);
			yield return null;
		}

		///load the clip:
		public void LoadInstruments(ClipSave save)
		{
			for (int i = 0; i < save.mClipInstrumentSaves.Count; i++)
			{
				ClipInstrumentSave instrumentSave = save.mClipInstrumentSaves[i];
				mInstrumentSet.mInstruments.Add(new Instrument());
				mInstrumentSet.mInstruments[i].Init(i);

				Instrument instrument = mInstrumentSet.mInstruments[i];

				for (int x = 0; x < instrumentSave.mClipMeasures.Count; x++)
				{
					for (int y = 0; y < instrumentSave.mClipMeasures[x].timestep.Count; y++)
					{
						for (int z = 0; z < instrumentSave.mClipMeasures[x].timestep[y].notes.Count; z++)
							instrument.mClipNotes[x][y][z] = instrumentSave.mClipMeasures[x].timestep[y].notes[z];
					}
				}
				instrument.SetVolume(instrumentSave.mVolume);
				uint index = MusicGenerator.Instance.LoadBaseClips(instrumentSave.mInstrumentType);
				instrument.SetInstrumentType(instrumentSave.mInstrumentType);
				instrument.SetInstrumentTypeIndex(index);
				instrument.SetStaffPlayerColor((eStaffPlayerColors)instrumentSave.mStaffPlayerColor);
				instrument.SetTimestep(instrumentSave.mTimestep);
				instrument.SetSuccessionType(instrumentSave.mSuccessionType);
				instrument.SetStereoPan(instrumentSave.mStereoPan);
			}
		}
	}
}