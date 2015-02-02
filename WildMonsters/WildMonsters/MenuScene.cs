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
		
		private Sce.PlayStation.HighLevel.UI.Scene uiScene;
		
		
		public MenuScene ()
		{
			this.Camera.SetViewFromViewport ();
			Sce.PlayStation.HighLevel.UI.Panel dialog = new Panel();
			dialog.Width = Director.Instance.GL.Context.GetViewport ().Width;
			dialog.Height = Director.Instance.GL.Context.GetViewport().Height;
			
			ImageBox imBox = new ImageBox();
			imBox.Width = dialog.Width;
			imBox.Image = new ImageAsset("Application/Textures/PlaceholderMenuScreen.png", false);
			imBox.Height = dialog.Height; 
			imBox.SetPosition(0.0f, 0.0f);
			
			Button buttonUI1 = new Button();
			buttonUI1.Name = "GameModes";
			buttonUI1.Text = "Play";
			buttonUI1.Width = 300;
			buttonUI1.Height = 50;
			buttonUI1.Alpha = 0.8f;
			buttonUI1.SetPosition(dialog.Width/2 - 150, 200.0f);
			buttonUI1.TouchEventReceived += (sender, e) => {
				Director.Instance.ReplaceScene (new GameScene());
			};
			
			Button buttonUI2 = new Button();
			buttonUI2.Name = "Instructions";
			buttonUI2.Text = "How to play";
			buttonUI2.Width = 300;
			buttonUI2.Height = 50;
			buttonUI2.Alpha = 0.8f;
			buttonUI2.SetPosition(dialog.Width/2 - 150, 200.0f);
			buttonUI2.TouchEventReceived += (sender, e) => {
				Director.Instance.ReplaceScene (new InstructionScene());
			};
			
			dialog.AddChildLast (imBox);
			dialog.AddChildLast (buttonUI1);
			dialog.AddChildLast (buttonUI2);
			uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
			uiScene.RootWidget.AddChildLast(dialog);
			UISystem.SetScene (uiScene);
			Scheduler.Instance.ScheduleUpdateForTarget (this, 0, false);
			
			
			//Clears the touch data so the game does not instantly switch to the menu scene
			Touch.GetData (0).Clear ();
			
			
			//Scheduler.Instance.ScheduleUpdateForTarget(this, 1, false);	// Tells the director to call the update function of this "node"
		}
		
		public override void Update(float dt)
		{
			base.Update (dt);
			UISystem.Render();
			
			var touches = Touch.GetData (0).ToArray();
			
			if((touches.Length >0 && touches[0].Status == TouchStatus.Down) || Input2.GamePad0.Cross.Press)
			{
				Director.Instance.ReplaceScene (new GameScene());	
			}
			
			
		}
		
		public override void Draw()
		{
			base.Draw ();
			UISystem.Render();
		}
	}
}

