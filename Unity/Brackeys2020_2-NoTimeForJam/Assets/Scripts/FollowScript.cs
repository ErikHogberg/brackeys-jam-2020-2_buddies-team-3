using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour
{

	public GameObject FollowTarget;

	public Vector2 Buffer = Vector2.zero;

	public bool UseOffset = false;
	public bool LockRotation = false;

	Vector3 offset;

	private void Start()
	{
		offset = transform.position - FollowTarget.transform.position;
	}

	private void LateUpdate()
	{
		Vector3 posOffset = Vector3.zero;
		if (UseOffset)
			posOffset = offset;

		Vector3 delta = transform.position - FollowTarget.transform.position - posOffset;

		Vector3 outPos = transform.position;
		if (Mathf.Abs(delta.x) > Buffer.x)
		{
			if (transform.position.x > FollowTarget.transform.position.x - posOffset.x)
			{
				outPos.x = FollowTarget.transform.position.x + posOffset.x + Buffer.x;
			}
			else
			{
				outPos.x = FollowTarget.transform.position.x + posOffset.x - Buffer.x;
			}
		}

		if (Mathf.Abs(delta.y) > Buffer.y)
		{
			if (transform.position.y > FollowTarget.transform.position.y - posOffset.y)
			{

				outPos.y = FollowTarget.transform.position.y + posOffset.y + Buffer.y;
			}
			else
			{
				outPos.y = FollowTarget.transform.position.y + posOffset.y - Buffer.y;

			}
		}

		transform.position = outPos;

	}

}
