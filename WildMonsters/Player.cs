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
		
		//starting colour
		private Colour firstColour;
		//colour immediately after fired
		private Colour secondColour;
		//Colour help in label
		private Colour thirdColour;
		
		//private Random rng
		
		private List <Ball> ballList;
		
		public Player (Scene scene, bool _isLeftSide)
		{			
			
			isLeftSide = _isLeftSide;
						
			texInfo = new TextureInfo("/Application/textures/Blocks2.png");
			sprite = new SpriteUV(texInfo);
			sprite.Quad.S = new Vector2(50.0f,50.0f);
			sprite.Position = new Vector2(0.0f, 0.0f);
			
			//Change colour of block
			//NextColour ();
			firstColour = (Colour)WMRandom.GetNextInt(0, 5, this.GetHashCode());
			float spriteWidth = 1.0f / 6.0f;
			sprite.UV.S = new Vector2(spriteWidth, 1.0f);
			sprite.UV.T = new Vector2(spriteWidth * (int)firstColour, 0.0f);	
			
			secondColour = (Colour)WMRandom.GetNextInt(0, 5, this.GetHashCode());
			thirdColour = (Colour)WMRandom.GetNextInt(0, 5, this.GetHashCode());

			
			//Messy test to initialise next ball display with game
//			Ball nextBall = new Ball(scene, isLeftSide);
//			nextBall.SetState(BallState.Nostate);
//			if(nextBall.OnLeftSide)
//			{
//				nextBall.Sprite.Position = new Vector2(0.0f, 0.0f);
//			}
//			else
//			{
//				nextBall.Sprite.Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width - 25.0f, 0.0f);
//			}
//			nextBall.SetColour(nextColour3);
//			nextBall.Sprite.Scale = new Vector2(0.5f, 0.5f);
//			nextColour = nextColour2;
//			nextColour2 = nextColour3;
//			nextColour3 = (Colour)WMRandom.GetNextInt(0, 5, this.GetHashCode());
			//end of messy test
			
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
			ball.SetColour(firstColour);
			
			//sets the sprite, which is the ball stationed in the cannon, to be 1/6th of the whole spritesheet
			float spriteWidth = 1.0f / 6.0f;
			sprite.UV.S = new Vector2(spriteWidth, 1.0f);
			//set the ball stationed in the colour to be the colour of 'nextcolour2' which is the second different random colour val
			sprite.UV.T = new Vector2(spriteWidth * (int)secondColour, 0.0f);	
			
			displayNextBall(scene);
			
			ballList.Add(ball);
						
//			colourList.RemoveAt(0);
		//	colourList.RemoveAt(1);
		}
		
		public void displayNextBall(Scene scene)
		{
			//Display for the next upcoming ball's colour
			NextBallDisplay nextBall = new NextBallDisplay(scene, isLeftSide);			
			//set the next ball display to be a THIRD different random colour
			nextBall.SetColour(thirdColour);
			//scale down the nextball display, so its smaller than the fireable balls
			nextBall.Sprite.Scale = new Vector2(0.5f, 0.5f);
			

			//NextColour ();
			
			//shift the random colours across one, and the third final one generates a new random coloru
			firstColour = secondColour;
			secondColour = thirdColour;
			
			thirdColour = (Colour)WMRandom.GetNextInt(0, 5, this.GetHashCode());
			
			
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
						ballList[i].Update();
					}

				}
		
		}
		public List<Ball> getBalls()
		{
			return this.ballList;
		}
		
//		private void NextColour()
//		{
//			// 0 = red
//			// 1 = blue
//			// 2 = yellow
//			// 3 = purple
//			// 4 = green
//			// 5 = Grey?
//
//	//			
//				
//				nextColour = (Colour)WMRandom.GetNextInt(0, 5, this.GetHashCode());
//
//				
////				float spriteWidth = 1.0f / 6.0f;
////				sprite.UV.S = new Vector2(spriteWidth, 1.0f);
////				sprite.UV.T = new Vector2(spriteWidth * (int)currentColour, 0.0f);	
//			
//			
//		}
		
		
	}
}

