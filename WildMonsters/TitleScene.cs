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
		
		int[] layer = new int[4]{0,100,200,300};
		private SpriteUV backLayer;
		private SpriteUV frontLayer;
		private SpriteUV logoLayer;
		
		private List<SpriteUV> blockList;
		private const int blockAmount = 20;
		
		public TitleScene ()
		{
			this.Camera.SetViewFromViewport();
			Scheduler.Instance.ScheduleUpdateForTarget(this, 0, false);
		
			SetUpMenuLayers();
		
			//Add everything to the scene
			this.AddChild (backLayer,layer[0]);
			CreateBlocks(blockAmount,layer[1]);
			this.AddChild (logoLayer,layer[2]);
			this.AddChild (frontLayer,layer[3]);
			
			//Clears the touch data so the game does not instantly switch to the menu scene
			Touch.GetData (0).Clear ();
		}
		
		public override void Update (float dt)
		{
			backLayer.Angle += 0.06f;
			
			base.Update (dt);
			var touches = Touch.GetData (0).ToArray();
			
			if((touches.Length >0 && touches[0].Status == TouchStatus.Down) || Input2.GamePad0.Cross.Press)
			{
				Director.Instance.ReplaceScene (new MenuScene());	
			}
			
			UpdateBlocks(dt);
		}
		
		private void SetUpMenuLayers()
		{
			TextureInfo texInfo1 = new TextureInfo("Application/textures/MenuSpiral.png");
			TextureInfo texInfo2 = new TextureInfo("Application/textures/MenuLogo.png");
			TextureInfo texInfo3 = new TextureInfo("Application/textures/MenuFrontLayer.png");
			
			backLayer = new SpriteUV(texInfo1);
			backLayer.Scale = texInfo1.TextureSizef;
			backLayer.Pivot = new Vector2 (0.5f, 0.5f);
			backLayer.Position = new Vector2(Director.Instance.GL.Context.GetViewport ().Width/2,
			                                 Director.Instance.GL.Context.GetViewport ().Height/2);
			
			logoLayer = new SpriteUV(texInfo2);
			logoLayer.Scale = texInfo2.TextureSizef;
			logoLayer.Pivot = new Vector2 (0.5f, 0.5f);
			logoLayer.Position = new Vector2(Director.Instance.GL.Context.GetViewport ().Width/2,
			                                 Director.Instance.GL.Context.GetViewport ().Height/2);
			
			frontLayer = new SpriteUV(texInfo3);
			frontLayer.Scale = texInfo3.TextureSizef;
			frontLayer.Pivot = new Vector2 (0.5f, 0.5f);
			frontLayer.Position = new Vector2(Director.Instance.GL.Context.GetViewport ().Width/2,
			                                 Director.Instance.GL.Context.GetViewport ().Height/2);
		}
		 
		
		private void CreateBlocks(float amount, int layer)
		{
			blockList = new List<SpriteUV>();
			TextureInfo texInfo = new TextureInfo("Application/textures/Blocks4.png");
			
			for(int a = 0; a < amount; a++)
			{
				SpriteUV sprite = new SpriteUV(texInfo);
				sprite.Quad.S = new Vector2(50.0f,50.0f);
				
				float spriteWidth = 1.0f / 6.0f;
				sprite.UV.S = new Vector2(spriteWidth, 1.0f);
				sprite.UV.T = new Vector2(spriteWidth * WMRandom.GetNextInt(0,6), 0.0f);	
				sprite.Pivot = new Vector2(25.0f,25.0f);
				float scale = ((float)WMRandom.GetNextInt (5,20))/10.0f;
				sprite.Scale = new Vector2(scale,scale);
				
				sprite.Position = new Vector2(WMRandom.GetNextInt (0, (int)Constants.ScreenWidth), 
				                              WMRandom.GetNextInt (0, (int)Constants.ScreenHeight));
				
				this.AddChild (sprite, layer + (int)(scale*10));
				blockList.Add (sprite);
			}
		}
		
		private void UpdateBlocks(float dt)
		{
			for(int a = 0; a < blockList.Count; a++)
			{
				blockList[a].Angle+=0.01f;
				blockList[a].Position = new Vector2(blockList[a].Position.X + (blockList[a].Scale.X), blockList[a].Position.Y);
				
				if(blockList[a].Position.X > Constants.ScreenWidth+50.0f)
				{
					blockList[a].Position = new Vector2(-100.0f, blockList[a].Position.Y);
				}
			}
		}
		
		
		
		
         ~TitleScene()
        {
            //texture.Dispose();
            //tInfo.Dispose ();
        }
	}
}

