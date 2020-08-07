using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Triggerable : MonoBehaviour
{
	public abstract void Press();
	public abstract void Release();
}

public class Button : Triggerable, IResettable
{

	public bool TriggerOnce = false;
	int objectPressing = 0;

	public Triggerable[] ThingsToTrigger;

	[Space]
	public Color PressColor;
	private Color initColor;

	private SpriteRenderer spriteRenderer;

	bool isPressed = false;

	// Start is called before the first frame update
	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		initColor = spriteRenderer.color;
	}

	private void Awake()
	{
		Globals.Resettables.Add(this);
	}

	private void OnDestroy()
	{
		Globals.Resettables.Remove(this);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{

		if (TriggerOnce && isPressed)
		{
			return;
		}

		if (other.CompareTag("Player") || other.CompareTag("Box"))
		{
			if (objectPressing < 0)
				objectPressing = 0;

			if (objectPressing < 1)
			{
				if (TriggerOnce)
				{
					isPressed = true;
				}
				spriteRenderer.color = PressColor;
				foreach (var item in ThingsToTrigger)
				{
					item?.Press();
				}
			}

			objectPressing++;
		}

	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player") || other.CompareTag("Box"))
		{
			objectPressing--;
			if (!TriggerOnce && objectPressing < 1)
			{
				spriteRenderer.color = initColor;
				foreach (var item in ThingsToTrigger)
				{
					item?.Release();
				}
			}

			if (objectPressing < 0)
				objectPressing = 0;
		}
	}

	public override void Press()
	{
		if (TriggerOnce)
		{
			isPressed = false;
			spriteRenderer.color = initColor;
			foreach (var item in ThingsToTrigger)
			{
				item?.Release();
			}
		}
	}
	
	public override void Release()
	{
	}

	public void ResetToInit()
	{
		isPressed = false;
		spriteRenderer.color = initColor;
		objectPressing = 0;

	}

}
