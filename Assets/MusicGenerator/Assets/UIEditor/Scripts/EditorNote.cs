namespace ProcGenMusic
{
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// For the measure editor. staff player notes that know if they're clicked on.
public class EditorNote : MonoBehaviour, IPointerClickHandler
 {

	public Image mBaseImage = null;
	public Vector2 index = new Vector2(0, 0);

	private StaffPlayerUI mStaffPlayer = null;
	public void Init(StaffPlayerUI staffPlayer){mStaffPlayer = staffPlayer;}

	/// we remove this note from the measure editor when right clicked.
	public void OnPointerClick(PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Right)
			mStaffPlayer.RemoveNote(this);
	}

	void Awake()
	{
		Component[] components =	GetComponentsInChildren (typeof(Image),true);
		foreach (Component cp in components)
		{
			if(cp.name == "noteImage")
				mBaseImage = cp.GetComponent<Image>();
		}
	}
}
}