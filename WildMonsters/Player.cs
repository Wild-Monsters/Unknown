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
		private int touchMovementSpeed = 50;
		private bool isLeftSide = false;	//deciding which side the player is on
		private int spriteHeight = 0;
		
		private Timer playerShootTimer;
        private const float playerShootDelay = 700;
		
		private Vector2 initialTouchPosition;
		private Vector2 currentTouchPosition;
		private bool screenTouched = false;
		private bool canFire = false;
		
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
		
		private bool bCanMove = true;
		
		private bool gameOver = false;
		
		//touch screen pressed booleans
		bool topLeftTouched = false;
		bool bottomLeftTouched = false;
		bool topRightTouched = false;
		bool bottomRightTouched = false;
		
		//bool[] touchButtons = new bool[4];
		
		//touch screen Delays
		
		float topLeftDelay = 8.0f;
		float topRightDelay = 8.0f;
		float bottomLeftDelay = 8.0f;
		float bottomRightDelay = 8.0f;
		
		float touchDelay = 8.0f;
		
		public Player (Scene scene, bool _isLeftSide)
		{	
			playerShootTimer = new Timer();
			
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
		public SpriteUV Sprite
		{
			get{ return sprite; }
			set{ sprite = value; }
		}
		public void Update(Scene scene, float deltaTime)
		{
			if(!gameOver)
			{
				alignCannon();
				
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
				
				if (!(Input.KeyDown (upButton) && Input.KeyDown (downButton)))
				{	
					//Use analog or the buttons to move the character
					if (Input.AnalogPress(moveAnalog, false, deadzone, analogDelay) || Input.KeyPressed (upButton, buttonDelay) && bCanMove) //Go left (up)
					{
						sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y + movementSpeed);
					}
					
					if (Input.AnalogPress(moveAnalog, true, deadzone, analogDelay) || Input.KeyPressed (downButton, buttonDelay) && bCanMove) //go right (down)
					{
						sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y - movementSpeed);
					}
				}
				if(bCanMove)
				{
					TouchScreenControls(scene);
				}
				
				// For fire delay
				totalTime += deltaTime;
				
				//Perform action
				if(Input.KeyPressed(actionButton) && bCanMove)
				{
					Fire (scene);
				}
				
				//Touch screen Fire, using either flick or double Tap
	//			Sce.PlayStation.HighLevel.UI.FlickGestureDetector flickDetector = new Sce.PlayStation.HighLevel.UI.FlickGestureDetector();
	//			
	//			flickDetector.Direction = Sce.PlayStation.HighLevel.UI.FlickDirection.Horizontal;
	//			flickDetector.MinSpeed = 0.1f;
	//			flickDetector.MaxSpeed = 10000.0f;
	//       
	//			
	//			flickDetector.FlickDetected += 
	//			delegate(object sender, Sce.PlayStation.HighLevel.UI.FlickEventArgs e) 
	//			{
	//				//TODO Check position of flick, using e (args)
	//	
	//				System.Diagnostics.Debug.WriteLine("Flick!");
	//				//TODO: seperate flick 'Fire' calls out to player 1 and player 2
	//				Fire (scene);
	//			
	//			};
	//			
	//			Sce.PlayStation.HighLevel.UI.DoubleTapGestureDetector doubleTapDetector = new Sce.PlayStation.HighLevel.UI.DoubleTapGestureDetector();
	//			
	//			doubleTapDetector.DoubleTapDetected +=
	//			delegate(object sender, Sce.PlayStation.HighLevel.UI.DoubleTapEventArgs e)
	//			{
	//				Console.WriteLine("Double clicked!");
	//				Fire (scene);
	//				
	//			};
			
			
				UpdateBalls();
	
				//lock it to screen 
				ScreenCollision();
			}

		}
		
		private bool ScreenTouched()
		{
			var touches = Touch.GetData(0).ToArray();
			return (touches.Length > 0 && (touches[0].Status == TouchStatus.Down || touches[0].Status == TouchStatus.Move));
		}
		
		private bool FlickGesture(bool leftSide)
		{
			float yDiff = (initialTouchPosition.Y - currentTouchPosition.Y);
			float maxYDiff = 30.0f;
			
			if(leftSide)
			{
				return (currentTouchPosition.Distance(initialTouchPosition) > 40.0f 
					&& initialTouchPosition.X < currentTouchPosition.X && (yDiff < maxYDiff && yDiff > -maxYDiff));
			}
			else
			{
				return (currentTouchPosition.Distance(initialTouchPosition) > 40.0f 
					&& initialTouchPosition.X > currentTouchPosition.X && (yDiff < maxYDiff && yDiff > -maxYDiff));
			}
		}
		
		private bool PositionOnPlayersSide(Vector2 position)
		{
			return ((position.X < Constants.ScreenWidth/2) == isLeftSide);
		}
		
		private void TouchScreenControls(Scene scene)
		{
			var touches = Touch.GetData(0).ToArray();
		
			if(ScreenTouched ())
			{
				if(screenTouched == false)
				{
					screenTouched = true;
					initialTouchPosition = GetTouchPosition(touches[0], Constants.ScreenWidth, Constants.ScreenHeight);
				}
				
				currentTouchPosition = GetTouchPosition(touches[0], Constants.ScreenWidth, Constants.ScreenHeight);
					
				if(canFire && FlickGesture(isLeftSide))
				{
					if(PositionOnPlayersSide(currentTouchPosition))
					{
						canFire = false;
						Fire(scene);
					}
				}

                touchMovement(currentTouchPosition);
				
			}
			else
			{
				screenTouched = false;
				canFire = true;
			}
		}
		
		private void touchMovement(Vector2 touchPos)
		{
			float touchZone = 100.0f;
			
			if(isLeftSide)
			{
				if(touchPos.X < (Constants.ScreenWidth / 2) && touchPos.Y < touchZone)
				{
					topLeftTouched = true;
					bottomLeftTouched = false;
					
					if(topLeftTouched)
					{
						if(topLeftDelay < 1.0f)
						{
							topLeftDelay = touchDelay;
							//top left quadrant touched
							sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y + touchMovementSpeed);
						}
						else
						{
							topLeftDelay--;
						}
					}
					else
					{
						topLeftDelay = 0.0f;
					}
				}
			 
				if(touchPos.X < (Constants.ScreenWidth / 2) && touchPos.Y > (Constants.ScreenHeight - touchZone))
			    {
					bottomLeftTouched = true;
					topLeftTouched = false;
					if(bottomLeftTouched)
					{
						if(bottomLeftDelay < 1.0f)
						{
							bottomLeftDelay = touchDelay;
							//bottom left quadrant pressed
							sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y - touchMovementSpeed);
						}
						else
						{
							bottomLeftDelay--;
						}
					}
					else
					{
						bottomLeftDelay = 0.0f;
					}
				}	
			}
			
			if(!isLeftSide)
			{
				if(touchPos.X > (Constants.ScreenWidth / 2) && touchPos.Y < touchZone)
			    {
					topRightTouched = true;
					bottomRightTouched = false;
					if(topRightTouched)
					{
						if(topRightDelay < 1.0f)
						{
							topRightDelay = touchDelay;
							//Top right quadrant pressed
							sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y + touchMovementSpeed);
						}
						else
						{
							topRightDelay--;
						}
					}
					else
					{
						topRightDelay = 0.0f;
					}
					
				}
			
				if(touchPos.X > (Constants.ScreenWidth / 2) && touchPos.Y > (Constants.ScreenHeight - touchZone))
			    {
					
					topRightTouched = false;
					bottomRightTouched = true;
					if(bottomRightTouched)
					{
						if(bottomRightDelay < 1.0f)
						{
							bottomRightDelay = touchDelay;
							//Bottom right quadrant pressed
							sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y - touchMovementSpeed);
						}
						else
						{
							bottomRightDelay--;
						}
					}
					else
					{
						bottomRightDelay = 0.0f;
					}
				}
			}
		}
		
		private double Distance(Vector2 pointA, Vector2 pointB)
        {
            return FMath.Sqrt((pointB.X - pointA.X) * (pointB.X - pointA.X) + (pointB.Y - pointA.Y) * (pointB.Y - pointA.Y));
            
        }
        
        private void alignCannon()
        {
            //0 to 450
            if(sprite.Position.Y % 50 != 0)
            {
                //not aligned, because not divisible by 50
                
                //loop 0 to 450 in increments of 25
                for(int i = 0; i < 450; i+=50)
                {
                    //loop for subtraction
                    if(sprite.Position.Y > i && sprite.Position.Y < i+25)
                    {
                        sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y - 1);
                    }
                }
                for(int i = 25; i < 450; i+=50)
                {
                    //loop for addition
                    if(sprite.Position.Y >= i && sprite.Position.Y < i+25)
                    {
                        sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y + 1);
                    }
                }
            }
        }
		
		private Vector2 GetTouchPosition(TouchData touch, float screenWidth, float screenHeight)
		{
			float touchX =  (int)((touch.X + 0.5f) * screenWidth);
			float touchY =  (int)((touch.Y + 0.5f) * screenHeight);
			
			return new Vector2(touchX, touchY);
		}
		
		public void ScreenCollision ()
		{
			//if((sprite.Position.Y + spriteHeight )>= 544 )
			if((sprite.Position.Y + spriteHeight )>= 450 )
			{
				//sprite.Position = new Vector2(sprite.Position.X, 544 - spriteHeight - 44);
				sprite.Position = new Vector2(sprite.Position.X, 450 - spriteHeight);
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
		
		public void AIFiresDifferently(Scene scene)// array of balls 
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
			foreach(Ball ball in ballList)
			{
				ball.Update();
			}
		}
		
		public List<Ball> getBalls()
		{
			return this.ballList;
		}
		public Colour GetCurrentColour()
		{
			return this.currentColour;
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
				
		public void MoveDown()
		{
			sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y - movementSpeed);
		}
		public void MoveUp()
		{
			sprite.Position = new Vector2 (sprite.Position.X, sprite.Position.Y + movementSpeed);
		}
		
		public void CanMove(bool bCanMove)
		{
			this.bCanMove = bCanMove;
		}
		public void SetGameOver(bool b)
		{
			gameOver = b;
		}
	}
}

