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
	
	public class GameScene : Sce.PlayStation.HighLevel.GameEngine2D.Scene
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
		private AudioManager audio;
		
		private float top = 960/2;
		
		private Player player1;
		private Player player2;
		// Fyring blocks
		
		private Sce.PlayStation.HighLevel.UI.Scene uiScene;
		private Sce.PlayStation.HighLevel.UI.Label p1Score;
		
		private SpriteUV TEMPBackgroundImage;
		
		private Button touch1,touch2,touch3,touch4;
		
		private TextureInfo bgTexInfo;
		private string name = "GameScene";
		public string GetName()
		{
			return this.name;
		}
		public GameScene()
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
			player2 = new Player(this, false);
			
			player1.SetLevelGrid(ref grid1);
			player2.SetLevelGrid(ref grid2);

			levelUI.divider.Top = top;
			levelUI.divider.TopTarget = top;
			
			uiScene.RootWidget.AddChildLast(panel);
			uiScene.RootWidget.AddChildLast(p1Score);
			
			UISystem.SetScene(uiScene);
			
			TouchScreenButtons ();
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
			grid1Properties.powerUps = true;
			
			grid2Properties = new GridProperties();
			grid2Properties.height = 10;
			grid2Properties.width = 20;
			grid2Properties.cellSize = 50;
			grid2Properties.flipped = false;
			grid2Properties.xMargin = 50.0f;
			grid2Properties.yMargin = 0.0f;
			grid2Properties.top = 960.0f/2;
			grid2Properties.startRows = 3;
			grid2Properties.powerUps = true;
			
			grid1 = new LevelGrid(grid1Properties, levelUI, this, null);
			grid2 = new LevelGrid(grid2Properties, levelUI, this, null);
		}
		public override void Update(float deltaTime)
		{	
			player1.Update (this, deltaTime);
			player2.Update (this, deltaTime);
			
			levelUI.Update (deltaTime);
			
			grid1.Update (deltaTime, ref audio);
			grid2.Update (deltaTime, ref audio);
			
			grid1.UpdateBallPositions();
			grid2.UpdateBallPositions();
			
			
		
			//GC.GetTotalMemory(true);
			
			
			//Rotate background; faster if it gets close to a player
			float rotationSpeed = 0.002f*(Constants.ScreenWidth/2 - grid1.GetProperties ().top);
			if(rotationSpeed == 0.0f) rotationSpeed = 0.04f;
			TEMPBackgroundImage.Angle += rotationSpeed;
			
			//Collision Stuff 
			//Made it so the collision check returns true if a collision was found 
			if(CollisionHandler.CheckBlockCollision2(player1.getBalls(), grid1))
			{
				player1.CheckBallDisplay();
			}
			
			if(CollisionHandler.CheckBlockCollision2(player2.getBalls(), grid2))
			{
				player2.CheckBallDisplay();
			}
			
			ParticleManager.AddClickTrail(this);
			ParticleManager.Update(this);
			
			if(Input2.GamePad0.Start.Press)
            {
                audio.StopGameMusic();
                audio.Dispose();
				Director.Instance.ReplaceScene(new PauseScreen("GameScene"));
                //Director.Instance.PushScene(new PauseScreen("GameScene"));
            }
			
			touch1.Update();
			touch2.Update();
			touch3.Update();
			touch4.Update();
		}
		
		
		private void TouchScreenButtons()
		{
			TextureInfo tex1 = new TextureInfo("/Application/textures/TouchscreenButton.png");
			touch1 = new Button(this, 100, tex1, new Vector2(240.0f,50.0f), 2);
			
			TextureInfo tex2 = new TextureInfo("/Application/textures/TouchscreenButton.png");
			touch2 = new Button(this, 100, tex2, new Vector2(240.0f,494.0f), 2);
			
			TextureInfo tex3 = new TextureInfo("/Application/textures/TouchscreenButton.png");
			touch3 = new Button(this, 100, tex3, new Vector2(720.0f,50.0f), 2);
			
			TextureInfo tex4 = new TextureInfo("/Application/textures/TouchscreenButton.png");
			touch4 = new Button(this, 100, tex4, new Vector2(720.0f,494.0f), 2);
			
		}
		
		public void DrawBackgroundTempFunction()
		{
			bgTexInfo = null;
			bgTexInfo = new TextureInfo("/Application/textures/MenuSpiralLight.png");
			SpriteUV sprite = new SpriteUV(bgTexInfo);
			sprite.Scale =  new Vector2(1.2f,1.2f);
			sprite.Quad.S = new Vector2(1100.0f,1100.0f);
			sprite.Position = new Vector2(-68.0f,-280.0f);
			sprite.Pivot = new Vector2(550.0f,550.0f);
			
			TEMPBackgroundImage = sprite;
			
			this.AddChild (sprite);
		}
		
		public LevelGrid GetGrid1()
		{
			return grid1;
		}
		
		public LevelGrid GetGrid2()
		{
			return grid2;	
		}
		
		public void StartMusic()
		{
			audio.PlayGameMusic();
		}
		
		public void DisposeMusic()
		{
			audio.Dispose ();
		}
		
		public void SetGameOver()
		{
			player1.SetGameOver (true);
			player2.SetGameOver (true);
		}
		
	}
}