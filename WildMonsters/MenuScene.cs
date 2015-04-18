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
	
	
	public class MenuScene : Sce.PlayStation.HighLevel.GameEngine2D.Scene
	{
		int[] layer = new int[4]{0,100,200,300};
		
		private SpriteUV backLayer, logoLayer;
		private Button spButton, vsButton;
		
		private Vector2 screenCenter;
		
		
		private double timeSinceStart = 0;
		private AudioManager audio;
			
		public MenuScene ()
		{
			
			this.Camera.SetViewFromViewport();
			Scheduler.Instance.ScheduleUpdateForTarget (this, 0, false);
			
			screenCenter = new Vector2(Director.Instance.GL.Context.GetViewport ().Width/2, 
		                               Director.Instance.GL.Context.GetViewport ().Height/2);
			
			if(audio == null)
				audio = new AudioManager();
			audio.PlayMenuMusic();
			CreateMenuLayers ();
			CreateButtons ();
		}
		
		public override void Update(float dt)
		{
			backLayer.Angle -= 0.06f;
			
			//Rotate Logo using sine wave
			timeSinceStart += dt;
			logoLayer.Angle = FMath.Sin ((float)timeSinceStart*2)/4;
			
			spButton.Update ();
			vsButton.Update ();
			
			if(spButton.GetState () == ButtonState.Released)
			{
				audio.StopMenuMusic();
				audio.Dispose();
				Director.Instance.ReplaceScene (new AIGameScene());
			}
			
			if(vsButton.GetState () == ButtonState.Released)
			{
				
				audio.StopMenuMusic();
				audio.Dispose();
				Director.Instance.ReplaceScene (new GameScene());
			}
		}
		
		public override void Draw()
		{
			base.Draw ();
			UISystem.Render();
		}
		
		
		
		private void CreateButtons()
		{
			TextureInfo texInfo1 = new TextureInfo("Application/textures/ButtonSinglePlayer.png");
			TextureInfo texInfo2 = new TextureInfo("Application/textures/ButtonVersus.png");
			
			spButton = new Button(this, 400, texInfo1,new Vector2(screenCenter.X-300.0f, screenCenter.Y),2);
			vsButton = new Button(this, 400, texInfo2,new Vector2(screenCenter.X+300.0f, screenCenter.Y),2);
		}
		
		
		
		private void CreateMenuLayers()
		{
			TextureInfo texInfo1 = new TextureInfo("Application/textures/MenuSpiralAlt4.png");
			TextureInfo texInfo2 = new TextureInfo("Application/textures/MenuLogo.png");
			
			backLayer = new SpriteUV(texInfo1);
			backLayer.Scale = texInfo1.TextureSizef;
			backLayer.Pivot = new Vector2 (0.5f,0.5f);
			backLayer.Position = new Vector2(screenCenter.X, screenCenter.Y);
			
			logoLayer = new SpriteUV(texInfo2);
			logoLayer.Scale = texInfo2.TextureSizef/1.5f;
			logoLayer.Pivot = new Vector2 (0.5f, 0.5f);
			logoLayer.Position = new Vector2(screenCenter.X, screenCenter.Y);
			
			logoLayer.Angle = 45 * FMath.DegToRad;
			
			this.AddChild (backLayer,layer[0]);
			this.AddChild (logoLayer,layer[2]);
		}
		
		
		
//		
//		private void UIStuff()
//		{
//			audio.PlayMenuMusic();
//			Sce.PlayStation.HighLevel.UI.Panel dialog = new Panel();
//			dialog.Width = Director.Instance.GL.Context.GetViewport ().Width;
//			dialog.Height = Director.Instance.GL.Context.GetViewport().Height;
//			
//			ImageBox imBox = new ImageBox();
//			imBox.Width = dialog.Width;
//			imBox.Image = new ImageAsset("Application/Textures/PlaceholderMenuScreen.png", false);
//			imBox.Height = dialog.Height; 
//			imBox.SetPosition(0.0f, 0.0f);
//			
//			Button buttonUI1 = new Button();
//			buttonUI1.Name = "GameModes";
//			buttonUI1.Text = "Play";
//			buttonUI1.Width = 300;
//			buttonUI1.Height = 50;
//			buttonUI1.Alpha = 0.8f;
//			buttonUI1.SetPosition(dialog.Width/2 - 150, 400.0f);
//			buttonUI1.TouchEventReceived += (sender, e) => {
//				
//				audio.StopMenuMusic();
//				audio.Dispose();
//				
//				Director.Instance.ReplaceScene (new GameScene());
//			};
//			
//			Button buttonUI2 = new Button();
//			buttonUI2.Name = "Instructions";
//			buttonUI2.Text = "How to play";
//			buttonUI2.Width = 300;
//			buttonUI2.Height = 50;
//			buttonUI2.Alpha = 0.8f;
//			buttonUI2.SetPosition(dialog.Width/2 - 150, 200.0f);
//			buttonUI2.TouchEventReceived += (sender, e) => {
//				Director.Instance.ReplaceScene (new InstructionScene());
//			};
//			
//			dialog.AddChildLast (imBox);
//			dialog.AddChildLast (buttonUI1);
//			dialog.AddChildLast (buttonUI2);
//			uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
//			uiScene.RootWidget.AddChildLast(dialog);
//			//UISystem.SetScene(uiScene);
//			
//			
//			
//			//Clears the touch data so the game does not instantly switch to the menu scene
//			Touch.GetData(0).Clear ();
//		}
	}
}

