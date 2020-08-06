using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

using static UnityEngine.InputSystem.InputAction;

public class Character : MonoBehaviour, IComparable<Character>
{
	private enum InputType
	{
		PressLeft,
		ReleaseLeft,
		PressRight,
		ReleaseRight,
		Jump
	}

	private class InputHistoryEntry
	{
		public InputType input;
		public float timeStamp;
	}

	public static int CurrentCharacterIndex = 0;
	private static List<Character> instances = new List<Character>();

	public int CharacterNumber = 0;

	[Min(0)]
	public float Speed = 100f;
	[Min(0)]
	public float MinSpeed = 5f;
	public bool UseMinSpeedInAir = false;
	[Space]

	[Range(0f, 1f)]
	public float AirSpeedPercentage = .5f;
	[Min(0)]
	public float JumpForce = 2f;
	public float JumpSteeringMax = 1f;
	public float CoyoteTime = .1f;
	[Space]
	public bool RecordInput = true;
	public float VelocityCap = 100f;

	[Space]
	public InputActionReference LeftBinding;
	public InputActionReference RightBinding;
	public InputActionReference JumpBinding;
	public InputActionReference ResetBinding;

	public bool active => CurrentCharacterIndex < instances.Count && this == instances[CurrentCharacterIndex];
	bool moving => Mathf.Abs(xDir) > 0;
	bool recording => RecordInput && active;

	Rigidbody2D rb;
	SpriteRenderer[] spriteRenderers;

	float xDir = 0f;
	Vector3 initPos;
	bool touchingGround => groundTimer < CoyoteTime;
	float groundTimer = 0f;
	bool jumpAllowed = true;

	float timer = 0f;

	Queue<InputHistoryEntry> inputHistory = new Queue<InputHistoryEntry>();
	InputHistoryEntry nextEntry;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

		LeftBinding.action.started += PressLeft;
		RightBinding.action.started += PressRight;
		JumpBinding.action.started += PressJump;
		ResetBinding.action.started += PressReset;

		LeftBinding.action.canceled += ReleaseLeft;
		RightBinding.action.canceled += ReleaseRight;
		JumpBinding.action.canceled += ReleaseJump;

