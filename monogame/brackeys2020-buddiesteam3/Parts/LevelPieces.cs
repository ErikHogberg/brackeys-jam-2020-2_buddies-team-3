using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace brackeys2020_buddiesteam3
{

	public interface IButtonTriggerable
	{
		public void Press();
		public void Release();
	}


	public class Button
	{
		public Rectangle Rect;
		public IButtonTriggerable Triggerable;

		private bool isPressed = false;

		public Button(Rectangle rect, IButtonTriggerable triggerable)
		{
			Rect = rect;
			Triggerable = triggerable;
		}

		public void Press()
		{
			Triggerable.Press();
		}

		public void Release()
		{
			Triggerable.Release();
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(
				Game1.Dot, // texture
				Rect, // position
				new Rectangle(0, 0, 1, 1), // texture source rectangle, which part of the texture will be used
				Globals.Colors.Button, // color filter for the texture
				0f, // rotation
				Vector2.Zero, // origin, rotation center point
							  // 10f, // scale
				SpriteEffects.None, // if the texture should be flipped
				0 // depth, decides which 
			);
		}

		public class BreakingPlatforms
		{
			public Rectangle Rect;

			private bool wasTouched = false;

			public BreakingPlatforms(Rectangle rect, IButtonTriggerable triggerable)
			{
				Rect = rect;
			}

			// return true when it breaks
			public bool Update(bool touched)
			{
				if (!touched && wasTouched)
				{
					return true;
				}
				wasTouched = touched;
				return false;
			}

			public void Draw(SpriteBatch spriteBatch)
			{
				spriteBatch.Draw(
					Game1.Dot, // texture
					Rect, // position
					new Rectangle(0, 0, 1, 1), // texture source rectangle, which part of the texture will be used
					Globals.Colors.BreakingPlatform, // color filter for the texture
					0f, // rotation
					Vector2.Zero, // origin, rotation center point
								  // 10f, // scale
					SpriteEffects.None, // if the texture should be flipped
					0 // depth, decides which 
				);
			}


		}
	}
}