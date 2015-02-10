using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace WildMonsters
{
	public struct GridProperties
	{
		public int height, width, cellSize, startRows;
		public float xMargin, yMargin;
		//to know which player we are
		public bool flipped;
		public float top;
	};
	
	public class LevelGrid
	{	
		private GridProperties props;
		private Ball[,] grid;
		private LevelUI levelUI;
		private bool bExploded;
		
		public LevelGrid (GridProperties _properties, LevelUI _levelUI)
		{
			props = _properties;
			
			grid = new Ball[props.height, props.width];
			
			levelUI = _levelUI;
						
			for(int a = 0; a < props.height; a++)
			{
				for(int b = 0; b < props.width; b++)
				{
					grid[a,b] = null;
				}
			}
			
			bExploded = false;
		}
		
		public void Update(float t)
		{
			levelUI.Update(t);
			props.top = levelUI.divider.Top;
			
			for(int a = 0; a < props.height; a++)
			{
				for(int b = 0; b < props.width; b++)
				{
					if(grid[a,b] != null)
					{
						float spriteX;
						float spriteY;
						
						if(props.flipped) //flipped = which way the grid is facing.
						{
							spriteX = ((props.top - props.xMargin) - (props.cellSize*b))-props.cellSize;
							spriteY = props.yMargin + (props.cellSize*a);
						}
						else
						{
							spriteX = (props.top + props.xMargin) + (props.cellSize*b);
							spriteY = props.yMargin + (props.cellSize*a);
						}
						
						grid[a,b].Sprite.Position = new Vector2 (spriteX,spriteY);
					}
				}
			}
		}
		
		public void SearchGrid(int xPos, int yPos, int matchesNeeded)
		{
			
			List<Vector2i> searchList = new List<Vector2i>();
			List<Vector2i> specialList = new List<Vector2i>();
			
			searchList.Add (new Vector2i(xPos,yPos));
			
			//The colour that we're searching for
			Colour targetColour =  grid[yPos,xPos].GetColour();
			
			int searchIndex = 0;
			Vector2i target = searchList[0];
			
			//Keep searching blocks until there's no more in the list
			while(searchIndex < searchList.Count)
			{
				//Start searching from the next block in the list
				target = searchList[searchIndex];
				
				//Search the blocks horizontally adjacent to the target
				for(int x = -1; x <= 1; x+=2)
				{
					int targetY = target.Y;
					int targetX = target.X+x;
					
					//This function checks the position of the current brick
					if(CompareGridPosition (targetX, targetY))
					{
						Colour thisColour = grid[targetY, targetX].GetColour();
						
						//Check the colour of the brick
						if(thisColour == targetColour)
						{
							Vector2i newVec = new Vector2i(targetX, targetY);
					
							//If the current block isn't already in the search list then add it
							if(!searchList.Contains(newVec))
							{
								searchList.Add(newVec);
							}
						}
						else
						{
							specialList.Add(new Vector2i(targetX, targetY));
						}
					}
				}
				
				//Search the blocks vertically adjacent to the target
				for(int y = -1; y <= 1; y+= 2)
				{
					int targetY = target.Y+y;
					int targetX = target.X;
					
					//This function checks the position and colour of the current brick
					if(CompareGridPosition (targetX, targetY))
					{
						Colour thisColour = grid[targetY, targetX].GetColour();
						
						//Check the colour of the brick
						if(thisColour == targetColour)
						{
							Vector2i newVec = new Vector2i(targetX, targetY);
					
							//If the current block isn't already in the search list then add it
							if(!searchList.Contains(newVec))
							{
								searchList.Add(newVec);
							}
						}
						else
						{
							specialList.Add(new Vector2i(targetX, targetY));
						}
					}
				}
				
				//"search the next block" 
				searchIndex++;
			}
			
			//If more than one matching colour was found
			if(searchIndex+1 > matchesNeeded)
			{
				CollisionHandler.BExploded = true;
				CollisionHandler.ResetExplodeAtArray();
				
				CheckSpecialCases (specialList);
				
				for(int a = 0; a < searchList.Count; a++)
				{
					int targetY = (int)searchList[a].Y;
					int targetX = (int)searchList[a].X;
					
					CollisionHandler.ExplodeAtArray[a] = new Vector2((float)grid[targetY, targetX].GetBounds().Min.X, 
					                                         (float)grid[targetY, targetX].GetBounds().Min.Y);
					
					grid[targetY, targetX].RemoveObject();
					grid[targetY, targetX] = null;	
					
					if(props.flipped)
					{
						levelUI.divider.TopTarget = levelUI.divider.TopTarget + 10;
						levelUI.divider.P1Score += 1;
					}
					else
					{
						levelUI.divider.TopTarget = levelUI.divider.TopTarget - 10;
						levelUI.divider.P2Score += 1;
					}
				}
			}
		}
		
		
		private bool CompareGridPosition(int targetX, int targetY)
		{
			//Check the search target is in-bounds
			//and then check if the colour matches the target's colour
			if(targetX >= 0 && targetX < props.width
			&& targetY >= 0 && targetY < props.height
			&& grid[targetY, targetX] != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		
		private void CheckSpecialCases(List<Vector2i> specialList)
		{
			for(int a = 0; a <specialList.Count; a++)
			{
				int targetX = specialList[a].X;
				int targetY = specialList[a].Y;
				
				Colour thisColour = grid[targetY, targetX].GetColour();
				
				switch(thisColour)
				{
					case Colour.Grey:
						grid[targetY,targetX].RandomiseColour(false);
					break;
				}
			}
			
			
		}
		

		
		
		public void SetTop(float x)
		{
			//The Top value is used to dictate where the "top" of the grid is, the center of the divider.
			props.top = x;
		}
		
		public void Draw(Scene _scene)
		{
			//Adds objects to the scene, the Director handles the actual drawing automatically
			for(int a = 0; a < props.height; a++)
			{
				for(int b = 0; b < props.startRows; b++)
				{
					Ball ball = new Ball(_scene);
					ball.RandomiseColour(true);
					grid[a,b] = ball;
				}
			}
		}
		
		public Ball[,] getBalls()
		{
			return this.grid;
		}
		
		public Bounds2 GetBounds(int i, int x)//row and column
		{
			//how?
			return grid[i,x].GetBounds();	
		}
		
		//write a function that gets a position in the grid 
		//write a function that adds a block at that position in the grid now 
		
		public GridProperties GetProperties()
		{
			return props;
		}
	}
}

