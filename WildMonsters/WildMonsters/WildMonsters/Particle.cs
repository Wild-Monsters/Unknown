using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace WildMonsters
{
	public class Particle
	{
		// Private variables:
		//Vector2 _myPos;
		Vector2 _speed;
		SpriteUV sprite;
		
		// Accessors:
		
		public Particle (Scene _scene, Vector2 position)
		{
			TextureInfo texInfo = new TextureInfo("/Application/textures/Particle.png");
			
			sprite = new SpriteUV(texInfo);
			sprite.Quad.S = texInfo.TextureSizef;
			sprite.Position = position;
			
			_scene.AddChild (sprite);
			
			// Initialise variables:
			//_myPos = new Vector2(0.0f, 0.0f);
			_speed = new Vector2(10.0f, 0.0f);
		}
		
		public Vector2 getMyPos()
		{
			return sprite.Position;
		}
		
		public void Update()
		{
			//_myPos.Add (_speed);
			//sprite.Position.Add ();
			sprite.Position = Vector2.Add (sprite.Position, _speed);
		}
	}
}

