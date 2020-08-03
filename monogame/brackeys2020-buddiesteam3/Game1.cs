using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace brackeys2020_buddiesteam3
{

	public struct GameState
	{
		public float dt;
		public KeyboardState keyState;
		public KeyboardState oldKeyState;
	}

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
		public static Texture2D Dot;

		public static Texture2D FirstCharacterTexture;
		public static Texture2D SecondCharacterTexture;

		// Sounds
		// make sure to set the correct processor in the content pipeline tool, song or sound effect
		// difference between song and sound effects is that songs can be paused and looped easily, 
		// while sound effects cant but sound effects can be played multiple times at the same time
		float SoundVolume = 0.5f;

		Song industrialAmbientSpaces;
		public static SoundEffect box_crush_005;

		//

		Level currentLevel;
		public static Character FirstCharacter;
		public static Character SecondCharacter;


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
			graphics.PreferredBackBufferWidth = 1280;//640;
			graphics.PreferredBackBufferHeight = 720;//360;
			graphics.ApplyChanges();

			// disable framerate limit
			IsFixedTimeStep = true;
			// TargetElapsedTime = TimeSpan.FromSeconds(1f / 60f);

			base.Initialize();

			// Cache screen size once on startup
			ScreenWidth = Window.ClientBounds.Width;
			ScreenHeight = Window.ClientBounds.Height;

			// Make screen size cache update when resizing window
			this.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);

		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// Load the font from the Content Pipeline
			Arial = Content.Load<SpriteFont>("Arial");

			// Create a 1x1 pixel white texture for the dot
			Dot = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			Dot.SetData(new Color[] { Color.White });

			if (Globals.Textures.FirstCharacterTextureName == "")
				FirstCharacterTexture = Dot;
			else
				FirstCharacterTexture = Content.Load<Texture2D>(Globals.Textures.FirstCharacterTextureName);

			if (Globals.Textures.SecondCharacterTextureName == "")
				SecondCharacterTexture = Dot;
			else
				SecondCharacterTexture = Content.Load<Texture2D>(Globals.Textures.SecondCharacterTextureName);

			// how to load a texture
			// Texture2D texture = Content.Load<Texture2D>("folder/textureWithoutFileType");

			//industrialAmbientSpaces = Content.Load<Song>("Sounds/Songs/Industrial_Ambient_Spaces_04");
			box_crush_005 = Content.Load<SoundEffect>("Sounds/SFX/box_crush_005");
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// Starting Song
			MediaPlayer.Volume = SoundVolume;
			industrialAmbientSpaces = Content.Load<Song>("Sounds/Songs/Industrial_Ambient_Spaces_04");
			MediaPlayer.Play(industrialAmbientSpaces);
			// looping the song
			MediaPlayer.IsRepeating = true;

			// Level and characters

			currentLevel = new Level("Levels/level1_svg_test.svg");

			FirstCharacter = new Character();
			FirstCharacter.CharacterColor = Globals.Colors.FirstCharacter;
			FirstCharacter.CharacterTexture = FirstCharacterTexture;
			SecondCharacter = new Character();
			SecondCharacter.ControlsEnabled = false;
			SecondCharacter.CharacterColor = Globals.Colors.SecondCharacter;
			SecondCharacter.CharacterTexture = SecondCharacterTexture;

			currentLevel.Reset(FirstCharacter, SecondCharacter);

		}



		protected override void Update(GameTime gameTime)
		{
			// Get time since last update
			float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

			// Get which keyboard keys are pressed and where the mouse is, etc.
			var mouseState = Mouse.GetState();
			var keystate = Keyboard.GetState();

			GameState gameState = new GameState
			{
				dt = dt,
				keyState = keystate,
				oldKeyState = oldKeyboardState
			};

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

			if (keystate.IsKeyPressed(oldKeyboardState, Keys.Back))
			{
				// if backspace was pressed this update

				// Reset
				currentLevel.Reset(FirstCharacter, SecondCharacter);
			}

			//Changing Song Volume
			if (keystate.IsKeyPressed(oldKeyboardState, Keys.K))
			{
				SoundVolume += 0.1f;
				MediaPlayer.Volume = SoundVolume;

			}
			if (keystate.IsKeyPressed(oldKeyboardState, Keys.L))
			{
				SoundVolume -= 0.1f;
				MediaPlayer.Volume = SoundVolume;
			}

			FirstCharacter.Update(gameState, currentLevel);
			SecondCharacter.Update(gameState, currentLevel);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			// Background color
			GraphicsDevice.Clear(Globals.Colors.Background);

			// All 2D draw calls need to happen between spritebatch begin and end
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
			FirstCharacter.Draw(spriteBatch);
			SecondCharacter.Draw(spriteBatch);

			// draw ground
			currentLevel.Draw(spriteBatch);

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
