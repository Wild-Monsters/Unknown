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
			InitialiseGrids ();
			levelUI = new LevelUI(this);
			
			grid1.Draw (this);
			grid2.Draw (this);
			
			player1 = new Player(this, true);
			player2 = new Player(this, false);
			
			Scheduler.Instance.ScheduleUpdateForTarget(this, 1, false);	// Tells the director to call the update function of this "node"
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
			
			grid1 = new LevelGrid(grid1Properties);
			grid2 = new LevelGrid(grid2Properties);
		}
		public override void Update(float deltaTime)
		{
			if(Input.KeyPressed (GamePadButtons.R))
				topTarget += 50.0f;
			
			
			if(Input.KeyPressed (GamePadButtons.L))
				topTarget -= 50.0f;
			
			
			if(top < topTarget)
				top += 10.0f;
			
			
			if(top > topTarget)
				top -= 10.0f;
			
			
			player1.Update (this);
			player2.Update (this);
			
			levelUI.divider.SetTop (top);
			levelUI.Update (deltaTime);
			
			grid1.SetTop (top);
			grid2.SetTop (top);
			
			grid1.Update (deltaTime);
			grid2.Update (deltaTime);
			
			//Collision Stuff trial 
			CollisionHandler.CheckBlockCollision(player1.getBalls(), grid1);
			CollisionHandler.CheckBlockCollision(player2.getBalls(), grid2);
		}
		

		
	}
}