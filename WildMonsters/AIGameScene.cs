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
	//private AudioManager audio;
	
	public class AIGameScene : Sce.PlayStation.HighLevel.GameEngine2D.Scene
	{
		   /*if(audio == null)
			{
				audio = new AudioManager();
			}
			//Play in game music
			audio.PlayGameMusic();*/
		
		private GridProperties grid1Properties, grid2Properties;
		private LevelGrid grid1, grid2;
		private LevelUI levelUI;
		
		private float top = 960/2;
		
		private Player player1;
		private Player player2;
		// Fyring blocks
		
		private Sce.PlayStation.HighLevel.UI.Scene uiScene;
		private Sce.PlayStation.HighLevel.UI.Label p1Score;
		
		private SpriteUV TEMPBackgroundImage;
		private Timer AIShootDelay;
        private const double AITimeDelay = 700.0;
		private AudioManager audio;
		private string name = "AI";
		public string GetName()
		{
			return this.name;
		}
		public AIGameScene()
		{	
			Scheduler.Instance.ScheduleUpdateForTarget(this, 1, false);	// Tells the director to call the update function of this "node"
			
			if(audio == null)
			{
				audio = new AudioManager();
			}
			//Play in game music
			audio.PlayGameMusic();
			
			uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
			Panel panel = new Panel();
			panel.Width = Director.Instance.GL.Context.GetViewport().Width;
			panel.Height = Director.Instance.GL.Context.GetViewport().Height;
			
			//Ai shooting delay 
            AIShootDelay = new Timer();

			
			p1Score = new Sce.PlayStation.HighLevel.UI.Label();
			p1Score.X = 10;
			p1Score.Y = Director.Instance.GL.Context.GetViewport().Height / 2;
			p1Score.Width = 300;
			p1Score.TextColor = (UIColor)Colors.Orange;
			p1Score.Text = "Player 1 Score: ";
			
			DrawBackgroundTempFunction ();
			
			this.Camera.SetViewFromViewport ();
			levelUI = new LevelUI(this);

			InitialiseGrids ();
			
			grid1.Draw (this);
			grid2.Draw (this);
			
			player1 = new Player(this, true);
			player1.CanMove(false);
			player2 = new Player(this, false);
			
			player1.SetLevelGrid(ref grid1);
			player2.SetLevelGrid(ref grid2);

			levelUI.divider.Top = top;
			levelUI.divider.TopTarget = top;
			
			uiScene.RootWidget.AddChildLast(panel);
			uiScene.RootWidget.AddChildLast(p1Score);
			
			UISystem.SetScene(uiScene);
			

			
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
			grid1Properties.powerUps = false;
			
			grid2Properties = new GridProperties();
			grid2Properties.height = 10;
			grid2Properties.width = 20;
			grid2Properties.cellSize = 50;
			grid2Properties.flipped = false;
			grid2Properties.xMargin = 50.0f;
			grid2Properties.yMargin = 0.0f;
			grid2Properties.top = 960.0f/2;
			grid2Properties.startRows = 3;
			grid2Properties.powerUps = false;
			
			grid1 = new LevelGrid(grid1Properties, levelUI, null, this);
			grid2 = new LevelGrid(grid2Properties, levelUI, null, this);
		}
		
		public override void Update(float deltaTime)
		{	
			player1.Update (this, deltaTime);
			player2.Update (this, deltaTime);
			
			levelUI.Update (deltaTime);
			
			grid1.Update (deltaTime, ref audio);
			grid2.Update (deltaTime, ref audio);
			
			TEMPBackgroundImage.Angle += 0.06f;
			
			//Collision Stuff trial 
			CollisionHandler.CheckBlockCollision2(player1.getBalls(), grid1);
			CollisionHandler.CheckBlockCollision2(player2.getBalls(), grid2);
			
			grid1.UpdateBallPositions();
			grid2.UpdateBallPositions();
			
			if(AIShootDelay.Milliseconds() >= AITimeDelay)
            {
 
                grid1.StoreTopLevelBalls(grid1, player1, this);
                AIShootDelay.Reset();
            }
            
            //Console.WriteLine("Available memory " + GC.GetTotalMemory(true).ToString()); //retrieves the total number of bytes currently allocated
			
			ParticleManager.AddClickTrail(this);
			ParticleManager.Update(this);
			    if(Input2.GamePad0.Start.Press)
            {
                audio.StopGameMusic();
                audio.Dispose ();
				Director.Instance.ReplaceScene(new PauseScreen("AI"));
                //Director.Instance.PushScene(new PauseScreen("AI"));
            }
		}
		
		
		public void DrawBackgroundTempFunction()
		{
			TextureInfo texInfo = new TextureInfo("/Application/textures/MenuSpiralLight.png");
			SpriteUV sprite = new SpriteUV(texInfo);
			sprite.Scale =  new Vector2(1.2f,1.2f);
			sprite.Quad.S = new Vector2(1100.0f,1100.0f);
			sprite.Position = new Vector2(-68.0f,-280.0f);
			sprite.Pivot = new Vector2(550.0f,550.0f);
			
			TEMPBackgroundImage = sprite;
			
			this.AddChild (sprite);
		}
		public void SetGameOver()
		{
 			player1.SetGameOver (true);
			player2.SetGameOver (true);
		} 
	}
}