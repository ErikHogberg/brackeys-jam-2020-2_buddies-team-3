using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace brackeys2020_buddiesteam3
{


	public class Level
	{

		List<Rectangle> ground;
		List<Rectangle> spikes;
		List<Button> buttons;
		Vector2 firstCharacterSpawn;
		Vector2 secondCharacterSpawn;

		public Level(string svgFilePath)
		{
			ground = new List<Rectangle>();
			spikes = new List<Rectangle>();
			buttons = new List<Button>();
			firstCharacterSpawn = new Vector2(100, 200);
			secondCharacterSpawn = new Vector2(75, 200);

			List<List<SVGLevelPiece>> pieces = Parsing.Parse(svgFilePath);

			foreach (var itemList in pieces)
			{
				foreach (var item in itemList)
				{
					switch (item.Type)
					{
						case LevelPieceType.Ground:
						case LevelPieceType.Platform:
							ground.Add(item.Rect);
							break;
						case LevelPieceType.Spikes:
							spikes.Add(item.Rect);
							break;
						default:
							break;
					}
				}
			}

		}

		public Level(List<Rectangle> ground, List<Rectangle> spikes)
		{
			this.ground = ground;
			this.spikes = spikes;
		}

		public void Reset(Character firstCharacter, Character secondCharacter)
		{
			firstCharacter.X = firstCharacterSpawn.X;
			firstCharacter.Y = firstCharacterSpawn.Y;
			secondCharacter.X = secondCharacterSpawn.X;
			secondCharacter.Y = secondCharacterSpawn.Y;

			Debug.WriteLine("Reset characters to spawn");
		}

		// x, y, damaged
		public (bool, bool, bool) CheckCollision(Vector2 pos, Vector2 dim, float xMovement, float yMovement)
		{
			bool damaged = false;

			// var xCheckRect = new Rectangle((int)pos.X + (int)xMovement, rect.Y, rect.Width, rect.Height);
			// var yCheckRect = new Rectangle(rect.X, rect.Y + (int)yMovement, rect.Width, rect.Height);
			var xCheckPos = new Vector2(pos.X + xMovement, pos.Y);
			var yCheckPos = new Vector2(pos.X, pos.Y + yMovement);

			bool outX = true;
			// check horizontal collision
			// ground
			if (ground.Any(g => Collision.CheckCollision(
				xCheckPos, dim, new Vector2(g.X, g.Y), new Vector2(g.Width, g.Height)
				)))
			{
				outX = false;
			}
			// spikes
			if (spikes.Any(g => Collision.CheckCollision(
				xCheckPos, dim, new Vector2(g.X, g.Y), new Vector2(g.Width, g.Height)
			)))
			{
				outX = false;
				damaged = true;
			}

			// check vertical collision
			if (outX)
				yCheckPos.X += xMovement;

			bool outY = true;

			if (ground.Any(g => Collision.CheckCollision(
				yCheckPos, dim, new Vector2(g.X, g.Y), new Vector2(g.Width, g.Height)
			)))
			{
				outY = false;
			}
			// spikes
			if (spikes.Any(g => Collision.CheckCollision(
				yCheckPos, dim, new Vector2(g.X, g.Y), new Vector2(g.Width, g.Height)
			)))
			{
				outY = false;
				damaged = true;
			}

			// TODO: trigger buttons
			// TODO: destroy platforms on jump/leave

			// TODO: return collision results
			return (outX, outY, damaged);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			// draw ground
			foreach (var item in ground)
			{
				spriteBatch.Draw(
					Game1.Dot, // texture
					item, // position
					new Rectangle(0, 0, 1, 1), // texture source rectangle, which part of the texture will be used
					Color.Brown, // color filter for the texture
					0f, // rotation
					Vector2.Zero, // origin, rotation center point
					SpriteEffects.None, // if the texture should be flipped
					0 // depth, decides which 
				);
			}

			// draw ground
			foreach (var item in spikes)
			{
				spriteBatch.Draw(
					Game1.Dot, // texture
					item, // position
					new Rectangle(0, 0, 1, 1), // texture source rectangle, which part of the texture will be used
					Color.Red, // color filter for the texture
					0f, // rotation
					Vector2.Zero, // origin, rotation center point
					SpriteEffects.None, // if the texture should be flipped
					0 // depth, decides which 
				);
			}
		}
	}
}