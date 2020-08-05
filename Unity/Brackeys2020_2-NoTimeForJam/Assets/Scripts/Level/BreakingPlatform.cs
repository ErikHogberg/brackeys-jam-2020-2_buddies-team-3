using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlatform : Triggerable
{
	private void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Player"))
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
