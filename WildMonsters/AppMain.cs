using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.UI;
	
namespace WildMonsters
{
	public class AppMain
	{
		private enum Scene {Game, Menu};
		
		private static GameScene	gameScene;
		private static TitleScene   	titleScene;
		//private static Scene		currentScene;
		
		private static Timer		timer;
		private static float		deltaTime;
		private static bool			quit;
		
		private enum WMScene {Title, Game};
				
		public static void Main (string[] args)
		{
			Initialize();
			
			while (!quit)
			{
				deltaTime = (float)timer.Milliseconds();
				
				//Update (deltaTime);
				
				Director.Instance.Update();
				Director.Instance.Render();
				UISystem.Render();
				
				Director.Instance.GL.Context.SwapBuffers();
				Director.Instance.PostSwap();
			}
			
			Director.Terminate ();
		}
		
		
		public static void Initialize ()
		{
			quit = false;
			timer = new Timer();
			
			//Set up director
			Director.Initialize ();
			UISystem.Initialize(Director.Instance.GL.Context);
			
			Sce.PlayStation.HighLevel.UI.Scene blankUI = new Sce.PlayStation.HighLevel.UI.Scene();
			UISystem.SetScene(blankUI);
			
			WMScene startScene = WMScene.Title;
			
			switch(startScene)
			{
			case WMScene.Game:
				gameScene = new GameScene();
				gameScene.Camera.SetViewFromViewport();

				//Run the scene.
				Director.Instance.RunWithScene(gameScene, false);
				break;
				
			case WMScene.Title:
				titleScene = new TitleScene();
				titleScene.Camera.SetViewFromViewport();

				//Run the scene.
				Director.Instance.RunWithScene(titleScene, false);
				break;
			}

		}
	}
}
