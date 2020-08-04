using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlatform : Triggerable
{
	private void OnCollisionExit2D(Collision2D other)
	{
		Debug.Log("breaking platform collision exit something");
		if (other.gameObject.CompareTag("Player"))
		{
			Debug.Log("breaking platform collision exit player");
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
