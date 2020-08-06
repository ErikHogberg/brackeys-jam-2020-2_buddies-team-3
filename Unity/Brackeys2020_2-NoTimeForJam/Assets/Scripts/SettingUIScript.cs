using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class SettingUIScript : MonoBehaviour
{

	public TMP_Dropdown ResolutionDropdown;

	bool fullscreen = false;
	int width = 1920;
	int height = 1080;
	int hz;

	private void Start()
	{
		width = Screen.width;
		height = Screen.height;
		hz = Screen.currentResolution.refreshRate;

		List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

		foreach (var item in Screen.resolutions)
		{
			options.Add(new TMP_Dropdown.OptionData(
				item.width.ToString()
				 + "x" + item.width.ToString()
				  + ", " + item.refreshRate.ToString() + "hz"
			));
		}

		ResolutionDropdown.ClearOptions();
		ResolutionDropdown.options = options;

	}

	public void ToggleFullscreen(bool value)
	{
		fullscreen = value;
	}

	public void ApplyButton()
	{
		Screen.SetResolution(width, height, fullscreen, hz);
	}

	public void SelectResolutionDropdown(int value)
	{
		var option = Screen.resolutions[value];
		width = option.width;
		height = option.height;
		hz = option.refreshRate;

	}

	public void WidthInput(string value)
	{
		if (value == "")
		{
			width = 1920;
		}
		else
		{
			width = Int32.Parse(value);
		}

	}
	public void HeightInput(string value)
	{
		if (value == "")
		{
			height = 1080;
		}
		else
		{
			height = Int32.Parse(value);
		}

	}

}
