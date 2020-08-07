using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : Triggerable, IResettable
{

	public bool StartOnInit = false;

	public bool Loop = false;
	public bool LoopToBeginning = false;
	public float Speed = 100f;

	private int currentTarget = 1;
	public Transform[] Targets;
	List<Vector3> targetPositions = new List<Vector3>();

	// private Vector3 initPos;
	Rigidbody2D rb;

	bool running = false;
	bool runningForward = true;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();

		// initPos = transform.position;
		running = StartOnInit;

		targetPositions.Add(transform.position);
		foreach (var item in Targets)
		{
			targetPositions.Add(item.position);
		}
	}

	private void Awake()
	{
		Globals.Resettables.Add(this);
	}

	private void OnDestroy()
	{
		Globals.Resettables.Remove(this);
	}

	// Update is called once per frame
	void FixedUpdate()
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

		float oldX = transform.position.x;

		Vector3 targetPos = targetPositions[currentTarget];
		if (runningForward)
		{

			if (rb)
			{
				rb.MovePosition(Vector3.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime));
			}
			else
			{
				transform.position = Vector3.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
			}

			if (transform.position == targetPos)
			{
				currentTarget++;
				if (currentTarget >= targetPositions.Count)
				{
					currentTarget -= 2;
					if (Loop)
					{
						if (LoopToBeginning)
						{
							currentTarget = 0;
						}
						else
						{
							runningForward = false;
						}
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

			if (rb)
			{
				rb.MovePosition(Vector3.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime));
			}
			else
			{
				transform.position = Vector3.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
			}

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
					currentTarget--;
				}
				running = true;
				runningForward = false;
			}
			else
			{
				if (running && !runningForward)
				{
					currentTarget++;
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
					currentTarget--;
				}
				running = true;
				runningForward = false;
			}
			else
			{
				if (running && !runningForward)
				{
					currentTarget++;
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

	public void ResetToInit()
	{
		transform.position = targetPositions[0];
		running = StartOnInit;
		runningForward = true;
		currentTarget = 1;
	}

}
