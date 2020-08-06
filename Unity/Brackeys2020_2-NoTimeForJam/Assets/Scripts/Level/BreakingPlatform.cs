using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlatform : Triggerable, IResettable
{

	private void Awake() {
		Globals.Resettables.Add(this);
	}

	private void OnDestroy() {
		Globals.Resettables.Remove(this);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("GroundDetection"))
		{
			gameObject.SetActive(false);
		}
	}

	public override void Press()
	{
		gameObject.SetActive(true);
	}
	public override void Release()
	{
	}

	public void ResetToInit()
	{
		gameObject.SetActive(true);
	}


}
