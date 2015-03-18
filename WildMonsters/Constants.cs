using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace WildMonsters
{
	public static class Constants
	{
		// Constant Variables:
		private static float screenWidth = Director.Instance.GL.Context.GetViewport().Width;
		private static float screenHeight = Director.Instance.GL.Context.GetViewport().Height;
		private static int spriteSheetSize = 9;
		
		// Accessors:
		public static float ScreenWidth{get{return screenWidth;}}
		public static float ScreenHeight{get{return screenHeight;}}
		public static int SpriteSheetSize{get{return spriteSheetSize;}}
	}
}

