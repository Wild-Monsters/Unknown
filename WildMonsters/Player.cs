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
		
		private List <Ball> ballList;
		private LevelGrid levelGrid;
		
		const int queueSize = 3;
		private NextBallDisplay[] nextBallArray;
		private Colour currentColour;
		
		int spriteSheetLength = 5;
		
		public Player (Scene scene, bool _isLeftSide)
		{	
			//Set up sprite		
			texInfo = new TextureInfo("/Application/textures/Cannon2.png");
			sprite = new SpriteUV(texInfo);
			sprite.Quad.S = new Vector2(50.0f,50.0f);
			
			//Sets up the player sprite cannon/ball
			float spriteWidth = 1.0f / spriteSheetLength;
			sprite.UV.S = new Vector2(spriteWidth, 1.0f);
			
			//Initialise stuff based on the side the player is on
			isLeftSide = _isLeftSide;
			SetAngleOfSprite (isLeftSide);
			
			if(isLeftSide)
				sprite.Position = new Vector2(0, 250);
			
			if(!isLeftSide)
				sprite.Position = new Vector2(910, 250);
			
			//List that stores the balls that are currently being fired
			ballList = new List<Ball>();
			
			//Set up nextBall display
			currentColour = GetRandomColour ();
			SetColour (currentColour);
			
			nextBallArray = new NextBallDisplay[queueSize];
			
			for(int i = 0; i < queueSize; i++)
			{
				Colour newColour = GetRandomColour ();
				nextBallArray[i] = new NextBallDisplay(scene, isLeftSide, i + 1);
				nextBallArray[i].SetColour(newColour);
			}
			
			//Audio Stuff
			if(audio == null)
				audio = new AudioManager();
			
			//Add to the scene
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
			audio.PlayBlockShot();
			CreateNewBall(scene);
			
			//Push the queue of balls along, changing the current colour
			DisplayNextBall(scene);
			SetColour (currentColour);
		}
		
		public void DisplayNextBall(Scene scene)
		{	
			//Gets the colours that are available on the grid
			List<Colour> colourList = levelGrid.GetColoursOnGrid();
			
			//Get the next "Current Colour"
			currentColour = nextBallArray[0].GetColour ();
			
			if(colourList.Count > 0 && !colourList.Contains (currentColour))
			{
				currentColour = GetColourFromList(colourList);
			}
			else if(colourList.Count == 0)
			{
				currentColour = GetRandomColour ();
			}
			
			//Shift the queue of balls, generating new colours when a colour is no longer available
			for(int i = 0; i < queueSize-1; i++)
			{
				nextBallArray[i].SetColour (nextBallArray[i+1].GetColour());
				
				if(colourList.Count > 0 && !colourList.Contains (nextBallArray[i].GetColour()))
				{
					nextBallArray[i].SetColour (GetColourFromList(colourList));
				}
				else if(colourList.Count == 0)
				{
					nextBallArray[i].SetColour (GetRandomColour ());
				}
			}
			
			//Generate a new colour for the last position of the queue
			nextBallArray[queueSize-1].SetColour (GetColourFromList(colourList));
		}
		
		public void CheckBallDisplay()
		{
			//Gets the colours that are available on the grid
			List<Colour> colourList = levelGrid.GetColoursOnGrid();
			
			//Get the next "Current Colour"
			if(colourList.Count > 0 && !colourList.Contains (currentColour))
			{
				currentColour = GetColourFromList(colourList);
			}
			else if(colourList.Count == 0)
			{
				currentColour = GetRandomColour ();
			}
			SetColour(currentColour);
			
			//Shift the queue of balls, generating new colours when a colour is no longer available
			for(int i = 0; i < queueSize-1; i++)
			{			
				if(colourList.Count > 0 && !colourList.Contains (nextBallArray[i].GetColour()))
				{
					nextBallArray[i].SetColour (GetColourFromList(colourList));
				}
				else if(colourList.Count == 0)
				{
					nextBallArray[i].SetColour (GetRandomColour ());
				}
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
			ball.Sprite.Position = this.sprite.Position; 
			ball.SetColour(currentColour);
			
			ballList.Add(ball);
		}
		
		private void SetColour(Colour col)
		{
			//Sets the sprite of the cannon to the current colour
			float spriteWidth = 1.0f / spriteSheetLength;
			sprite.UV.S = new Vector2(spriteWidth, 1.0f);
			sprite.UV.T = new Vector2(spriteWidth * (int)col, 0.0f);	
		}
		
		private Colour GetRandomColour()
		{
			return (Colour)WMRandom.GetNextInt(0, 5, this.GetHashCode());
		}
		
		private Colour GetColourFromList(List<Colour> colourList)
		{
			int colourIndex = WMRandom.GetNextInt(0, colourList.Count, this.GetHashCode());
			
			return colourList[colourIndex];
		}
	}
}

