using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;
using System.Collections.Generic;

namespace WildMonsters
{
	public class Player
	{
		private SpriteUV sprite;
		private TextureInfo textureInfo;
		private int movementSpeed = 50;
		private bool isLeftSide = false;	//deciding which side the player is on
		private int spriteWidth = 0;
		private int spriteHeight = 0;
		
									
		//Taking the bull by the balls
		private List <Ball> ballList;

		public Player (Scene scene, bool isLeftSides)
		{
			isLeftSide = isLeftSides;
			textureInfo = new TextureInfo("/Application/Textures/1.png");
			
			Vector2  peanut = textureInfo.TextureSizef;
			sprite = new SpriteUV(textureInfo);
			sprite.Quad.S  = textureInfo.TextureSizef;
			spriteWidth = sprite.TextureInfo.Texture.Width;
			spriteHeight = sprite.TextureInfo.Texture.Height;
			
			if(isLeftSide)
			{
				
				sprite.Position = new Vector2(0, 250);
			}
			else
			{
				sprite.Position = new Vector2(900, 250);
			}
			
			ballList = new List<Ball>();
			
			
			scene.AddChild(sprite);
			


			
		}
		public void Update(Scene scene)
		{
			if(isLeftSide)
			{
				if (Input.KeyPressed (GamePadButtons.Up)) //Go left
				{
					sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y + movementSpeed);
				}
				if(Input.KeyPressed (GamePadButtons.Down)) //go right
				{
					sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y - movementSpeed);
				}
			}
			if(!isLeftSide) 
			{
				if (Input.KeyPressed (GamePadButtons.Triangle)) //Go left
				{
					sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y + movementSpeed);
				}
				if(Input.KeyPressed (GamePadButtons.Cross)) //go right
				{
					sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y - movementSpeed);
				}	
			}
			if(Input.KeyPressed (GamePadButtons.Square))
			{
				Fire (scene);
			}
//			if (ball != null)
//			{
//				if(ball.GetFired())
//				{
//					ball.Update(getSide());
//				}
//			}
			UpdateBalls();
			Console.WriteLine ("Key Number Is::: " + (int)GamePadButtons.Triangle);
			Console.WriteLine ("Key Number Is::: " + (int)GamePadButtons.Cross);
			Console.WriteLine ("Key Number Is::: " + (int)GamePadButtons.Up);
			Console.WriteLine ("Key Number Is::: " + (int)GamePadButtons.Down);

			//lock it to screen 
			ScreenCollision();
		}
		public void ScreenCollision ()
		{
			Console.WriteLine("sprite pos x: "+sprite.Position.X);
			Console.WriteLine("sprite pos y: "+sprite.Position.Y);
			
			if((sprite.Position.Y + spriteHeight )>= 544 )
			{
				sprite.Position = new Vector2(sprite.Position.X, 544 - spriteHeight);
			}
			if((sprite.Position.Y ) < 0 )
			{
				sprite.Position = new Vector2(sprite.Position.X, 0);
			}
	
		}
		
		public void Fire(Scene scene)// array of balls 
		{
			Ball ball = new Ball(scene);
			ball.SetFired(true);
			ball.Sprite.Position = this.sprite.Position;
			ballList.Add(ball);
		}
		public bool getSide()
		{
			return this.isLeftSide;
		}
		private void UpdateBalls()
		{
			for(int i=0; i<ballList.Count; i++)
			{
				if (ballList[i].GetFired())
				{
					ballList[i].Update(getSide());
				}
			   // ballList[i].CheckCollision(AsteroidManager.getAsteroidArray(), this);	
			   //delete asset
			}
		}
			
	}
}

