using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using static UnityEngine.InputSystem.InputAction;

public class Character : MonoBehaviour, IComparable<Character>
{

	public static int CurrentCharacterIndex = 0;
	private static List<Character> instances = new List<Character>();

	public int CharacterNumber = 0;

	[Min(0)]
	public float Speed = 100f;
	[Min(0)]
	public float JumpForce = 2f;

	[Space]
	public InputActionReference LeftBinding;
	public InputActionReference RightBinding;
	public InputActionReference JumpBinding;
	public InputActionReference ResetBinding;

	public bool active => CurrentCharacterIndex < instances.Count && this == instances[CurrentCharacterIndex];

	Rigidbody2D rb;

	float xDir = 0f;
	Vector3 initPos;
	bool touchingGround = false;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();

		LeftBinding.action.performed += PressLeft;
		RightBinding.action.performed += PressRight;
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
		transform.position = initPos;
		rb.velocity = Vector2.zero;
		rb.angularVelocity = 0f;
		transform.rotation = Quaternion.identity;
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

	void PressLeft(CallbackContext c)
	{
		xDir = -c.ReadValue<float>();
	}
	void ReleaseLeft(CallbackContext c)
	{
		if (xDir < 0)
			xDir = 0;
	}

	void PressRight(CallbackContext c)
	{
		xDir = c.ReadValue<float>();
	}
	void ReleaseRight(CallbackContext c)
	{
		if (xDir > 0)
			xDir = 0;
	}

	void PressJump(CallbackContext c)
	{
		if (active)
		{
			if (touchingGround)
			{
				rb.AddForce(Vector3.up * JumpForce, ForceMode2D.Impulse);
			}
		}
	}
	void ReleaseJump(CallbackContext c)
	{

	}

	void PressReset(CallbackContext c)
	{

	}



	// Update is called once per frame
	void Update()
	{
		if (!active)
		{
			return;
		}

		rb.AddForce(Vector3.right * xDir * Speed * Time.deltaTime, ForceMode2D.Force);
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

		if (other.CompareTag("Goal"))
		{
			RestartAll();
			NextChar();
			return;
		}


	}

	private void OnCollisionStay2D(Collision2D other)
	{
		touchingGround = true;
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		touchingGround = false;

	}
}
