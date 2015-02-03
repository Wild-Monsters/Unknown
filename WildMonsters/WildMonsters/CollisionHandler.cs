using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;
using System.Collections.Generic;

namespace WildMonsters
{
	public static class CollisionHandler
	{
		// Private variables:
		
		// Accessors:
		
		// Box collision
		
		//Blocks colliding
		public static void CheckBlockCollision(List<Ball> playerBalls, LevelGrid levelGrid)
		{
			//check if each member of the list has collided 
		
			Ball[,] gridBalls = levelGrid.getBalls ();
			GridProperties props = levelGrid.GetProperties();
			
			int bound0 = gridBalls.GetUpperBound(0);
			int bound1 = gridBalls.GetUpperBound(1);
						
			for (int k = 0 ; k<playerBalls.Count ; k++)
			{	
				int row = (int)(playerBalls[k].Sprite.Position.Y/50.0f);
				
				for(int x = 0; x < 20; x++)
				{
					if(gridBalls[row, x] != null && playerBalls[k].GetFired())
					{
						Ball b1 = gridBalls[row,x];// this should make it iterate through the whole 2D array
						
						if (playerBalls[k].GetBounds().Overlaps(b1.GetBounds()))
						{	
							//add ball to grid pos below colided obj
							if(props.flipped)
							{
								int gridX = (int)( (props.top - b1.Sprite.Position.X) / 50.0f) - 1;
								int gridY = (int)(b1.Sprite.Position.Y / 50.0f);
								
								gridBalls[gridY,gridX] = playerBalls[k];
							}
							else
							{
								int gridX = (int)( (b1.Sprite.Position.X - props.top) / 50.0f);
								int gridY = (int)(b1.Sprite.Position.Y / 50.0f);
								
								gridBalls[gridY,gridX] = playerBalls[k];
								
							}
							
							playerBalls[k].SetFired(false);
						}
					}
				}
			}
			
//			for (int y = 0; y <= bound0; y++)
//			{
//	  		  for (int x = 0; x <= bound1; x++)
//	 		   {
//					
//					
//	  		   }
//				Console.WriteLine();
//			}
			
		}
		
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

