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
<<<<<<< HEAD
			textureInfo = new TextureInfo("/Application/Textures/Player/1.png");
=======
			textureInfo = new TextureInfo("/Application/textures/1.png");
>>>>>>> 458b8efc3b413faa704e6f8b99085441ba4ccb0f
			
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
		public void Update()
		{
			GamePadButtons actionButton, upButton, downButton;
			Analog moveAnalog;
			
			//Set the control buttons based on which side you're on
			if(isLeftSide)
			{
<<<<<<< HEAD
				if (Input.KeyDown (GamePadButtons.Up)) //Go left
				{
					sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y + movementSpeed);
				}
				if(Input.KeyDown (GamePadButtons.Down)) //go right
				{
					sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y - movementSpeed);
				}
=======
				actionButton = GamePadButtons.Right;
				moveAnalog = Analog.leftY;
				upButton = GamePadButtons.Up;
				downButton = GamePadButtons.Down;
>>>>>>> 458b8efc3b413faa704e6f8b99085441ba4ccb0f
			}
			else
			{
<<<<<<< HEAD
				if (Input.KeyDown (GamePadButtons.Triangle)) //Go left
				{
					sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y + movementSpeed);
				}
				if(Input.KeyDown (GamePadButtons.Cross)) //go right
				{
					sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y - movementSpeed);
				}	
			}
			
			Console.WriteLine ("Key Number Is::: " + (int)GamePadButtons.Triangle);
			Console.WriteLine ("Key Number Is::: " + (int)GamePadButtons.Cross);
			Console.WriteLine ("Key Number Is::: " + (int)GamePadButtons.Up);
			Console.WriteLine ("Key Number Is::: " + (int)GamePadButtons.Down);
=======
				actionButton = GamePadButtons.Square;
				moveAnalog = Analog.rightY;
				upButton = GamePadButtons.Triangle;
				downButton = GamePadButtons.Cross;
			}
			
			//Use analog or the buttons to move the character
			if (Input.AnalogPress(moveAnalog, false, 0.5f, 12.0f) || Input.KeyPressed (upButton, 12.0f)) //Go left (up)
			{
				sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y + movementSpeed);
			}
			
			if (Input.AnalogPress(moveAnalog, true, 0.5f, 12.0f) || Input.KeyPressed (downButton, 12.0f)) //go right (down)
			{
				sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y - movementSpeed);
			}
			
			//Perform action
			if(Input.KeyPressed(actionButton))
			{
				Fire (scene);
			}
		
			UpdateBalls();
>>>>>>> 458b8efc3b413faa704e6f8b99085441ba4ccb0f

			//lock it to screen 
			ScreenCollision();
		}
		public void ScreenCollision ()
		{
<<<<<<< HEAD
			Console.WriteLine("sprite pos x: "+sprite.Position.X);
			Console.WriteLine("sprite pos y: "+sprite.Position.Y);
			
=======
>>>>>>> 458b8efc3b413faa704e6f8b99085441ba4ccb0f
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

