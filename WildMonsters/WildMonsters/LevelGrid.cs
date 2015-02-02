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
		
		
		
	}
}

