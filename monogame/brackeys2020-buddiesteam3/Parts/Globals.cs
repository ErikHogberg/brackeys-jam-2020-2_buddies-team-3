using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace brackeys2020_buddiesteam3
{
	public static class Globals
	{
		public static class Colors
		{
			public static Color Background = Color.CornflowerBlue;

			public static Color FirstCharacter = Color.White;
			public static Color SecondCharacter = Color.DarkRed;

			// Level pieces
			public static Color Ground = Color.Brown;
			public static Color Platform = Color.LightSteelBlue;
			public static Color Spikes = Color.DarkRed;

			public static Color Goal = Color.Gold;

		}

		public static class Textures
		{
			// Set the path to the texture to load to use it instead of a colored square

			// Characters
			public static string FirstCharacterTextureName = 
			"";
			// "Images/stickguy";
			public static string SecondCharacterTextureName = "";

		}

		public static Vector2 PlayerSize = new Vector2(20, 25);

		public static Vector2 LevelScale = new Vector2(1, 1);
		public static Vector2 LevelOffset = new Vector2(1, 1);

		public const float GravityAmount = 3.5f;
		public const float JumpForce = 2.0f;
		public const float RunSpeed = 100.0f;

	}
}