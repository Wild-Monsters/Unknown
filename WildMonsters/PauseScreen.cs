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
	
	
	public class PauseScreen : Sce.PlayStation.HighLevel.GameEngine2D.Scene
	{
		int[] layer = new int[4]{0,100,200,300};
		
		private SpriteUV backLayer;
		private Button quitButton, restartButton, continueButton;
		
		private Vector2 screenCenter;
		
		
		private double timeSinceStart = 0;
		private AudioManager audio;
		
		private string restartLevel;

		public PauseScreen (string restartLevel)//ref GameScene gamescene
		{
			this.Camera.SetViewFromViewport();
			Scheduler.Instance.ScheduleUpdateForTarget (this, 0, false);
			
			screenCenter = new Vector2(Director.Instance.GL.Context.GetViewport ().Width/2, 
		                               Director.Instance.GL.Context.GetViewport ().Height/2);
			
			GC.Collect ();
			
			if(audio == null)
				audio = new AudioManager();
			
			CreateMenuLayers ();
			CreateButtons ();
			
			this.restartLevel = restartLevel;
			
		}
		
		public override void Update(float dt)
		{
			backLayer.Angle -= 0.06f;
			
			//Rotate Logo using sine wave
			timeSinceStart += dt;

			
			quitButton.Update ();
			restartButton.Update ();
			//continueButton.Update ();
			
			if(quitButton.GetState () == ButtonState.Released)
			{
				audio.StopGameMusic();
				audio.Dispose();
				Director.Instance.ReplaceScene (new MenuScene());
			}
			
			if(restartButton.GetState () == ButtonState.Released)
			{
				audio.StopGameMusic();
				audio.Dispose();
				if(restartLevel.Equals("AI"))
				{
					Director.Instance.ReplaceScene (new AIGameScene());
				}
				else if(restartLevel.Equals("GameScene"))
				{
					Director.Instance.ReplaceScene (new GameScene());
				}
			}
			
			
//			if(continueButton.GetState () == ButtonState.Released)
//			{
//			//	gamescene.StartMusic ();
//			//	Scheduler.Instance.ScheduleUpdateForTarget(gamescene, 1, false);
//				Director.Instance.PopScene();
//			}
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
			//TextureInfo texInfo3 = new TextureInfo("Application/textures/ButtonContinue.png");
			
			quitButton = new Button(this, 400, texInfo1,new Vector2(screenCenter.X-300.0f, screenCenter.Y),2);
			restartButton = new Button(this, 400, texInfo2,new Vector2(screenCenter.X+300.0f, screenCenter.Y),2);
			//continueButton = new Button(this, 400, texInfo3, new Vector2(screenCenter.X, screenCenter.Y),2);
		}
		
		
		private void CreateMenuLayers()
		{
			TextureInfo texInfo1 = new TextureInfo("Application/textures/MenuSpiralAlt4.png");
			
			backLayer = new SpriteUV(texInfo1);
			backLayer.Scale = texInfo1.TextureSizef;
			backLayer.Pivot = new Vector2 (0.5f,0.5f);
			backLayer.Position = new Vector2(screenCenter.X, screenCenter.Y);
			
			this.AddChild (backLayer,layer[0]);
		}
	}
}