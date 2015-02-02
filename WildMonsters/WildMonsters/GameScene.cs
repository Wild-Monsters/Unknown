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
		private GridProperties grid1Props, grid2Props;
		private LevelGrid grid1, grid2;
		private LevelUI levelUI;
		
		private float top = 960/2;
		
		private Player player1;
		private Player player2;
		// Fyring blocks
		
		
		public GameScene()
		{
			this.Camera.SetViewFromViewport ();
			Scheduler.Instance.ScheduleUpdateForTarget(this, 1, false);

			InitialiseGrids ();
			levelUI = new LevelUI(this);
			
			grid1.Draw (this);
			grid2.Draw (this);
			
			player1 = new Player(this, true);
			player2 = new Player(this, false);
		}

		private void InitialiseGrids()
		{
			grid1Props = new GridProperties();
			grid1Props.height = 10;
			grid1Props.width = 3;
			grid1Props.cellSize = 50;
			grid1Props.flipped = true;
			grid1Props.xMargin = 50.0f;
			grid1Props.yMargin = 0.0f;
			grid1Props.top = 960.0f/2;
			grid1Props.startRows = 3;
			
			grid2Props = new GridProperties();
			grid2Props.height = 10;
			grid2Props.width = 3;
			grid2Props.cellSize = 50;
			grid2Props.flipped = false;
			grid2Props.xMargin = 50.0f;
			grid2Props.yMargin = 0.0f;
			grid2Props.top = 960.0f/2;
			grid2Props.startRows = 3;
			
			grid1 = new LevelGrid(grid1Props);
			grid2 = new LevelGrid(grid2Props);
		}

		public override void Update(float t)
		{
			UpdateTop (t);

			player1.Update (this);
			player2.Update (this);
			levelUI.Update (t);
			grid1.Update (t);
			grid2.Update (t);
			
		}
		
		private void UpdateTop(float t)
		{
			if(Input.KeyPressed (GamePadButtons.Right))
				top += 50.0f;
			
			if(Input.KeyPressed (GamePadButtons.Left))
				top -= 50.0f;
			
			levelUI.divider.SetTop (top);
			
			grid1.SetTop (top);
			grid2.SetTop (top);
		}
	}
}