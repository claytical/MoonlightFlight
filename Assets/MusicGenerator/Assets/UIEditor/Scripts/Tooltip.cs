using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// tooltip class. Basically just a frame sprite with some text info that knows whether the mouse is hovered over or not.
public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public string mDescription = "";
	public bool mIsHovered = false;
	private float mXOffset = 3.0f;
	private float mYOffset = 1.0f;
	public RectTransform mParentRect = null;
	private Text mTooltipText = null;
	public GameObject mTooltipObject = null;
	public void Init()
	{
		Component[] components = this.GetComponentsInChildren (typeof(Transform),true); 
		foreach (Component cp in components)
		{
			if(cp.name == "Text")
				mTooltipText = cp.GetComponent<Text> ();
			if (cp.name == "TooltipObject")
				mTooltipObject = cp.gameObject;
		}
		mTooltipText.text = mDescription;
		mTooltipObject.SetActive (false);
	}

	public void OnPointerEnter (PointerEventData eventData) 
	{
		mIsHovered = true;
	}

	public void OnPointerExit (PointerEventData eventData) 
	{
		mIsHovered = false;
	}

	void Update()
	{
		if(!mParentRect)
			return;

		bool shiftHeld =  Input.GetKey ("left shift");
		mTooltipObject.SetActive (mIsHovered && shiftHeld);
		if(mIsHovered && shiftHeld)
		{
			Vector2 offset = new Vector2 (0, 0);
			offset.x = (Input.mousePosition.x > Screen.width / 2) ? -mXOffset : mXOffset;
			offset.y = (Input.mousePosition.y > Screen.height / 2) ? -mYOffset : mYOffset;
			mTooltipObject.GetComponent<RectTransform> ().position = new Vector2(mParentRect.position.x, mParentRect.position.y) +  offset;
		}	
	}
}
