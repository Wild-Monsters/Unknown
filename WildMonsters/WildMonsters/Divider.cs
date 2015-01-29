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
	public class Divider
	{
		private SpriteUV sprite;
		private float top;
		
		public Divider (Scene _scene)
		{
			TextureInfo texInfo = new TextureInfo("/Application/textures/Divider.png");
			
			sprite = new SpriteUV(texInfo);
			sprite.Quad.S = texInfo.TextureSizef;
			sprite.Position = new Vector2(0.0f, 0.0f);
			
			_scene.AddChild (sprite);
		}
		
		public void Update(float t)
		{
			sprite.Position = new Vector2(top-(sprite.Quad.S.X/2), 0.0f);
		}
		
		public void SetTop(float _top)
		{
			top = _top;
		}
	}
}

