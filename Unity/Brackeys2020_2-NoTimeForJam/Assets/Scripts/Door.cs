using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Triggerable
{

	public float Speed = 100f;

	public Transform Target;

	private Vector3 targetPos;
	private Vector3 initPos;

	bool running = false;
	bool runningForward = true;


	// Start is called before the first frame update
	void Start()
	{
        targetPos = Target.position;
		initPos = transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		if (!running)
		{
			return;
		}

		if (runningForward)
		{
			transform.position = Vector3.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
			if (transform.position == targetPos)
			{
				running = false;
			}
		}
		else
		{
			transform.position = Vector3.MoveTowards(transform.position, initPos, Speed * Time.deltaTime);
			if (transform.position == initPos)
			{
				running = false;
			}
		}
	}

	public override void Press()
	{
		running = true;
		runningForward = true;
	}
	public override void Release()
	{
		running = true;
		runningForward = false;
	}

}
