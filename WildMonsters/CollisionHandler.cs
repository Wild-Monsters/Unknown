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
		public static bool CheckBlockCollision2(List<Ball> pBalls, LevelGrid levelGrid)
		{
			Ball[,] gridBalls = levelGrid.getBalls ();
			GridProperties props = levelGrid.GetProperties();
			List<int> deleteList = new List<int>();
				
			bool ballCollided = false;
			
			for (int k = 0 ; k < pBalls.Count ; k++)
			{	
				ballCollided = false;
				
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
				if(column > 0 && gridBalls[row, column-1] != null)
				{
					//Add to the grid
					gridBalls[row, column] = pBalls[k];
					
					//Search the grid
					levelGrid.SearchGrid(column, row, 2);
					
					ballCollided = true;
					
					if(pBalls.Count > 2)
					{
						Console.WriteLine (pBalls.Count);
					}
					
					pBalls[k].SetState(BallState.Locked);
				}
				
				//If no collision was detected, did the ball collide with the divider?
				if(ballCollided == false)
					ballCollided = CheckCollisionWithDivider (pBalls, levelGrid, k, row);
				
				//Remove the ball if it collided
				if(ballCollided)
				{
					deleteList.Add (k);// Add the index of the ball that needs to be deleted to the list
					Console.WriteLine(pBalls[k].GetColour());
				}
				
			}//End of 'k' for loop
			
			for(int a = deleteList.Count-1; a >= 0; a--)
			{
				pBalls.RemoveAt(deleteList[a]);
			}
			
			if(ballCollided)
			{
				return true;
			}
			else
			{
				return false;
			}
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

