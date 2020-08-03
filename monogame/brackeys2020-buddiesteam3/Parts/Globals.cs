using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace brackeys2020_buddiesteam3
{
	public static class Globals
	{
		public static class Colors
		{
			public static Color Background = Color.CornflowerBlue;

			public static Color FirstCharacter = new Color(0,0,255); 	//blue, hex: 0000ff
			public static Color SecondCharacter = new Color(0,255,255); //cyan/teal, hex: 00ffff

			// Level pieces
			public static Color Ground = new Color(0,127,0); 			//darkish green, hex: 007f00
			public static Color Platform = new Color(127,0,0);			//darkish red, hex: 7f0000
			public static Color Spikes = new Color(191,0,191); 			//pink, hex: bf00bf 

			public static Color Goal = new Color(255,144,00); 			//gold, hex: ff9000
			
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