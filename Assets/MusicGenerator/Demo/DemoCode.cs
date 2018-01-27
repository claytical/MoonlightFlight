namespace ProcGenMusic
{
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DemoCode : MonoBehaviour {

	MusicGenerator mMusicGenerator = null;
	SingleClip mSingleClip = null;
	List<string> mConfigs = new List<string>{"DemoClip", "DemoClip2"};
	int mConfigIndex = 0;
	float mVolumeFadeRate = 15.0f;


	/// Our generator is a singleton (I know, I know, but it's for the best in this case, for a few reasons). Just grab a reference. 
	/// Important: Never try to access the instance within Awake(). Let it initialize itself from the gameObject.
	/// You can use the Prefab included with the asset and put in your scene to create it,
	/// or simply add the MusicGenerator script to an object in your app.
	void Start()
	{
			
		mMusicGenerator = MusicGenerator.Instance;
		mMusicGenerator.SetVolFadeRate(mVolumeFadeRate);	///< set the speed we fade. value multiplied against deltaT in MusicGenerator update().
	}

	/// to start the player, set the state to 'playing'
	public void Play(Slider volSlider)
	{
		mMusicGenerator.SetState(eGeneratorState.playing);
		mMusicGenerator.SetVolume(volSlider.value);
	}

	/// to stop the player, set the state to 'stopped'. This will also reset the player
	public void Stop()
	{
		mMusicGenerator.SetState(eGeneratorState.stopped);
	}

	/// to pause the player, set the state to 'paused'
	public void Pause()
	{
		mMusicGenerator.SetState(eGeneratorState.paused);
	}

	/// resets all our timing variables and starts over, but will continue playing:
	public void Reset()
	{
		mMusicGenerator.ResetPlayer();
	}

	/// You can change any of the public timing variables of the InstrumentSet class
	/// to adjust the global player settings:
	/// to change the tempo, adjust the instrumentSet mTempo variables.
	public void ChangeTempo(Slider sliderIN)
	{
		float tempo = sliderIN.value;
		mMusicGenerator.mInstrumentSet.SetTempo(tempo);
	}

	/// use the Generator's mMixer to adjust any global effects variables
	/// See the AudioSource properties in the Unity manual for details and sensible values: https://docs.unity3d.com/Manual/class-AudioSource.html
	/// See comments for MusicGenerator::SetGlobalEffect() for min/max values and mixer's effects names:
	public void AdjustGlobalDistortion(Slider sliderIN)
	{
		mMusicGenerator.mDistortion.Second = sliderIN.value;
		mMusicGenerator.SetGlobalEffect(mMusicGenerator.mDistortion);
	}

	/// Additionally you can adjust an individual instrument's effects as well:
	/// currently supported effect values: RoomSize, Reverb, Echo, EchoDelay, EchoDecay, Flanger, Distortion, Chorus
	/// See the Instrument class to add others. See Unity documnetation, or MusicGenerator::SetGlobalEffect() for min/max values on these.
	public void AdjustInstrumentEcho(Slider sliderIN)
	{
		List<Instrument>  instruments = mMusicGenerator.mInstrumentSet.mInstruments;
		int instrumentIndex = 1;
		instruments[instrumentIndex].SetEcho(sliderIN.value);/// This is mostly for saving, the actual set occurs below:
		mMusicGenerator.mMixer.SetFloat("Echo" + (instrumentIndex).ToString(), sliderIN.value);
	}

	/// Sets the mode for the generator's InstrumentSet. Along with the scale, this will change the 'feel' 
	/// of the music quite a lot.
	public void SetMode(Dropdown dropdownIN)
	{
		mMusicGenerator.mMode = (eMode)dropdownIN.value;
	}

	/// sets the key for the generator from the dropdown value
	public void SetKey(Dropdown dropdownIN)
	{
		mMusicGenerator.mKey = (eKey)dropdownIN.value;
	}

	/// sets the scale for the generator from the dropdown value
	public void SetScale(Dropdown dropdownIN)
	{
		mMusicGenerator.mScale = (eScale)dropdownIN.value;
	}

	/// Sets the first instrument's timestep to the dropdown value.
	/// Timestep will be how often the instrument plays per measure.
	/// for example: 1/16 plays 16 times per measure. 1/4 plays 4 times per measure
	public void SetTimestep(Dropdown dropdownIN)
	{
		mMusicGenerator.mInstrumentSet.mInstruments[0].SetTimestep((eTimestep)dropdownIN.value);
	}

	/// Loads a new Generator configuration. This includes loads new instruments, key, scale, mode, tempo...everything.
	/// Use the included executable program to create new configurations.
	public void LoadConfig()
	{
		mConfigIndex = mConfigIndex == 0 ? 1 : 0;
		StartCoroutine(mMusicGenerator.FadeLoadConfiguration(mConfigs[mConfigIndex]));
	}

	/// Sets the global volume.
	public void SetVolume(Slider sliderIN)
	{
		mMusicGenerator.SetVolume(sliderIN.value);
	}

	/// plays a single clip. Loads if necessary.
	public void PlayClip(Toggle isRepeatingToggle)
	{
		if(mSingleClip == null)
		{
			mSingleClip = gameObject.AddComponent<SingleClip> ();
			string clipName = "AAADefault.txt"; /// This already exists in the IntrumentClips folder. we load it below with Init().
			mSingleClip.Init(MusicFileConfig.LoadClipConfigurations(clipName));
			mSingleClip.mIsRepeating = isRepeatingToggle.isOn;
		}
	//	mSingleClip.ResetClip();///Just in case it was already playing
		mSingleClip.SetState(eClipState.Play);
	}

	/// Sets whether our clip repeats or plays once.
	public void RepeatClip(Toggle toggleIN)
	{
		if(mSingleClip == null)
		{
			mSingleClip = gameObject.AddComponent<SingleClip> ();
			string clipName = "AAADefault.txt"; /// This already exists in the IntrumentClips folder. we load it below with Init().
			mSingleClip.Init( MusicFileConfig.LoadClipConfigurations(clipName));
		}
		mSingleClip.mIsRepeating = toggleIN.isOn;
	}

	/// Fades volume in/ out
	public void Fade()
	{
		switch(mMusicGenerator.mVolumeState)
		{
			case eVolumeState.fadingOut:
			case eVolumeState.fadedOutIdle:
				mMusicGenerator.VolumeFadeIn();
				break;
			case eVolumeState.fadingIn:
			case eVolumeState.idle:
				mMusicGenerator.VolumeFadeOut();
				break;
			default:
				break;
		}
	}
}
}