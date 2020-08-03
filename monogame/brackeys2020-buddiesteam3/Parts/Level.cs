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
		List<Rectangle> platforms;
		List<Rectangle> spikes;
		List<Button> buttons;
		Vector2 firstCharacterSpawn;
		Vector2 secondCharacterSpawn;

		Rectangle Goal;

		public Level(string svgFilePath)
		{
			ground = new List<Rectangle>();
			platforms = new List<Rectangle>();
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
							ground.Add(item.Rect);
							break;
						case LevelPieceType.Platform:
							platforms.Add(item.Rect);
							break;
						case LevelPieceType.Spikes:
							spikes.Add(item.Rect);
							break;
						case LevelPieceType.Goal:
							Goal = item.Rect;
							break;
						case LevelPieceType.FirstCharacterStart:
							firstCharacterSpawn = new Vector2(item.Rect.X, item.Rect.Y);
							Debug.WriteLine("Set character 1 spawn");
							break;
						case LevelPieceType.SecondCharacterStart:
							secondCharacterSpawn = new Vector2(item.Rect.X, item.Rect.Y);
							Debug.WriteLine("Set character 2 spawn");
							break;

						default:
							Debug.WriteLine("piece not parsed of type " + item.Type);
							break;
					}
				}
			}

		}

		public void Reset(Character firstCharacter, Character secondCharacter)
		{
			firstCharacter.X = firstCharacterSpawn.X;
			firstCharacter.Y = firstCharacterSpawn.Y;
			firstCharacter.Reset();
			secondCharacter.X = secondCharacterSpawn.X;
			secondCharacter.Y = secondCharacterSpawn.Y;
			secondCharacter.Reset();

			Debug.WriteLine("Reset characters to spawn");
		}

		// x, y, damaged
		public (bool, bool) CheckCollision(Vector2 pos, Vector2 dim, float xMovement, float yMovement)
		{
			// var xCheckRect = new Rectangle((int)pos.X + (int)xMovement, rect.Y, rect.Width, rect.Height);
			// var yCheckRect = new Rectangle(rect.X, rect.Y + (int)yMovement, rect.Width, rect.Height);
			var xCheckPos = new Vector2(pos.X + xMovement, pos.Y);
			var yCheckPos = new Vector2(pos.X, pos.Y + yMovement);

			bool outX = true;
			// check horizontal collision
			// ground
			if (ground.Concat(platforms).Any(g => Collision.CheckCollision(
				xCheckPos, dim, g
				)))
			{
				outX = false;
			}


			// check vertical collision
			if (outX)
				yCheckPos.X += xMovement;

			bool outY = true;

			if (ground.Concat(platforms).Any(g => Collision.CheckCollision(
				yCheckPos, dim, g
			)))
			{
				outY = false;
			}


			// TODO: trigger buttons
			// TODO: destroy platforms on jump/leave

			// TODO: return collision results
			return (outX, outY);
		}

		public bool CheckSpikeCollision(Vector2 pos, Vector2 dim)
		{
			return spikes.Any(g => Collision.CheckCollision(pos, dim, g));
		}

		public bool CheckGoalCollision(Vector2 pos, Vector2 dim)
		{
			return Collision.CheckCollision(pos, dim, Goal);
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
					Globals.Colors.Ground, // color filter for the texture
					0f, // rotation
					Vector2.Zero, // origin, rotation center point
					SpriteEffects.None, // if the texture should be flipped
					0 // depth, decides which 
				);
			}

			foreach (var item in platforms)
			{
				spriteBatch.Draw(
					Game1.Dot, // texture
					item, // position
					new Rectangle(0, 0, 1, 1), // texture source rectangle, which part of the texture will be used
					Globals.Colors.Platform, // color filter for the texture
					0f, // rotation
					Vector2.Zero, // origin, rotation center point
					SpriteEffects.None, // if the texture should be flipped
					0 // depth, decides which 
				);
			}

			// draw spikes
			foreach (var item in spikes)
			{
				spriteBatch.Draw(
					Game1.Dot, // texture
					item, // position
					new Rectangle(0, 0, 1, 1), // texture source rectangle, which part of the texture will be used
					Globals.Colors.Spikes, // color filter for the texture
					0f, // rotation
					Vector2.Zero, // origin, rotation center point
					SpriteEffects.None, // if the texture should be flipped
					0 // depth, decides which 
				);
			}

			spriteBatch.Draw(
				Game1.Dot, // texture
				Goal, // position
				new Rectangle(0, 0, 1, 1), // texture source rectangle, which part of the texture will be used
				Globals.Colors.Goal, // color filter for the texture
				0f, // rotation
				Vector2.Zero, // origin, rotation center point
				SpriteEffects.None, // if the texture should be flipped
				0 // depth, decides which 
			);

		}
	}
}