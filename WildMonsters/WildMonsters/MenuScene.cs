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
	public class MenuScene : Scene
	{
		public MenuScene ()
		{
			Scheduler.Instance.ScheduleUpdateForTarget(this, 1, false);	// Tells the director to call the update function of this "node"
		}
		
		public override void Update(float deltaTime)
		{
			
		}
	}
}

