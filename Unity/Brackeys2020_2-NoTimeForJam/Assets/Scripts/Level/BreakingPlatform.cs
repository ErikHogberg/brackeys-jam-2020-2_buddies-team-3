using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlatform : Triggerable
{
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

}
