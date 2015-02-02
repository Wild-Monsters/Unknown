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
		
		public static void AddParticle(Scene _scene, Vector2 position)
		{
			objectList.Add (new Particle(_scene, position));
		}
		
		public static void Update()
		{
			foreach(Particle p in objectList)
			{
				p.Update();
			}
		}
	}
}

