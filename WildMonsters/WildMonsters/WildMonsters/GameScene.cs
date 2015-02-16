using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

using Sce.PlayStation.HighLevel.UI;

namespace WildMonsters
{
	public class GameScene : Sce.PlayStation.HighLevel.GameEngine2D.Scene
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
		
		private Sce.PlayStation.HighLevel.UI.Scene uiScene;
		private Sce.PlayStation.HighLevel.UI.Label p1Score;
		
		
		public GameScene()
		{	
			uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
			Panel panel = new Panel();
			panel.Width = Director.Instance.GL.Context.GetViewport().Width;
			panel.Height = Director.Instance.GL.Context.GetViewport().Height;

			
			p1Score = new Sce.PlayStation.HighLevel.UI.Label();
			p1Score.X = 10;
			p1Score.Y = Director.Instance.GL.Context.GetViewport().Height / 2;
			p1Score.Width = 300;
			p1Score.TextColor = (UIColor)Colors.Orange;
			p1Score.Text = "Player 1 Score: ";
			
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
			
			uiScene.RootWidget.AddChildLast(panel);
			uiScene.RootWidget.AddChildLast(p1Score);
			
			UISystem.SetScene(uiScene);
			
			CollisionHandler.ResetExplodeAtMovingArray();
			

			
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
			
			if(CollisionHandler.BExploded)
			{
				ParticleGenerator(CollisionHandler.ExplodeAtArray);
			}
			
			ClickParticleGenerator();
			
			//MovingParticleGenerator(CollisionHandler.ExplodeAtMovingArray);
			
			ParticleManager.Update(this);
		}
		
		public void ParticleGenerator(Vector2[] explodeAt)
		{
			int quadAssign = 0;
			Colour colour = CollisionHandler.Colourblock;
			// Loop each element in the array
			for(int j = 0; j < 10; j++)
			{
				// If the element contains a position of 0,0 - don't produce any particles at that position
				if(explodeAt[j].X != 0.0f && explodeAt[j].Y != 0.0f)
				{
					// Produce 10 particles per call
					for(int i = 0; i < 16; i++)
					{
						quadAssign++;
						if(quadAssign >= 4)
						{
							quadAssign = 0;
						}
						Vector2 randPos = new Vector2(ParticleManager.CreateRandomPosition().X, ParticleManager.CreateRandomPosition().Y);
						ParticleManager.AddParticle (this, new Vector2(explodeAt[j].X + (float)randPos.X,
							explodeAt[j].Y + (float)randPos.Y), 1, 0, quadAssign, colour);
					}
				}
			}

			CollisionHandler.BExploded = false;
		}
		
		
		public void ClickParticleGenerator()
		{
			List<TouchData> touches = Touch.GetData(0);
			
			foreach(TouchData data in touches)
			{
				Vector2 randPos = new Vector2(ParticleManager.CreateRandomPosition().X, ParticleManager.CreateRandomPosition().Y);
			
				ParticleManager.AddParticle (this, new Vector2((((data.X + 0.5f) * Constants.ScreenWidth) + (float)randPos.X),
						Constants.ScreenHeight - ((data.Y + 0.5f) * Constants.ScreenHeight) + (float)randPos.Y), 1, 1, 0, Colour.Yellow);
			}
		}
		
		public void MovingParticleGenerator(Vector2[] explodeAtMoving)
		{	
			// Keep here, will be used for having the particle the same colour as the block when moving.
			Colour colour = CollisionHandler.Colourblock;
			
			// States whether the ball is fired from the left (true) or from the right (false).
			bool bLeft = CollisionHandler.BLeft;
			
			// Loop each element in the array
			for(int j = 0; j < 10; j++)
			{
				// Set to -100 as if I set it to 0.0f it doesn't allow particles to be created when shooting a block from the bottom of the scene.
				if(explodeAtMoving[j].X != -100.0f && explodeAtMoving[j].Y != -100.0f)
				{
					// 10 particles will be created per 'batch'
					for(int i = 0; i < 10; i++)
					{
						// If a block is moving...
						if(CollisionHandler.BMoving)
						{
							// Trail on the left
							Vector2 randPos = new Vector2(ParticleManager.CreateRandomPosition().X, ParticleManager.CreateRandomPosition().Y);
							float x = explodeAtMoving[j].X;
							// If we're not firing the ball from the left, adjust the position of the origin of the particles to be behind
							// the ball.
							if(!bLeft)
							{
								x += 40.0f;
							}
							ParticleManager.AddParticle (this, new Vector2(x,
								explodeAtMoving[j].Y + (float)randPos.Y), 1, 2, 0, colour);
						}
					}
				}
			}
		}
	}
}