		LeftBinding.action.Enable();
		RightBinding.action.Enable();
		JumpBinding.action.Enable();
		ResetBinding.action.Enable();

	}

	private void OnEnable()
	{
		initPos = transform.position;

		instances.Add(this);
		instances.Sort();
	}

	private void OnDisable()
	{
		instances.Remove(this);
	}

	public void Restart()
	{
		timer = 0;
		transform.position = initPos;
		rb.velocity = Vector2.zero;
		rb.angularVelocity = 0f;
		transform.rotation = Quaternion.identity;

		xDir = 0;
	}

	public void ClearInputHistory(){
		inputHistory.Clear();
	}

	public static void NextChar()
	{
		if (CurrentCharacterIndex + 1 < instances.Count)
		{
			CurrentCharacterIndex++;
		}
		else
		{
			CurrentCharacterIndex = 0;
		}
	}

	public void RestartAll()
	{
		foreach (var character in instances)
		{
			character.Restart();
		}
	}

	public void RestartToBeginning()
	{
		RestartAll();
		CurrentCharacterIndex = 0;
		foreach (var character in instances)
		{
			character.ClearInputHistory();
		}
	}

	void Record(InputType input)
	{
		if (recording)
		{
			inputHistory.Enqueue(new InputHistoryEntry
			{
				input = input,
				timeStamp = timer
			});
			print("recorded input " + input + " for character " + CharacterNumber);
		}
	}

	void PressLeft(CallbackContext c)
	{
		Record(InputType.PressLeft);
		if (!active)
			return;

		xDir = -1;
		// xDir = -c.ReadValue<float>();
	}
	void ReleaseLeft(CallbackContext c)
	{
		Record(InputType.ReleaseLeft);
		if (!active)
			return;

		if (xDir < 0)
			xDir = 0;
	}

	void PressRight(CallbackContext c)
	{
		Record(InputType.PressRight);
		if (!active)
			return;

		xDir = 1;
		// xDir = c.ReadValue<float>();
	}
	void ReleaseRight(CallbackContext c)
	{
		Record(InputType.ReleaseRight);
		if (!active)
			return;

		if (xDir > 0)
			xDir = 0;
	}

	void PressJump(CallbackContext c)
	{
		PressJump();
	}

	void PressJump()
	{
		Record(InputType.Jump);

		if (active)
		{
			if (touchingGround && jumpAllowed)
			{
				var rotation = Quaternion.Euler(0f, 0f, -xDir * JumpSteeringMax);
				rb.AddForce(rotation * Vector3.up * JumpForce, ForceMode2D.Impulse);
				jumpAllowed = false;
			}
		}
	}
	void ReleaseJump(CallbackContext c)
	{

	}

	void PressReset(CallbackContext c)
	{
		RestartToBeginning();
	}

	void Move(float dt)
	{
		float speed = xDir * Speed * dt;

		if (moving)
		{
			if (!touchingGround)
			{
				speed *= AirSpeedPercentage;
			}
			else
			{
				float angleDiff = Quaternion.Angle(Quaternion.identity, transform.rotation);
				transform.rotation = Quaternion.Euler(0, 0, angleDiff % 90f);

				if (xDir > 0)
				{
					foreach (var item in spriteRenderers)
					{
						item.flipX = false;
					}
				}
				else
				{
					foreach (var item in spriteRenderers)
					{
						item.flipX = true;
					}
				}
			}

			rb.AddForce(Vector3.right * speed, ForceMode2D.Force);
		}
	}

	void PlayRecording()
	{
		if (!active && RecordInput)
		{
			if (nextEntry == null)
			{
				if (inputHistory.Count < 1)
					return;

				nextEntry = inputHistory.Dequeue();
			}

			if (nextEntry.timeStamp < timer)
			{
				switch (nextEntry.input)
				{
					case InputType.PressLeft:
						xDir = -1;
						break;
					case InputType.ReleaseLeft:
						if (xDir < 0)
							xDir = 0;

						break;
					case InputType.PressRight:
						xDir = 1;
						break;
					case InputType.ReleaseRight:
						if (xDir > 0)
							xDir = 0;
						break;
					case InputType.Jump:
						if (touchingGround && jumpAllowed)
						{
							var rotation = Quaternion.Euler(0f, 0f, -xDir * JumpSteeringMax);
							rb.AddForce(rotation * Vector3.up * JumpForce, ForceMode2D.Impulse);
							jumpAllowed = false;
						}
						break;
				}
				nextEntry = null;
			}
		}

	}

	// Update is called once per frame
	void FixedUpdate()
	{
		timer += Time.deltaTime;

		PlayRecording();

		if (groundTimer < CoyoteTime)
		{
			groundTimer += Time.deltaTime;
		}

		Move(Time.deltaTime);

		if (rb.velocity.sqrMagnitude > VelocityCap * VelocityCap)
		{
			rb.velocity = rb.velocity.normalized * VelocityCap;
			// print("hit velocity cap");
		}
		else if (moving && (UseMinSpeedInAir || touchingGround))
		{
			float xVelocity = Mathf.Abs(rb.velocity.x) * xDir;
			if (Mathf.Abs(rb.velocity.x) < MinSpeed)
			{
				xVelocity = MinSpeed * xDir;
			}
			rb.velocity = new Vector2(xVelocity, rb.velocity.y);

			// if ((xDir > 0 && rb.velocity.x < 0) || (xDir < 0 && rb.velocity.x > 0))
			// {
			// 	rb.velocity= new Vector2(-rb.velocity.x, rb.velocity.y);
			// }
			// rb.velocity = rb.velocity.normalized * MinSpeed;
			// print("hit velocity cap");
		}
	}

	public int CompareTo(Character other)
	{
		return CharacterNumber - other.CharacterNumber;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{

		if (other.CompareTag("Spike"))
		{
			RestartAll();
			return;
		}

		if (active && other.CompareTag("Goal"))
		{
			RestartAll();
			NextChar();
			return;
		}

		// if (other.CompareTag(""))
		{
			groundTimer = 0f;
			// jumpAllowed = true;

		}


	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("Spike"))
		{
			return;
		}

		if (other.CompareTag("Goal"))
		{
			return;
		}

		// if (other.CompareTag(""))
		{
			groundTimer = 0f;

			// jumpAllowed = true;
		}


	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		// if (other.gameObject.CompareTag("StopBounce"))
		// 	rb.velocity = new Vector2(rb.velocity.x, 0);

		// groundTimer = 0f;
		jumpAllowed = true;
	}

	private void OnCollisionStay2D(Collision2D other)
	{
		// groundTimer = 0f;
	}

}
