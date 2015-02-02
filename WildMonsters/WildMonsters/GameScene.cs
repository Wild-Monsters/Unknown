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
				ParticleManager.AddParticle (this, new Vector2(data.X, data.Y));
			}
			
			levelManager.Update(deltaTime);
			ParticleManager.Update();
		}
	}
}