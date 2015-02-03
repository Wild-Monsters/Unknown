using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
//using Sce.PlayStation.HighLevel.UI;

namespace WildMonsters
{
	public class Ball
	{
		enum Colour {Red, Blue, Yellow, Purple, Green, Grey};
		private SpriteUV sprite;
		private TextureInfo texInfo;
		//private bool alive;
		private Colour colour;
		bool hasFired;
		private int speed = 1;

		private Bounds2 bounds;
		public Ball (Scene scene)
		{
			//alive = false;
			hasFired = false;
			
			Random rng = new Random(this.GetHashCode());
			colour = (Colour)(int)FMath.Floor(rng.Next(6));
			
			//colour = Colour.Red;
			
			//TODO: Assign tex info to texture image file
			texInfo = new TextureInfo("/Application/textures/Blocks.png");
			
			//this.TextureInfo = new TextureInfo("/Application/assets/testSpritesheet.png");
			sprite = new SpriteUV(texInfo);
			sprite.Quad.S = new Vector2(50.0f,50.0f);
			
			//this.UV.S = new Vector2(0.2f, 1.0f);
			sprite.Position = new Vector2(0.0f, 0.0f);
			//this.Position = new Vector2(0.0f, 0.0f);
			
			//TODO: Dynamically change part of the spritesheet to be shown, based on Colour var
			float spriteWidth = 1.0f / 6.0f;
			
			Sprite.UV.S = new Vector2(spriteWidth, 1.0f); //<--- uses 1/5 of the horizontal of the spritesheet
			
			//Which part of the spritesheet to draw based on the enum
			sprite.UV.T = new Vector2(spriteWidth * (int)colour, 0.0f);		
			
//			switch(colour)
//			{
//				case Colour.Red:
//					sprite.UV.T = new Vector2(0.0f, 0.0f);
//					//this.UV.T = new Vector2(0.0f, 0.0f);
//					break;
//				case Colour.Blue:
//					sprite.UV.T = new Vector2(spriteWidth * (int)colour, 0.0f);
//					//this.UV.T = new Vector2(spriteWidth, 0.0f);
//					break;
//				case Colour.Yellow:
//					sprite.UV.T = new Vector2(spriteWidth * 2, 0.0f);
//					//this.UV.T = new Vector2(spriteWidth * 2, 0.0f);
//					break;
//				case Colour.Purple:
//					sprite.UV.T = new Vector2(spriteWidth * 3, 0.0f);
//					//this.UV.T = new Vector2(spriteWidth * 3, 0.0f);
//					break;
//			    case Colour.Green:					
//					sprite.UV.T = new Vector2(spriteWidth * 4, 0.0f);
//					//this.UV.T = new Vector2(spriteWidth * 4, 0.0f);
//					break;
//				//default: 
//					//MessageDialog.CreateAndShow(MessageDialogStyle.Ok, "Fatal error", "Ball has no colour");
//					//break;
//			}			
			
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
		public void SetFired(bool peanut)
		{
			this.hasFired = peanut;
		}
		
		public bool GetFired()
		{
			return this.hasFired;
		}
	
	}
}

