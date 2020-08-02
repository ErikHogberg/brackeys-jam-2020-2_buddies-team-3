using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace brackeys2020_buddiesteam3
{


	public class Level
	{

		List<Rectangle> ground;
		List<Rectangle> spikes;
		List<Button> buttons;

		public Level(string svgFilePath)
		{

		}

		public Level(List<Rectangle> ground, List<Rectangle> spikes)
		{
			this.ground = ground;
			this.spikes = spikes;
		}

		public (bool, bool) CheckCollision()
		{
			// TODO: check horizontal collision
			// TODO: check vertical collision

			// TODO: trigger buttons
			// TODO: destroy platforms on jump/leave

			// TODO: return collision results
			return (true, true);
		}
	}
}