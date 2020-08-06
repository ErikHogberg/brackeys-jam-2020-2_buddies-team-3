using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RbDoor : Triggerable, IResettable
{

	public float Speed = 100f;

	public Transform Target;

	private Vector3 targetPos;
	private Vector3 initPos;

	bool running = false;
	bool runningForward = true;

	Rigidbody2D rb;


	// Start is called before the first frame update
	void Start()
	{
		targetPos = Target.position;
		initPos = transform.position;

		rb = GetComponent<Rigidbody2D>();
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
	void Update()
	{
		if (!running)
		{
			return;
		}

		if (runningForward)
		{
			rb.MovePosition(
			// transform.position = 
			Vector3.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime)
			);

			if (transform.position == targetPos)
			{
				running = false;
			}
		}
		else
		{
			rb.MovePosition(
			// transform.position = 
			Vector3.MoveTowards(transform.position, initPos, Speed * Time.deltaTime)
			);
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

	public void ResetToInit()
	{
		rb.MovePosition(initPos);
		running = false;
		runningForward = true;

	}

}
