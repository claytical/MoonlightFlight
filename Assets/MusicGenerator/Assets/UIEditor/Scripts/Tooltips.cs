namespace ProcGenMusic
{
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Tooltips : HelperSingleton<Tooltips>
{
	public GameObject mTooltipBase = null;
	private Dictionary<string, string> mTooltips = new Dictionary<string, string>();
	[SerializeField]
	private Canvas mTooltipCanvas = null;

	public override void Awake()
	{
		base.Awake();
	}

	public void Init()
	{
		TooltipSave save = MusicFileConfig.LoadTooltips();
		for(int i = 0; i < save.mTooltips.Count; i++)
		{
			mTooltips.Add(save.mTooltips[i].mTooltips[0],save.mTooltips[i].mTooltips[1]);
		}
		/// For saving the tooltips:
		//MusicGenerator.Instance.mConfigurations.SaveTooltips("tooltips", save);
	}

	/// creates and returns a tooltip game object.
	public GameObject AddTooltip(string jsonIndexIN, RectTransform parentIN)
	{
		GameObject tooltip = Instantiate(mTooltipBase);
		tooltip.GetComponent<Tooltip>().mDescription = mTooltips[jsonIndexIN];
		tooltip.GetComponent<Tooltip>().Init();
		RectTransform tooltipRect = tooltip.GetComponent<RectTransform>();
		tooltip.GetComponent<Tooltip>().mParentRect = parentIN;

		//Just an easy fix to get them at the top of the sorting layer for the canvas.
		//not ideal, but it works.
		tooltip.transform.SetParent(parentIN, false);
		tooltip.GetComponent<Tooltip>().mTooltipObject.transform.SetParent(mTooltipCanvas.transform, false);

		//resize the raycastable image to its parent:
		tooltipRect.sizeDelta = new Vector2(parentIN.rect.width, parentIN.rect.height);
		tooltipRect.position = new Vector2(parentIN.position.x, parentIN.position.y);
		tooltipRect.pivot = new Vector2(parentIN.pivot.x, parentIN.pivot.y);

		return tooltip;
	}

	/// This will handle creating references for the tooltip / ui element (i.e. slider, dropdown, etc.)
	public void AddUIElement<T>(ref T objIN, Component cp, string nameIN) where T : Component
	{
		objIN = cp.gameObject.GetComponentInChildren<T>();
		if (objIN == null)
			objIN = cp.gameObject.GetComponent<T>();

		bool handleFound = false;
		Component[] components = objIN.transform.parent.GetComponentsInChildren(typeof(Transform), true);
		foreach (Component cp2 in components)
		{
			if (cp2.name.Contains("Title"))
				AddTooltip(nameIN, cp2.GetComponent<RectTransform>());
			if (cp2.name == "TooltipHandle")
			{
				handleFound = true;
				AddTooltip(nameIN, cp2.gameObject.GetComponent<RectTransform>());
			}
		}
		if (!handleFound)
			AddTooltip(nameIN, objIN.GetComponent<RectTransform>());
	}
}
}