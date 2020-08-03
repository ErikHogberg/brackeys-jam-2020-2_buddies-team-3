using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace brackeys2020_buddiesteam3
{
	public class Character
	{
		public bool ControlsEnabled = true;

		public float X;
		public float Y;
		// public float Width = 10f;
		// public float Height = 10f;

		public Color CharacterColor;
		public Texture2D CharacterTexture;

		public Vector2 Pos => new Vector2(X, Y);
		public Vector2 Dim => Globals.PlayerSize;
		public Rectangle Rect => new Rectangle((int)X, (int)Y, (int)Globals.PlayerSize.X, (int)Globals.PlayerSize.Y);

		// How strong gravity is
		// const float gravityVelocityAmount = 3.5f;
		// Player y velocity
		float yVelocity = 0;

		public void Reset()
		{
			yVelocity = 0;
		}

		bool touchingGround = false;

		public void Update(GameState gameState, Level level)
		{

			float xDir = 0;
			if (ControlsEnabled)
			{
				if (gameState.keyState.IsKeyDown(Keys.A) || gameState.keyState.IsKeyDown(Keys.Left))
				{
					// if A or left is held down this update, called every update either is still held
					xDir = -1;
				}
				if (gameState.keyState.IsKeyDown(Keys.D) || gameState.keyState.IsKeyDown(Keys.Right))
				{
					// if D or right is held down this update, called every update either is still held
					xDir = 1;
				}
				if (gameState.keyState.IsKeyPressed(gameState.oldKeyState, Keys.W) || gameState.keyState.IsKeyPressed(gameState.oldKeyState, Keys.Space))
				{
					// if W or up was pressed this update

					// Jump
					if (touchingGround)
					{
						yVelocity = -Globals.JumpForce;
						//playsfx

					}
				}
			}


			// Move test character
			// float speed = 100f;
			float deltaX = xDir * gameState.dt * Globals.RunSpeed;
			float newX = X + deltaX;

			// Falling and jumping collision
			const float yVelocityCap = 4f;
			float fallSpeed = 100f;
			yVelocity += Globals.GravityAmount * gameState.dt;
			if (yVelocity > yVelocityCap)
				yVelocity = yVelocityCap;

			float deltaY = yVelocity * gameState.dt * fallSpeed;
			float newY = Y + deltaY;

			// Collision with level
			(bool x, bool y) = level.CheckCollision(new Vector2(X, Y), Globals.PlayerSize, deltaX, deltaY);

			if (level.CheckSpikeCollision(Pos, Dim))
			{
				level.Reset(Game1.FirstCharacter, Game1.SecondCharacter);
				return;
			}

			float checkX = X;
			if (x)
				checkX = newX;

			Character otherCharacter = Game1.SecondCharacter;
			if (this == Game1.SecondCharacter)
				otherCharacter = Game1.FirstCharacter;

			bool collideOtherY = Collision.CheckCollision(
				checkX, Y, Globals.PlayerSize.X, Globals.PlayerSize.Y,
				otherCharacter.X, otherCharacter.Y, Globals.PlayerSize.X, Globals.PlayerSize.Y
			);

			if (x)
				X = newX;

			bool wasTouchingGround = touchingGround;
			touchingGround = false;

			if (y && !collideOtherY)
			{
				Y = newY;

			}
			else
			{
				yVelocity = 0f;

				//SFX plays when player touches ground
				if (!wasTouchingGround)
				{
					//Game1.box_crush_005.CreateInstance().Play();
				}
				touchingGround = true;
			}

			// Debug.WriteLine("Updated character");
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(
				// Game1.Dot, // texture
				CharacterTexture,
				// Rect, // position
				new Vector2(X, Y),
				// Rect,
				new Rectangle(0, 0, 1, 1), // texture source rectangle, which part of the texture will be used
				CharacterColor, // color filter for the texture
				0f, // rotation
				Vector2.Zero, // origin, rotation center point
				Globals.PlayerSize,
				//   10f, // scale
				SpriteEffects.None, // if the texture should be flipped
				0 // depth, decides which 
			);
		}

	}
}