using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : Triggerable
{

	public bool StartOnInit = false;
    
	public bool Loop = false;
    public float Speed = 100f;

	public Transform[] Targets;

	private Vector3 initPos;

	bool running = false;
	bool runningForward = true;

	// Start is called before the first frame update
	void Start()
	{
		initPos = transform.position;
		// running = StartOnInit;
	}

	// Update is called once per frame
	void Update()
	{
		if (!running)
		{
			return;
		}


	}

	public override void Press()
	{
		running = true;
		runningForward = true;
	}
	public override void Release()
	{
		running = false;
		// runningForward = false;
	}

}
