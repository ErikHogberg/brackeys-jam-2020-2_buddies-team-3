using System.Collections.Generic;

public interface IResettable
{
	void ResetToInit();
}

public static class Globals
{

	public static List<IResettable> Resettables = new List<IResettable>();
	public static void Reset()
	{
		foreach (var item in Resettables)
		{
			item.ResetToInit();
		}
	}
}