using Microsoft.Xna.Framework;

namespace brackeys2020_buddiesteam3
{
	public static class Globals
	{
		public static class Colors
		{
			public static Color FirstCharacter = Color.DarkSlateBlue;
			public static Color SecondCharacter = Color.DarkRed;

			public static Color Ground = Color.Brown;
			public static Color Platform = Color.LightSteelBlue;
			public static Color Spikes = Color.DarkRed;

			public static Color Goal = Color.Gold;
			
			
		}

		public static Vector2 PlayerSize = new Vector2(10,10);
		
		public static Vector2 LevelScale = new Vector2(1,1);
		public static Vector2 LevelOffset = new Vector2(1,1);

	}
}