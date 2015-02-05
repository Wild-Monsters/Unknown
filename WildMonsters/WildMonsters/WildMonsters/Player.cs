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
			textureInfo = new TextureInfo("/Application/textures/1.png");
			
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
			GamePadButtons actionButton, upButton, downButton;
			Analog moveAnalog;
			
			//Set the control buttons based on which side you're on
			if(isLeftSide)
			{
				actionButton = GamePadButtons.Right;
				moveAnalog = Analog.leftY;
				upButton = GamePadButtons.Up;
				downButton = GamePadButtons.Down;
			}
			else
			{
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

			//lock it to screen 
			ScreenCollision();
		}
		public void ScreenCollision ()
		{
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

				}
		
		}
		public List<Ball> getBalls()
		{
			return this.ballList;
		}
	}
}

