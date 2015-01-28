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

		
		public LevelManager (Scene _scene)
		{
			InitialiseGrids ();
			
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
			grid1.Update (t);
			grid2.Update (t);
		}
	}
}

