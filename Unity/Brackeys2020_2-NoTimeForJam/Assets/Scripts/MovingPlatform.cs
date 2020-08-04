using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : Triggerable
{

	public bool StartOnInit = false;

	public bool Loop = false;
	public float Speed = 100f;

	private int currentTarget = 1;
	public Transform[] Targets;
	List<Vector3> targetPositions = new List<Vector3>();

	// private Vector3 initPos;

	bool running = false;
	bool runningForward = true;

	// Start is called before the first frame update
	void Start()
	{
		// initPos = transform.position;
		running = StartOnInit;

		targetPositions.Add(transform.position);
		foreach (var item in Targets)
		{
			targetPositions.Add(item.position);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (!running)
		{
			return;
		}

		if (targetPositions.Count < 2)
		{
			running = false;
			return;
		}

		Vector3 targetPos = targetPositions[currentTarget];
		if (runningForward)
		{

			transform.position = Vector3.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
			if (transform.position == targetPos)
			{
				currentTarget++;
				if (currentTarget >= targetPositions.Count)
				{
					currentTarget -= 2;
					if (Loop)
					{
						runningForward = false;
					}
					else
					{
						running = false;
					}
				}
			}
		}
		else
		{
			transform.position = Vector3.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
			if (transform.position == targetPos)
			{
				currentTarget--;
				if (currentTarget < 0)
				{
					currentTarget = 1;
					if (Loop)
					{
						runningForward = true;
					}
					else
					{
						running = false;
					}
				}
			}
		}


	}

	public override void Press()
	{
		if (!Loop)
		{
			if (StartOnInit)
			{
                if (running && runningForward)
                {
                    currentTarget --;
                }
				running = true;
				runningForward = false;
			}
			else
			{
                if (running && !runningForward)
                {
                    currentTarget ++;
                }
				running = true;
				runningForward = true;
			}
		}
		else
		{
			running = !StartOnInit;
		}
		// runningForward = true;
	}
	public override void Release()
	{
		if (!Loop)
		{
			if (!StartOnInit)
			{
                 if (running && runningForward)
                {
                    currentTarget --;
                }
				running = true;
				runningForward = false;
			}
			else
			{
                 if (running && !runningForward)
                {
                    currentTarget ++;
                }
				running = true;
				runningForward = true;
			}
		}
		else
		{
			running = StartOnInit;
		}
	}

    // private void OnCollisionEnter2D(Collision2D other) {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         other.transform.parent = transform;
    //     }
    // }

    // private void OnCollisionExit2D(Collision2D other) {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         other.transform.parent = transform.parent;
    //     }
    // }

}
