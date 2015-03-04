using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;
using System.Collections.Generic;
using Sce.PlayStation.Core.Audio;

namespace WildMonsters
{
	public class Player
	{
		private AudioManager audio;
		private SpriteUV sprite;
		private TextureInfo texInfo;
		private int movementSpeed = 50;
		private bool isLeftSide = false;	//deciding which side the player is on
		private int spriteHeight = 0;
		
		// Vars for fire delay
		private float fireDelay = 0.6f;
		private float totalTime = 0.0f;
		//--------------------
		
		private Colour[] colourArray;
		
		private List <Ball> ballList;
		private LevelGrid levelGrid;
		
		private NextBallDisplay[] nextBallArray;
		
		int spriteSheetLength = 5;
		
		public Player (Scene scene, bool _isLeftSide)
		{	
			if(audio == null)
			{
				audio = new AudioManager();
			}
			
			nextBallArray = new NextBallDisplay[3];
			colourArray = new Colour[5];
			
			isLeftSide = _isLeftSide;
						
			texInfo = new TextureInfo("/Application/textures/Cannon2.png");
			sprite = new SpriteUV(texInfo);
			sprite.Quad.S = new Vector2(50.0f,50.0f);
			
		
			for(int i = 0; i < 5; i++)
			{
				colourArray[i] = (Colour)WMRandom.GetNextInt(0, 5, this.GetHashCode());
			}
			
			//sets up the player sprite cannon/ball
			float spriteWidth = 1.0f / spriteSheetLength;
			sprite.UV.S = new Vector2(spriteWidth, 1.0f);
			sprite.UV.T = new Vector2(spriteWidth * (int)colourArray[0], 0.0f);	
			
			if(isLeftSide)
			{
				sprite.Position = new Vector2(0, 250);
			}
			else
			{
				sprite.Position = new Vector2(910, 250);
			}
			
			ballList = new List<Ball>();
			
			for(int i = 0; i < 3; i++)
			{
				nextBallArray[i] = new NextBallDisplay(scene, isLeftSide, i + 1);
				nextBallArray[i].SetColour(colourArray[i + 1]);
			}
			
			//Flip sprite based on the side of the screen
			SetAngleOfSprite (isLeftSide);
			
			scene.AddChild(sprite);
		}
		public void Update(Scene scene, float deltaTime)
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
			
			// For fire delay
			totalTime += deltaTime;
			
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
			if(totalTime > fireDelay)
			{
				audio.PlayBlockShot();
				CreateNewBall(scene);
				totalTime = 0.0f;
			}
						
//			colourList.RemoveAt(0);
		//	colourList.RemoveAt(1);
		}
		
		public void displayNextBall(Scene scene)
		{	
			int queueSize = 4;
			
			List<Colour> colourList = levelGrid.GetColoursOnGrid();

			//shift the random colours across one, and the fifth final one generates a new random coloru
			for(int i = 0; i < queueSize; i++)
			{
				if(!colourList.Contains (colourArray[i+1]))
				{
					colourArray[i+1] = levelGrid.GetRandomAvailableColour();
				}
				
				colourArray[i] = colourArray[i+1];
			}
			
			colourArray[queueSize] = levelGrid.GetRandomAvailableColour();
			
			for(int i = 0; i < 3; i++)
			{
				nextBallArray[i].SetColour(colourArray[i + 2]);
			}
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
		
		private void SetAngleOfSprite(bool onLeftSide)
		{
			//Set the point of rotation to the center of the sprite
			sprite.Pivot = new Vector2(25,25);
			
			//Rotate sprite left or right depending on the side of the screen
			if(onLeftSide)
			{
				sprite.Angle = Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.Deg2Rad(-90.0f);
			}
			else
			{
				sprite.Angle = Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.Deg2Rad(90.0f);
			}
		}
		
		public void SetLevelGrid(ref LevelGrid levelGrid)
		{
			this.levelGrid = levelGrid;
		}
		
		private void CreateNewBall(Scene scene)
		{
			//the firing ball moving across the screen
			Ball ball = new Ball(scene, isLeftSide);
			ball.SetState(BallState.Rising);
			//initialise at "cannon" pos
			ball.Sprite.Position = this.sprite.Position;
			//next colour being a random colour 
			ball.SetColour(colourArray[0]);
			
			//sets the sprite, which is the ball stationed in the cannon, to be 1/6th of the whole spritesheet
			float spriteWidth = 1.0f / spriteSheetLength;
			sprite.UV.S = new Vector2(spriteWidth, 1.0f);
			//set the ball stationed in the colour to be the colour of 'nextcolour2' which is the second different random colour val
			sprite.UV.T = new Vector2(spriteWidth * (int)colourArray[1], 0.0f);	
			
			displayNextBall(scene);
			
			ballList.Add(ball);
		}
	}
}

