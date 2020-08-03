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

	Rigidbody rb;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();

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

	}
	void ReleaseLeft(CallbackContext c)
	{

	}

	void PressRight(CallbackContext c)
	{

	}
	void ReleaseRight(CallbackContext c)
	{

	}

	void PressJump(CallbackContext c)
	{

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

	}
}
