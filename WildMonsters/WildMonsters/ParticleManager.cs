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
	public static class ParticleManager
	{
		
		private static List<Particle> objectList = new List<Particle>();
		private static TextureInfo texInfo = new TextureInfo("/Application/textures/star.png");
		private static Sce.PlayStation.HighLevel.GameEngine2D.SpriteList _spriteList;
		private static System.Collections.Generic.List<SpriteUV> _sprites;
		
		//public static void AddParticle(Scene _scene, Vector2 position, int numParticle)
		public static void AddParticle(Scene _scene, Vector2 position, int numParticle)
		{
			_spriteList = new Sce.PlayStation.HighLevel.GameEngine2D.SpriteList(texInfo);
			_sprites = new System.Collections.Generic.List<SpriteUV>();
		
			for(int i = 0; i < numParticle; i++)
			{
				// Add particle to the list
				//objectList.Add (new Particle(_scene, position));
				objectList.Add (new Particle(position));
				_sprites.Add(objectList[i].Sprite);
			}
			
			foreach(var sprite in _sprites)
   			{
    			_spriteList.AddChild(sprite);
   			}
			
			_scene.AddChild(_spriteList);
		}
		
		//public static void Update(Scene _scene)
		public static void Update(Scene _scene)
		{
			for(int i = 0; i < objectList.Count; i++)
			{
				//objectList[i].Update(_scene);
				objectList[i].Update();
				
				if(objectList[i].TTL <= 0)
				{
					_spriteList.RemoveChild(_sprites[i], true);
					_sprites.Remove(objectList[i].Sprite);
					
					objectList.RemoveAt(i);
					i--;
				}
			}
		}
	}
}

