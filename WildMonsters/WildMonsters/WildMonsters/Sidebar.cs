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
	public class Sidebar
	{
		private SpriteUV sprite;
		
		public Sidebar (Scene _scene)
		{
			TextureInfo texInfo = new TextureInfo("/Application/textures/SideBar.png");
			
			sprite = new SpriteUV(texInfo);
			sprite.Quad.S = texInfo.TextureSizef;
			sprite.Position = new Vector2(0.0f, 544.0f-sprite.Quad.S.Y);
			
			_scene.AddChild (sprite);
		}
		
		public void SetPosition(float x, float y)
		{
			sprite.Position = new Vector2(x,y);
		}
	}
}

