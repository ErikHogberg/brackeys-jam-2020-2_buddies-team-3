using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class Character : MonoBehaviour, IComparable<Character>
{
	private enum InputType
	{
		PressLeft,
		ReleaseLeft,
		PressRight,
		ReleaseRight,
		Jump,
		PressDown
	}

	private class InputHistoryEntry
	{
		public InputType Input;
		public float TimeStamp;
	}

	private class TransformHistoryEntry
	{
		public Vector2 Pos;
		public float Rot;
		public float TimeStamp;

		public TransformHistoryEntry(Vector2 pos, float rot, float timeStamp)
		{
			Pos = pos;
			Rot = rot;
			TimeStamp = timeStamp;
		}
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
	public float RewindSpeedPercentage = 2f;
	public float VelocityCap = 100f;
	public bool RedirectVelocityOnDown = false;
	[Range(0, 1)]
	public float DownVelocityLossPercentage = .4f;

	[Space]
	public InputActionReference LeftBinding;
	public InputActionReference RightBinding;
	public InputActionReference DownBinding;
	public InputActionReference JumpBinding;
	public InputActionReference ResetBinding;
	public InputActionReference NextBinding;

	[Space]
	public SpriteRenderer FinishSprite;
	public SpriteRenderer ActiveSprite;

	public bool active => CurrentCharacterIndex < instances.Count && this == instances[CurrentCharacterIndex];
	bool moving => Mathf.Abs(xDir) > 0;
	bool recording => RecordInput && active && !rewinding;

	Rigidbody2D rb;
	List<SpriteRenderer> spriteRenderers;

	float xDir = 0f;
	Vector3 initPos;
	bool jumpAllowed = true;
	bool touchingGround => groundTimer < CoyoteTime && jumpAllowed;
	float groundTimer = 0f;

	bool _finished = false;
	bool finished
	{
		get
		{
			return _finished;
		}
		set
		{
			FinishSprite?.gameObject.SetActive(value);
			_finished = value;
		}
	}

	public static bool AllFinished => instances.All(i => i.finished);
	public static bool AllPrevFinished => instances.All(i => i.CharacterNumber >= CurrentCharacterIndex || i.finished);

	float timer = 0f;

	Queue<InputHistoryEntry> inputHistory = new Queue<InputHistoryEntry>();
	InputHistoryEntry nextEntry;

	Stack<TransformHistoryEntry> transformHistory = new Stack<TransformHistoryEntry>();
	TransformHistoryEntry nextTransformEntry;
	bool rewinding = false;


	private void Awake()
	{
		instances.Add(this);
		instances.Sort();
	}

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		spriteRenderers = GetComponentsInChildren<SpriteRenderer>().ToList();
		spriteRenderers.Add(FinishSprite);

		LeftBinding.action.started += PressLeft;
		RightBinding.action.started += PressRight;
		DownBinding.action.started += PressDown;
		JumpBinding.action.started += PressJump;
		ResetBinding.action.started += PressReset;
		NextBinding.action.started += PressNext;

		LeftBinding.action.canceled += ReleaseLeft;
		RightBinding.action.canceled += ReleaseRight;
		JumpBinding.action.canceled += ReleaseJump;

		LeftBinding.action.Enable();
		RightBinding.action.Enable();
		DownBinding.action.Enable();
		JumpBinding.action.Enable();
		ResetBinding.action.Enable();
		NextBinding.action.Enable();

		initPos = transform.position;

		CurrentCharacterIndex = 0;
		UpdateActiveSprite();
	}

	private void OnEnable()
	{
		UpdateActiveSprite();
		FinishSprite?.gameObject.SetActive(finished);
	}

	private void OnDisable()
	{
		instances.Remove(this);
	}

	void UpdateActiveSprite()
	{
		// print("active sprite set to " + active);
		ActiveSprite?.gameObject.SetActive(active);
	}

	public void Restart()
	{
		timer = 0;
		transform.position = initPos;
		rb.velocity = Vector2.zero;
		rb.angularVelocity = 0f;
		transform.rotation = Quaternion.identity;

		xDir = 0;
		finished = false;

		UpdateActiveSprite();
	}

	public void ClearInputHistory()
	{
		inputHistory.Clear();
		nextEntry = null;
	}

	public static void NextChar()
	{
		if (CurrentCharacterIndex + 1 < instances.Count)
		{
			CurrentCharacterIndex++;
		}
		else
		{
			// CurrentCharacterIndex = 0;
			return;
		}

		foreach (var item in instances)
		{
			item.UpdateActiveSprite();
		}

		instances[CurrentCharacterIndex].ClearInputHistory();
	}

	public void RestartAll()
	{
		foreach (var character in instances)
		{
			character.Restart();
		}
		Globals.Reset();
	}

	public void RestartToBeginning()
	{
		// IDEA: reload scene instead?

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
				Input = input,
				TimeStamp = timer
			});
			// print("recorded input " + input + " for character " + CharacterNumber);
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

	void PressDown(CallbackContext c)
	{
		PressDown();
	}

	void PressDown()
	{
		Record(InputType.PressDown);

		if (active && touchingGround)
		{
			if (RedirectVelocityOnDown)
			{
				rb.velocity = new Vector2(0, rb.velocity.y * (1f - DownVelocityLossPercentage));
			}
			else
			{
				rb.velocity = Vector2.down * rb.velocity.magnitude * (1f - DownVelocityLossPercentage);
			}
		}

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
		if (active)
		{
			RestartToBeginning();
		}
	}

	void PressNext(CallbackContext c)
	{
		if (active)
		{
			NextChar();
			RestartAll();
		}
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

			if (nextEntry.TimeStamp < timer)
			{
				switch (nextEntry.Input)
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
					case InputType.PressDown:
						if (touchingGround)
						{
							if (RedirectVelocityOnDown)
							{
								rb.velocity = new Vector2(0, rb.velocity.y * (1f - DownVelocityLossPercentage));
							}
							else
							{
								rb.velocity = Vector2.down * rb.velocity.magnitude * (1f - DownVelocityLossPercentage);
							}
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
		if (rewinding)
		{
			timer -= Time.deltaTime * RewindSpeedPercentage;
			if (timer < 0)
			{
				rewinding = false;
				// TODO: call reset?
				return;
			}

			bool entriesAvailable = true;

			while (entriesAvailable)
			{
				entriesAvailable = false;

				if (nextTransformEntry == null)
				{
					if (transformHistory.Count < 1)
					{
						return;
					}
					nextTransformEntry = transformHistory.Pop();
				}

				if (nextTransformEntry.TimeStamp > timer)
				{
					rb.MovePosition(nextTransformEntry.Pos);
					rb.MoveRotation(nextTransformEntry.Rot);
					nextTransformEntry = null;
					entriesAvailable = true;
				}
			}

			return;
		}

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

		}

		transformHistory.Push(new TransformHistoryEntry(rb.position, rb.rotation, timer));

	}

	public int CompareTo(Character other)
	{
		return CharacterNumber - other.CharacterNumber;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{

		if (other.CompareTag("Spike"))
		{
			RestartToBeginning();
			// RestartAll();
			return;
		}

		if (other.CompareTag("Goal"))
		{
			if (active)
			{
				finished = true;

				if (AllFinished)
				{
					// CurrentCharacterIndex = 0;
					// RestartToBeginning();
					string sceneName = SceneManager.GetActiveScene().name;
					print("finished level " + sceneName + "!");
					if (LevelButtonUIScript.LevelProgress.ContainsKey(sceneName))
					{
						LevelButtonUIScript.LevelProgress[sceneName] = true;
					}
					else
					{
						LevelButtonUIScript.LevelProgress.Add(sceneName, true);
					}
					SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
				}
				else if (AllPrevFinished)
				{

					RestartAll();
					NextChar();
				}
				else
				{
					finished = false;
				}

			}
			else
			{
				finished = true;

			}

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
		// if (other.CompareTag("Spike"))
		// {
		// 	return;
		// }

		if (other.CompareTag("Goal"))
		{
			// finished = true;

			if (AllFinished)
			{
				string sceneName = SceneManager.GetActiveScene().name;
				print("finished level " + sceneName + "!");
				if (LevelButtonUIScript.LevelProgress.ContainsKey(sceneName))
				{
					LevelButtonUIScript.LevelProgress[sceneName] = true;
				}
				else
				{
					LevelButtonUIScript.LevelProgress.Add(sceneName, true);
				}
				SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
			}

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
		if (other.gameObject.CompareTag("Spike"))
		{
			RestartToBeginning();
			// RestartAll();
			return;
		}

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
