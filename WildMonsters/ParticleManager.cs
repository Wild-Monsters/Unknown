
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
	public enum Side {Left, Right};
	
	public static class ParticleManager
	{
		private static List<Particle> objectList = new List<Particle>();	// Change back tp private or getters/setters
		
		public static Vector2 CreateRandomPosition()
		{	
			Random rand = new Random();
			
		    double randomNumWidth = rand.NextDouble();
			randomNumWidth *= 40.0;
			
			double randomNumHeight = rand.NextDouble();
			randomNumHeight *= 40.0;
			
			Vector2 randPos = new Vector2((float)randomNumWidth, (float)randomNumHeight);
			
			return randPos;
		}		
		
		public static void AddExplosion(Scene _scene, Vector2 position, Colour colour)
		{
			int quadAssign = 0;
			// Produce 10 particles per call
			for(int i = 0; i < 2; i++)
			{
				quadAssign++;
				if(quadAssign >= 4)
				{
					quadAssign = 0;
				}
				Vector2 randPos = new Vector2(CreateRandomPosition().X, CreateRandomPosition().Y);
				AddParticle (_scene, new Vector2(position.X + (float)randPos.X,
					position.Y + (float)randPos.Y), 1, 0, quadAssign, colour);
			}
		}
		
		public static void AddLeftTrail(Scene _scene, Vector2 position, Colour colour)
		{
//			Vector2 randPos = new Vector2(CreateRandomPosition().X, CreateRandomPosition().Y);
//			AddParticle(_scene, new Vector2(position.X, (position.Y + 7.0f) + (float)randPos.Y), 1, 2, 0, colour);
		}
		
		public static void AddRightTrail(Scene _scene, Vector2 position, Colour colour)
		{
//			Vector2 randPos = new Vector2(CreateRandomPosition().X, CreateRandomPosition().Y);
//			AddParticle(_scene, new Vector2(position.X + 40.0f, (position.Y + 7.0f) + (float)randPos.Y), 1, 2, 0, colour);
		}
		
		public static void AddClickTrail(Scene _scene)
		{
//			List<TouchData> touches = Touch.GetData(0);
//			
//			foreach(TouchData data in touches)
//			{
//				Vector2 randPos = new Vector2(CreateRandomPosition().X, CreateRandomPosition().Y);
//				AddParticle (_scene, new Vector2((((data.X + 0.5f) * Constants.ScreenWidth) + (float)randPos.X),
//						Constants.ScreenHeight - ((data.Y + 0.5f) * Constants.ScreenHeight) + (float)randPos.Y), 1, 1, 0, Colour.Yellow);
//			}
		}
		
		public static void AddParticle(Scene _scene, Vector2 position, int numParticle, int type, int quadAssign, Colour colour) // playerid var, if 1 it's left and if 2 it's right
		{
			for(int i = 0; i < numParticle; i++)
			{
				// Add particle to the list
				objectList.Add (new Particle(_scene, position, type, quadAssign, colour));
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

