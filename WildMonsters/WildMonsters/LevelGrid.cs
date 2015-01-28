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
		public int height, width, cellSize;
		public float xMargin, yMargin;
		public bool flipped;
		public float top;
	};
	
	
	public class LevelGrid
	{	
		private GridProperties props;
		private SpriteUV[,] grid;
		private float time = 0;
		
		public LevelGrid (GridProperties _properties)
		{
			props = _properties;
			
			grid = new SpriteUV[props.height, props.width];
			
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
			//Sine Wave Movement (just for show) 
			time += t/2;
			props.top = (float)System.Math.Sin((double)(time*2*System.Math.PI))*40 + 960/2;
			///////
			
			
			for(int a = 0; a < props.height; a++)
			{
				for(int b = 0; b < props.width; b++)
				{
					float spriteX;
					float spriteY;
					
					if(props.flipped)
					{
						spriteX = ((props.top - props.xMargin) - (props.cellSize*b))-props.cellSize;
						spriteY = props.yMargin + (props.cellSize*a);
					}
					else
					{
						spriteX = (props.top + props.xMargin) + (props.cellSize*b);
						spriteY = props.yMargin + (props.cellSize*a);
					}
					
					grid[a,b].Position = new Vector2 (spriteX,spriteY);
				}
			}
		}
		
		public void Draw(Scene _scene)
		{
			//Adds objects to the scene, the Director handles the actual drawing automatically
			
			for(int a = 0; a < props.height; a++)
			{
				for(int b = 0; b < props.width; b++)
				{
					TextureInfo texInfo = new TextureInfo("/Application/textures/TestSprite.png");
					SpriteUV sprite = new SpriteUV(texInfo);
					
					sprite.Quad.S = texInfo.TextureSizef;
					sprite.Position = new Vector2 (0.0f,0.0f);

					_scene.AddChild (sprite);
					grid[a,b] = sprite;
				}
			}
		}
	}
}

