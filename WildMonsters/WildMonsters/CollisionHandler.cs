using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;


namespace WildMonsters
{
	public static class CollisionHandler
	{
		// Private variables:
		
		// Accessors:
		
		// Box collision
		public static void BoxHasCollided(SpriteUV ballOne, SpriteUV ballTwo)
		{
			Bounds2 oneBounds = new Bounds2();
			Bounds2 twoBounds = new Bounds2();
			
			//ballOne.Sprite.GetContentWorldBounds(ref oneBounds);
			ballOne.GetContentWorldBounds(ref oneBounds);
			ballTwo.GetContentWorldBounds(ref twoBounds);
			
			// Box collision
			if(oneBounds.Overlaps(twoBounds))
        	{
				// If the two balls have touched, do something...
				// Check whether the two balls are the same colour, and somehow communicate that to another ball to say 'We're all the same colour, pop'.

        	}
		}
		
		// Circle collision
		public static void CircleHasCollided(Particle particleOne, Particle particleTwo)
		{
			Vector3 diff = new Vector3(0.0f, 0.0f, 0.0f);
			
			// When the particle objects are passed in, this will determine the distance between the two, and when they're close enough they'll collide
			diff.X = particleOne.getMyPos().X - particleTwo.getMyPos().X;
			diff.Y = particleOne.getMyPos().Y - particleTwo.getMyPos().Y;

			float distance = Sce.PlayStation.Core.FMath.Sqrt(diff.X*diff.X + diff.Y*diff.Y + diff.Z*diff.Z);
			
			if (distance < 5.0f)
			{
				// Do something
			}
		}
	}
}

