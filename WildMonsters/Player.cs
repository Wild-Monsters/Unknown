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
		private int spriteHeight = 0;
		
		private Colour[] colourArray;
		
		private List <Ball> ballList;
		
		private NextBallDisplay[] nextBallArray;

		private float p1MaxWaitTime;
		
		public Player (Scene scene, bool _isLeftSide)
		{			
			nextBallArray = new NextBallDisplay[3];
			colourArray = new Colour[5];
			
			isLeftSide = _isLeftSide;
						
			texInfo = new TextureInfo("/Application/textures/Blocks2.png");
			sprite = new SpriteUV(texInfo);
			sprite.Quad.S = new Vector2(50.0f,50.0f);
			sprite.Position = new Vector2(0.0f, 0.0f);
		
			for(int i = 0; i < 5; i++)
			{
				colourArray[i] = (Colour)WMRandom.GetNextInt(0, 5, this.GetHashCode());
			}
			
			//sets up the player sprite cannon/ball
			float spriteWidth = 1.0f / 6.0f;
			sprite.UV.S = new Vector2(spriteWidth, 1.0f);
			sprite.UV.T = new Vector2(spriteWidth * (int)colourArray[0], 0.0f);	
			
			if(isLeftSide)
			{
				sprite.Position = new Vector2(0, 250);
			}
			else
			{
				sprite.Position = new Vector2(900, 250);
			}
			
			ballList = new List<Ball>();
			
			for(int i = 0; i < 3; i++)
			{
				nextBallArray[i] = new NextBallDisplay(scene, isLeftSide, i + 1);
				nextBallArray[i].SetColour(colourArray[i + 1]);
			}
			//clear any previous touches, start fresh.
			Touch.GetData(0).Clear();
			
			scene.AddChild(sprite);
		}
		public void Update(Scene scene)
		{

			GamePadButtons actionButton, upButton, downButton;
			Analog moveAnalog;
			
			var touches = Touch.GetData(0);
			
			//Set the control buttons based on which side you're on
			if(isLeftSide)
			{
				//Player one touch controls
				
				//check there are touches first
				if(touches.Count > 0)
				{
					if(touches[0].Status == TouchStatus.Down)
					{
					}
					//top left quadrant of screen has been pressed
					if(touches[0].X < 0 && touches[0].Y <= 0)
					{
						//touches.at(0).X
						//Lazy bug fix for cannons moving on to sideba	
						sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y + movementSpeed);
						if(sprite.Position.Y >= 450)
						{
							sprite.Position = new Vector2(sprite.Position.X, 450);
						}
					}
					//Bottom left quadrant of screen has been pressed
					else if(touches[0].X < 0 && touches[0].Y >= 0)
					{
						sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y - movementSpeed);
					}		
				}
				
				actionButton = GamePadButtons.Right;
				moveAnalog = Analog.leftY;
				upButton = GamePadButtons.Up;
				downButton = GamePadButtons.Down;
			}
			else
			{
				//Player two touch controls

				//check there are touches first
				if(touches.Count > 0)
				{
					//Console.WriteLine("Player two: " + touches[0].Status);
					if(touches[0].Status == TouchStatus.Up)
					{
					}
					
					//top right quadrant of screen has been pressed
					if(touches[0].X > 0 && touches[0].Y <= 0)
					{
						//touches.at(0).X
						sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y + movementSpeed);
						if(sprite.Position.Y >= 450)
						{
							sprite.Position = new Vector2(sprite.Position.X, 450);
						}
					}
					
					//bottom right quadrant of screen has been pressed
					else if(touches[0].X > 0 && touches[0].Y >= 0)
					{				
						sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y - movementSpeed);
					}
				}
				
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
				//Lazy bug fix for cannons moving on to sidebar
				if(sprite.Position.Y >= 450)
				{
					sprite.Position = new Vector2(sprite.Position.X, 450);
				}
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
			//the firing ball moving across the screen
			Ball ball = new Ball(scene, isLeftSide);
			ball.SetState(BallState.Rising);
			//initialise at "cannon" pos
			ball.Sprite.Position = this.sprite.Position;
			//next colour being a random colour 
			ball.SetColour(colourArray[0]);
			
			//sets the sprite, which is the ball stationed in the cannon, to be 1/6th of the whole spritesheet
			float spriteWidth = 1.0f / 6.0f;
			sprite.UV.S = new Vector2(spriteWidth, 1.0f);
			//set the ball stationed in the cannon to be the colour of 'nextcolour2' which is the second different random colour val
			sprite.UV.T = new Vector2(spriteWidth * (int)colourArray[1], 0.0f);	
			
			displayNextBall(scene);
			
			ballList.Add(ball);
						
//			colourList.RemoveAt(0);
		//	colourList.RemoveAt(1);
		}
		
		public void displayNextBall(Scene scene)
		{	
			for(int i = 0; i < 3; i++)
			{
				nextBallArray[i].SetColour(colourArray[i + 2]);
			}
			
			//shift the random colours across one, and the fifth final one generates a new random coloru
			for(int i = 0; i < 4; i++)
			{
				colourArray[i] = colourArray[i+1];
			}
			
			colourArray[4] = (Colour)WMRandom.GetNextInt(0, 5, this.GetHashCode());	
		}
		
		public bool getSide()
		{
			return this.isLeftSide;
		}
		
		private void UpdateBalls()
		{
				for(int i = 0; i < ballList.Count; i++) 
				{
					if (ballList[i].GetState() == BallState.Rising)
					{
						ballList[i].Update();
					}
				}
		}
		
		public List<Ball> getBalls()
		{
			return this.ballList;
		}
	}
}

