using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace brackeys2020_buddiesteam3
{
	public static class Utilities
	{

		public static void DrawLine(
			SpriteBatch spriteBatch,
			Texture2D texture,
			Point from,
			float angle,
			int distance,
			Color? color = null,
			int width = 1,
			float depth = 1.0f
		)
		{

			if (color == null)
			{
				color = Color.Beige;
			}

			spriteBatch.Draw(
				texture,
				new Rectangle(from.X, from.Y, width, (int)distance),
				null,
				color.Value,
				angle,
				new Vector2(.5f, 0),
				SpriteEffects.None,
				depth
			);
		}

		public static void DrawLine(
			SpriteBatch spriteBatch,
			Texture2D texture,
			Vector2 from,
			float angle,
			float distance,
			Color? color = null,
			int width = 1,
			float depth = 1.0f
		)
		{

			DrawLine(spriteBatch, texture, from.ToPoint(), angle, (int)distance, color, width, depth);

		}

		public static void DrawLine(
			SpriteBatch spriteBatch,
			Texture2D texture,
			Vector2 from,
			Vector2 to,
			Color? color = null,
			int width = 1,
			float depth = 1.0f
		)
		{

			Vector2 delta = to - from;
			float angle = MathF.Atan2(delta.Y, delta.X) - MathHelper.PiOver2;
			float distance = delta.Length(); // NOTE: expensive? because square root?

			DrawLine(spriteBatch, texture, from, angle, distance, color, width, depth);
		}


		public static void DrawHpBar(
			SpriteBatch spriteBatch,
			Texture2D texture,
			Vector2 pos,
			float percent
		)
		{

			float distance = 50;
			Vector2 from = pos + new Vector2(-distance * 0.5f, -35.0f);

			DrawLine(spriteBatch, texture, from, -MathHelper.PiOver2, distance, Color.Beige, 7, 0.99f);
			DrawLine(spriteBatch, texture, from, -MathHelper.PiOver2, distance * percent, Color.DarkRed, 5, 1.0f);

			DrawLine(spriteBatch, texture, pos, 0, 3, Color.DarkViolet, 3, 1.0f);


		}

		public static void DrawArrow(
			SpriteBatch spriteBatch,
			Texture2D texture,
			Vector2 to,
			float angle,
			float distance,
			float headSize,
			Color? color = null,
			int width = 1,
			float depth = 1.0f
		)
		{

			DrawLine(spriteBatch, texture, to, angle, distance, color, width, depth);

			DrawLine(spriteBatch, texture, to, angle + MathHelper.PiOver4, headSize, color, width, depth);
			DrawLine(spriteBatch, texture, to, angle - MathHelper.PiOver4, headSize, color, width, depth);

		}

		public static void DrawArrow(
			SpriteBatch spriteBatch,
			Texture2D texture,
			Vector2 from,
			Vector2 to,
			float headSize,
			Color? color = null,
			int width = 1,
			float depth = 1.0f
		)
		{

			Vector2 delta = to - from;
			float angle = MathF.Atan2(delta.Y, delta.X) - MathHelper.PiOver2;
			float distance = delta.Length(); // NOTE: computationally expensive?

			DrawArrow(spriteBatch, texture, to, angle + MathHelper.Pi, distance, headSize, color, width, depth);
		}


		public static Vector2 Slerp(this Vector2 pos, Vector2 target, float amount)
		{


			Quaternion rot = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathF.Atan2(pos.Y, pos.X));
			Quaternion targetRot = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathF.Atan2(target.Y, target.X));

			Quaternion resultRot = Quaternion.Slerp(rot, targetRot, amount);

			Vector3 resultVec3 = Vector3.Transform(Vector3.UnitY, resultRot);

			Vector2 result = new Vector2(resultVec3.X, resultVec3.Y);

			return result;
		}

		public static bool IsKeyPressed(this KeyboardState keyboardState, KeyboardState oldKeyboardState, Keys key)
		{
			if (keyboardState.IsKeyDown(key))
			{
				if (!oldKeyboardState.IsKeyDown(key))
				{
					return true;
				}
			}
			return false;
		}

		

		public static void DrawTilingBackground(this SpriteBatch spriteBatch, Texture2D texture, Vector2 pos, float scale = 1f, bool flipTiles = true, float depth = 0.0f)
		{

			float tilingWidth = texture.Width * scale * 2;
			float tilingHeight = texture.Height * scale * 2;

			while (pos.X > 0)
			{
				pos.X -= tilingWidth;
			}
			while (pos.X < -tilingWidth)
			{
				pos.X += tilingWidth;
			}

			while (pos.Y > 0)
			{
				pos.Y -= tilingHeight;
			}
			while (pos.Y < -tilingHeight)
			{
				pos.Y += tilingHeight;
			}


			for (int x = 0; x * texture.Width * scale < Game1.ScreenWidth * 8; x++)
			{
				SpriteEffects flip = SpriteEffects.None;
				if (flipTiles && x % 2 == 0)
				{
					flip |= SpriteEffects.FlipHorizontally;
				}

				for (int y = 0; y * texture.Height * scale < Game1.ScreenHeight * 8; y++)
				{
					if (flipTiles && y % 2 == 0)
					{
						flip |= SpriteEffects.FlipVertically;
					}

					spriteBatch.Draw(texture,
						new Vector2(
							pos.X + scale * texture.Width * x,
							pos.Y + scale * texture.Height * y
						),
						null,
						Color.White,
						0,
						Vector2.Zero,
						scale,
						flip,
						depth
					);
				}
			}
		}

	}
}