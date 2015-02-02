using System;
using Sce.PlayStation.Core.Input;


namespace WildMonsters
{
	public static class Input
	{
		private static bool[] 	keyPress = new bool[130];
		private static bool[] 	keyDown = new bool[130];

		public static bool KeyDown(GamePadButtons _button)
		{
			GetKey ( _button);
			return keyDown[(int)_button];
		}
				
		public static bool KeyPressed(GamePadButtons _button)
		{
			GetKey (_button);
			return keyPress[(int)_button];
		}

		private static void GetKey(GamePadButtons _button)
		{
			var gamePadData = GamePad.GetData (0);
			
			if((gamePadData.Buttons & _button) != 0)
			{
				if(keyDown[(int)_button] == false)
				{
					keyPress[(int)_button] = true;
				}
				else
				{
					keyPress[(int)_button] = false;
				}
				
				keyDown[(int)_button] = true;
			}
			else
			{
				keyPress[(int)_button] = false;
				keyDown[(int)_button] = false;
			}
		}
	}
}
