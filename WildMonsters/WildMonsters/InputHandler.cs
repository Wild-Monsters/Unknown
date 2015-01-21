using System;
using Sce.PlayStation.Core.Input;

namespace WildMonsters
{
	public static class InputHandler
	{
		public enum Key { Right, Left, Up, Down };
		
		private static bool[] 	keyPressed = new bool[4];
		
		
		public static bool KeyPressed(Key _key)
		{
			Update ();
			return keyPressed[(int)_key];
		}
	
		public static void Update()
		{
			InitKeys ();
			
			var gamePadData = GamePad.GetData (0);
			
			if((gamePadData.Buttons & GamePadButtons.Left) != 0)
			{
				keyPressed[(int)Key.Left] = true;
			}
			
			if((gamePadData.Buttons & GamePadButtons.Right) != 0)
			{
				keyPressed[(int)Key.Right] = true;
			}
			
			if((gamePadData.Buttons & GamePadButtons.Down) != 0)
			{
				keyPressed[(int)Key.Down] = true;
			}
			
			if((gamePadData.Buttons & GamePadButtons.Up) != 0)
			{
				keyPressed[(int)Key.Up] = true;
			}
			
		}
		
		private static void InitKeys()
		{
			for(int a = 0; a < keyPressed.Length; a++)
			{
				keyPressed[a] = false;
			}
		}
	}
}

