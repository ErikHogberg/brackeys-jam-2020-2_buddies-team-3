using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class UITimer : MonoBehaviour
{
	static UITimer mainInstance = null;

	TMP_Text text;

	public Color RewindColor;
	Color initColor;

	bool wasRewind = false;

	private void Start()
	{
		text = GetComponent<TMP_Text>();
		initColor = text.color;

		mainInstance = this;
	}

	private void OnDestroy()
	{
		mainInstance = null;
	}

	public static void SetTime(float time, bool rewind)
	{
		if (!mainInstance)
		{
			return;
		}

		if (rewind)
		{
			if (!mainInstance.wasRewind)
			{
				mainInstance.text.color = mainInstance.RewindColor;
			}
		}
		else
		{
			if (mainInstance.wasRewind)
			{
				mainInstance.text.color = mainInstance.initColor;
			}
		}
		mainInstance.wasRewind = rewind;

		var timeSpan = TimeSpan.FromSeconds(time);
		if (mainInstance)
		{
			string str = timeSpan.Milliseconds.ToString("00");
			if (str.Length > 2)
				str = str.Substring(0, 2);

			mainInstance.text.text = timeSpan.Seconds.ToString() + ":"
			+ str;
		}
	}
}
