using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace brackeys2020_buddiesteam3
{
	public static class Collision
	{
		public static bool CheckCollision(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
		{
			return
				x1 < x2 + w2 &&
				x1 + w1 > x2 &&
				y1 < y2 + h2 &&
				y1 + h1 > y2
			;
		}

		public static bool CheckCollision(int x1, int y1, int w1, int h1, int x2, int y2, int w2, int h2)
		{
			return
				x1 < x2 + w2 &&
				x1 + w1 > x2 &&
				y1 < y2 + h2 &&
				y1 + h1 > y2
			;
		}

		public static bool CheckCollision(Vector2 pos1, Vector2 dim1, Vector2 pos2, Vector2 dim2)
		{
			return CheckCollision(pos1.X, pos1.Y, dim1.X, dim1.Y, pos2.X, pos2.Y, dim2.X, dim2.Y);
		}

		public static bool CheckCollision(Rectangle rect1, Rectangle rect2)
		{
			return CheckCollision(rect1.X, rect1.Y, rect1.Width, rect1.Height, rect2.X, rect2.Y, rect2.Width, rect2.Height);
		}

	}
}
