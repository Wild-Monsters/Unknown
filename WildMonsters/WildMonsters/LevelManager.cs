using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;


namespace WildMonsters
{
	public class LevelManager
	{
		private GridProperties grid1Properties, grid2Properties;
		private LevelGrid grid1, grid2;
		private LevelUI levelUI;
		private float time = 0;
		
		private float top = 960/2;
		private float topTarget = 960/2;
		private float divSpeed = 20.0f;
		
		public LevelManager (Scene _scene)
		{
			InitialiseGrids ();
			levelUI = new LevelUI(_scene);
			
			grid1.Draw (_scene);
			grid2.Draw (_scene);
			
		}
		
		private void InitialiseGrids()
		{
			grid1Properties = new GridProperties();
			grid1Properties.height = 10;
			grid1Properties.width = 3;
			grid1Properties.cellSize = 50;
			grid1Properties.flipped = true;
			grid1Properties.xMargin = 50.0f;
			grid1Properties.yMargin = 0.0f;
			grid1Properties.top = 960.0f/2;
			
			grid2Properties = new GridProperties();
			grid2Properties.height = 10;
			grid2Properties.width = 3;
			grid2Properties.cellSize = 50;
			grid2Properties.flipped = false;
			grid2Properties.xMargin = 50.0f;
			grid2Properties.yMargin = 0.0f;
			grid2Properties.top = 960.0f/2;
			
			grid1 = new LevelGrid(grid1Properties);
			grid2 = new LevelGrid(grid2Properties);
		}
		
		public void Update(float t)
		{
			//Sine Wave Movement (just for show) 
			//time += t/2;
			//float top = (float)System.Math.Sin((double)(time*2*System.Math.PI))*40 + 960/

			if(Input.KeyPressed (GamePadButtons.Right))
			{
				topTarget += 50.0f;
			}
			
			if(Input.KeyPressed (GamePadButtons.Left))
			{
				topTarget -= 50.0f;
			}
			
			if(top < topTarget)
			{
				top += FMath.Clamp ((topTarget-top)/divSpeed,0.5f,10.0f);
			}
			
			if(top > topTarget)
			{
				top -= FMath.Clamp ((top-topTarget)/divSpeed,0.5f,10.0f);
			}
			
			top = FMath.Floor (top);
			
			levelUI.divider.SetTop (top);
			levelUI.Update (t);
			
			grid1.SetTop (top);
			grid2.SetTop (top);
			
			grid1.Update (t);
			grid2.Update (t);
		}
	}
}

