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
		private static List<int> indexesToDelete = new List<int>();
		
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
			//List<int> indexesToDelete = new List<int>();
			for(int i = 0; i < objectList.Count; i++)
			{
				objectList[i].Update(_scene);
				
				if(objectList[i].TTL <= 0)
				{
					_scene.RemoveChild(objectList[i].Sprite, false);
					objectList.RemoveAt(i);
					i--;
				}
			}
			
//			//If indexesToDelete is not empty, loop through
//			if(indexesToDelete.Count > 0)
//			{
//				// For each element in the array...
//				for(int i = 0; i < indexesToDelete.Count; i++)
//				{
//					// Delete the particle at that position
//					objectList.RemoveAt(indexesToDelete[i]);
//				}
//			}
//			// Clear the list, otherwise we'll try to delete the same element in the next pass.
//			indexesToDelete.Clear();
			
		}
	}
}

