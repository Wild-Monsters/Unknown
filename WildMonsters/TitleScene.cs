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
	public class TitleScene : Scene
	{
		private TextureInfo 	tInfo;
		private Texture2D 		texture;
		
		public TitleScene ()
		{
			this.Camera.SetViewFromViewport();
			Scheduler.Instance.ScheduleUpdateForTarget(this, 0, false);
		
			this.Camera.SetViewFromViewport();
			texture = new Texture2D("Application/textures/TitleScreen.png", false);
			tInfo = new TextureInfo(texture);
			SpriteUV titleScreen = new SpriteUV(tInfo);
			titleScreen.Scale = tInfo.TextureSizef;
			titleScreen.Pivot = new Vector2 (0.5f, 0.5f);
			titleScreen.Position = new Vector2(Director.Instance.GL.Context.GetViewport ().Width/2,
			                                   Director.Instance.GL.Context.GetViewport ().Height/2);
			
			//Adds the title screen to the scene
			this.AddChild (titleScreen);
			
			
			//Clears the touch data so the game does not instantly switch to the menu scene
			Touch.GetData (0).Clear ();

		}
		
		public override void Update (float dt)
		{
			base.Update (dt);
			var touches = Touch.GetData (0).ToArray();
			
			if((touches.Length >0 && touches[0].Status == TouchStatus.Down) || Input2.GamePad0.Cross.Press)
			{
				Director.Instance.ReplaceScene (new MenuScene());	
			}
			
		}
		
		    
         ~TitleScene()
        {
            texture.Dispose();
            tInfo.Dispose ();
        }
	}
}

