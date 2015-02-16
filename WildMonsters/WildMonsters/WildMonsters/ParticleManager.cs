
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
		private static List<Particle> leftList = new List<Particle>();
		private static List<Particle> rightList = new List<Particle>();
		// 2 lists left and right
		// if player1 that has fired or player2 has fired
		
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
		
		public static void AddParticle(Scene _scene, Vector2 position, int numParticle, int type, int quadAssign, Colour colour) // playerid var, if 1 it's left and if 2 it's right
		{
			for(int i = 0; i < numParticle; i++)
			{
				// Add particle to the list
				objectList.Add (new Particle(_scene, position, type, quadAssign, colour));
			
				
//				switch(side)
//				{
//					// add particle to left
//					case Side.Left:
//						leftList.Add (new Particle(_scene, position, type, quadAssign, colour));
//						break;
//					// add particle to right
//					case Side.Right:
//						break;
//					default:
//						break;
//				}
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

//			for(int i = 0; i < leftList.Count; i++)
//			{
//				// change to left and right object list
//				//objectList[i].Update();
//				
//				// update left object list and right object list
//				// if one list is shorter than the other
//					leftList[i].Update();
//					
//					if(leftList[i].TTL <= 0 || leftList[i].SpriteColor.A <= 0)
//					{
//						_scene.RemoveChild(leftList[i].Sprite, false);
//						leftList.RemoveAt(i);	
//						i--;
//					}
//			}
//			
//			for(int i = 0; i < rightList.Count; i++)
//			{
//					rightList[i].Update();
//					
//					if(rightList[i].TTL <= 0 || rightList[i].SpriteColor.A <= 0)
//					{
//						_scene.RemoveChild(rightList[i].Sprite, false);
//						rightList.RemoveAt(i);	
//						i--;
//					}
//			}
		}
	}
}

