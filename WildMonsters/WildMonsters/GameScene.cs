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
		Player player;
		Player player2;
		GamePadData gamepad;
		public GameScene()
		{
			
			Scheduler.Instance.ScheduleUpdateForTarget(this, 1, false);	// Tells the director to call the update function of this "node"
			player = new Player(this, true);
			player2 = new Player(this, false);
		}
		public override void Update(float deltaTime)
		{
			gamepad  = GamePad.GetData(0);
			player.Update(gamepad);
			player2.Update(gamepad);
		}
	}
}