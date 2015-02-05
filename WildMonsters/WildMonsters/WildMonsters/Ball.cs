using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
//using Sce.PlayStation.HighLevel.UI;

namespace WildMonsters
{
	public enum Colour {Red, Blue, Yellow, Purple, Green, Grey};
	
	public class Ball
	{
		private Scene parentScene;
		
		private SpriteUV sprite;
		private TextureInfo texInfo;
		//private bool alive;
		private Colour colour;
		bool hasFired;
		private int speed = 8;

		private Bounds2 bounds;
		public Ball (Scene scene)
		{
			parentScene = scene;
			
			hasFired = false;
			
			texInfo = new TextureInfo("/Application/textures/Blocks.png");
			sprite = new SpriteUV(texInfo);
			sprite.Quad.S = new Vector2(50.0f,50.0f);
			sprite.Position = new Vector2(0.0f, 0.0f);
			
			SetColour (Colour.Grey);
			
			//TODO: Initialise ball position to be at the position of the cannon
			scene.AddChild(sprite);
		}
		public Bounds2 GetBounds()
		{
			sprite.GetContentWorldBounds(ref bounds);
			
			return this.bounds;
		}
		public SpriteUV Sprite
		{
			get{ return sprite; }
			set{ sprite = value; }
		}
		
		
		public void Update(bool playerLeftOfScreen)
		{
			//TODO: Logic to travel up to and/or lock on to grid herel
			if(this.hasFired)
			{
				if(playerLeftOfScreen)
				{
					sprite.Position = new Vector2(sprite.Position.X + speed, sprite.Position.Y);
				}
				else
				{
					sprite.Position = new Vector2(sprite.Position.X - speed, sprite.Position.Y);
				}
			}
			
		}
		
		public void Cleanup()
		{
			texInfo.Dispose();
			sprite = null;
			//alive = false;
		}
		
		public void RemoveObject()
		{
			parentScene.RemoveChild (this.Sprite, true);
		}
		
		
		//Getters and Setters
		public void SetFired(bool peanut)
		{
			this.hasFired = peanut;
		}
		public bool GetFired()
		{
			return this.hasFired;
		}
	
		public Colour GetColour()
		{
			return colour;
		}
		
		public void SetColour(Colour col)
		{
			colour = col;
			
			float spriteWidth = 1.0f / 6.0f;
			sprite.UV.S = new Vector2(spriteWidth, 1.0f);
			sprite.UV.T = new Vector2(spriteWidth * (int)colour, 0.0f);	
		}
		
		public void RandomiseColour()
		{
			Random rng = new Random(this.GetHashCode());
			Colour nextColour = (Colour)(int)FMath.Floor(rng.Next(6));
			
			SetColour (nextColour);
		}
		
		
	}
}
