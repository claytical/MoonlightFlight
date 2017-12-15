namespace ProcGenMusic
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	/// UI for instrument list
	public class InstrumentListPanelUI : HelperSingleton<InstrumentListPanelUI>
	{
		public List<InstrumentListUIObject> mInstrumentIcons { get; private set; }
		private MusicGenerator mMusicGenerator = null;
		private RectTransform mAddInstrumentPoint = null;
		private Vector3 mBaseAddInstrumentPos;
		[SerializeField]
		private float mIconPadding = 1.05f;
		private GameObject mInstrumentUIObjectBase = null;

		public override void Awake()
		{
			base.Awake();
			CreateInstrumentUIObjectBase();
			mInstrumentIcons = new List<InstrumentListUIObject>();
		}
		
		public void Init(MusicGenerator managerIN)
		{
			mMusicGenerator = managerIN;
			Tooltips tooltiops = UIManager.Instance.mTooltips;
			Component[] components = this.GetComponentsInChildren(typeof(Transform), true);
			foreach (Component cp in components)
			{
				if (cp.name == "AddInstrumentPoint")
				{
					mAddInstrumentPoint = cp.gameObject.GetComponent<RectTransform>();
					mBaseAddInstrumentPos = mAddInstrumentPoint.localPosition;
				}
				if (cp.name == "NewInstrumentButton")
				{
					tooltiops.AddTooltip("NewInstrument", cp.gameObject.GetComponent<RectTransform>());
				}
			}
		}

		/// creates our base ui object to instantiate other instruments.
		private void CreateInstrumentUIObjectBase()
		{
			string platform = "/Windows";
			if (Application.platform == RuntimePlatform.LinuxPlayer || Application.platform == RuntimePlatform.LinuxEditor)
				platform = "/Linux";
			if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
				platform = "/Mac";

			string path = Application.streamingAssetsPath + "/MusicGenerator" + platform + "/InstrumentUIObject";
			if (!System.IO.File.Exists(path))
			{
				Debug.Log("InstrumentUIObject asset bundle does not exist.");
				throw new System.ArgumentNullException("InstrumentUIObject base file does not exist.");
			}
			else
			{
				var myLoadedAssetBundle = AssetBundle.LoadFromFile(path);

				if (myLoadedAssetBundle != null)
					mInstrumentUIObjectBase = myLoadedAssetBundle.LoadAsset<GameObject>("InstrumentUIObject");
			}
		}

		/// Adds an instrument to our ui object list:
		public void AddInstrument()
		{
			InstrumentSet set = (mMusicGenerator.mState >= eGeneratorState.editorInitializing) ? MeasureEditor.Instance.mCurrentInstSet : mMusicGenerator.mInstrumentSet;
			List<Instrument> instruments = set.mInstruments;
			if (instruments.Count <= MusicGenerator.mMaxInstruments)
			{
				mInstrumentIcons.Add((Instantiate(mInstrumentUIObjectBase, transform)as GameObject).GetComponent<InstrumentListUIObject>());
				InstrumentListUIObject icon = mInstrumentIcons[mInstrumentIcons.Count - 1];
				icon.Init(mMusicGenerator);
				icon.transform.position = mAddInstrumentPoint.transform.position;
				mAddInstrumentPoint.localPosition -= new Vector3(0, mAddInstrumentPoint.rect.height * mIconPadding, 0);
			}
		}

		/// Adds an instrument to the Music generator and creates its ui object.
		public void AddMusicGeneratorInstrument(bool isPercussion)
		{
			InstrumentSet set = (mMusicGenerator.mState >= eGeneratorState.editorInitializing) ? MeasureEditor.Instance.mCurrentInstSet : mMusicGenerator.mInstrumentSet;
			List<Instrument> instruments = set.mInstruments;
			if (instruments.Count < MusicGenerator.mMaxInstruments)
			{
				mMusicGenerator.AddInstrument(set);
				AddInstrument();
				InstrumentListUIObject icon = mInstrumentIcons[mInstrumentIcons.Count - 1];

				icon.mInstrument = instruments[instruments.Count - 1];
				Color color = StaffPlayerUI.Instance.mColors[(int)icon.mInstrument.mStaffPlayerColor];
				icon.mPanelBack.color = color;

				icon.SetDropdown(isPercussion);
			}
		}

		/// Removes an instrument from our list. Fixes icon positions:
		public void RemoveInstrument(int indexIN)
		{
			for (int i = indexIN; i < mInstrumentIcons.Count; i++)
			{
				mAddInstrumentPoint.localPosition +=
					new Vector3(0, mAddInstrumentPoint.rect.height * mIconPadding, 0);
			}
			for (int i = indexIN + 1; i < mInstrumentIcons.Count; i++)
			{
				mInstrumentIcons[i].transform.position = mAddInstrumentPoint.transform.position;
				mAddInstrumentPoint.localPosition -=
					new Vector3(0, mAddInstrumentPoint.rect.height * mIconPadding, 0);
			}
			Destroy(mInstrumentIcons[indexIN].gameObject);
			mInstrumentIcons.RemoveAt(indexIN);
			InstrumentPanelUI.Instance.SetInstrument(null);
		}

		/// deletes all ui instrument objects.
		public void ClearInstruments()
		{
			if (mInstrumentIcons.Count == 0)
				return;
			for (int i = mInstrumentIcons.Count - 1; i >= 0; i--)
			{
				Destroy(mInstrumentIcons[i].gameObject);
			}
			mInstrumentIcons.Clear();
			mAddInstrumentPoint.localPosition = mBaseAddInstrumentPos;
		}
	}
}