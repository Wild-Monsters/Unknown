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
		public bool flipped;
		public float top;
	};
	
	public class LevelGrid
	{	
		private GridProperties props;
		private Ball[,] grid;
		
		public LevelGrid (GridProperties _properties)
		{
			props = _properties;
			
			grid = new Ball[props.height, props.width];
			
			for(int a = 0; a < props.height; a++)
			{
				for(int b = 0; b < props.width; b++)
				{
					grid[a,b] = null;
				}
			}
		}
		
		public void Update(float t)
		{
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
			List<Vector2> searchList = new List<Vector2>();
			searchList.Add (new Vector2(xPos,yPos));
			
			Colour targetColour =  grid[yPos,xPos].GetColour();
			
			int searchIndex = 0;
			Vector2 target = searchList[0];
			
			while(searchIndex < searchList.Count)
			{
				target = searchList[searchIndex];
				
				for(int x = -1; x <= 1; x+=2)
				{
					int targetY = (int)target.Y;
					int targetX = (int)target.X+x;

					if(CompareGridPosition (targetX, targetY, targetColour))
					{
						Vector2 newVec = new Vector2(targetX, targetY);
				
						if(!searchList.Contains(newVec))
						{
							searchList.Add(newVec);
						}
					}
				}
				for(int y = -1; y <= 1; y+= 2)
				{
					int targetY = (int)target.Y+y;
					int targetX = (int)target.X;
					
					if(CompareGridPosition (targetX, targetY, targetColour))
					{
						Vector2 newVec = new Vector2(targetX, targetY);
				
						if(!searchList.Contains(newVec))
						{
							searchList.Add(newVec);
						}
					}
				}
				
				searchIndex++;
			}
			
			//If more than one matching colour was found
			if(searchIndex+1 > matchesNeeded)
			{
				for(int a = 0; a < searchList.Count; a++)
				{
					int targetY = (int)searchList[a].Y;
					int targetX = (int)searchList[a].X;
					
					grid[targetY, targetX].RemoveObject();
					grid[targetY, targetX] = null;
				}
			}
		}
		
		
		private bool CompareGridPosition(int targetX, int targetY, Colour targetColour)
		{
			//Check the search target is in-bounds
			//and then check if the colour matches the target's colour
			if(targetX > 0 && targetX < props.width
			&& targetY > 0 && targetY < props.height
			&& grid[targetY, targetX] != null
			&& grid[targetY, targetX].GetColour() == targetColour)
			{
				return true;
			}
			else
			{
				return false;
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

