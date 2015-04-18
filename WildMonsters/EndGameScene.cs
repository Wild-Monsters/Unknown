/*using System;
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
	public class EndgameScene : Sce.PlayStation.HighLevel.GameEngine2D.Scene
	{
				
		private Sce.PlayStation.HighLevel.UI.Scene uiScene;
		
		private AudioManager audio;
		
		public EndgameScene ()
		{
			if(audio == null)
			{
				audio = new AudioManager();
			}
			
			this.Camera.SetViewFromViewport();
			Sce.PlayStation.HighLevel.UI.Panel dialog = new Panel();
			dialog.Width = Director.Instance.GL.Context.GetViewport ().Width;
			dialog.Height = Director.Instance.GL.Context.GetViewport().Height;
			
			ImageBox imBox = new ImageBox();
			imBox.Width = dialog.Width;
			imBox.Image = new ImageAsset("Application/Textures/Endgame.png", false);
			imBox.Height = dialog.Height; 
			imBox.SetPosition(0.0f, 0.0f);
			
			Button buttonUI1 = new Button();
			buttonUI1.Name = "GameModes";
			buttonUI1.Text = "Play Again";
			buttonUI1.Width = 300;
			buttonUI1.Height = 50;
			buttonUI1.Alpha = 0.8f;
			buttonUI1.SetPosition(dialog.Width/2 - 150, 400.0f);
			buttonUI1.TouchEventReceived += (sender, e) => 
			{
				Director.Instance.ReplaceScene (new GameScene());
			};
			
			Button buttonUI2 = new Button();
			buttonUI2.Name = "Quit";
			buttonUI2.Text = "Quit to Menu";
			buttonUI2.Width = 300;
			buttonUI2.Height = 50;
			buttonUI2.Alpha = 0.8f;
			buttonUI2.SetPosition(dialog.Width/2 - 150, 200.0f);
			buttonUI2.TouchEventReceived += (sender, e) => 
			{
				Director.Instance.ReplaceScene (new MenuScene());
			};
			
			dialog.AddChildLast (imBox);
			dialog.AddChildLast (buttonUI1);
			dialog.AddChildLast (buttonUI2);
			uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
			uiScene.RootWidget.AddChildLast(dialog);
			UISystem.SetScene(uiScene);
			Scheduler.Instance.ScheduleUpdateForTarget (this, 0, false);
			
		}
		
		public override void Update(float dt)
		{
			base.Update (dt);
			UISystem.Update(Touch.GetData(0));
		}
		
		public override void Draw()
		{
			base.Draw ();
			UISystem.Render();
		}
		
		
	}
}*/

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
	
	
	public class EndGameScene : Sce.PlayStation.HighLevel.GameEngine2D.Scene
	{
		int[] layer = new int[4]{0,100,200,300};
		
		private SpriteUV backLayer, playerWins;
		private Button quitButton, restartButton;
		
		private bool singlePlayer;
		
		private Vector2 screenCenter;
		
		private int playerWon = 0;
		private double timeSinceStart = 0;
		private AudioManager audio;
		
		private string restartLevel;
			
		private float explosionDelay = 0.0f;
		private float delayAmount = 1.0f;
		
		public EndGameScene (int playerWon, bool singlePlayer, string restartLevel)
		{
			this.playerWon = playerWon;
			this.singlePlayer = singlePlayer;
			this.Camera.SetViewFromViewport();
			Scheduler.Instance.ScheduleUpdateForTarget (this, 0, false);
			
			//GC.GetTotalMemory(true);
			GC.Collect ();
			
			screenCenter = new Vector2(Director.Instance.GL.Context.GetViewport ().Width/2, 
		                               Director.Instance.GL.Context.GetViewport ().Height/2);
			
			if(audio == null)
				audio = new AudioManager();
			
			CreateMenuLayers ();
			CreateButtons ();
			
			this.restartLevel = restartLevel;
		}
		public override void Update(float dt)
		{
			backLayer.Angle -= 0.06f;
			if(playerWon != 0)
			{
				//Rotate Logo using sine wave
				timeSinceStart += dt;
				playerWins.Angle = FMath.Sin ((float)timeSinceStart*2)/4;
			}
			
			quitButton.Update ();
			restartButton.Update ();
			
			if(quitButton.GetState () == ButtonState.Released)
			{
				Director.Instance.ReplaceScene (new MenuScene());
			}
			
			if(restartButton.GetState () == ButtonState.Released)
			{
				if(restartLevel.Equals("GameScene"))
				{
					Director.Instance.ReplaceScene (new GameScene());	
				}
				else if(restartLevel.Equals("AI"))
				{
					Director.Instance.ReplaceScene (new AIGameScene());
				}
			}
			
			if(explosionDelay <= 0.0f)
			{
				float xpos = WMRandom.GetNextInt (0,960);
				float ypos = WMRandom.GetNextInt (0,544);
				explosionDelay = delayAmount;
				ParticleManager.AddExplosion (this, new Vector2(xpos, ypos), Colour.Yellow);
			}
			else
			{
				explosionDelay --;
			}
			
			ParticleManager.Update (this);
		}
		
		public override void Draw()
		{
			base.Draw ();
			UISystem.Render();
		}
		private void CreateButtons()
		{
			TextureInfo texInfo1 = new TextureInfo("Application/textures/ButtonQuit.png");
			TextureInfo texInfo2 = new TextureInfo("Application/textures/ButtonRestart.png");
			
			quitButton = new Button(this, 400, texInfo1,new Vector2(screenCenter.X-300.0f, screenCenter.Y),2);
			restartButton = new Button(this, 400, texInfo2,new Vector2(screenCenter.X+300.0f, screenCenter.Y),2);
		}
		private void CreateMenuLayers()
		{
			TextureInfo texInfo1;
			
			if(playerWon == 2)
			{
				texInfo1 = new TextureInfo("Application/textures/MenuSpiralAlt.png");
			}
			else
			{
				texInfo1 = new TextureInfo("Application/textures/MenuSpiralAlt4.png");
			}
			
			backLayer = new SpriteUV(texInfo1);
			backLayer.Scale = texInfo1.TextureSizef;
			backLayer.Pivot = new Vector2 (0.5f,0.5f);
			backLayer.Position = new Vector2(screenCenter.X, screenCenter.Y);
			
			this.AddChild (backLayer,layer[0]);
			
			if(playerWon != 0)
			{
				TextureInfo texInfo2;
				
				if(playerWon == 1)
				{
					texInfo2 = new TextureInfo("Application/textures/Player1Wins.png");
				}
				else
				{
					texInfo2 = new TextureInfo("Application/textures/Player2Wins.png");
				}
				
				playerWins = new SpriteUV(texInfo2);
				playerWins.Scale = texInfo2.TextureSizef*2;
				playerWins.Pivot = new Vector2 (0.5f,0.5f);
				playerWins.Position = new Vector2(screenCenter.X, screenCenter.Y);
				
				this.AddChild (playerWins, layer[1]);
			}

		}
	}
}