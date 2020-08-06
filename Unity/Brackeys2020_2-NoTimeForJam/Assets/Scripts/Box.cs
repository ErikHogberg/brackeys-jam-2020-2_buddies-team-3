using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IResettable
{

	Rigidbody2D rb;
	Vector2 initPos;
	float initRot;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		initPos = rb.position;
		initRot = rb.rotation;
	}

	private void Awake()
	{
		Globals.Resettables.Add(this);
	}

	private void OnDestroy()
	{
		Globals.Resettables.Remove(this);
	}

	public void ResetToInit()
	{
		rb.MovePosition(initPos);
		rb.SetRotation(initRot);
        rb.velocity = Vector2.zero;
	}
}
