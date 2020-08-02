using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace brackeys2020_buddiesteam3
{
	public class Game1 : Game
	{

		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		// values for caching window size
		public static float ScreenWidth;
		public static float ScreenHeight;


		// Input

		private KeyboardState oldKeyboardState;
		private MouseState oldMouseState;

		// Stuff for resetting mouse to center of screen, etc.
		private Point mouseReset;
		bool mouseLocked = false;

		// content

		// Arial font for rendering text
		public static SpriteFont Arial;

		// A simple dot for drawing single color boxes and lines
		Texture2D dot;

		// position for an example player character
		Vector2 testPlayerPosition = new Vector2(200, 200);
		// reset position
		Vector2 testPlayerResetPosition;
		// ground
		List<Rectangle> ground = new List<Rectangle>();
		// How strong gravity is
		float gravityVelocityAmount = 1f;
		// Player y velocity
		float yVelocity = 0;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			// Show mouse
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{

			// Graphics settings

			// graphics.GraphicsProfile = GraphicsProfile.HiDef;
			// graphics.PreferMultiSampling = true;
			// GraphicsDevice.PresentationParameters.MultiSampleCount = 4;
			// GraphicsDevice.BlendState = BlendState.AlphaBlend;
			graphics.SynchronizeWithVerticalRetrace = true; // vsync
			graphics.PreferredBackBufferWidth = 640;
			graphics.PreferredBackBufferHeight = 360;
			graphics.ApplyChanges();

			// disable framerate limit
			IsFixedTimeStep = false;
			// TargetElapsedTime = TimeSpan.FromSeconds(1f / 60f);

			base.Initialize();

			// Cache screen size once on startup
			ScreenWidth = Window.ClientBounds.Width;
			ScreenHeight = Window.ClientBounds.Height;

			// Make screen size cache update when resizing window
			this.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);

			// Set the reset/restart position to the initial position of the example character
			testPlayerResetPosition = testPlayerPosition;

			// Add ground
			ground.Add(new Rectangle(100, 300, 400, 50));
			ground.Add(new Rectangle(300, 270, 40, 25));
			ground.Add(new Rectangle(300, 200, 40, 25));

		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// Load the font from the Content Pipeline
			Arial = Content.Load<SpriteFont>("Arial");

			// Create a 1x1 pixel white texture for the dot
			dot = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			dot.SetData(new Color[] { Color.White });

		}

		protected override void Update(GameTime gameTime)
		{
			// Get time since last update
			float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

			// Get which keyboard keys are pressed and where the mouse is, etc.
			var mouseState = Mouse.GetState();
			var keystate = Keyboard.GetState();

			// quit if esc is pressed
			if (keystate.IsKeyDown(Keys.Escape))
				Exit();

			// Get how far the mouse has moved since last update
			Point diff = mouseState.Position - oldMouseState.Position;

			switch (mouseState.LeftButton)
			{
				case ButtonState.Pressed:
					if (oldMouseState.LeftButton == ButtonState.Released)
					{
						// if mouse was clicked this update
					}
					else
					{
						// if mouse was held this update
					}
					break;
				case ButtonState.Released:
					if (oldMouseState.LeftButton == ButtonState.Pressed)
					{
						// if mouse was released this update
					}
					else
					{
						// if mouse was not pressed at all this update or the previous update
					}
					break;
			}

			float xDir = 0;

			if (keystate.IsKeyDown(Keys.A) || keystate.IsKeyDown(Keys.Left))
			{
				// if A or left is held down this update, called every update either is still held
				xDir = -1;
			}
			if (keystate.IsKeyDown(Keys.D) || keystate.IsKeyDown(Keys.Right))
			{
				// if D or right is held down this update, called every update either is still held
				xDir = 1;
			}
			if (keystate.IsKeyPressed(oldKeyboardState, Keys.W) || keystate.IsKeyPressed(oldKeyboardState, Keys.Space))
			{
				// if W or up was pressed this update

				// Jump
				yVelocity = -1f;
			}
			if (keystate.IsKeyPressed(oldKeyboardState, Keys.Back))
			{
				// if backspace was pressed this update

				// Reset
				testPlayerPosition = new Vector2(200, 200);
			}

			// Move test character
			float speed = 100f;
			float newX = testPlayerPosition.X + xDir * dt * speed;

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
			} else {
				// Reset falling speed if hitting ground or ceiling
				yVelocity = 0f;
			}


			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin(SpriteSortMode.FrontToBack);

			spriteBatch.DrawString(
				Arial,
				"Test text",
				new Vector2(20, 20),
				Color.Beige,
				0,
				Vector2.Zero,
				1,
				SpriteEffects.None,
				1.0f
			);

			// draw player square
			spriteBatch.Draw(
				dot, // texture
				testPlayerPosition, // position
				new Rectangle(0, 0, 1, 1), // texture source rectangle, which part of the texture will be used
				Color.Orange, // color filter for the texture
				0f, // rotation
				Vector2.Zero, // origin, rotation center point
				10f, // scale
				SpriteEffects.None, // if the texture should be flipped
				0 // depth, decides which 
			);

			// draw ground
			foreach (var item in ground)
			{
				spriteBatch.Draw(
					dot, // texture
					item, // position
					new Rectangle(0, 0, 1, 1), // texture source rectangle, which part of the texture will be used
					Color.Brown, // color filter for the texture
					0f, // rotation
					Vector2.Zero, // origin, rotation center point
					SpriteEffects.None, // if the texture should be flipped
					0 // depth, decides which 
				);
			}


			spriteBatch.End();

			base.Draw(gameTime);
		}

		private void Window_ClientSizeChanged(object sender, EventArgs e)
		{
			graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
			graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;

			// ScreenWidth = graphics.PreferredBackBufferWidth;
			ScreenWidth = Window.ClientBounds.Width;
			// ScreenHeight = graphics.PreferredBackBufferHeight;
			ScreenHeight = Window.ClientBounds.Height;

			graphics.ApplyChanges();
		}
	}
}
