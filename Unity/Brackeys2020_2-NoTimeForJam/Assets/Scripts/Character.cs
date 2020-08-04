using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using static UnityEngine.InputSystem.InputAction;

public class Character : MonoBehaviour
{

	[Min(0)]
	public float Speed = 100f;
	[Min(0)]
	public float JumpForce = 2f;

	[Space]
	public InputActionReference LeftBinding;
	public InputActionReference RightBinding;
	public InputActionReference JumpBinding;
	public InputActionReference ResetBinding;

	Rigidbody2D rb;

	float xDir = 0f;

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

	void PressLeft(CallbackContext c)
	{
		xDir = -c.ReadValue<float>();
	}
	void ReleaseLeft(CallbackContext c)
	{
		if(xDir < 0)
			xDir = 0;
	}

	void PressRight(CallbackContext c)
	{
		xDir = c.ReadValue<float>();
	}
	void ReleaseRight(CallbackContext c)
	{
		if(xDir > 0)
			xDir = 0;
	}

	void PressJump(CallbackContext c)
	{
		// TODO: check if touching ground
		rb.AddForce(Vector3.up * JumpForce, ForceMode2D.Impulse);
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
		rb.AddForce(Vector3.right* xDir * Speed * Time.deltaTime, ForceMode2D.Force);
	}
}
