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
		private Ball testBall;

		public GameScene()
		{
			Scheduler.Instance.ScheduleUpdateForTarget(this, 1, false);	// Tells the director to call the update function of this "node"
			
			levelManager = new LevelManager(this);
			testBall = new Ball(this);
		}
		
		public override void Update(float deltaTime)
		{
			levelManager.Update(deltaTime);
		}
	}
}