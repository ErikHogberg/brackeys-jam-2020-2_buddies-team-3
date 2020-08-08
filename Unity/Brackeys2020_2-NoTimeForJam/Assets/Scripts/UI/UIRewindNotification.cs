using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRewindNotification : MonoBehaviour
{

	static UIRewindNotification mainInstance = null;

    public GameObject NotificationParent;

	private void Awake()
	{
		mainInstance = this;
        Hide();
	}

	private void OnDestroy()
	{
		mainInstance = null;
	}

	public static void Show()
	{
        mainInstance?.NotificationParent?.SetActive(true);
	}

	public static void Hide()
	{
        mainInstance?.NotificationParent?.SetActive(false);

	}


}
