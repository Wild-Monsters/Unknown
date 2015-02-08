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
	public class GameScene : Scene
	{
		private GridProperties grid1Properties, grid2Properties;
		private LevelGrid grid1, grid2;
		private LevelUI levelUI;
		private float time = 0;
		
		private float top = 960/2;
		private float topTarget = 960/2;
		private float divSpeed = 20.0f;
		
		private Player player1;
		private Player player2;
		// Fyring blocks
		
		
		public GameScene()
		{	
			this.Camera.SetViewFromViewport ();
			levelUI = new LevelUI(this);
			InitialiseGrids ();
			
			grid1.Draw (this);
			grid2.Draw (this);
			
			player1 = new Player(this, true);
			player2 = new Player(this, false);
			
			Scheduler.Instance.ScheduleUpdateForTarget(this, 1, false);	// Tells the director to call the update function of this "node"
			
			levelUI.divider.Top = top;
			levelUI.divider.TopTarget = top;

			
		}
		private void InitialiseGrids()
		{
			grid1Properties = new GridProperties();
			grid1Properties.height = 10;
			grid1Properties.width = 20;
			grid1Properties.cellSize = 50;
			grid1Properties.flipped = true;
			grid1Properties.xMargin = 50.0f;
			grid1Properties.yMargin = 0.0f;
			grid1Properties.top = 960.0f/2;
			grid1Properties.startRows = 3;
			
			grid2Properties = new GridProperties();
			grid2Properties.height = 10;
			grid2Properties.width = 20;
			grid2Properties.cellSize = 50;
			grid2Properties.flipped = false;
			grid2Properties.xMargin = 50.0f;
			grid2Properties.yMargin = 0.0f;
			grid2Properties.top = 960.0f/2;
			grid2Properties.startRows = 3;
			
			grid1 = new LevelGrid(grid1Properties, levelUI);
			grid2 = new LevelGrid(grid2Properties, levelUI);
		}
		public override void Update(float deltaTime)
		{	
			player1.Update (this);
			player2.Update (this);
			
			levelUI.Update (deltaTime);
			
			
			//grid top now set in LevelGrid
			//grid's top constantly updated to be equal to the top of the 'divider'
//			grid1.SetTop (top);
//			grid2.SetTop (top);
			
			grid1.Update (deltaTime);
			grid2.Update (deltaTime);
			
			//Collision Stuff trial 
			CollisionHandler.CheckBlockCollision(player1.getBalls(), grid1);
			CollisionHandler.CheckBlockCollision(player2.getBalls(), grid2);
		}
		
		

		
	}
}