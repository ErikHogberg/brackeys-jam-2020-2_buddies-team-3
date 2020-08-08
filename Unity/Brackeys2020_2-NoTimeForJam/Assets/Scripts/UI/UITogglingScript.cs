using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITogglingScript : MonoBehaviour
{

	public bool StartOn = false;
	public Toggle OptionalToggle;
	public GameObject[] ObjectsToToggle;

	private void Start()
	{
		if (!OptionalToggle)
		{
			OptionalToggle = GetComponent<Toggle>();
		}

		if (OptionalToggle)
		{
			Toggle(OptionalToggle.isOn);
		}
		else
		{
			Toggle(StartOn);
		}
	}

	public void Toggle(bool value)
	{
		foreach (var item in ObjectsToToggle)
		{
			item?.SetActive(value);
		}
	}

}
