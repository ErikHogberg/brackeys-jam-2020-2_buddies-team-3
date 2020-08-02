using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace brackeys2020_buddiesteam3
{
	public class Character
	{
		public bool ControlsEnabled = true;

		public float X;
		public float Y;
		public float Width = 10f;
		public float Height = 10f;

		public Rectangle Rect => new Rectangle((int)X, (int)Y, (int)Width, (int)Height);

		// How strong gravity is
		float gravityVelocityAmount = 1f;
		// Player y velocity
		float yVelocity = 0;

		public void Update(GameState gameState)
		{



			Input(gameState);

		}

		public void Input(GameState gameState)
		{
			if (!ControlsEnabled)
				return;


			float xDir = 0;

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
				yVelocity = -1f;
			}


			// Move test character
			float speed = 100f;
			float newX = X + xDir * gameState.dt * speed;

			// Check collision

			// Left and right movement collision
			if (!ground.Any(g => Collision.CheckCollision(
				newX,
				testPlayerPosition.Y,
				10f, 10f,
				g.X, g.Y, g.Width, g.Height
			)))
			{
				testPlayerPosition.X = newX;
			}

			// Falling and jumping collision
			yVelocity += gravityVelocityAmount * dt;
			float newY = testPlayerPosition.Y + yVelocity * dt * speed;

			if (!ground.Any(g => Collision.CheckCollision(
				testPlayerPosition.X,
				newY,
				10f, 10f,
				g.X, g.Y, g.Width, g.Height
			)))
			{
				testPlayerPosition.Y = newY;
			}
			else
			{
				// Reset falling speed if hitting ground or ceiling
				yVelocity = 0f;
			}
		}

	}
}