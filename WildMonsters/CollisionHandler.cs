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
		
		public static void CheckBlockCollision2(List<Ball> pBalls, LevelGrid levelGrid)
		{
			Ball[,] gridBalls = levelGrid.getBalls ();
			GridProperties props = levelGrid.GetProperties();
			bool ballCollided = false;
			
			for (int k = 0 ; k < pBalls.Count ; k++)
			{	
				//Set the row/x grid position 
				int row = (int)((pBalls[k].Sprite.Position.Y+5.0f)/50.0f);
				
				//lazy bug fix!! blocks occasionally added outside of grid
				if(row >= 10)
					row = 9;
				
				//Set the column/x grid position depending on which grid it is
				int column;
				if(props.flipped)
				{
					column = (int)((props.top - pBalls[k].Sprite.Position.X - props.cellSize) / 50.0f);
				}
				else
				{
					column = (int)((pBalls[k].Sprite.Position.X - props.top)/50.0f);
				}
				
				//Check if the space infront of the brick is occupied
				if(column != 0 && gridBalls[row, column-1] != null)
				{
					//Add to the grid
					gridBalls[row, column] = pBalls[k];
					
					//Search the grid
					levelGrid.SearchGrid(column, row, 2);
					
					ballCollided = true;
					
					pBalls[k].SetState(BallState.Locked);
				}
				
				//If no collision was detected, did the ball collide with the divider?
				if(ballCollided == false)
					ballCollided = CheckCollisionWithDivider (pBalls, levelGrid, k, row);
				
				//Remove the ball if it collided
				if(ballCollided)
					pBalls.RemoveAt(k);
				
			}//End of 'k' for loop
		}
		
		
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
				int row = (int)((pBalls[k].Sprite.Position.Y+1.0f)/50.0f);
				
				if(row >= props.height)
				{
					row = props.height-1;
				}
				
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
							
							pBalls[k].SetState(BallState.Locked);
						}
					}//End of 'y' for loop
				}// End of 'x' for loop
				
				if(ballCollided == false)
					ballCollided = CheckCollisionWithDivider (pBalls, levelGrid, k, row);
				
				//Remove the ball if it collided
				if(ballCollided)
					pBalls.RemoveAt(k);
				
			}//End of 'k' for loop

		}
		
		private static bool CheckCollisionWithDivider(List<Ball> pBalls, LevelGrid levelGrid, int index, int row)
		{
			Ball[,] gridBalls = levelGrid.getBalls ();
			GridProperties props = levelGrid.GetProperties();
			
			if(props.flipped)
			{
				float frontOfBlock = pBalls[index].Sprite.Position.X+props.cellSize;
				float topOfGrid = props.top-props.xMargin;
				
				//Set the blocks X position to 0 on the grid if it hits the top
				if(frontOfBlock > topOfGrid
				&& pBalls[index].GetState() == BallState.Rising)
				{
					gridBalls[row, 0] = pBalls[index];
					levelGrid.SearchGrid(0, row, 2);
					
					pBalls[index].SetState(BallState.Locked);
					return true;
				}
			}
			else
			{
				float frontOfBlock = pBalls[index].Sprite.Position.X;
				float topOfGrid = props.top+props.xMargin;
				
				//Set the blocks X position to 0 on the grid if it hits the top
				if(frontOfBlock < topOfGrid
				&& pBalls[index].GetState() == BallState.Rising)
				{
					gridBalls[row, 0] = pBalls[index];
					levelGrid.SearchGrid(0, row, 2);
					
					pBalls[index].SetState(BallState.Locked);
					return true;
				}
			}
			
			return false;
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
			//diff.X = particleOne.getMyPos().X - particleTwo.getMyPos().X;
			//diff.Y = particleOne.getMyPos().Y - particleTwo.getMyPos().Y;

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

