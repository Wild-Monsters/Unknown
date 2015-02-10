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
		private TextureInfo texInfo;
		private int movementSpeed = 50;
		private bool isLeftSide = false;	//deciding which side the player is on
		private int spriteWidth = 0;
		private int spriteHeight = 0;
		
		//stuff zac added
		private Colour nextColour;
		private Random rng;
									
		//Taking the bull by the balls
		private List <Ball> ballList;

		public Player (Scene scene, bool _isLeftSide)
		{
			isLeftSide = _isLeftSide;
			
			texInfo = new TextureInfo("/Application/textures/Blocks.png");
			sprite = new SpriteUV(texInfo);
			sprite.Quad.S = new Vector2(50.0f,50.0f);
			sprite.Position = new Vector2(0.0f, 0.0f);
			
			//Change colour of block
			NextColour ();
			
//			Vector2  peanut = textureInfo.TextureSizef;
//			sprite = new SpriteUV(textureInfo);
//			sprite.Quad.S  = textureInfo.TextureSizef;
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
			
			float analogDelay = 8.0f;
			float buttonDelay = 8.0f;
			float deadzone = 0.5f;
			
			//Use analog or the buttons to move the character
			if (Input.AnalogPress(moveAnalog, false, deadzone, analogDelay) || Input.KeyPressed (upButton, buttonDelay)) //Go left (up)
			{
				sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y + movementSpeed);
			}
			
			if (Input.AnalogPress(moveAnalog, true, deadzone, analogDelay) || Input.KeyPressed (downButton, buttonDelay)) //go right (down)
			{
				sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y - movementSpeed);
			}
			
			//Perform action
			if(Input.KeyPressed(actionButton))
			{
				Fire (scene);
			}
			for(int i  = 0; i < ballList.Count; i++)
			{
				//ballList[i].Update();
				//ballList[i].Particles.List.Clear();
			}
			UpdateBalls();

			//lock it to screen 
			ScreenCollision();
		}
		public void ScreenCollision ()
		{
			if((sprite.Position.Y + spriteHeight )>= 544 )
			{
				sprite.Position = new Vector2(sprite.Position.X, 544 - spriteHeight - 44);
			}
			if((sprite.Position.Y ) < 0 )
			{
				sprite.Position = new Vector2(sprite.Position.X, 0);
			}
		}
		
		public void Fire(Scene scene)// array of balls 
		{
			Ball ball = new Ball(scene);
			ball.SetState(BallState.Rising);
			ball.Sprite.Position = this.sprite.Position;
			
			CollisionHandler.Colourblock = nextColour;
			ball.SetColour(nextColour);
			NextColour ();
			
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
					if (ballList[i].GetState() == BallState.Rising)
					{
						CollisionHandler.BMoving = true;
					
						CollisionHandler.BLeft = getSide();
					
						ballList[i].Update(getSide());
					
						CollisionHandler.ExplodeAtMovingArray[i].X = ballList[i].GetBounds().Min.X;
						CollisionHandler.ExplodeAtMovingArray[i].Y = ballList[i].GetBounds().Min.Y;
					}
					else
					{
						ballList.Remove(ballList[i]);
						CollisionHandler.BMoving = false;
					}
				}
		}
		public List<Ball> getBalls()
		{
			return this.ballList;
		}
		
		private void NextColour()
		{
			nextColour = (Colour)WMRandom.GetNextInt(0,5,this.GetHashCode());
			
			float spriteWidth = 1.0f / 6.0f;
			sprite.UV.S = new Vector2(spriteWidth, 1.0f);
			sprite.UV.T = new Vector2(spriteWidth * (int)nextColour, 0.0f);	
		}
	}
}

