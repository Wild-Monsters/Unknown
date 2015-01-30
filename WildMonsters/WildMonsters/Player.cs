using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;

namespace WildMonsters
{
	public class Player
	{
		private SpriteUV sprite;
		private TextureInfo textureInfo;
		private int movementSpeed = 5;
		private bool isLeftSide = false;	//deciding which side the player is on
		private int spriteWidth = 0;
		private int spriteHeight = 0;
		
		public Player (Scene scene, bool isLeftSides)
		{
			isLeftSide = isLeftSides;
			textureInfo = new TextureInfo("/Application/Textures/Player/1.png");
			
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
			scene.AddChild(sprite);
			
		}
		public void Update(GamePadData gamepad)
		{
			if(isLeftSide)
			{
				if ((gamepad.Buttons & GamePadButtons.Up ) != 0) //Go left
				{
					sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y + movementSpeed);
				}
				if((gamepad.Buttons & GamePadButtons.Down) != 0) //go right
				{
					sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y - movementSpeed);
				}
			}
			if(!isLeftSide) 
			{
				if ((gamepad.Buttons & GamePadButtons.Triangle ) != 0) //Go left
				{
					sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y + movementSpeed);
				}
				if((gamepad.Buttons & GamePadButtons.Cross) != 0) //go right
				{
					sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y - movementSpeed);
				}	
			}
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
		public void Fire()// array of balls 
		{
			
		}
	}
}

