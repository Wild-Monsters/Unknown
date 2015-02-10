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
		private static bool bExploded = false;
		private static Vector2 explodeAt = new Vector2(0.0f, 0.0f);
		private static Vector2[] explodeAtArray = new Vector2[10];
		private static Vector2[] explodeAtMovingArray = new Vector2[10];
		private static bool bMoving = false;
		private static bool bLocked = false;
		
		// Accessors:
		public static bool BExploded {get{return bExploded;} set{bExploded = value;}}
		public static Vector2 ExplodeAt {get {return explodeAt;} set{explodeAt = value;}}
		public static Vector2[] ExplodeAtArray {get{return explodeAtArray;} set{explodeAtArray = value;}}
		public static Vector2[] ExplodeAtMovingArray {get{return explodeAtMovingArray;} set{explodeAtMovingArray = value;}}
		public static bool BMoving {get{return bMoving;} set{bMoving = value;}}
		public static bool BLocked {get{return bLocked;} set{bLocked = value;}}
		
		public static void ResetExplodeAtArray()
		{
			for(int i = 0; i < 10; i++)
			{
				explodeAtArray[i] = new Vector2(0,0);
			}
		}
		
		public static void ResetExplodeAtMovingArray()
		{
			for(int i = 0; i < 10; i++)
			{
				explodeAtMovingArray[i] = new Vector2(0,0);
			}
		}
		
		// Box collision
		
		//Blocks colliding
		public static void CheckBlockCollision(List<Ball> pBalls, LevelGrid levelGrid)
		{
			//check if each member of the list has collided 
		
			Ball[,] gridBalls = levelGrid.getBalls ();
			GridProperties props = levelGrid.GetProperties();
			
			int bound0 = gridBalls.GetUpperBound(0);
			int bound1 = gridBalls.GetUpperBound(1);
			
			bool ballCollided = false;
			
			for (int k = 0 ; k < pBalls.Count ; k++)
			{	
				int row = (int)(pBalls[k].Sprite.Position.Y/50.0f);
				
				for(int x = 0; x < 20; x++)
				{
					
					if(gridBalls[row, x] != null && pBalls[k].GetState() == BallState.Rising)
					{
						if (pBalls[k].GetBounds().Overlaps(gridBalls[row,x].GetBounds()))
						{
							Vector2i gPos = GetGridPosition (gridBalls[row,x], props);
							
							//Add to the grid
							gridBalls[gPos.Y,gPos.X] = pBalls[k];
							
							//Search the grid
							levelGrid.SearchGrid(gPos.X, gPos.Y, 2);
							
							ballCollided = true;
							
							bMoving = false;
							
							pBalls[k].SetState(BallState.Locked);
						}
					}//End of 'y' for loop
				}// End of 'x' for loop
				
				
				if(props.flipped)
				{
					float frontOfBlock = pBalls[k].Sprite.Position.X+props.cellSize;
					float topOfGrid = props.top-props.xMargin;
					
					//Set the blocks X position to 0 on the grid if it hits the top
					if(frontOfBlock > topOfGrid
					&& pBalls[k].GetState() == BallState.Rising)
					{
						gridBalls[row, 0] = pBalls[k];
						levelGrid.SearchGrid(0, row, 2);
						
						pBalls[k].SetState(BallState.Locked);
						ballCollided = true;
					}
				}
				else
				{
					float frontOfBlock = pBalls[k].Sprite.Position.X;
					float topOfGrid = props.top+props.xMargin;
					
					//Set the blocks X position to 0 on the grid if it hits the top
					if(frontOfBlock < topOfGrid
					&& pBalls[k].GetState() == BallState.Rising)
					{
						gridBalls[row, 0] = pBalls[k];
						levelGrid.SearchGrid(0, row, 2);
						
						pBalls[k].SetState(BallState.Locked);
						ballCollided = true;
					}
				}
				

				
				//Remove the ball if it collided
				if(ballCollided)
					pBalls.RemoveAt(k);
				
				
			}//End of 'k' for loop

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
		
		private static Vector2i GetGridPosition(Ball target, GridProperties props)
		{
			int gridX;
			int gridY;
			
			//find grid pos at collided obj
			if(props.flipped)
			{
				gridX = (int)((props.top - target.Sprite.Position.X) / 50.0f) - 1 ;
				gridY = (int)(target.Sprite.Position.Y / 50.0f);
			}
			else
			{
				gridX = (int)( (target.Sprite.Position.X - props.top) / 50.0f);
				gridY = (int)(target.Sprite.Position.Y / 50.0f);
			}
			
			return new Vector2i(gridX, gridY);
		}
	}
}

