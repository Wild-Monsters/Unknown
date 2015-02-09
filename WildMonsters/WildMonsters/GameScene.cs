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
	public class GameScene : Scene
	{
		private LevelManager levelManager;

		public GameScene()
		{
			this.Camera.SetViewFromViewport();
			Scheduler.Instance.ScheduleUpdateForTarget(this, 1, false);	// Tells the director to call the update function of this "node"
			
			levelManager = new LevelManager(this);
			
		}
		
		public override void Update(float deltaTime)
		{
			List<TouchData> touches = Touch.GetData(0);
			
			foreach(TouchData data in touches)
			{
					float screenWidth = Director.Instance.GL.Context.GetViewport().
					Width;
			
					float screenHeight = Director.Instance.GL.Context.GetViewport().
					Height;
				
				//ParticleManager.AddParticle (this, new Vector2(data.X*960, data.Y*544));
//				ParticleManager.AddParticle (this, new Vector2((data.X + 0.5f) * screenWidth,
//					screenHeight - ((data.Y + 0.5f) * screenHeight)), 100);
				
			Random rand = new Random();
				
		    double randomNumWidth = rand.NextDouble();
			randomNumWidth *= 50.0;
				
			double randomNumHeight = rand.NextDouble();
			randomNumHeight *= 50.0;
			
			
			ParticleManager.AddParticle (this, new Vector2((((data.X + 0.5f) * screenWidth) + (float)randomNumWidth),
					screenHeight - ((data.Y + 0.5f) * screenHeight) + (float)randomNumHeight), 1);
			}
			
			levelManager.Update(deltaTime);
			ParticleManager.Update(this);
		}
		
		public override void Draw ()
		{
			base.Draw ();
		}
	}
}