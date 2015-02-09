
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
		
		public static Vector2 CreateRandomPosition()
		{	
			Random rand = new Random();
			
		    double randomNumWidth = rand.NextDouble();
			randomNumWidth *= 50.0;
				
			double randomNumHeight = rand.NextDouble();
			randomNumHeight *= 50.0;
			
			Vector2 randPos = new Vector2((float)randomNumWidth, (float)randomNumHeight);
			
			return randPos;
		}		
		
		public static void AddParticle(Scene _scene, Vector2 position, int numParticle)
		{
			for(int i = 0; i < numParticle; i++)
			{
				// Add particle to the list
				objectList.Add (new Particle(_scene, position));
			}
		}
		
		public static void Update(Scene _scene)
		{
			for(int i = 0; i < objectList.Count; i++)
			{
				objectList[i].Update();
				
				if(objectList[i].TTL <= 0 || objectList[i].SpriteColor.A <= 0)
				{
					_scene.RemoveChild(objectList[i].Sprite, false);
					objectList.RemoveAt(i);
					i--;
				}
			}
			
		}
	}
}

