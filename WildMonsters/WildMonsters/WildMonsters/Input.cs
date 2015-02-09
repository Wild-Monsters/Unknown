using System;
using Sce.PlayStation.Core.Input;


namespace WildMonsters
{
	public enum Analog {leftX, leftY, rightX, rightY};
	
	public static class Input
	{
		private static GamePadData gamePadData = new GamePadData();
		
		private static bool[] 	keyPress = new bool[16];
		private static bool[] 	keyDown = new bool[16];
		private static float[] 	keyTimer = new float[16];
		
		private static bool[]   analogPress = new bool[8];
		private static bool[]   analogDown = new bool[8];
		private static float[]  analogTimer = new float[8];
		
		//////////////////
		//// ANALOG INPUTS
		// Analog = gets the x or y position of the specified analog stick
		// Positive = the direction the analog is being pushed in.
		// Deadzone = how much the analog stick needs to be pushed to return true
		
		
		//Returns true if the analog is "pressed" down
		public static bool AnalogDown(Analog a, bool positive, float deadzone)
		{
			return analogDown[CheckAnalog (a, positive, deadzone)];
		}
		
		//Only returns true on the frame that the analog is pressed
		public static bool AnalogPress(Analog a, bool positive, float deadzone)
		{
			return analogPress[CheckAnalog (a, positive, deadzone)];
		}
		
		//Returns true on press and after the specified delay
		public static bool AnalogPress(Analog a, bool positive, float deadzone, float delay)
		{
			int index = CheckAnalog (a, positive, deadzone);
			
			if(analogDown[index])
			{
				if(analogTimer[index] < 1)
				{
					analogTimer[index] = delay;
					return true;
				}
				else
				{
					analogTimer[index] --;
				}
			}
			else
			{
				analogTimer[index] = 0;
			}
			
			return false;
		}
		
		// Returns X or Y position of the chosen analog stick 
		public static float GetAnalog(Analog a) 
		{
			gamePadData = GamePad.GetData (0);
			
			switch(a)
			{
				case Analog.leftX:
					return gamePadData.AnalogLeftX;
				
				case Analog.leftY:
					return gamePadData.AnalogLeftY;
				
				case Analog.rightX:
					return gamePadData.AnalogRightX;
				
				case Analog.rightY:
					return gamePadData.AnalogRightY;
			}
			
			return 0;
		}

		/////////////////////////////////
		///////GAME PAD
		
		//Returns true if button is down
		public static bool KeyDown(GamePadButtons _button)
		{
			return keyDown[CheckKey (_button)];
		}
		
		//Returns true if the button was pressed that frame
		public static bool KeyPressed(GamePadButtons _button)
		{
			return keyPress[CheckKey (_button)];
		}
		
		//Returns true when pressed and after the specified delay
		public static bool KeyPressed(GamePadButtons _button, float delay)
		{
			int index = CheckKey (_button);
			
			if(keyDown[index])
			{
				if(keyTimer[index] < 1)
				{
					keyTimer[index] = delay;
					return true;
				}
				else
				{
					keyTimer[index] --;
				}
			}
			else
			{
				keyTimer[index] = 0;
			}
			
			return false;
		}
		
		
		
		
		
		
		
		

		
		
		
		
		
		
		//////////////////////
		/////Private Functions
		//////////////////////
		
		private static int CheckAnalog(Analog a, bool positive, float deadzone)
		{
			bool pressed = false;
			int index = 0;
			
			gamePadData = GamePad.GetData (0);
			
			if(!positive)
				deadzone = 0-deadzone;
			
			switch(a)
			{
				case Analog.leftX:
					index = 0;
					pressed = gamePadData.AnalogLeftX > deadzone == positive;
					break;

				case Analog.leftY:
					index = 2;
					pressed = gamePadData.AnalogLeftY > deadzone == positive;
					break;
				
				case Analog.rightX:
					index = 4;
					pressed = gamePadData.AnalogRightX > deadzone == positive;
					break;
				
				case Analog.rightY:
					index = 6;
					pressed = gamePadData.AnalogRightY > deadzone == positive;
					break;
			}
			
			if(!positive)
				index++;
			
			if(pressed)
			{
				if(analogDown[index] == false)
				{
					analogPress[index] = true;
				}
				else
				{
					analogPress[index] = false;
				}
			
				analogDown[index] = true;
			}
			else
			{
				analogDown[index] = false;
				analogPress[index] = false;
			}
			
			return index;
		}
		
		private static int CheckKey(GamePadButtons _button)
		{
			gamePadData = GamePad.GetData (0);
			int index = GetBIndex(_button);
			
			if((gamePadData.Buttons & _button) != 0)
			{
				if(keyDown[index] == false)
				{
					keyPress[index] = true;
				}
				else
				{
					keyPress[index] = false;
				}
				
				keyDown[index] = true;
			}
			else
			{
				keyPress[index] = false;
				keyDown[index] = false;
			}
			
			return index;
		}

		private static int GetBIndex(GamePadButtons _button)
		{
			switch(_button)
			{
				case GamePadButtons.Left:
					return 0;
				case GamePadButtons.Up:
					return 1;
				case GamePadButtons.Down:
					return 2;
				case GamePadButtons.Right:
					return 3;
				case GamePadButtons.Triangle:
					return 4;
				case GamePadButtons.Square:
					return 5;
				case GamePadButtons.Circle:
					return 6;
				case GamePadButtons.Cross:
					return 7;
				case GamePadButtons.L:
					return 8;
				case GamePadButtons.R:
					return 9;
				case GamePadButtons.Start:
					return 10;
				case GamePadButtons.Select:
					return 11;
				case GamePadButtons.Enter:
					return 12;
				case GamePadButtons.Back:
					return 13;
				default:
					return 14;
			}
		}
		/////////////////////////////////
	}
}
