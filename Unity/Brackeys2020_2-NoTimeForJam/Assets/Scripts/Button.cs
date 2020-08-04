using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Triggerable : MonoBehaviour
{
	public abstract void Press();
	public abstract void Release();
}

public class Button : MonoBehaviour
{

	public bool TriggerOnce = false;
	int objectPressing = 0;

	public Triggerable[] ThingsToTrigger;

	[Space]
	public Color PressColor;
	private Color initColor;

	private SpriteRenderer spriteRenderer;

	// Start is called before the first frame update
	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		initColor = spriteRenderer.color;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") || other.CompareTag("Box"))
		{
			if (objectPressing < 1)
			{
				spriteRenderer.color = PressColor;
				foreach (var item in ThingsToTrigger)
				{
					item.Press();
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
					item.Release();
				}
			}
		}
	}

}
