namespace ProcGenMusic
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	/// UI panel for Advanced settings:
	public class AdvancedSettingsPanel : HelperSingleton<AdvancedSettingsPanel>
	{
		private Animator mAnimator = null;
		public Slider mTonicInfluence = null;           ///< handles likelihood of playing tonic notes
		public Slider mSubdominantInfluence = null;     ///< handles likelihood of playing subdiminant notes
		public Slider mDominantInfluence = null;        ///< handles likelihood of playing dominant notes
		public Slider mTritoneSubInfluence = null;      ///< Odds that a flat-5 or tritone substitution will play for a dominant chord
		public Slider mAscendDescendKey = null;         ///< whether our chord progressions are likely to ascend or descend
		public Dropdown mGroupRate = null;              ///< sets 'when' we roll die on new groups (end of progression or end of measure)
		public Dropdown mDynamicStyle = null;           ///< sets 'when' we roll die on new groups (end of progression or end of measure)
		public Slider mVolumeFadeRate = null;			///< the rate at which the volume can fade in/out.
		public Toggle mAsyncLoading = null;				///< whether we'll use async loading.

		//output text fields for above variables:
		private Text mTonicInfluenceOutput = null;
		private Text mSubdominantInfluenceOutput = null;
		private Text mDominantInfluenceOutput = null;
		private Text mTritoneSubInfluenceOutput = null;
		private Text mAscendDescendKeyOutput = null;
		private Text mVolumeFadeRateOutput = null;

		private MusicGenerator mMusicGenerator = null;
		private UIManager mUIManager = null;
		public List<Toggle> mExcludedSteps = new List<Toggle>();        //< list of which steps are excluded from chord progressions

		public void Init()
		{
			mUIManager = UIManager.Instance;
			mAnimator = GetComponentInParent<Animator>();
			Component[] components = this.GetComponentsInChildren(typeof(Transform), true);
			mMusicGenerator = MusicGenerator.Instance;
			Tooltips tooltips = mUIManager.mTooltips;
			ChordProgressions progressions = mMusicGenerator.mChordProgressions;
			mTonicInfluence.value = progressions.mTonicInfluence;
			mSubdominantInfluence.value = progressions.mSubdominantInfluence;
			mDominantInfluence.value = progressions.mDominantInfluence;
			mTritoneSubInfluence.value = progressions.mTritoneSubInfluence;
			mGroupRate.value = (int)mMusicGenerator.mGroupRate;

			//sets up our UI elements:
			foreach (Component cp in components)
			{
				if (cp.name == "AsyncLoading")
					tooltips.AddUIElement(ref mAsyncLoading, cp, "AsyncLoading");
				if (cp.name == "GroupRate")
					tooltips.AddUIElement(ref mGroupRate, cp, "GroupRate");
				if (cp.name == "DynamicStyle")
					tooltips.AddUIElement(ref mDynamicStyle, cp, "DynamicStyle");
				if (cp.name == "DominantInfluence")
					tooltips.AddUIElement(ref mDominantInfluence, cp, "DominantInfluence");
				if (cp.name == "TonicInfluence")
					tooltips.AddUIElement(ref mTonicInfluence, cp, "TonicInfluence");
				if (cp.name == "SubdominantInflunce")
					tooltips.AddUIElement(ref mSubdominantInfluence, cp, "SubdominantInfluence");
				if (cp.name == "TritoneSubInf")
					tooltips.AddUIElement(ref mTritoneSubInfluence, cp, "TritoneSubstitution");
				if (cp.name == "AscendDescendKey")
					tooltips.AddUIElement(ref mAscendDescendKey, cp, "KeyAscendDescend");
				if (cp.name == "VolumeFadeRate")
					tooltips.AddUIElement(ref mVolumeFadeRate, cp, "VolumeFadeRate");
				if (cp.name == "DominantInfluenceOutput")
					mDominantInfluenceOutput = cp.GetComponentInChildren<Text>();
				if (cp.name == "TonicInfluenceOutput")
					mTonicInfluenceOutput = cp.GetComponentInChildren<Text>();
				if (cp.name == "SubdominantInfluenceOutput")
					mSubdominantInfluenceOutput = cp.GetComponentInChildren<Text>();
				if (cp.name == "TritoneSubInfOutput")
					mTritoneSubInfluenceOutput = cp.GetComponentInChildren<Text>();
				if (cp.name == "AscendDescendKeyOutput")
					mAscendDescendKeyOutput = cp.GetComponentInChildren<Text>();
				if (cp.name == "VolumeFadeRateOutput")
					mVolumeFadeRateOutput = cp.GetComponentInChildren<Text>();
			}

			//these ui objects have a bunch of parts. Handled differently:
			for (int i = 0; i < mExcludedSteps.Count; i++)
			{
				Component[] components2 = this.GetComponentsInChildren(typeof(Transform), true);
				foreach (Component cp2 in components2)
				{
					if (cp2.name.Contains("Exclude"))
						tooltips.AddTooltip("TonicSubdominantDominantExcludes", cp2.GetComponent<RectTransform>());
				}
			}
		}

		//set code values from UI values
		void Update()
		{
			if(mMusicGenerator == null || mMusicGenerator.mState == eGeneratorState.initializing)
				return;

			ChordProgressions progressions = mMusicGenerator.mChordProgressions;
			progressions.SetTonicInflience(mTonicInfluence.value);
			progressions.SetDominantInfluence(mDominantInfluence.value);
			progressions.SetSubdominantInfluence(mSubdominantInfluence.value);
			progressions.SetTritoneSubInfluence(mTritoneSubInfluence.value);
			mMusicGenerator.SetKeyChangeAscendDescend(mAscendDescendKey.value);
			mMusicGenerator.SetGroupRate((uint)mGroupRate.value);
			mMusicGenerator.mDynamicStyle = (eDynamicStyle)mDynamicStyle.value;
			mMusicGenerator.SetVolFadeRate(mVolumeFadeRate.value);
			mMusicGenerator.SetAsyncLoadingUse(mAsyncLoading.isOn);
			mTonicInfluenceOutput.text = ((int)mTonicInfluence.value).ToString() + "%";
			mDominantInfluenceOutput.text = ((int)mDominantInfluence.value).ToString() + "%";
			mSubdominantInfluenceOutput.text = ((int)mSubdominantInfluence.value).ToString() + "%";
			mTritoneSubInfluenceOutput.text = ((int)mTritoneSubInfluence.value).ToString() + "%";
			mAscendDescendKeyOutput.text = ((int)mAscendDescendKey.value).ToString() + "%";
			mVolumeFadeRateOutput.text = ((int)mVolumeFadeRate.value).ToString();
			CheckAvoidSteps();
		}

		/// Toggles the panel animation:
		public void AdvancedPanelToggle()
		{
			if (mAnimator.GetInteger("mState") == 0)
				mAnimator.SetInteger("mState", 2);
			else
				mAnimator.SetInteger("mState", 0);
		}

		/// Checks the avoid steps. Hacky fix to make sure the user hasn't excluded an entire tonal type:
		public void CheckAvoidSteps()
		{
			for (int i = 0; i < mExcludedSteps.Count; i++)
			{
				mMusicGenerator.mChordProgressions.mExcludedProgSteps[i] = mExcludedSteps[i].isOn;
			}
			//idiot proofing:
			List<bool> excludes = mMusicGenerator.mChordProgressions.mExcludedProgSteps;
			if (excludes[0] && excludes[2] && excludes[5])
			{
				int exclude = 0;
				excludes[exclude] = false;
				mExcludedSteps[exclude].isOn = false;
			}
			if (excludes[1] && excludes[3])
			{
				int exclude = 1;
				excludes[exclude] = false;
				mExcludedSteps[exclude].isOn = false;
			}
			if (excludes[4] && excludes[6])
			{
				int exclude = 4;
				excludes[exclude] = false;
				mExcludedSteps[exclude].isOn = false;
			}
		}
	}
}