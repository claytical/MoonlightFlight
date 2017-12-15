namespace ProcGenMusic
{
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//just a basic class for the ui slider to set values, update text, handle tooltip, etc:
public class EffectsOption 
{
	private float mBaseValue = 0.0f;
	public Pair<string, float> mGeneratorValue = new Pair<string, float>();
	public Slider mSlider = null;
	public Text mOutput = null;
	public Button mResetButton = null;

	public EffectsOption( Pair<string, float> generatorValueIN, Tooltips tooltipsIN, Component cp,float baseIN)
	{
		mBaseValue = baseIN;
		mGeneratorValue = generatorValueIN;
		tooltipsIN.AddUIElement (ref mSlider, cp, mGeneratorValue.First);
		mResetButton = cp.GetComponentInChildren<Button>();
		mResetButton.onClick.AddListener (ResetSlider);

		Component[] components = cp.gameObject.GetComponentsInChildren (typeof(Transform),true); 
		foreach (Component cp2 in components)
		{
			if (cp2.name == generatorValueIN.First + "Output")
				mOutput = cp2.GetComponentInChildren<Text> ();		
		}
		Init ();
	}

	public void Init()
	{
		mOutput.text = mGeneratorValue.Second.ToString ();
		mSlider.value = mGeneratorValue.Second;
	}

	/// Sets the music generator value from the slider each tick.
	public void Update()
	{
		mGeneratorValue.Second = mSlider.value;
		mOutput.
		text = mSlider.value.ToString ();
	}

	/// Resets the value to the base value.
	public void ResetSlider()
	{
		mSlider.value = mBaseValue;
	}
};
	
/// UI Global effects pannel. Handles sliders, creation of tooltips, setting values, etc.
public class GlobalEffectsPanel : HelperSingleton<GlobalEffectsPanel> 
{
	//reset values. We can't bypass the effect on the master group entirely 
	//via scripting (I don't think), so these are essentially minimizing the effect to nothing.
	static readonly private float mBaseCenterFrequency = 226.0f;
	static readonly private float mBaseOctaveRange = 3.78f;
	static readonly private float mBaseFrequencyGain = 1.63f;
	static readonly private float mBaseLowpassCutoffFreq = 22000.00f;
	static readonly private float mBaseLowpassResonance = 1.00f;
	static readonly private float mBaseHighpassCutoffFreq = 10.00f;
	static readonly private float mBaseHighpassResonance = 1.00f;
	static readonly private float mBaseEchoDelay = 13.0f;
	static readonly private float mBaseEchoDecay = .23f;
	static readonly private float mBaseEchoDry = 100.0f;
	static readonly private float mBaseEchoWet = 0.0f;
	static readonly private float mBaseNumEchoChannels = 0.00f;
	static readonly private float mBaseReverb = -10000.00f;
	static readonly private float mBaseRoomSize = -10000.00f;
	static readonly private float mBaseReverbDecay = 0.1f;

	private MusicGenerator mMusicGenerator = null;
	private Animator mAnimator = null;

	public EffectsOption mDistortion = null;
	public EffectsOption mCenterFrequency = null;
	public EffectsOption mOctaveRange = null;
	public EffectsOption mFrequencyGain = null;
	public EffectsOption mLowpassCutoffFreq = null;
	public EffectsOption mLowpassResonance = null;
	public EffectsOption mHighpassCutoffFreq = null;
	public EffectsOption mHighpassResonance = null;
	public EffectsOption mEchoDelay = null;
	public EffectsOption mEchoDecay = null;
	public EffectsOption mEchoDry = null;
	public EffectsOption mEchoWet = null;
	public EffectsOption mNumEchoChannels = null;
	public EffectsOption mReverb = null;
	public EffectsOption mRoomSize = null;
	public EffectsOption mReverbDecay = null;
	public List<EffectsOption> mOptions = new List<EffectsOption> ();

	public void Init (MusicGenerator managerIN) {
		mMusicGenerator = managerIN;
		Tooltips tooltips = UIManager.Instance.mTooltips;
		mAnimator = GetComponentInParent<Animator> ();

		/// we create an EffectsOption for each slider, which will set its base value, tooltip, etc.
		Component[] components = this.GetComponentsInChildren (typeof(Transform),true); 
		foreach (Component cp in components)
		{
			if (cp.name == "MasterDistortion")
				mOptions.Add (mDistortion = new EffectsOption (mMusicGenerator.mDistortion, tooltips, cp, 0.0f));
			if (cp.name == "MasterCenterFrequency")
				mOptions.Add(mCenterFrequency = new EffectsOption (mMusicGenerator.mCenterFreq, tooltips, cp, mBaseCenterFrequency));
			if(cp.name == "MasterOctaveRange")
				mOptions.Add(mOctaveRange = new EffectsOption (mMusicGenerator.mOctaveRange, tooltips, cp, mBaseOctaveRange));
			if(cp.name == "MasterFrequencyGain")
				mOptions.Add(mFrequencyGain = new EffectsOption (mMusicGenerator.mFreqGain, tooltips, cp, mBaseFrequencyGain));
			if (cp.name == "MasterLowpassCutoffFreq")
				mOptions.Add(mLowpassCutoffFreq = new EffectsOption (mMusicGenerator.mLowpassCutoffFreq, tooltips, cp, mBaseLowpassCutoffFreq));
			if(cp.name == "MasterLowpassResonance")
				mOptions.Add(mLowpassResonance = new EffectsOption (mMusicGenerator.mLowpassResonance, tooltips, cp, mBaseLowpassResonance));
			if (cp.name == "MasterHighpassCutoffFreq")
				mOptions.Add(mHighpassCutoffFreq = new EffectsOption (mMusicGenerator.mHighpassCutoffFreq, tooltips, cp, mBaseHighpassCutoffFreq));
			if(cp.name == "MasterHighpassResonance")
				mOptions.Add(mHighpassResonance = new EffectsOption (mMusicGenerator.mHighpassResonance, tooltips, cp, mBaseHighpassResonance));
			if(cp.name == "MasterEchoDelay")
				mOptions.Add(mEchoDelay = new EffectsOption (mMusicGenerator.mEchoDelay, tooltips, cp, mBaseEchoDelay));
			if(cp.name == "MasterEchoDecay")
				mOptions.Add(mEchoDecay = new EffectsOption (mMusicGenerator.mEchoDecay, tooltips, cp, mBaseEchoDecay));
			if (cp.name == "MasterEchoDry")
				mOptions.Add(mEchoDry = new EffectsOption (mMusicGenerator.mEchoDry, tooltips, cp, mBaseEchoDry));
			if(cp.name == "MasterEchoWet")
				mOptions.Add(mEchoWet = new EffectsOption (mMusicGenerator.mEchoWet, tooltips, cp, mBaseEchoWet));
			if(cp.name == "MasterNumEchoChannels")
				mOptions.Add(mNumEchoChannels = new EffectsOption (mMusicGenerator.mNumEchoChannels, tooltips, cp, mBaseNumEchoChannels));
			if(cp.name == "MasterReverb")
				mOptions.Add(mReverb = new EffectsOption (mMusicGenerator.mReverb, tooltips, cp, mBaseReverb));
			if(cp.name == "MasterRoomSize")
				mOptions.Add(mRoomSize = new EffectsOption (mMusicGenerator.mRoomSize, tooltips, cp, mBaseRoomSize));
			if(cp.name == "MasterReverbDecay")
				mOptions.Add(mReverbDecay = new EffectsOption (mMusicGenerator.mReverbDecay, tooltips, cp, mBaseReverbDecay));
		}
	}

	void Update () 
	{
		/// updates all our effects from the sliders:
		for (int i = 0; i < mOptions.Count; i++)
			mOptions [i].Update ();
	}

	/// Toggles the effects panel animation:
	public void GlobalEffectsPanelToggle()
	{
		if(mAnimator.GetInteger("mState") == 0)
			mAnimator.SetInteger("mState", 3);
		else
			mAnimator.SetInteger("mState", 0);
	}
}
}